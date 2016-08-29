using Xamarin.Forms;
using PropertyChanged;
using FreshMvvm;
using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace FreshMvvmSampleApp
{
    [ImplementPropertyChanged]
    public class ContactPageModel : FreshBasePageModel
    {
        IDatabaseService _dataService;

        public ICommand SaveCommand { get; private set; }
        public ICommand TestModal { get; private set; }
        public ICommand TestModalNavigationBasic { get; private set; }
        public ICommand TestModalNavigationTabbed { get; private set; }
        public ICommand TestModalNavigationMasterDetail { get; private set; }

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

            var sharedLock = new SharedLock();
            SaveCommand = CoreMethods.CreateCommand(SaveCommandLogic, sharedLock);
            TestModal = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<ModalPageModel>(null, true), sharedLock);
            TestModalNavigationBasic = CoreMethods.CreateCommand(TestModalNavigationBasicLogic, sharedLock);
            TestModalNavigationTabbed = CoreMethods.CreateCommand(TestModalNavigationTabbedLogic, sharedLock);
            TestModalNavigationMasterDetail = CoreMethods.CreateCommand(TestModalNavigationMasterDetailLogic, sharedLock);
        }

        private async Task SaveCommandLogic()
        {
            _dataService.UpdateContact(Contact);
            await CoreMethods.PopPageModel(Contact);
        }

        private async Task TestModalNavigationBasicLogic()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page, Guid.NewGuid().ToString());
            await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });
        }

        public async Task TestModalNavigationTabbedLogic() {
            var tabbedNavigation = new FreshTabbedNavigationContainer (Guid.NewGuid ().ToString ());
            tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
            tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
            await CoreMethods.PushNewNavigationServiceModal(tabbedNavigation);
        }

        public async Task TestModalNavigationMasterDetailLogic()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer(Guid.NewGuid().ToString());
            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<ContactListPageModel>("Contacts", null);
            masterDetailNav.AddPage<QuoteListPageModel>("Quotes", null);
            await CoreMethods.PushNewNavigationServiceModal(masterDetailNav);
        }
    }
}
