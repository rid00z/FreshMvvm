using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class FreshNavigationContainerFixture
	{
		[Test]
		public void Test_Register_IFreshNavigationService()
		{
            var page = new MockContentPage();
            page.BindingContext = new MockContentPageModel();

			var navigation = new FreshNavigationContainer(page);
			var navigationService = FreshIOC.Container.Resolve<IFreshNavigationService>(Constants.DefaultNavigationServiceName);

			Assert.IsNotNull(navigationService);
			Assert.AreEqual(navigation, navigationService);
		}

		[Test]
		public async Task Test_PushPage()
		{
			var mainPageViewModel = new MockFreshBasePageModel();
			var mainPage = new MockContentPage();
            mainPage.BindingContext = new MockContentPageModel();

			var detailsPage = new MockContentPage();
            detailsPage.BindingContext = new MockContentPageModel();

            var navigation = new FreshNavigationContainer(mainPage);

			await navigation.PushPage(detailsPage, mainPageViewModel);

			var page = navigation.Navigation.NavigationStack.FirstOrDefault(p => p.Id.Equals(detailsPage.Id));

			Assert.IsNotNull(page);
			Assert.AreSame(detailsPage, page);
		}

		[Test]
		public async Task Test_PushPage_Modal()
		{
			var mainPageViewModel = new MockFreshBasePageModel();
			var mainPage = new MockContentPage();
            mainPage.BindingContext = new MockContentPageModel();

			var detailsPage = new MockContentPage();
            detailsPage.BindingContext = new MockContentPageModel();

			var navigation = new FreshNavigationContainer(mainPage);

			Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(0));

			await navigation.PushPage(detailsPage, mainPageViewModel, true);

			Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task Test_PopPage()
		{
			var mainPageViewModel = new MockFreshBasePageModel();
			var mainPage = new MockContentPage();
            mainPage.BindingContext = new MockContentPageModel();

			var detailsPage = new MockContentPage();
            detailsPage.BindingContext = new MockContentPageModel();

			var navigation = new FreshNavigationContainer(mainPage);

			await navigation.PushPage(detailsPage, mainPageViewModel);
			await navigation.PopPage();

			var page = navigation.Navigation.NavigationStack.FirstOrDefault(p => p.Id.Equals(detailsPage.Id));
			var firstPage = navigation.Navigation.NavigationStack.FirstOrDefault();

			Assert.IsNull(page);
			Assert.IsNotNull(firstPage);
			Assert.AreSame(mainPage, firstPage);
		}

		[Test]
		public async Task Test_PopPage_Modal()
		{
			var mainPageViewModel = new MockFreshBasePageModel();
			var mainPage = new MockContentPage();
            mainPage.BindingContext = new MockContentPageModel();

			var detailsPage = new MockContentPage();
            detailsPage.BindingContext = new MockContentPageModel();

			var navigation = new FreshNavigationContainer(mainPage);

			await navigation.PushPage(detailsPage, mainPageViewModel, true);

			Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(1));

			await navigation.PopPage(true);

			Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task Test_PopToRoot()
		{
            var mainPage = new MockContentPage();
            mainPage.BindingContext = new MockContentPageModel();
			var navigation = new FreshNavigationContainer(mainPage);
            
			await navigation.PushPage(new MockContentPage {BindingContext = new MockContentPageModel() }, new MockFreshBasePageModel());
			await navigation.PushPage(new MockContentPage { BindingContext = new MockContentPageModel() }, new MockFreshBasePageModel());
			await navigation.PopToRoot();

			var firstPage = navigation.Navigation.NavigationStack.FirstOrDefault();

			Assert.IsNotNull(firstPage);
			Assert.AreSame(mainPage, firstPage);
		}
	}
}
