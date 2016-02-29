using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FreshMvvm
{
    public class FreshMasterDetailNavigationContainer : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        Dictionary<string, Page> _pages = new Dictionary<string, Page> ();
        ContentPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string> ();

        public Dictionary<string, Page> Pages { get { return _pages; } }
        protected ObservableCollection<string> PageNames { get { return _pageNames; } }

        public FreshMasterDetailNavigationContainer () : this(Constants.DefaultNavigationServiceName)
        {			
        }

        public FreshMasterDetailNavigationContainer (string navigationServiceName)
        {                       
            NavigationServiceName = navigationServiceName;   
            RegisterNavigation ();
        }

        public void Init (string menuTitle, string menuIcon = null)
        {
            CreateMenuPage (menuTitle, menuIcon);
            RegisterNavigation ();
        }

        protected virtual void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
        }

        public virtual void AddPage<T> (string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationServiceName = NavigationServiceName;
            var navigationContainer = CreateContainerPage (page);
            _pages.Add (title, navigationContainer);
            _pageNames.Add (title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return new NavigationPage (page);
        }

        protected virtual void CreateMenuPage (string menuPageTitle, string menuIcon = null)
        {
            _menuPage = new ContentPage ();
            _menuPage.Title = menuPageTitle;
            var listView = new ListView ();

            listView.ItemsSource = _pageNames;

            listView.ItemSelected += (sender, args) => {
                if (_pages.ContainsKey ((string)args.SelectedItem)) {
                    Detail = _pages [(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            _menuPage.Content = listView;

            var navPage = new NavigationPage (_menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty (menuIcon))
                navPage.Icon = menuIcon;
            
            Master = navPage;
        }

        public Task PushPage (Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync (CreateContainerPage(page));
            return (Detail as NavigationPage).PushAsync (page, animate); //TODO: make this better
		}

		public Task PopPage (bool modal = false, bool animate = true)
		{
            if (modal)
                return Navigation.PopModalAsync (animate);
            return (Detail as NavigationPage).PopAsync (animate); //TODO: make this better            
		}

        public Task PopToRoot (bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync (animate);
        }

        public string NavigationServiceName { get; private set; }
    }
}

