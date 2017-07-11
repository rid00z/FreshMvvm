using System.Threading.Tasks;

namespace FreshMvvm.CoreMethods
{
    public interface IPageModelNotifications
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}

