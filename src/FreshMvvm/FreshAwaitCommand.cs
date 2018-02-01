using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreshMvvm
{
    /// <summary>
    /// FreshAwaitCommand is designed to avoid the double tap issue in Xamarin.Forms for Android,
    /// in Xamarin.Forms it's a common issue that double taps on command would open the same window multiple times.
    ///
    /// This command awaits TaskCompletionSource before allowing anymore of the command to execute.
    ///
    /// NB* You must call the SetResult on the TaskCompletionSource otherwise this command will wait forever.
    /// </summary>
    public class FreshAwaitCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;

        private readonly Action<object, TaskCompletionSource<bool>> _execute;

        private volatile bool _inProgress = false;

        /// <summary>
        /// NB* This command waits until you call SetResult on the TaskCompletionSource. The return
        /// value on the TaskCompletionSource is not used.
        /// </summary>
        /// <param name="execute">Execute.</param>
        public FreshAwaitCommand(Action<object, TaskCompletionSource<bool>> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// NB* This command waits until you call SetResult on the TaskCompletionSource. The return
        /// value on the TaskCompletionSource is not used.
        /// </summary>
        /// <param name="execute">Execute.</param>
        public FreshAwaitCommand(Action<TaskCompletionSource<bool>> execute) : this((o, tcs) => execute(tcs))
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <summary>
        /// NB* This command waits until you call SetResult on the TaskCompletionSource. The return
        /// value on the TaskCompletionSource is not used.
        /// </summary>
        /// <param name="execute">Execute.</param>
        public FreshAwaitCommand(Action<object, TaskCompletionSource<bool>> execute, Func<object, bool> canExecute) : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// NB* This command waits until you call SetResult on the TaskCompletionSource. The return
        /// value on the TaskCompletionSource is not used.
        /// </summary>
        /// <param name="execute">Execute.</param>
        public FreshAwaitCommand(Action<TaskCompletionSource<bool>> execute, Func<bool> canExecute) : this((o, tcs) => execute(tcs), o => canExecute())
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_inProgress)
            {
                return false;
            }
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            try
            {
                _inProgress = true;
                ChangeCanExecute();

                var tcs = new TaskCompletionSource<bool>();

                _execute(parameter, tcs);

                await tcs.Task;
            }
            finally
            {
                _inProgress = false;
                ChangeCanExecute();
            }
        }

        public void ChangeCanExecute()
        {
            try
            {
                var changed = CanExecuteChanged;

                changed?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
            }
        }
    }
}