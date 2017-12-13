using System;
using System.Threading;

namespace DDDify
{
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Null = new DisposeAction(null);
        private Action _action;

        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            Action action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
