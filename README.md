# FreshMvvm for Xamarin.Forms

FreshMvvm is a super light Mvvm Framework designed specifically for Xamarin.Forms. It's designed to be Easy, Simple and Flexible. 

### How does it compare to other options?

* It's super light and super simple
* It's specifically designed for Xamarin.Forms
* Designed to be easy to learn and develop (great when you are not ready for RxUI)
* Uses a Convention over Configuration 

### Features

* PageModel to PageModel Navigation
* Automatic wiring of BindingContext
* Automatic wiring of Page events (eg. appearing)
* Basic methods (with values) on PageModel (init, reverseinit)
* Built in IOC Container
* PageModel Constructor Injection 
* Basic methods available in Model, like Alert
* Built in Navigation types for SimpleNavigation, Tabbed and MasterDetail 

> *Note* ~~Different to standard naming conventions, FreshMvvm uses Page and PageModel instead of View and ViewModel, this is inline with Xamarin.Forms using Pages~~ Now we can use both the ViewModel naming conventions. 

### The Story

I [(Michael Ridland)](http://www.michaelridland.com/) was part-way into a Xamarin Traditional application when Xamarin.Forms was released. I wanted to move the project onto Xamarin.Forms but on that project I was using MvvmCross. At that time MvvmCross had no support for Xamarin.Forms, so I had the option of 1) adapting MvvmCross, 2) finding an alternative or 3) rolling my own Mvvm. The best part about MvvmCross was it's two-way databinding to the native iOS/Android controls but since Xamarin.Forms already had the Databinding builtin, that wasn't useful and the size with MvvmCross was an overhead when I didn't require it. I also wasn't able to find an alternative that I could easily move to. So that I could keep it simple and flexible, I ended up rolling my own Mvvm.

It's grown up from this post on [rolling your own Mvvm for Xamarin.Forms](http://www.michaelridland.com/xamarin/rolling-mvvm-xamarin-forms/). I try hard to keep the simplicity of rolling your own Mvvm for Xamarin.Forms. 

It was never a plan to create a framework but after presenting my Mvvm solution at a few events, I found many people wanted it and seemed to be really interested in it. Also considering I've been using this Framework in all my projects from the start of Xamarin.Forms I know that it works, so I created FreshMvvm and that's how it was born. 

### Conventions

This Framework, while simple, is also powerful and uses a Convention over Configuration style. 

> *Note* ~~Different to standard naming conventions, FreshMvvm uses Page and PageModel instead of View and ViewModel, this is inline with Xamarin.Forms using Pages~~ Now we can use both the ViewModel naming conventions.

* A Page must have a corresponding PageModel, with naming important so a QuotePageModel must have a QuotePage
The BindingContext on the page will be automatically set with the Model
* A PageModel can have a Init method that takes a object
* A PageModel can have a ReverseInit method that also take a object and is called when a model is poped with a object
* PageModel can have dependancies automatically injected into the Constructor

### Navigation

The Primary form of Navigation in FreshMvvm is PageModel to PageModel, this essentially means our views have no idea of Navigation. 

So to Navigate between PageModels use: 

```csharp
await CoreMethods.PushPageModel<QuotePageModel>(); // Pushes navigation stack
await CoreMethods.PushPageModel<QuotePageModel>(null, true); // Pushes a Modal
```

The engine for Navigation in FreshMvvm is done via a simple interface, with methods for Push and Pop. Essentially these methods can control the Navigation of the application in any way they like.

```csharp
public interface IFreshNavigationService
{
	Task PushPage(Page page, FreshBasePageModel model, bool modal = false);
	Task PopPage(bool modal = false);
}
```

Within the PushPage and PopPage you can do any type of navigation that you like, this can be anything from a simple navigation to a advanced nested navigation. 

The Framework contains some built in Navigation containers for the different types of Navigation.

###### Basic Navigation - Built In

```csharp
var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel> ();
var basicNavContainer = new FreshNavigationContainer (page);
MainPage = basicNavContainer;
```

###### Master Detail - Built In

```csharp
var masterDetailNav = new FreshMasterDetailNavigationContainer ();
masterDetailNav.Init ("Menu");
masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
masterDetailNav.AddPage<QuoteListPageModel> ("Pages", null);
MainPage = masterDetailNav;
```

###### Tabbed Navigation - Built In

```csharp
var tabbedNavigation = new FreshTabbedNavigationContainer ();
tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", null);
tabbedNavigation.AddTab<QuoteListPageModel> ("Pages", null);
MainPage = tabbedNavigation;
```

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

```csharp
FreshIOC.Container.Register<IDatabaseService, DatabaseService>();
```

To obtain a service use Resolve:

```csharp
FreshIOC.Container.Resolve<IDatabaseService>();
```

*This is also what drives constructor injection. 

### IOC Container Lifetime Registration Options

We now support a fluent API for setting the object lifetime of object inside the IOC Container.
```csharp
// By default we register concrete types as 
// multi-instance, and interfaces as singletons
FreshIOC.Container.Register<MyConcreteType>(); // Multi-instance
FreshIOC.Container.Register<IMyInterface, MyConcreteType>(); // Singleton 

// Fluent API allows us to change that behaviour
FreshIOC.Container.Register<MyConcreteType>().AsSingleton(); // Singleton
FreshIOC.Container.Register<IMyInterface, MyConcreteType>().AsMultiInstance(); // Multi-instance
```
As you can see below the IFreshIOC interface methods return the IRegisterOptions interface.

```csharp
public interface IFreshIOC
{
    object Resolve(Type resolveType);
    IRegisterOptions Register<RegisterType>(RegisterType instance) where RegisterType : class;
    IRegisterOptions Register<RegisterType>(RegisterType instance, string name) where RegisterType : class;
    ResolveType Resolve<ResolveType>() where ResolveType : class;
    ResolveType Resolve<ResolveType>(string name) where ResolveType : class;
    IRegisterOptions Register<RegisterType, RegisterImplementation> ()
        where RegisterType : class
        where RegisterImplementation : class, RegisterType;
}
```

The interface that's returned from the register methods is IRegisterOptions.
```csharp
public interface IRegisterOptions
{
    IRegisterOptions AsSingleton();
    IRegisterOptions AsMultiInstance();
    IRegisterOptions WithWeakReference();
    IRegisterOptions WithStrongReference();
    IRegisterOptions UsingConstructor<RegisterType>(Expression<Func<RegisterType>> constructor);
}
```

### PageModel - Constructor Injection

When PageModels are pushed services that are in the IOC container can be pushed into the Constructor. 

```csharp
FreshIOC.Container.Register<IDatabaseService, DatabaseService>();
```

### PageModel Important Methods

```csharp
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
```

### The CoreMethods

Each PageModel has a property called 'CoreMethods' which is automatically filled when a PageModel is pushed, it's the basic functions that most apps need like Alerts, Pushing, Poping etc. 

```csharp
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
```

### Page important methods

PageModel 
Init
PropertyChanged 

#### Sample PageModel

```csharp
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
```
## Multiple Navigation Services

It’s always been possible to do any type of navigation in FreshMvvm, with custom or advanced scenarios were done by implementing a custom navigation service. Even with this ability people found it a little hard to do advanced navigation scenarios in FreshMvvm. After I reviewed all the support questions that came in for FreshMvvm I found that the basic issue people had was they wanted to be able to use our built in navigation containers multiple times, two primary examples are 1) within a master detail having a navigation stack in a master and another in the detail 2) The ability to push modally with a new navigation container. In order to support both these scenarios I concluded that the FreshMvvm required the ability to have named NavigationServices so that we could support multiple NavigationService’s.

