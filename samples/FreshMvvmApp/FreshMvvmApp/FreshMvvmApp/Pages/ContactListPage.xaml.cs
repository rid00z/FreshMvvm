using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FreshMvvmApp
{
	public partial class ContactListPage : BasePage
	{
        static int count = 0;

		public ContactListPage ()
		{
			InitializeComponent ();
            count++;
            createdCount.Text = $"Created {count} Times";
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("//bottomtabs/contacts/contacts2/contactpage");
        }
    }
}

