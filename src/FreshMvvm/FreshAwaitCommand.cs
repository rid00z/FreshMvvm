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
        Action<object, TaskCompletionSource<object>> _execute;
        volatile bool _canExecute = true;

        /// <summary>
        /// NB* This command waits until you call SetResult on the TaskCompletionSource. The return 
        /// value on the TaskCompletionSource is not used.
        /// </summary>
        /// <param name="execute">Execute.</param>
        public FreshAwaitCommand(Action<object, TaskCompletionSource<object>> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public async void Execute(object parameter)
        {
            try
            {
                _canExecute = false;
                RaiseCanExecuteChanged();
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                _execute(parameter, tcs);
                await tcs.Task;
            }
            finally
            {
                _canExecute = true;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                try
                {
                    handler(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

