# FreshMvvm for Xamarin.Forms

FreshMvvm is a super light Mvvm Framework designed specifically for Xamarin.Forms. It's designed to be Easy, Simple and Flexible. 

### How does it compare to other options?

* It's super light and super simple
* It's specifically designed for Xamarin.Forms
* Designed to be easy to learn and develop (great when your not ready for RxUI)
* Uses a Convention over Configuration 

### Quick Start Guide

TODO: Quick Start Guide

### The Story

I [(Michael Ridland)](http://www.michaelridland.com/) was part-way into a Xamarin Traditional application when Xamarin.Forms was released. I wanted to move the project onto Xamarin.Forms but on that project I was using MvvmCross. At that time MvvmCross had no support for Xamarin.Forms, so I had the option of 1) adapting MvvmCross, 2) finding an alternative or 3) rolling my own Mvvm. The best part about MvvmCross was it's two-way databinding to the native iOS/Android controls but since Xamarin.Forms already had the Databinding builtin, that wasn't useful and the size with MvvmCross was an overhead when I didn't require it. I also wasn't able to find an alternative that I could easily move to. So that I could keep it simple and flexible, I ended up rolling my own Mvvm.

It's grown up from this post on [rolling your own Mvvm for Xamarin.Forms](http://www.michaelridland.com/xamarin/rolling-mvvm-xamarin-forms/). I try hard to keep the simplicity of rolling your own Mvvm for Xamarin.Forms. 

It was never a plan to create a framework but after presenting my Mvvm solution at a few events, I found many people wanted it and seemed to be really interested in it. Also considering I've been using this Framework in all my projects from the start of Xamarin.Forms I know that it works, so I created FreshMvvm and that's how it was born. 

### Conventions

This Framework, while simple, is also powerful and uses a Convention over Configuration style. 

*Note* Different to standard naming conventions, FreshMvvm uses Page and PageModel instead of View and ViewModel, this is inline with Xamarin.Forms using Pages
* A Page must have a corresponding PageModel, with naming important so a QuotePageModel must have a QuotePage
The BindingContext on the page will be automatically set with the Model
* A PageModel can have a Init method that takes a object
* A PageModel can have a ReverseInit method that also take a object and is called when a model is poped with a object
* PageModel can have dependancies automatically injected into the Constructor

### Navigation

The Primary form of Navigation in FreshMvvm is PageModel to PageModel, this essentially means our views have no idea of Navigation. 

So to Navigate between PageModels use: 

    await CoreMethods.PushPageModel<QuotePageModel>(); // Pushes navigation stack
    await CoreMethods.PushPageModel<QuotePageModel>(null, true); // Pushes a Modal

The engine for Navigation in FreshMvvm is done via a simple interface, with methods for Push and Pop. Essentially these methods can control the Navigation of the application in any way they like.

	public interface IFreshNavigationService
    {
        Task PushPage(Page page, FreshBasePageModel model, bool modal = false);
        Task PopPage(bool modal = false);
    }  

Within the PushPage and PopPage you can do any type of navigation that you like, this can from a simple navigation to a advanced nested navigation. 

The Framework contains some built in Navigation containers for the different types of Navigation.

###### Basic Navigation - Built In

	var page = FreshBasePageModel.ResolvePageModel<MainMenuPageModel> ();
	var basicNavContainer = new FreshNavigationContainer (page);
	MainPage = basicNavContainer;

###### Master Detail - Built In

	var masterDetailNav = new FreshMasterDetailNavigationContainer ();
	masterDetailNav.Init ("Menu");
	masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
	masterDetailNav.AddPage<QuoteListPageModel> ("Pages", null);
	MainPage = masterDetailNav;

###### Tabbed Navigation - Built In

	var tabbedNavigation = new FreshTabbedNavigationContainer ();
	tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", null);
	tabbedNavigation.AddTab<QuoteListPageModel> ("Pages", null);
	MainPage = tabbedNavigation;

###### Implementing Custom Navigation

It's possible to setup any type of Navigation by implementing IFreshNavigationService.There's a sample of this in Sample Application named CustomImplementedNav.cs. 

### Sample Apps

* Basic Navigation Sample
* Tabbed Navigation Sample
* MasterDetail Navigation Sample
* Tabbed Navigation with MasterDetail Popover Sample (This is called the CustomImplementedNav in the Sample App)

### Inversion of Control (IOC)

So that you don't need to include your own IOC container, FreshMvvm comes with a IOC container built in. It's using TinyIOC underneith, but with different naming to avoid conflicts. 

To Register services in the container use Register: 

	FreshIOC.Container.Register<IDatabaseService, DatabaseService> ();

To obtain a service use Resolve:

	FreshIOC.Container.Resolve<IDatabaseService> ();

*This is also what drives constructor injection. 

### PageModel - Constructor Injection

When PageModels are pushed services that are in the IOC container can be pushed into the Constructor. 

FreshIOC.Container.Register<IDatabaseService, DatabaseService> ();

### PageModel Important Methods

	/// <summary>
	/// The previous page model, that's automatically filled, on push
	/// </summary>
	public FreshBasePageModel PreviousPageModel { get; set; }

	/// <summary>
	/// A reference to the current page, that's automatically filled, on push
	/// </summary>
    public Page CurrentPage { get; set; }

    /// <summary>
    /// Core methods are basic built in methods for the App including Pushing, Pop and Alert
    /// </summary>
	public IPageModelCoreMethods CoreMethods { get; set; }    

	/// <summary>
	/// This method is called when a page is Pop'd, it also allows for data to be returned.
	/// </summary>
	/// <param name="returndData">This data that's returned from </param>
	public virtual void ReverseInit(object returndData) { }

	/// <summary>
	/// This method is called when the PageModel is loaded, the initData is the data that's sent from pagemodel before
	/// </summary>
	/// <param name="initData">Data that's sent to this PageModel from the pusher</param>
	public virtual void Init(object initData) { }

	/// <summary>
	/// This method is called when the view is disappearing. 
	/// </summary>
	protected virtual void ViewIsDisappearing (object sender, EventArgs e)
    {
    }

	/// <summary>
	/// This methods is called when the View is appearing
	/// </summary>
    protected virtual void ViewIsAppearing (object sender, EventArgs e)
    {
    }

### The CoreMethods

Each PageModel has a property called 'CoreMethods' which is automatically filled when a PageModel is pushed, it's the basic functions that most apps need like Alerts, Pushing, Poping etc. 

	public interface IPageModelCoreMethods
	{
		Task DisplayAlert (string title, string message, string cancel);
		Task<string> DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons);
		Task<bool> DisplayAlert (string title, string message, string accept, string cancel);
		Task PushPageModel<T>(object data, bool modal = false) where T : FreshBasePageModel;
		Task PopPageModel(bool modal = false);
		Task PopPageModel(object data, bool modal = false);
		Task PushPageModel<T>() where T : FreshBasePageModel;
	}

