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
            masterDetailNav.Init ("Menu", "Menu.png");
            masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
            masterDetailNav.AddPage<QuoteListPageModel> ("Quotes", null);
            MainPage = masterDetailNav;
        }

        public void LoadTabbedNav ()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer ();
            tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
            tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
            MainPage = tabbedNavigation;
        }

        public void LoadFOTabbedNav ()
        {
            var tabbedNavigation = new FreshTabbedFONavigationContainer ("CRM");
            tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
            tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
            MainPage = tabbedNavigation;
        }

        public void LoadCustomNav ()
        {
            MainPage = new CustomImplementedNav ();
        }

        public void LoadMultipleNavigation()
        {
            var masterDetailsMultiple = new MasterDetailPage (); //generic master detail page

            //we setup the first navigation container with ContactList
            var contactListPage = FreshPageModelResolver.ResolvePageModel<ContactListPageModel> ();
            contactListPage.Title = "Contact List";
            //we setup the first navigation container with name MasterPageArea
            var masterPageArea = new FreshNavigationContainer (contactListPage, "MasterPageArea");
            masterPageArea.Title = "Menu";

            masterDetailsMultiple.Master = masterPageArea; //set the first navigation container to the Master

            //we setup the second navigation container with the QuoteList 
            var quoteListPage = FreshPageModelResolver.ResolvePageModel<QuoteListPageModel> ();
            quoteListPage.Title = "Quote List";
            //we setup the second navigation container with name DetailPageArea
            var detailPageArea = new FreshNavigationContainer (quoteListPage, "DetailPageArea");

            masterDetailsMultiple.Detail = detailPageArea; //set the second navigation container to the Detail

            MainPage = masterDetailsMultiple;
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

