﻿using FreshMvvm;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvmSampleApp
{
    /// <summary>
    /// This is a sample custom implemented Navigation. It combines a MasterDetail and a TabbedPage.
    /// </summary>
    public class CustomImplementedNav : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        private FreshTabbedNavigationContainer _tabbedNavigationPage;
        private Page _contactsPage, _quotesPage;

        public CustomImplementedNav()
        {
            SetupTabbedPage();
            CreateMenuPage("Menu");
            RegisterNavigation();
        }

        private void SetupTabbedPage()
        {
            _tabbedNavigationPage = new FreshTabbedNavigationContainer();
            _contactsPage = _tabbedNavigationPage.AddTab<ContactListPageModel>("Contacts", "contacts.png");
            _quotesPage = _tabbedNavigationPage.AddTab<QuoteListPageModel>("Quotes", "document.png");
            this.Detail = _tabbedNavigationPage;
        }

        protected void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this);
        }

        protected void CreateMenuPage(string menuPageTitle)
        {
            var _menuPage = new ContentPage();
            _menuPage.Title = menuPageTitle;
            var listView = new ListView();

            listView.ItemsSource = new string[] { "Contacts", "Quotes", "Modal Demo" };

            listView.ItemSelected += async (sender, args) =>
            {
                switch ((string)args.SelectedItem)
                {
                    case "Contacts":
                        _tabbedNavigationPage.CurrentPage = _contactsPage;
                        break;

                    case "Quotes":
                        _tabbedNavigationPage.CurrentPage = _quotesPage;
                        break;

                    case "Modal Demo":
                        var modalPage = FreshPageModelResolver.ResolvePageModel<ModalPageModel>();
                        await PushPage(modalPage, null, true);
                        break;

                    default:
                        break;
                }

                IsPresented = false;
            };

            _menuPage.Content = listView;

            Master = new NavigationPage(_menuPage) { Title = "Menu" };
        }

        public async Task PopToRoot(bool animate = true)
        {
            await (Detail as NavigationPage).PopToRootAsync(animate);
        }

        public virtual async Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animated = true)
        {
            if (modal)
                await Navigation.PushModalAsync(new NavigationPage(page), animated);
            else
                await ((NavigationPage)_tabbedNavigationPage.CurrentPage).PushAsync(page, animated);
        }

        public virtual async Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                await Navigation.PopModalAsync();
            else
                await ((NavigationPage)_tabbedNavigationPage.CurrentPage).PopAsync();
        }
    }
}