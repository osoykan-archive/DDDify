using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Docker.DotNet;
using Docker.DotNet.Models;

namespace TestBase
{
    public static class DockerHelper
    {
        private static readonly DockerClient DockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();


        public static async Task BuildImageIfNotExists(string image, string buildContentTar, int timeoutMiliseconds = 10000)
        {
            using (var ts = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
            {
                IList<ImagesListResponse> imagesListResponses = await DockerClient.Images.ListImagesAsync(new ImagesListParameters(), ts.Token).ConfigureAwait(false);
                bool imageFound = imagesListResponses.Any(i =>
                     i.RepoTags != null &&
                     i.RepoTags.Any(t => string.Equals(t, image, StringComparison.OrdinalIgnoreCase)));

                if (!imageFound)
                {
                    //string[] imageAndTag = image.Split(':');
                    var parameters = new ImageBuildParameters
                    {
                        Tags = new List<string> { image },
                    };

                    using (var fs = new FileStream(buildContentTar, FileMode.Open, FileAccess.Read))
                    {
                        await DockerClient.Images.BuildImageFromDockerfileAsync(fs, parameters, ts.Token);
                    }
                }
            }

        }

        public static async Task<IDisposable> StartContainerAsync(
            string image,
            IReadOnlyCollection<int> ports = null,
            IReadOnlyDictionary<string, string> environment = null)
        {
            using (var ts = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
            {
                List<string> env = environment
                    ?.OrderBy(kv => kv.Key)
                     .Select(kv => $"{kv.Key}={kv.Value}")
                     .ToList();


                Debug.WriteLine($"Starting container with image '{image}'");
                CreateContainerResponse createContainerResponse = await DockerClient.Containers.CreateContainerAsync(
                    new CreateContainerParameters(
                        new Config
                        {
                            Image = image,
                            Env = env,
                            ExposedPorts = ports?
                                .ToDictionary(p => $"{p}/tcp", p => new EmptyStruct())
                        })
                    {
                        HostConfig = new HostConfig
                        {
                            PortBindings = ports?.ToDictionary(
                                p => $"{p}/tcp",
                                p => (IList<PortBinding>)new List<PortBinding>
                                {
                                    new PortBinding { HostPort = p.ToString() }
                                })
                        }
                    },
                    ts.Token)
                                                                                    .ConfigureAwait(false);
                Debug.WriteLine($"Successfully created container '{createContainerResponse.ID}'");

                await DockerClient.Containers.StartContainerAsync(
                    createContainerResponse.ID,
                    new ContainerStartParameters(),
                    ts.Token)
                                  .ConfigureAwait(false);
                Debug.WriteLine($"Successfully started container '{createContainerResponse.ID}'");

                return new DisposableAction(() =>
                {
                    StopContainer(createContainerResponse.ID);
                    DeleteImage(image);
                });
            }
        }

        private static void DeleteImage(string imagename)
        {
            try
            {
                DockerClient.Images.DeleteImageAsync(imagename,
                    new ImageDeleteParameters()
                    {
                        Force = true,
                    }).Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static void StopContainer(string id)
        {
            try
            {
                DockerClient.Containers.StopContainerAsync(
                    id,
                    new ContainerStopParameters
                    {
                        WaitBeforeKillSeconds = 5
                    }).Wait();
                Debug.WriteLine($"Stopped container {id}");

                DockerClient.Containers.RemoveContainerAsync(
                    id,
                    new ContainerRemoveParameters
                    {
                        Force = true
                    }).Wait();
                Debug.WriteLine($"Removed container {id}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
