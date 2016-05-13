using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FreshMvvm
{
    /// <summary>
    /// This Tabbed navigation container for when you only want the tabs to appear on the first page and then push to a second page without tabs
    /// </summary>
    public class FreshTabbedFONavigationContainer : NavigationPage, IFreshNavigationService
    {
        TabbedPage _innerTabbedPage;
        List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages { get { return _tabs; } }

        public FreshTabbedFONavigationContainer () : this(Constants.DefaultNavigationServiceName)
        {               
        }

        public FreshTabbedFONavigationContainer(string navigationServiceName) : base(new TabbedPage())
        {           
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
            _innerTabbedPage = (TabbedPage)this.CurrentPage;
        }

        protected void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
        }

        public virtual Page AddTab<T> (string title, string icon, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add (page);
            var container = CreateContainerPage (page);
            container.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                container.Icon = icon;
            _innerTabbedPage.Children.Add (container);
            return container;
        }

        internal Page CreateContainerPageSafe (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return page;
        }

        public System.Threading.Tasks.Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return this.Navigation.PushModalAsync (CreateContainerPageSafe (page));
            return this.Navigation.PushAsync (page);
        }

        public System.Threading.Tasks.Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
                return this.Navigation.PopModalAsync (animate);
            return this.Navigation.PopAsync (animate);
        }

        public Task PopToRoot (bool animate = true)
        {
            return this.Navigation.PopToRootAsync (animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in _innerTabbedPage.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }
    }
}

