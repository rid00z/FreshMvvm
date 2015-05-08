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

		public FreshMasterDetailNavigationContainer ()
        {			
        }

		public void Init(string menuTitle)
		{
			CreateMenuPage (menuTitle);
			RegisterNavigation ();
		}

		protected virtual void RegisterNavigation()
		{
			FreshIOC.Container.Register<IFreshNavigationService> (this);
		}

		public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
		{
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
			var navigationContainer = CreateContainerPage(page);
			_pages.Add (title, navigationContainer);
			_pageNames.Add (title);
			if (_pages.Count == 1)
				Detail = navigationContainer;
		}

		protected virtual Page CreateContainerPage(Page page)
		{
			return new NavigationPage (page);
		}

		protected virtual void CreateMenuPage(string menuPageTitle)
		{
			_menuPage = new ContentPage ();
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

		public virtual async Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false)
        {
            if (modal)
				await Navigation.PushModalAsync (new NavigationPage(page));
            else
				await (Detail as NavigationPage).PushAsync (page); //TODO: make this better
        }

		public virtual async Task PopPage (bool modal = false)
        {
            if (modal)
                await Navigation.PopModalAsync ();
            else
				await (Detail as NavigationPage).PopAsync (); //TODO: make this better
        }
    }
}

