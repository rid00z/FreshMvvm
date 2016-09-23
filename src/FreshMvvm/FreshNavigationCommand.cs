using System;
using System.Threading;
using System.Threading.Tasks;

namespace FreshMvvm
{
    /// <summary>
    /// Command that will execute and ignore all new events until it's finished.
    /// Typicaly used to ignore multiple events from doubleclicking by the user.
    /// </summary>
    public class FreshNavigationCommand : IFreshNavigationCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { _sharedLock.SharedCanExecuteChanged += value; }
            remove { _sharedLock.SharedCanExecuteChanged -= value; }
        }

        private readonly SharedLock _sharedLock;
        private readonly Func<object, Task> _execute;

        public FreshNavigationCommand(Func<object, Task> execute, SharedLock sharedLock = null)
        {
            if (execute == null)
                throw new ArgumentException(nameof(execute));

            _execute = execute;
            _sharedLock = sharedLock ?? new SharedLock();
        }

        public FreshNavigationCommand(Func<Task> execute, SharedLock sharedLock = null)
            : this((obj) => execute(), sharedLock)
        {
            if (execute == null)
                throw new ArgumentException(nameof(execute));
        }

        public bool CanExecute(object parameter)
        {
            return !_sharedLock.IsLocked;
        }

        /// <summary>
        /// Execute the command with specified parameter.
        /// The caller will not be able to await this command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public async void Execute(object parameter)
#pragma warning restore RECS0165
        {
            await ExecuteAsync(parameter);
        }

        /// <summary>
        /// Execute the async command with specified parameter.
        /// If more than one is executed at the same time, all other than the first one will be ignored.
        /// </summary>
        /// <returns>The async task, that can be awaited.</returns>
        /// <param name="parameter">Parameter.</param>
        public async Task ExecuteAsync(object parameter)
        {
            if (_sharedLock.TakeLock()) //Ignores code block if lock already taken in SharedLock.
            {
                try
                {
                    await _execute(parameter);
                }
                finally
                {
                    _sharedLock.ReleaseLock();
                }
            }
        }
    }

    public class FreshNavigationCommand<TValue> : FreshNavigationCommand
    {
        public FreshNavigationCommand(Func<TValue, Task> execute, SharedLock sharedLock = null)
            : base(obj => execute((TValue)obj), sharedLock)
        {
            if (execute == null)
                throw new ArgumentException(nameof(execute));
        }
    }

    /// <summary>
    /// Locking object that can be shared between commands.
    /// If multiple commands use the same shared object, only one of them can run simultanious.
    /// </summary>
    public class SharedLock
    {
        private int _lock;

        public bool IsLocked => _lock != 0;

        public event EventHandler SharedCanExecuteChanged;

        public SharedLock()
        {
            _lock = 0;
        }

        /// <summary>
        /// Will take a threadsafe lock on the object.
        /// </summary>
        /// <returns><c>true</c>, if lock was taken, <c>false</c> otherwise.</returns>
        public bool TakeLock()
        {
            var oldVal = Interlocked.Exchange(ref _lock, 1); //Atomic swap values.
            var lockTaken = oldVal == 0; //If the old value was 0, no one else is executing at this time.

            var events = SharedCanExecuteChanged;
            if (events != null)
                events(this, EventArgs.Empty);

            return lockTaken;
        }

        /// <summary>
        /// Will release the treadsafe lock on the object.
        /// </summary>
        /// <returns><c>true</c>, if lock was released, <c>false</c> otherwise.</returns>
        public bool ReleaseLock()
        {
            var oldVal = Interlocked.Exchange(ref _lock, 0);
            var lockReleased = oldVal == 1;

            var events = SharedCanExecuteChanged;
            if (events != null)
                events(this, EventArgs.Empty);

            return lockReleased;
        }
    }
}