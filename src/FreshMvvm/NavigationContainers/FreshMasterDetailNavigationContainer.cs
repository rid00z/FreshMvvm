using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public class FreshMasterDetailNavigationContainer : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        private ListView _listView = new ListView();

        protected List<Page> PagesInner = new List<Page>();
        protected ObservableCollection<string> PageNames = new ObservableCollection<string>();

        public Dictionary<string, Page> Pages = new Dictionary<string, Page>();

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
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            PagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);
            Pages.Add(title, navigationContainer);
            PageNames.Add(title);
            if (Pages.Count == 1)
                Detail = navigationContainer;
        }

        public virtual void AddPage(Type pageModel, string title, object data = null)
        {
            var page = FreshPageModelResolver.ResolvePageModel(pageModel, data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            PagesInner.Add(page);
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
            var _menuPage = new ContentPage();
            _menuPage.Title = menuPageTitle;

            _listView.ItemsSource = PageNames;

            _listView.ItemSelected += (sender, args) =>
            {
                if (Pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = Pages[(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            _menuPage.Content = _listView;

            var navPage = new NavigationPage(_menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return (Detail as NavigationPage).PushAsync(page, animate); //TODO: make this better
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PopModalAsync(animate);
            return (Detail as NavigationPage).PopAsync(animate); //TODO: make this better
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            if (Master is NavigationPage)
                ((NavigationPage)Master).NotifyAllChildrenPopped();
            if (Master is IFreshNavigationService)
                ((IFreshNavigationService)Master).NotifyChildrenPageWasPopped();

            foreach (var page in this.Pages.Values)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
                if (page is IFreshNavigationService)
                    ((IFreshNavigationService)page).NotifyChildrenPageWasPopped();
            }
            if (this.Pages != null && !this.Pages.ContainsValue(Detail) && Detail is NavigationPage)
                ((NavigationPage)Detail).NotifyAllChildrenPopped();
            if (this.Pages != null && !this.Pages.ContainsValue(Detail) && Detail is IFreshNavigationService)
                ((IFreshNavigationService)Detail).NotifyChildrenPageWasPopped();
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = PagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            _listView.SelectedItem = PageNames[tabIndex];

            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    }
}