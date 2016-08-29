using Xamarin.Forms;
using PropertyChanged;
using FreshMvvm;
using System.Windows.Input;
using System.Threading.Tasks;

namespace FreshMvvmSampleApp
{
    [ImplementPropertyChanged]
    public class QuotePageModel : FreshBasePageModel
    {
        IDatabaseService _databaseService;

        public ICommand SaveCommand { get; private set; }
        public ICommand TestModal { get; private set; }

        public Quote Quote { get; set; }

        public QuotePageModel (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public override void Init (object initData)
        {
            Quote = (initData as Quote) ?? new Quote();
            
            SaveCommand = CoreMethods.CreateCommand(SaveCommandLogic);
            TestModal = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<ModalPageModel>(null, true));
        }

        private async Task SaveCommandLogic()
        {
            _databaseService.UpdateQuote (Quote);
            await CoreMethods.PopPageModel (Quote);
        }
    }
}

