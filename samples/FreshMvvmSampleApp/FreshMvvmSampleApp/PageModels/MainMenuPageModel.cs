using FreshMvvm;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        public MainMenuPageModel ()
        {
        }

        public Command ShowQuotes {
            get {
                return new Command (async () => {
                    await Navigation.PushPageModel<QuoteListPageModel> ();
                });
            }
        }

        public Command ShowContacts {
            get {
                return new Command (async () => {
                    await Navigation.PushPageModel<ContactListPageModel> ();
                });
            }
        }
    }
}

