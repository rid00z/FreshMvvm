using Xamarin.Forms;
using FreshMvvm;
using System.Windows.Input;

namespace FreshMvvmSampleApp
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        public ICommand ShowQuotes { get; private set; }
        public ICommand ShowContacts { get; private set; }

        public override void Init(object initData)
        {
            var sharedLock = new SharedLock();
            ShowQuotes = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<QuoteListPageModel>(), sharedLock);
            ShowContacts = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<ContactListPageModel>(), sharedLock);
        }
    }
}

