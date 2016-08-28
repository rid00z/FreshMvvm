using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreshMvvm
{
    public interface IFreshNavigationCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
