using System;
using Xamarin.Forms;
using PropertyChanged;
using System.Collections.ObjectModel;
using FreshMvvm;
using System.Windows.Input;

namespace FreshMvvmSampleApp
{
    [ImplementPropertyChanged]
    public class ContactListPageModel : FreshBasePageModel
    {
        IDatabaseService _databaseService;

        public ContactListPageModel (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ObservableCollection<Contact> Contacts { get; set; }

        public override void Init (object initData)
        {
            Contacts = new ObservableCollection<Contact> (_databaseService.GetContacts ());
            AddContact = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<ContactPageModel>());
        }

        protected override void ViewIsAppearing (object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }

        public override void ReverseInit (object value)
        {
            var newContact = value as Contact;
            if (!Contacts.Contains (newContact)) {
                Contacts.Add (newContact);
            }
        }

        Contact _selectedContact;

        public Contact SelectedContact {
            get {
                return _selectedContact;
            }
            set {
                _selectedContact = value;
                if (value != null)
                    ContactSelected.Execute (value);
            }
        }

        public ICommand AddContact { get; private set; }

        public Command<Contact> ContactSelected {
            get {
                return new Command<Contact> (async (contact) => {
                    await CoreMethods.PushPageModel<ContactPageModel> (contact);
                });
            }
        }
    }
}

