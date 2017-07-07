using FreshMvvm;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class ModalPageModel : FreshBasePageModel
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

