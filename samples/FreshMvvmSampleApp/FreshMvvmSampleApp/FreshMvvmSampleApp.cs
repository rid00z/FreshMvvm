using System;
using Xamarin.Forms;
using System.Collections.Generic;
using FreshMvvm;

namespace FreshMvvmSampleApp
{
    public class App : Application
    {
        public App ()
        {
            FreshIOC.Container.Register<IDatabaseService, DatabaseService> ();

            MainPage = new NavigationPage (new LaunchPage (this));
        }

        public void LoadBasicNav ()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel> ();
            var basicNavContainer = new FreshNavigationContainer (page);
            MainPage = basicNavContainer;
        }

        public void LoadMasterDetail ()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer ();
            masterDetailNav.Init ("Menu");
            masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
            masterDetailNav.AddPage<QuoteListPageModel> ("Quotes", null);
            MainPage = masterDetailNav;
        }

        public void LoadTabbedNav ()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer ();
            tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", null);
            tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", null);
            MainPage = tabbedNavigation;
        }

        public void LoadCustomNav ()
        {
            MainPage = new CustomImplementedNav ();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}

