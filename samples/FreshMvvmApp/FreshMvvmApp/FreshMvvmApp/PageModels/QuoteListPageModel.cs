using System.Collections.ObjectModel;
using Xamarin.Forms;
using FreshMvvm;
using PropertyChanged;
using System.Diagnostics;
using System;

namespace FreshMvvmApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
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
                    await CoreMethods.PushPageModel<QuotePageModel> ();
                });
            }
        }

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

        public Command<Quote> QuoteSelected => new Command<Quote>(async (quote) =>
        {
            await CoreMethods.PushPageModel<QuotePageModel>(vm => vm.Quote = quote);
        });
    }
}

