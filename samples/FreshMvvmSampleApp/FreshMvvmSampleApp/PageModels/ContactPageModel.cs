using Xamarin.Forms;
using PropertyChanged;
using FreshMvvm;
using System;

namespace FreshMvvmSampleApp
{
    [ImplementPropertyChanged]
    public class ContactPageModel : FreshBasePageModel
    {
        IDatabaseService _dataService;

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

        public override void Init (object initData)
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
                    CoreMethods.PopPageModel (Contact);
                }
                );
            }
        }

        public Command TestModal {
            get {
                return new Command (async () => {
                    await CoreMethods.PushPageModel<ModalPageModel> (null, true);
                });
            }
        }

        public Command TestModalNavigationBasic {
            get {
                return new Command (async () => {

                    var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel> ();
                    var basicNavContainer = new FreshNavigationContainer (page, Guid.NewGuid ().ToString ());
                    await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() }); 
                });
            }
        }


        public Command TestModalNavigationTabbed {
            get {
                return new Command (async () => {

                    var tabbedNavigation = new FreshTabbedNavigationContainer (Guid.NewGuid ().ToString ());
                    tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
                    tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
                    await CoreMethods.PushNewNavigationServiceModal(tabbedNavigation);
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
                    await CoreMethods.PushNewNavigationServiceModal(masterDetailNav); 

                });
            }
        }
    }
}
