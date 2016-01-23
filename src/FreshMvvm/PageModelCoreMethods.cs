using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace FreshMvvm
{
    public class PageModelCoreMethods : IPageModelCoreMethods
    {
        Page _currentPage;
        FreshBasePageModel _currentPageModel;

		public PageModelCoreMethods (Page currentPage, FreshBasePageModel pageModel)
        {
            _currentPage = currentPage;
			_currentPageModel = pageModel;
        }

        public Task DisplayAlert (string title, string message, string cancel)
        {
            return _currentPage.DisplayAlert (title, message, cancel);
        }

        public Task<string> DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons)
        {
            return _currentPage.DisplayActionSheet (title, cancel, destruction, buttons);
        }

        public Task<bool> DisplayAlert (string title, string message, string accept, string cancel)
        {
            return _currentPage.DisplayAlert (title, message, accept, cancel);	
        }

        public Task PushPageModel<T> (object data, bool modal = false) where T : FreshBasePageModel
        {
            T pageModel = FreshIOC.Container.Resolve<T> ();

            return PushPageModel(pageModel, data, modal);
        }

        public Task PushPageModel(Type pageModelType)
        {
            return PushPageModel(pageModelType, null);
        }

        public Task PushPageModel(Type pageModelType, object data, bool modal = false)
        {
            var pageModel = FreshIOC.Container.Resolve(pageModelType) as FreshBasePageModel;

            return PushPageModel(pageModel, data, modal);
        }

        Task PushPageModel(FreshBasePageModel pageModel, object data, bool modal = false)
        {
            var page = FreshPageModelResolver.ResolvePageModel(data, pageModel);

            pageModel.PreviousPageModel = _currentPageModel; //This is the previous page model because it's push to a new one, and this is current
            pageModel.CurrentNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;

            if (string.IsNullOrWhiteSpace(pageModel.PreviousNavigationServiceName))
                pageModel.PreviousNavigationServiceName = _currentPageModel.PreviousNavigationServiceName;

            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (_currentPageModel.CurrentNavigationServiceName);

            return rootNavigation.PushPage (page, pageModel, modal);
        }

        public Task PopPageModel (bool modal = false)
        {
            string navServiceName = _currentPageModel.CurrentNavigationServiceName;
            if (_currentPageModel.IsModalFirstChild) {
                navServiceName = _currentPageModel.PreviousNavigationServiceName;
            }

            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navServiceName);
            return rootNavigation.PopPage (modal);
        }

        public Task PopToRoot(bool animate)
        {
            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (_currentPageModel.CurrentNavigationServiceName);
            return rootNavigation.PopToRoot (animate);
        }

        public Task PopPageModel (object data, bool modal = false)
        {
            if (_currentPageModel != null && _currentPageModel.PreviousPageModel != null && data != null) {
                _currentPageModel.PreviousPageModel.ReverseInit (data);
            }
            return PopPageModel (modal);
        }

        public Task PushPageModel<T> () where T : FreshBasePageModel
        {
            return PushPageModel<T> (null);
        }

        public Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer)
        {
            var models = tabbedNavigationContainer.TabbedPages.Select (o => o.GetModel ());
            return PushNewNavigationServiceModal (tabbedNavigationContainer, models.ToArray ());
        }

        public Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer)
        {
            var models = masterDetailContainer.Pages.Select (o => 
                {
                    if (o.Value is NavigationPage)
                        return ((NavigationPage)o.Value).CurrentPage.GetModel ();
                    else
                        return o.Value.GetModel();
                });
            
            return PushNewNavigationServiceModal (masterDetailContainer, models.ToArray());
        }

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels)
        {
            return PushNewNavigationServiceModal (newNavigationService, new FreshBasePageModel[] { basePageModels });
        }

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels)
        {
            var navPage = newNavigationService as Page;
            if (navPage == null)
                throw new Exception ("Navigation service is not Page");

            foreach (var pageModel in basePageModels) {
                pageModel.CurrentNavigationServiceName = newNavigationService.NavigationServiceName;
                pageModel.PreviousNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;
                pageModel.IsModalFirstChild = true;
            }

            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (_currentPageModel.CurrentNavigationServiceName);
            return rootNavigation.PushPage (navPage, null, true);
        }

        public Task PopModalNavigationService()
        {
            var navServiceName = _currentPageModel.PreviousNavigationServiceName;        
            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navServiceName);
            return rootNavigation.PopPage (true);
        }

		public void BatchBegin()
		{
			_currentPage.BatchBegin ();
		}

		public void BatchCommit()
		{
			_currentPage.BatchCommit ();
		}
    }
}

