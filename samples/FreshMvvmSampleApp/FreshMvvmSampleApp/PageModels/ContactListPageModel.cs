using System;
using Xamarin.Forms;
using PropertyChanged;
using System.Collections.ObjectModel;
using FreshMvvm;
using System.Diagnostics.Contracts;
using System.Linq;
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

        public Command AddContact {
            get {
                return new Command (async () => {
                    await CoreMethods.PushPageModel<ContactPageModel> ();
                });
            }
        }

        public Command<Contact> ContactSelected {
            get {
                return new Command<Contact> (async (contact) => {
                    await CoreMethods.PushPageModel<ContactPageModel> (contact);
                });
            }
        }

        public ICommand OpenFirst
        {
            get
            {
                return new FreshAwaitCommand(async (contact, tcs) =>
                {
                    await CoreMethods.PushPageModel<ContactPageModel>(this.Contacts.First());
                    tcs.SetResult(true);
                });
            }
        }
    }
}

