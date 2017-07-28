using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm.CoreMethods
{
    public class PageModelNotifications : IPageModelNotifications
    {
        private readonly Page _currentPage;

        public PageModelNotifications(Page currentPage)
        {
            _currentPage = currentPage;
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            if (_currentPage != null)
                await _currentPage.DisplayAlert(title, message, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayActionSheet(title, cancel, destruction, buttons);
            return null;
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayAlert(title, message, accept, cancel);
            return false;
        }
    }
}