﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FreshMvvm
{
    public class FreshMasterDetailNavigationContainer : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        List<Page> _pagesInner = new List<Page> ();
        Dictionary<string, Page> _pages = new Dictionary<string, Page> ();
        ContentPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string> ();
	ListView _listView = new ListView ();

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

        public virtual async Task AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = await FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage (page);
            _pages.Add (title, navigationContainer);
            _pageNames.Add (title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }
        public virtual async Task AddPage(string modelName, string title, object data = null)
        {
            var pageModelType = Type.GetType(modelName);
            var page = await FreshPageModelResolver.ResolvePageModel(pageModelType, null);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);
            _pages.Add(title, navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
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

        protected virtual void CreateMenuPage (string menuPageTitle, string menuIcon = null)
        {
            _menuPage = new ContentPage ();
            _menuPage.Title = menuPageTitle; 
	    
            _listView.ItemsSource = _pageNames;

            _listView.ItemSelected += (sender, args) => {
                if (_pages.ContainsKey ((string)args.SelectedItem)) {
                    Detail = _pages [(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            _menuPage.Content = _listView;

            var navPage = new NavigationPage (_menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty (menuIcon))
                navPage.Icon = menuIcon;
            
            Master = navPage;
        }

        public Task PushPage (Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync (CreateContainerPageSafe(page));
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
            var tabIndex = _pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            _listView.SelectedItem = _pageNames[tabIndex];

            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    }
}

