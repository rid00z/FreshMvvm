using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FreshMvvm
{
    public class FreshTabbedNavigationContainer : TabbedPage, IFreshNavigationService
    {
        List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages { get { return _tabs; } }

        public FreshTabbedNavigationContainer () : this(Constants.DefaultNavigationServiceName)
        {				
            
        }

        public FreshTabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation ();
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
            var navigationContainer = CreateContainerPageSafe (page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.Icon = icon;
            Children.Add (navigationContainer);
            return navigationContainer;
        }

        internal Page CreateContainerPageSafe (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return new NavigationPage (page);
        }

		public System.Threading.Tasks.Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PushModalAsync (CreateContainerPageSafe (page));
            return this.CurrentPage.Navigation.PushAsync (page);
        }

		public System.Threading.Tasks.Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PopModalAsync (animate);
            return this.CurrentPage.Navigation.PopAsync (animate);
        }

        public Task PopToRoot (bool animate = true)
        {
            return this.CurrentPage.Navigation.PopToRootAsync (animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in this.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }
            
        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            if (page > -1)
            {
                CurrentPage = this.Children[page];
                var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
                if (topOfStack != null)
                    return Task.FromResult(topOfStack.GetModel());

            }
            return null;
        }
    }
}

