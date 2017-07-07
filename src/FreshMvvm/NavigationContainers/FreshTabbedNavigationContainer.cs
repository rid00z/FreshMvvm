using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.IoC;
using Xamarin.Forms;

namespace FreshMvvm.NavigationContainers
{
    public class FreshTabbedNavigationContainer : TabbedPage, IFreshNavigationService
    {
        private readonly List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages => _tabs;

        public FreshTabbedNavigationContainer() : this(Constants.DefaultNavigationServiceName)
        {

        }

        public FreshTabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        protected void RegisterNavigation()
        {
            FreshIoC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual Page AddTab<T>(string title, string icon, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add(page);

            var navigationContainer = CreateContainerPageSafe(page);
            navigationContainer.Title = title;

            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.Icon = icon;

            Children.Add(navigationContainer);

            return navigationContainer;
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            return page is NavigationPage || page is MasterDetailPage || page is TabbedPage
                ? page
                : CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            return modal ? 
                CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page)) : 
                CurrentPage.Navigation.PushAsync(page);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? 
                CurrentPage.Navigation.PopModalAsync(animate) : 
                CurrentPage.Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return CurrentPage.Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in Children)
            {
                var navigationPage = page as NavigationPage;
                navigationPage?.NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            if (page <= -1) return null;

            CurrentPage = Children[page];

            var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();

            return topOfStack != null ? Task.FromResult(topOfStack.GetModel()) : null;
        }
    }
}

