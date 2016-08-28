using System.Collections.ObjectModel;
using Xamarin.Forms;
using FreshMvvm;
using PropertyChanged;
using System.Diagnostics;
using System.Windows.Input;

namespace FreshMvvmSampleApp
{
    [ImplementPropertyChanged]
    public class QuoteListPageModel : FreshBasePageModel
    {
        IDatabaseService _databaseService;

        public QuoteListPageModel (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ObservableCollection<Quote> Quotes { get; set; }

        public override void Init (object initData)
        {
            Quotes = new ObservableCollection<Quote> (_databaseService.GetQuotes ());
            AddQuote = CoreMethods.CreateCommand(() => CoreMethods.PushPageModel<QuotePageModel>());
        }

        protected override void ViewIsAppearing (object sender, System.EventArgs e)
        {
            Debug.WriteLine ("View is appearing");
            base.ViewIsAppearing (sender, e);
        }

        protected override void ViewIsDisappearing (object sender, System.EventArgs e)
        {
            base.ViewIsDisappearing (sender, e);
        }

        public override void ReverseInit (object value)
        {
            var newContact = value as Quote;
            if (!Quotes.Contains (newContact)) {
                Quotes.Add (newContact);
            }
        }

        public ICommand AddQuote { get; private set; }

        Quote _selectedQuote;

        public Quote SelectedQuote {
            get {
                return _selectedQuote;
            }
            set {
                _selectedQuote = value;
                if (value != null)
                    QuoteSelected.Execute (value);
            }
        }

        public Command<Quote> QuoteSelected {
            get {
                return new Command<Quote> (async (quote) => {
                    await CoreMethods.PushPageModel<QuotePageModel> (quote);
                });
            }
        }
    }
}

