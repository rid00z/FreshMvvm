using FreshMvvm;
using FreshMvvm.Base;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class ModalPageModel : FreshPageModel
    {
        public ModalPageModel ()
        {
        }

        public Command CloseCommand {
            get {
                return new Command (() => {
                    Navigation.PopPageModel (true);
                });
            }
        }
    }
}

