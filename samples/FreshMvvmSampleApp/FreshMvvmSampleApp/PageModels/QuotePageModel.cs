using FreshMvvm;
using FreshMvvmSampleApp.Models;
using FreshMvvmSampleApp.Services;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class QuotePageModel : FreshBasePageModel
    {
        readonly IDatabaseService _databaseService;

        public Quote Quote { get; set; }

        public QuotePageModel (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public override void Init (object initData)
        {			
            Quote = initData as Quote;
            if (Quote == null)
                Quote = new Quote ();
        }

        public Command SaveCommand {
            get {
                return new Command (async () => {
                    _databaseService.UpdateQuote (Quote);
                    await Navigation.PopPageModel (Quote);
                });
            }
        }

        public Command TestModal {
            get {
                return new Command (async () => {
                    await Navigation.PushPageModel<ModalPageModel> (null, true);
                });
            }
        }
    }
}