### Using multiple navigation containers

Below we’re running two navigation stacks, in a single MasterDetail.

```csharp
var masterDetailsMultiple = new MasterDetailPage (); //generic master detail page

//we setup the first navigation container with ContactList
var contactListPage = FreshPageModelResolver.ResolvePageModel<ContactListPageModel> ();
contactListPage.Title = "Contact List";
//we setup the first navigation container with name MasterPageArea
var masterPageArea = new FreshNavigationContainer (contactListPage, "MasterPageArea");
masterPageArea.Title = "Menu";

masterDetailsMultiple.Master = masterPageArea; //set the first navigation container to the Master

//we setup the second navigation container with the QuoteList 
var quoteListPage = FreshPageModelResolver.ResolvePageModel<QuoteListPageModel> ();
quoteListPage.Title = "Quote List";
//we setup the second navigation container with name DetailPageArea
var detailPageArea = new FreshNavigationContainer (quoteListPage, "DetailPageArea");

masterDetailsMultiple.Detail = detailPageArea; //set the second navigation container to the Detail

MainPage = masterDetailsMultiple;
```

### PushModally with new navigation stack

```csharp
//push a basic page Modally
var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel> ();
var basicNavContainer = new FreshNavigationContainer (page, "secondNavPage");
await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() }); 

//push a tabbed page Modally
var tabbedNavigation = new FreshTabbedNavigationContainer ("secondNavPage");
tabbedNavigation.AddTab<ContactListPageModel> ("Contacts", "contacts.png", null);
tabbedNavigation.AddTab<QuoteListPageModel> ("Quotes", "document.png", null);
await CoreMethods.PushNewNavigationServiceModal(tabbedNavigation);

//push a master detail page Modally
var masterDetailNav = new FreshMasterDetailNavigationContainer ("secondNavPage");
masterDetailNav.Init ("Menu", "Menu.png");
masterDetailNav.AddPage<ContactListPageModel> ("Contacts", null);
masterDetailNav.AddPage<QuoteListPageModel> ("Quotes", null);
await CoreMethods.PushNewNavigationServiceModal(masterDetailNav);
```

