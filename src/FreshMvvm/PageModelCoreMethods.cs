using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public class PageModelCoreMethods : IPageModelCoreMethods
    {
        Page _currentPage;
        FreshBasePageModel _pageModel;

        public PageModelCoreMethods (Page currentPage, FreshBasePageModel pageModel)
        {
            _currentPage = currentPage;
            _pageModel = pageModel;
        }

        public async Task DisplayAlert (string title, string message, string cancel)
        {
            if (_currentPage != null)
                await _currentPage.DisplayAlert (title, message, cancel);
        }

        public async Task<string> DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayActionSheet (title, cancel, destruction, buttons);
            return null;
        }

        public async Task<bool> DisplayAlert (string title, string message, string accept, string cancel)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayAlert (title, message, accept, cancel);	
            return false;
        }

        public async Task PushPageModel<T> (object data, bool modal = false) where T : FreshBasePageModel
        {
            T pageModel = FreshIOC.Container.Resolve<T> ();

            var page = FreshPageModelResolver.ResolvePageModel<T> (data, pageModel);

            pageModel.PreviousPageModel = _pageModel;

            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> ();

            await rootNavigation.PushPage (page, pageModel, modal);
        }

        public async Task PopPageModel (bool modal = false)
        {
            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> ();
            await rootNavigation.PopPage (modal);
        }

        public async Task PopPageModel (object data, bool modal = false)
        {
            if (_pageModel != null && _pageModel.PreviousPageModel != null && data != null) {
                _pageModel.PreviousPageModel.ReverseInit (data);
            }
            await PopPageModel (modal);
        }

        public Task PushPageModel<T> () where T : FreshBasePageModel
        {
            return PushPageModel<T> (null);
        }
    }
}

