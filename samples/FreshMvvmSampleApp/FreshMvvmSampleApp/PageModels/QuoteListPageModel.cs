using System.Collections.ObjectModel;
using System.Diagnostics;
using FreshMvvm;
using FreshMvvmSampleApp.Models;
using FreshMvvmSampleApp.Services;
using Xamarin.Forms;

namespace FreshMvvmSampleApp.PageModels
{
    public class QuoteListPageModel : FreshBasePageModel
    {
        readonly IDatabaseService _databaseService;

        public QuoteListPageModel (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ObservableCollection<Quote> Quotes { get; set; }

        public override void Init (object initData)
        {
            Quotes = new ObservableCollection<Quote> (_databaseService.GetQuotes ());
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

        public Command AddQuote {
            get {
                return new Command (async () => {
                    await Navigation.PushPageModel<QuotePageModel> ();
                });
            }
        }

        Quote _selectedQuote;

        public Quote SelectedQuote {
            get => _selectedQuote;
            set {
                _selectedQuote = value;
                if (value != null)
                    QuoteSelected.Execute (value);
            }
        }

        public Command<Quote> QuoteSelected {
            get {
                return new Command<Quote> (async (quote) => {
                    await Navigation.PushPageModel<QuotePageModel> (quote);
                });
            }
        }
    }
}

