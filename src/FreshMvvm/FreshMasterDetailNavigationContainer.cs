using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public class FreshMasterDetailNavigationContainer : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        private Dictionary<string, Page> _pages = new Dictionary<string, Page>();
        private ContentPage _menuPage;
        private ObservableCollection<string> _pageNames = new ObservableCollection<string>();

        protected Dictionary<string, Page> Pages { get { return _pages; } }

        protected ObservableCollection<string> PageNames { get { return _pageNames; } }

        public FreshMasterDetailNavigationContainer()
        {
        }

        public void Init(string menuTitle)
        {
            CreateMenuPage(menuTitle);
            RegisterNavigation();
        }

        protected virtual void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this);
        }

        public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            var navigationContainer = CreateContainerPage(page);
            _pages.Add(title, navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        protected virtual void CreateMenuPage(string menuPageTitle)
        {
            _menuPage = new ContentPage();
            _menuPage.Title = menuPageTitle;
            var listView = new ListView();

            listView.ItemsSource = _pageNames;

            listView.ItemSelected += (sender, args) =>
            {
                if (_pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = _pages[(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            _menuPage.Content = listView;

            Master = new NavigationPage(_menuPage) { Title = "Menu" };
        }

        public async Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                await Navigation.PushModalAsync(new NavigationPage(page));
            else
                await (Detail as NavigationPage).PushAsync(page, animate); //TODO: make this better
        }

        public async Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                await Navigation.PopModalAsync(animate);
            else
                await (Detail as NavigationPage).PopAsync(animate); //TODO: make this better
        }

        public async Task PopToRoot(bool animate = true)
        {
            await (Detail as NavigationPage).PopToRootAsync(animate);
        }
    }
}