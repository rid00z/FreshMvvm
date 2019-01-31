using System;
using FluentAssertions;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;
using System.Linq;
using Xamarin.Forms;
using NSubstitute;


namespace FreshMvvm.Tests
{
    /// <summary>
    /// Need the ability to have two different NavigationServices running at same time
    /// </summary>

    [TestFixture]
    public class MultipleNavigationProviderTests
    {
        public MultipleNavigationProviderTests ()
        {
        }

        /// <summary>
        /// This test ensures the first pagemodels are created with a link to the named navigation service
        /// </summary>
        [Test]
        public void pagemodel_should_be_link_to_when_created_firsttime()
        {
            //master detail navigation
            var masterDetailNavigation = new FreshMasterDetailNavigationContainer("TestingLinking");
            masterDetailNavigation.AddPage<MockContentPageModel> ("Page1");
            masterDetailNavigation.AddPage<MockContentPageModel> ("Page2");
            var pageModel1 = masterDetailNavigation.Pages ["Page1"].GetPageFromNav().GetModel ();
            var pageModel2 = masterDetailNavigation.Pages ["Page2"].GetPageFromNav().GetModel();
            pageModel1.CurrentNavigationServiceName.Should ().Be ("TestingLinking");
            pageModel2.CurrentNavigationServiceName.Should ().Be ("TestingLinking");

            if (FreshIOC.Container.Resolve<IFreshNavigationService> ("TestingLinking") == null)
                throw new Exception ("Should contain navigation service");

            //tabbed navigation 
            var tabbedNavigation = new FreshTabbedNavigationContainer("TestingLinking2");
            tabbedNavigation.AddTab<MockContentPageModel> ("Page1", null);
            tabbedNavigation.AddTab<MockContentPageModel> ("Page2", null);
            var tabbedPageModel1 = tabbedNavigation.TabbedPages.First ().GetModel ();
            var tabbedPageModel2 = tabbedNavigation.TabbedPages.Skip (1).Take (1).First ().GetModel ();
            tabbedPageModel1.CurrentNavigationServiceName.Should ().Be ("TestingLinking2");
            tabbedPageModel2.CurrentNavigationServiceName.Should ().Be ("TestingLinking2");

            if (FreshIOC.Container.Resolve<IFreshNavigationService> ("TestingLinking2") == null)
                throw new Exception ("Should contain navigation service");
            
            //standard navigation should set named navigation
            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            var pageModel = page.BindingContext as MockContentPageModel;
            new FreshNavigationContainer(page, "testingLinking3");
            pageModel.CurrentNavigationServiceName.Should ().Be ("testingLinking3");

            if (FreshIOC.Container.Resolve<IFreshNavigationService> ("testingLinking3") == null)
                throw new Exception ("Should contain navigation service");
            
            //standard navigation should throw exception when binding context isn't a FreshBasePageModel
            var pageEx = new Page();
            Action standardNavExeption = () => new FreshNavigationContainer(pageEx, "testingLinking");
            standardNavExeption.Should().Throw<InvalidCastException> ().WithMessage ("BindingContext was not a FreshBasePageModel on this Page");
        }

        /// <summary>
        /// Each time a new PageModel is pushed, the NavigationServiceName is passed on
        /// </summary>
        [Test]
        public void navigation_servicename_is_passed_on()
        {
            SetupFirstNavigationAndPage ();

            var coreMethods = new PageModelCoreMethods (_page, _pageModel);
            coreMethods.PushPageModel<MockContentPageModel> ();

            _navigationMock.Received ().PushPage (Arg.Any<Page> (), 
                Arg.Is<FreshBasePageModel> (o => o.CurrentNavigationServiceName == _pageModel.CurrentNavigationServiceName), false, true);        
        }

        /// <summary>
        /// The correct IFreshNavigationService should always be resolved name
        /// </summary>
        [Test]
        public void navigationservice_should_be_resolved_via_name()
        {
            SetupFirstNavigationAndPage ();

            var coreMethods = new PageModelCoreMethods (_page, _pageModel);
            coreMethods.PushPageModel<MockContentPageModel> ();
            coreMethods.PushPageModel<MockContentPageModel> ();

            _navigationMock.ReceivedCalls ();
            _navigationMock.ClearReceivedCalls ();

            coreMethods.PopPageModel ();

            _navigationMock.ReceivedCalls ();
            _navigationMock.ClearReceivedCalls ();

            coreMethods.PopToRoot (false);
        }

