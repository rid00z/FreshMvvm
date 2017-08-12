using System;
using FreshMvvm;
using FreshMvvm.Base;
using FreshMvvm.Extensions;
using FreshMvvm.NavigationContainers;
using FreshMvvmSampleApp.Models;
using FreshMvvmSampleApp.Services;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class ContactPageModel : FreshPageModel
    {
        readonly IDatabaseService _dataService;

        public ContactPageModel (IDatabaseService dataService)
        {
            _dataService = dataService;

            this.WhenAny(HandleContactChanged, o => o.Contact);
        }

        void HandleContactChanged(string propertyName)
        {
            //handle the property changed, nice
        }

        public Contact Contact { get; set; }

        public override void PushedData (object initData)
        {
            if (initData != null) {
                Contact = (Contact)initData;
            } else {
                Contact = new Contact ();
            }
        }

        public Command SaveCommand {
            get { 
                return new Command (() => {
                    _dataService.UpdateContact (Contact);
                    Navigation.PopPageModel (Contact);
                }
                );
            }
        }

        public Command TestModal {
            get {
                return new Command (async () => {
                    await Navigation.PushPageModel<ModalPageModel> (null, true);
                });
            }
        }

        public Command TestModalNavigationBasic {
            get {
                return new Command (async () => {

                    var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel> ();
                    var basicNavContainer = new FreshNavigationContainer (page, Guid.NewGuid ().ToString ());
                    await Navigation.PushNewNavigationServiceModal(basicNavContainer, new[] { page.GetModel() }); 
                });
            }
        }


        public Command TestModalNavigationTabbed {
            get {
                return new Command (async () => {

                    var tabbedNavigation = new FreshTabbedNavigationContainer (Guid.NewGuid ().ToString ());
                    tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
                    tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
                    await Navigation.PushNewNavigationServiceModal(tabbedNavigation);
                });
            }
        }

        public Command TestModalNavigationMasterDetail {
            get {
                return new Command (async () => {

                    var masterDetailNav = new FreshMasterDetailNavigationContainer (Guid.NewGuid ().ToString ());
                    masterDetailNav.Init ("Menu", "Menu.png");
                    masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
                    masterDetailNav.AddPage<QuoteListPageModel> ("Quotes", null);
                    await Navigation.PushNewNavigationServiceModal(masterDetailNav); 

                });
            }
        }
    }
}
