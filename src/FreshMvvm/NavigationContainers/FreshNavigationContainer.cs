using System;
using System.Threading.Tasks;
using FreshMvvm.Base;
using FreshMvvm.Extensions;
using FreshMvvm.IoC;
using Xamarin.Forms;

namespace FreshMvvm.NavigationContainers
{
    public class FreshNavigationContainer : NavigationPage, IFreshNavigationService
    {
        public FreshNavigationContainer(Page page)
            : this(page, Constants.DefaultNavigationServiceName)
        {
        }

        public FreshNavigationContainer(Page page, string navigationPageName)
            : base(page)
        {
            var pageModel = page.GetModel();
            pageModel.CurrentNavigationServiceName = navigationPageName;
            NavigationServiceName = navigationPageName;
            RegisterNavigation();
        }

        protected void RegisterNavigation()
        {
            FreshIoC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        public virtual Task PushPage(Page page, FreshPageModel model, bool modal = false, bool animate = true)
        {
            return modal ?
                Navigation.PushModalAsync(CreateContainerPageSafe(page), animate) :
                Navigation.PushAsync(page, animate);
        }

        public virtual Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? 
                Navigation.PopModalAsync(animate) : 
                Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            this.NotifyAllChildrenPopped();
        }

        public Task<FreshPageModel> SwitchSelectedRootPageModel<T>() where T : FreshPageModel
        {
            throw new Exception("This navigation container has no selected roots, just a single root");
        }
    }
}

