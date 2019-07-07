using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.TestHelpers.Mocks;
using Xunit;

namespace FreshMvvm.Tests
{
	public class FreshNavigationContainerTests
	{
		[Fact]
		public void Test_Register_IFreshNavigationService()
		{
            var page = new MockContentPage();
            page.BindingContext = new MockContentPageModel();

			var navigation = new FreshNavigationContainer(page);
			var navigationService = FreshIoc.Container.Resolve<IFreshNavigationService>(Constants.DefaultNavigationServiceName);

			Assert.IsNotNull(navigationService);
			Assert.AreEqual(navigation, navigationService);
		}

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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
