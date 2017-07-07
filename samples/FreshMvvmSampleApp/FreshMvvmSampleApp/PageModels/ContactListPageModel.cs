using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FreshMvvm;
using FreshMvvmSampleApp.Models;
using FreshMvvmSampleApp.Services;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class ContactListPageModel : FreshBasePageModel
    {
        readonly IDatabaseService _databaseService;

        public ContactListPageModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ObservableCollection<Contact> Contacts { get; set; }

        public override void Init(object initData)
        {
            Contacts = new ObservableCollection<Contact>(_databaseService.GetContacts());
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }

        public override void ReverseInit(object value)
        {
            var newContact = value as Contact;
            if (!Contacts.Contains(newContact))
            {
                Contacts.Add(newContact);
            }
        }

        Contact _selectedContact;

        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                if (value != null)
                    ContactSelected.Execute(value);
            }
        }

        public Command AddContact
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushPageModel<ContactPageModel>();
                });
            }
        }

        public Command<Contact> ContactSelected
        {
            get
            {
                return new Command<Contact>(async (contact) =>
                {
                    await Navigation.PushPageModel<ContactPageModel>(contact);
                });
            }
        }

        public ICommand OpenFirst
        {
            get
            {
                return new FreshAwaitCommand(async (contact, tcs) =>
                {
                    await Navigation.PushPageModel<ContactPageModel>(Contacts.First());
                    tcs.SetResult(true);
                });
            }
        }
    }
}

