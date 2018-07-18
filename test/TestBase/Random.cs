using System.Linq;
using AutoFixture;

namespace TestBase
{
    public static class Random<T>
    {
        private static readonly Fixture fixture = new Fixture();

        public static T _ => fixture.Create<Generator<T>>().First();
    }
}