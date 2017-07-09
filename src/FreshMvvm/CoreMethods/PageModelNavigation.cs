using System;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.Base;
using FreshMvvm.Extensions;
using FreshMvvm.IoC;
using FreshMvvm.NavigationContainers;
using Xamarin.Forms;

namespace FreshMvvm.CoreMethods
{
    public class PageModelNavigation : IPageModelNavigation
    {
        private readonly Page _currentPage;
        private readonly FreshPageModel _currentPageModel;

        public PageModelNavigation(Page currentPage, FreshPageModel pageModel)
        {
            _currentPage = currentPage;
            _currentPageModel = pageModel;
        }
        
        public async Task PushPageModel<T>(object data, bool modal = false, bool animate = true) where T : FreshPageModel
        {
            T pageModel = FreshIoC.Container.Resolve<T>();

            await PushPageModel(pageModel, data, modal, animate);
        }

        public async Task PushPageModel<T, TPage>(object data, bool modal = false, bool animate = true) where T : FreshPageModel where TPage : Page
        {
            T pageModel = FreshIoC.Container.Resolve<T>();
            TPage page = FreshIoC.Container.Resolve<TPage>();
            FreshPageModelResolver.BindingPageModel(data, page, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        public Task PushPageModel(Type pageModelType, bool animate = true)
        {
            return PushPageModel(pageModelType, null, animate);
        }

        public Task PushPageModel(Type pageModelType, object data, bool modal = false, bool animate = true)
        {
            var pageModel = FreshIoC.Container.Resolve(pageModelType) as FreshPageModel;

            return PushPageModel(pageModel, data, modal, animate);
        }

        private async Task PushPageModel(FreshPageModel pageModel, object data, bool modal = false, bool animate = true)
        {
            var page = FreshPageModelResolver.ResolvePageModel(data, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        private async Task PushPageModelWithPage(Page page, FreshPageModel pageModel, object data, bool modal = false, bool animate = true)
        {
            pageModel.PreviousPageModel = _currentPageModel; //This is the previous page model because it's push to a new one, and this is current
            pageModel.CurrentNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;

            if (string.IsNullOrWhiteSpace(pageModel.PreviousNavigationServiceName))
                pageModel.PreviousNavigationServiceName = _currentPageModel.PreviousNavigationServiceName;

            if (page is FreshMasterDetailNavigationContainer)
            {
                await PushNewNavigationServiceModal((FreshMasterDetailNavigationContainer)page, pageModel, animate);
            }
            else if (page is FreshTabbedNavigationContainer)
            {
                await PushNewNavigationServiceModal((FreshTabbedNavigationContainer)page, pageModel, animate);
            }
            else
            {
                IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);

                await rootNavigation.PushPage(page, pageModel, modal, animate);
            }
        }

        public async Task PopPageModel(bool modal = false, bool animate = true)
        {
            string navServiceName = _currentPageModel.CurrentNavigationServiceName;
            if (_currentPageModel.IsModalFirstChild)
            {
                await PopModalNavigationService(animate);
            }
            else
            {
                if (modal)
                    _currentPageModel.RaisePageWasPopped();

                IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(navServiceName);
                await rootNavigation.PopPage(modal, animate);
            }
        }

        public async Task PopToRoot(bool animate)
        {
            IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);
            await rootNavigation.PopToRoot(animate);
        }

        public async Task PopPageModel(object data, bool modal = false, bool animate = true)
        {
            if (_currentPageModel?.PreviousPageModel != null && data != null)
            {
                _currentPageModel.PreviousPageModel.PoppedData(data);
            }
            await PopPageModel(modal, animate);
        }

        public Task PushPageModel<T>(bool animate = true) where T : FreshPageModel
        {
            return PushPageModel<T>(null, false, animate);
        }

        public Task PushPageModel<T, TPage>(bool animate = true) where T : FreshPageModel where TPage : Page
        {
            return PushPageModel<T, TPage>(null, animate);
        }

        public Task PushNewNavigationServiceModal(FreshTabbedNavigationContainer tabbedNavigationContainer, FreshPageModel basePageModel = null, bool animate = true)
        {
            var models = tabbedNavigationContainer.TabbedPages.Select(o => o.GetModel()).ToList();
            if (basePageModel != null)
                models.Add(basePageModel);
            return PushNewNavigationServiceModal(tabbedNavigationContainer, models.ToArray(), animate);
        }

        public Task PushNewNavigationServiceModal(FreshMasterDetailNavigationContainer masterDetailContainer, FreshPageModel basePageModel = null, bool animate = true)
        {
            var models = masterDetailContainer.Pages.Select(o =>
            {
                var page = o.Value as NavigationPage;
                return page != null ? page.CurrentPage.GetModel() : o.Value.GetModel();
            }).ToList();

            if (basePageModel != null)
                models.Add(basePageModel);

            return PushNewNavigationServiceModal(masterDetailContainer, models.ToArray(), animate);
        }

        public Task PushNewNavigationServiceModal(IFreshNavigationService newNavigationService, FreshPageModel basePageModels, bool animate = true)
        {
            return PushNewNavigationServiceModal(newNavigationService, new[] { basePageModels }, animate);
        }

        public async Task PushNewNavigationServiceModal(IFreshNavigationService newNavigationService, FreshPageModel[] basePageModels, bool animate = true)
        {
            var navPage = newNavigationService as Page;
            if (navPage == null)
                throw new Exception("Navigation service is not Page");

            foreach (var pageModel in basePageModels)
            {
                pageModel.CurrentNavigationServiceName = newNavigationService.NavigationServiceName;
                pageModel.PreviousNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;
                pageModel.IsModalFirstChild = true;
            }

            IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);
            await rootNavigation.PushPage(navPage, null, true, animate);
        }

        public void SwitchOutRootNavigation(string navigationServiceName)
        {
            IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(navigationServiceName);

            if (!(rootNavigation is Page))
                throw new Exception("Navigation service is not a page");

            Application.Current.MainPage = rootNavigation as Page;
        }

        public async Task PopModalNavigationService(bool animate = true)
        {
            var currentNavigationService = FreshIoC.Container.Resolve<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);
            currentNavigationService.NotifyChildrenPageWasPopped();

            FreshIoC.Container.Unregister<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);