### Switching out NavigationStacks on the Xamarin.Forms MainPage

There's some cases in Xamarin.Forms you might want to run multiple navigation stacks. A good example of this is when you have a navigation stack for the authentication and a stack for the primary area of your application.

To begin with we can setup some names for our navigation containers.
```csharp
public class NavigationContainerNames
{
    public const string AuthenticationContainer = "AuthenticationContainer";
    public const string MainContainer = "MainContainer";
}
```

Then we can create our two navigation containers and assign to the MainPage. 

```csharp
var loginPage = FreshMvvm.FreshPageModelResolver.ResolvePageModel<LoginViewModel>();
var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

var myPitchListViewContainer = new FreshTabbedNavigationContainer(NavigationContainerNames.MainContainer);

MainPage = loginContainer;
```

The Navigation Container will use the name passed as argument to register in this method

```csharp
public FreshTabbedNavigationContainer(string navigationServiceName)
{
    NavigationServiceName = navigationServiceName;
    RegisterNavigation ();
}

protected void RegisterNavigation ()
{
    FreshIOC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
}
```

Once we've set this up we can now switch out our navigation containers.
```csharp
CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
```

That name will be resolved in this method to find the correct Navigation Container
```csharp
public void SwitchOutRootNavigation (string navigationServiceName)
{
    IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navigationServiceName);
}
```

## Custom IOC Containers

The second major request for FreshMvvm 1.0 was to allow custom IOC containers. In the case that your application already has a container that you want to leverage.

Using a custom IOC container is very simple in that you only need to implement a single interface.

```csharp
public interface IFreshIOC
{
    object Resolve(Type resolveType);
    void Register<RegisterType>(RegisterType instance) where RegisterType : class;
    void Register<RegisterType>(RegisterType instance, string name) where RegisterType : class;
    ResolveType Resolve<ResolveType>() where ResolveType : class;
    ResolveType Resolve<ResolveType>(string name) where ResolveType : class;
    void Register<RegisterType, RegisterImplementation> ()
        where RegisterType : class
        where RegisterImplementation : class, RegisterType;
```

And then set the IOC container in the System.

```csharp
FreshIOC.OverrideContainer(myContainer);
```

### Related Videos/Quick Start Guides

[FreshMvvm n=0 – Mvvm in Xamarin.Forms and Why FreshMvvm](http://www.michaelridland.com/xamarin/mvvminxamarinformsfreshmvvm/)

[FreshMvvm n=1 : Your first FreshMvvm Application](http://www.michaelridland.com/xamarin/xamarinforms-mvvm-first-freshmvvm-application/)

[FreshMvvm n=2 – IOC and Constructor Injection](http://www.michaelridland.com/xamarin/freshmvvm-n2-ioc-constructor-injection/)

[FreshMvvm n=3: Navigation in FreshMvvm](http://www.michaelridland.com/xamarin/freshmvvm-n3-navigation-in-freshmvvm/)

[Implementing custom navigation in FreshMvvm for Xamarin.Forms](http://www.michaelridland.com/xamarin/implementing-freshmvvm-mvvm-xamarin-forms/)

[TDD in Xamarin Studio – Live Coding FreshMvvm](http://www.michaelridland.com/xamarin/tdd-in-xamarin-studio-live-coding-freshmvvm/)