        /// People want the ability to modal with new NavigationService, this is the case where a ModalScreen also has
        ///         navigation stack

        ///   - needs ability to push modally with a new navigation service
        [Test]
        public void push_modally_new_navigation_service()
        {
            SetupFirstNavigationAndPage ();

            PushSecondNavigationStack ();

            //navigationService has push modal with new navigation service
            _navigationMock.Received ().PushPage (_secondNavService, Arg.Any<FreshBasePageModel>(), true);
        }

        ///   - when a new navigation service is pushed then models stores the previous navigationname
        [Test]
        public void new_navigationservice_the_model_stores_previous_navigationname()
        {
            SetupFirstNavigationAndPage ();

            PushSecondNavigationStack ();

            _pageModelSecond.CurrentNavigationServiceName.Should ().Be ("secondNav");
            _pageModelSecond.PreviousNavigationServiceName.Should ().Be ("firstNav");
            _pageModelSecond.IsModalFirstChild.Should().Be(true);
        }

        ///   - when model are pushed then we need to keep a reference to previous navigationname
        [Test]
        public void model_pushed_store_previous_navigationname()
        {
            SetupFirstNavigationAndPage ();

            PushSecondNavigationStack ();

            _coreMethods.PushPageModel<MockContentPageModel> ();

            var pageModelLatest = _secondNavService.CurrentPage.BindingContext as FreshBasePageModel;

            pageModelLatest.PreviousNavigationServiceName.Should ().Be ("firstNav");
        }

        ///   - when the first new page is pop'd we pop from older navigation service
        [Test]
        public void firstmodelchild_poped_popfrom_previous_navigation()
        {
            SetupFirstNavigationAndPage ();

            PushSecondNavigationStack ();

            _coreMethodsSecondPage.PopPageModel (true);

            //previousNavigation has pop modal called
            _navigationMock.Received().PopPage(true);
        }


        ///   - when is someone pushes a new MasterDetail or TabbedPages, how do we go back, we need like a PopModalNavigation
        [Test]
        public void should_allow_popmodalnavigation()
        {
            SetupFirstNavigationAndPage ();

            PushSecondNavigationStack ();

            _coreMethodsSecondPage.PushPageModel<MockContentPageModel> ();

            var pageModelLatest = _secondNavService.CurrentPage.BindingContext as FreshBasePageModel;

            pageModelLatest.CoreMethods.PopModalNavigationService ();

            //previousNavigation has pop modal called
            _navigationMock.Received().PopPage(true);
        }

        //TODO: test for this
        //if (navPage == null)
        //throw new Exception ("Navigation service is not Page");

        //TODO: throw exception when two stacks have same name

        //TODO: if page is already NavigationPage don't create another one

        IFreshNavigationService _navigationMock;
        Page _page;
        FreshBasePageModel _pageModel;

        void SetupFirstNavigationAndPage()
        {
            _navigationMock = Substitute.For<IFreshNavigationService> ();                
            FreshIOC.Container.Register<IFreshNavigationService> (_navigationMock, "firstNav");

            _page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            _pageModel = _page.BindingContext as MockContentPageModel;   
            _pageModel.CurrentNavigationServiceName = "firstNav";
        }

        PageModelCoreMethods _coreMethodsSecondPage;
        PageModelCoreMethods _coreMethods;
        Page _pageSecond;
        FreshBasePageModel _pageModelSecond;
        FreshNavigationContainer _secondNavService;

        void PushSecondNavigationStack()
        {
            _coreMethods = new PageModelCoreMethods (_page, _pageModel);
            _coreMethods.PushPageModel<MockContentPageModel> ();

            _pageSecond = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            _pageModelSecond = _pageSecond.BindingContext as MockContentPageModel;   
            _coreMethodsSecondPage = new PageModelCoreMethods (_pageSecond, _pageModelSecond);
            _secondNavService = new FreshNavigationContainer (_pageSecond, "secondNav");

            _coreMethods.PushNewNavigationServiceModal (_secondNavService, new FreshBasePageModel[] { _pageModelSecond });
        }
    }
}