            var navServiceName = _currentPageModel.PreviousNavigationServiceName;
            IFreshNavigationService rootNavigation = FreshIoC.Container.Resolve<IFreshNavigationService>(navServiceName);
            await rootNavigation.PopPage(animate);
        }

        /// <summary>
        /// This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom pages also
        /// </summary>
        public Task<FreshPageModel> SwitchSelectedRootPageModel<T>() where T : FreshPageModel
        {
            var currentNavigationService = FreshIoC.Container.Resolve<IFreshNavigationService>(_currentPageModel.CurrentNavigationServiceName);

            return currentNavigationService.SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<FreshPageModel> SwitchSelectedTab<T>() where T : FreshPageModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<FreshPageModel> SwitchSelectedMaster<T>() where T : FreshPageModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        public async Task<string> PushPageModelWithNewNavigation<T>(object data, bool animate = true) where T : FreshPageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            var navigationName = Guid.NewGuid().ToString();
            var naviationContainer = new FreshNavigationContainer(page, navigationName);
            await PushNewNavigationServiceModal(naviationContainer, page.GetModel(), animate);
            return navigationName;
        }
        
        /// <summary>
        /// Removes current page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation()
        {
            _currentPageModel.RaisePageWasPopped();
            _currentPage.Navigation.RemovePage(_currentPage);
        }

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation<TPageModel>(bool removeAll = false) where TPageModel : FreshPageModel
        {
            foreach (var page in _currentPage.Navigation.NavigationStack.Reverse().ToList())
            {
                if (!(page.BindingContext is TPageModel)) continue;
                page.GetModel()?.RaisePageWasPopped();
                _currentPage.Navigation.RemovePage(page);
                if (!removeAll)
                    break;
            }
        }
    }
}