### Page important methods

PageModel 
Init
PropertyChanged 

#### Sample PageModel

	[ImplementPropertyChanged] // Use Fody for Property Changed Notifications
    public class QuoteListPageModel : FreshBasePageModel
    {
        IDatabaseService _databaseService;

        //These are automatically filled via Constructor Injection IOC
        public QuoteListPageModel (IDatabaseService databaseService) 
        {
            _databaseService = databaseService;
        }

        public ObservableCollection<Quote> Quotes { get; set; }

        public override void Init (object initData)
        {
            Quotes = new ObservableCollection<Quote> (_databaseService.GetQuotes ());
        }

        //The Framework support standard functions list appeaing and disappearing
        protected override void ViewIsAppearing (object sender, System.EventArgs e)
        {
            CoreMethods.DisplayAlert ("Page is appearing", "", "Ok");
            base.ViewIsAppearing (sender, e);
        }

        protected override void ViewIsDisappearing (object sender, System.EventArgs e)
        {
            base.ViewIsDisappearing (sender, e);
        }

        //This is called when a pushed Page returns to this Page
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
                    //Push A Page Model
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

        public Command<Quote> QuoteSelected {
            get {
                return new Command<Quote> (async (quote) => {
                    await CoreMethods.PushPageModel<QuotePageModel> (quote);
                });
            }
        }
    }

## Setup Guide

Please watch this video/read this post to get started. 




