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

            await PushPageModel(pageModel, data, modal);
        }

        public async Task PushPageModel<T, TPage> (object data, bool modal = false) where T : FreshBasePageModel where TPage : Page
        {
            T pageModel = FreshIOC.Container.Resolve<T> ();
            TPage page = FreshIOC.Container.Resolve<TPage> ();
            await PushPageModelWithPage(page, pageModel, data, modal);
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

        async Task PushPageModel(FreshBasePageModel pageModel, object data, bool modal = false)
        {
            var page = FreshPageModelResolver.ResolvePageModel(data, pageModel);
            PushPageModelWithPage(page, pageModel, data, modal);
        }

        async Task PushPageModelWithPage(Page page, FreshBasePageModel pageModel, object data, bool modal = false)
        {
            pageModel.PreviousPageModel = _currentPageModel; //This is the previous page model because it's push to a new one, and this is current
            pageModel.CurrentNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;

            if (string.IsNullOrWhiteSpace(pageModel.PreviousNavigationServiceName))
                pageModel.PreviousNavigationServiceName = _currentPageModel.PreviousNavigationServiceName;

            if (page is FreshMasterDetailNavigationContainer) 
            {
                this.PushNewNavigationServiceModal ((FreshMasterDetailNavigationContainer)page, pageModel);
            } 
            else if (page is FreshTabbedNavigationContainer) 
            {
                this.PushNewNavigationServiceModal ((FreshTabbedNavigationContainer)page, pageModel);
            } 
            else 
            {
                IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (_currentPageModel.CurrentNavigationServiceName);

                await rootNavigation.PushPage (page, pageModel, modal);
            }
        }

        public async Task PopPageModel (bool modal = false)
        {
            string navServiceName = _currentPageModel.CurrentNavigationServiceName;
            if (_currentPageModel.IsModalFirstChild) {
                navServiceName = _currentPageModel.PreviousNavigationServiceName;
            }

            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navServiceName);
            await rootNavigation.PopPage (modal);
        }

        public async Task PopToRoot(bool animate)
        {
            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (_currentPageModel.CurrentNavigationServiceName);
            await rootNavigation.PopToRoot (animate);
        }

        public async Task PopPageModel (object data, bool modal = false)
        {
            if (_currentPageModel != null && _currentPageModel.PreviousPageModel != null && data != null) {
                _currentPageModel.PreviousPageModel.ReverseInit (data);
            }
            await PopPageModel (modal);
        }

        public Task PushPageModel<T> () where T : FreshBasePageModel
        {
            return PushPageModel<T> (null);
        }

        public Task PushPageModel<T, TPage> () where T : FreshBasePageModel where TPage : Page
        {
            return PushPageModel<T, TPage> (null);
        }

        public Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null)
        {
            var models = tabbedNavigationContainer.TabbedPages.Select (o => o.GetModel ()).ToList();
            if (basePageModel != null)
                models.Add (basePageModel);
            return PushNewNavigationServiceModal (tabbedNavigationContainer, models.ToArray ());
        }

        public Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null)
        {
            var models = masterDetailContainer.Pages.Select (o => 
                {
                    if (o.Value is NavigationPage)
                        return ((NavigationPage)o.Value).CurrentPage.GetModel ();
                    else
                        return o.Value.GetModel();
                }).ToList();

            if (basePageModel != null)
                models.Add (basePageModel);
            
            return PushNewNavigationServiceModal (masterDetailContainer, models.ToArray());
        }

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels)
        {
            return PushNewNavigationServiceModal (newNavigationService, new FreshBasePageModel[] { basePageModels });
        }

        public async Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels)
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
            await rootNavigation.PushPage (navPage, null, true);
        }

        public async Task PopModalNavigationService()
        {
            var navServiceName = _currentPageModel.PreviousNavigationServiceName;        
            IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navServiceName);
            await rootNavigation.PopPage (true);
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

