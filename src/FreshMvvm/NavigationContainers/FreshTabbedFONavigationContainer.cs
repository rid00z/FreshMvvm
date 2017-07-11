using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreshMvvm.Base;
using FreshMvvm.Extensions;
using FreshMvvm.IoC;
using Xamarin.Forms;

namespace FreshMvvm.NavigationContainers
{
    /// <summary>
    /// This Tabbed navigation container for when you only want the tabs to appear on the first page and then push to a second page without tabs
    /// </summary>
    public class FreshTabbedFONavigationContainer : NavigationPage, IFreshNavigationService
    {
        private readonly List<Page> _tabs = new List<Page>();

        public TabbedPage FirstTabbedPage { get; }
        public IEnumerable<Page> TabbedPages => _tabs;

        public FreshTabbedFONavigationContainer (string titleOfFirstTab) : this(titleOfFirstTab, Constants.DefaultNavigationServiceName)
        {               
        }

        public FreshTabbedFONavigationContainer(string titleOfFirstTab, string navigationServiceName) : base(new TabbedPage())
        {           
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
            FirstTabbedPage = (TabbedPage)CurrentPage;
            FirstTabbedPage.Title = titleOfFirstTab;
        }

        protected void RegisterNavigation ()
        {
            FreshIoC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
        }

        public virtual Page AddTab<T> (string title, string icon, object data = null) where T : FreshPageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add (page);

            var container = CreateContainerPageSafe (page);
            container.Title = title;

            if (!string.IsNullOrWhiteSpace(icon))
                container.Icon = icon;
            FirstTabbedPage.Children.Add (container);

            return container;
        }

        internal Page CreateContainerPageSafe (Page page)
        {
            return page is NavigationPage || page is MasterDetailPage || page is TabbedPage
                ? page
                : CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return page;
        }

        public Task PushPage (Page page, FreshPageModel model, bool modal = false, bool animate = true)
        {
            return modal ? 
                Navigation.PushModalAsync (CreateContainerPageSafe (page)) : 
                Navigation.PushAsync (page);
        }

        public Task PopPage (bool modal = false, bool animate = true)
        {
            return modal ? 
                Navigation.PopModalAsync (animate) : 
                Navigation.PopAsync (animate);
        }

        public Task PopToRoot (bool animate = true)
        {
            return Navigation.PopToRootAsync (animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in FirstTabbedPage.Children)
            {
                var navigationPage = page as NavigationPage;
                navigationPage?.NotifyAllChildrenPopped();
            }
        }

        public Task<FreshPageModel> SwitchSelectedRootPageModel<T>() where T : FreshPageModel
        {
            if (CurrentPage != FirstTabbedPage)
                throw new Exception("Cannot switch tabs when the tab screen is not visible");

            var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);
            if (page <= -1) return null;
            FirstTabbedPage.CurrentPage = FirstTabbedPage.Children[page];
            return Task.FromResult(FirstTabbedPage.CurrentPage.GetModel());
        }
    }
}

