using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.IoC;
using Xamarin.Forms;

namespace FreshMvvm.NavigationContainers
{
    public class FreshMasterDetailNavigationContainer : MasterDetailPage, IFreshNavigationService
    {
        private readonly List<Page> _pagesInner = new List<Page>();
        private ContentPage _menuPage;

        public Dictionary<string, Page> Pages { get; } = new Dictionary<string, Page>();

        protected ObservableCollection<string> PageNames { get; } = new ObservableCollection<string>();

        public FreshMasterDetailNavigationContainer() : this(Constants.DefaultNavigationServiceName)
        {
        }

        public FreshMasterDetailNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        public void Init(string menuTitle, string menuIcon = null)
        {
            CreateMenuPage(menuTitle, menuIcon);
            RegisterNavigation();
        }

        protected virtual void RegisterNavigation()
        {
            FreshIoC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);

            var navigationContainer = CreateContainerPage(page);
            Pages.Add(title, navigationContainer);
            PageNames.Add(title);

            if (Pages.Count == 1)
                Detail = navigationContainer;
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

        protected virtual void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            _menuPage = new ContentPage
            {
                Title = menuPageTitle
            };

            var listView = new ListView
            {
                ItemsSource = PageNames
            };
            
            listView.ItemSelected += (sender, args) =>
            {
                if (Pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = Pages[(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            _menuPage.Content = listView;

            var navPage = new NavigationPage(_menuPage)
            {
                Title = "Menu"
            };

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            return modal ? 
                Navigation.PushModalAsync(CreateContainerPageSafe(page)) : 
                (Detail as NavigationPage)?.PushAsync(page, animate);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? 
                Navigation.PopModalAsync(animate) : 
                (Detail as NavigationPage)?.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage)?.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            var master = Master as NavigationPage;
            master?.NotifyAllChildrenPopped();
            
            foreach (var page in Pages.Values)
            {
                var navigationPage = page as NavigationPage;
                navigationPage?.NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = _pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            Detail = Pages.Values.ElementAt(tabIndex);

            return Task.FromResult((Detail as NavigationPage)?.CurrentPage.GetModel());
        }
    }
}

