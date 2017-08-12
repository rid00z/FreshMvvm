using System.Linq;
using System.Threading.Tasks;
using FreshMvvm.IoC;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures.NavigationContainers
{
    [TestFixture]
    public class FreshNavigationContainerTests
    {
        [Test]
        public void Test_Register_IFreshNavigationService()
        {
            var page = new MockContentPage { BindingContext = new MockContentPageModel() };

            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(page);
            var navigationService = FreshIoC.Container.Resolve<IFreshNavigationService>(Constants.DefaultNavigationServiceName);

            Assert.IsNotNull(navigationService);
            Assert.AreEqual(navigation, navigationService);
        }

        [Test]
        public async Task Test_PushPage()
        {
            var mainPageViewModel = new MockFreshPageModel();
            var mainPage = new MockContentPage { BindingContext = mainPageViewModel };
            var detailsPage = new MockContentPage { BindingContext = mainPageViewModel };
            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(mainPage);

            await navigation.PushPage(detailsPage, mainPageViewModel);

            var page = navigation.Navigation.NavigationStack.FirstOrDefault(p => p.Id.Equals(detailsPage.Id));

            Assert.IsNotNull(page);
            Assert.AreSame(detailsPage, page);
        }

        [Test]
        public async Task Test_PushPage_Modal()
        {
            var mainPageViewModel = new MockFreshPageModel();
            var mainPage = new MockContentPage { BindingContext = mainPageViewModel };
            var detailsPage = new MockContentPage { BindingContext = mainPageViewModel };
            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(mainPage);

            Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(0));

            await navigation.PushPage(detailsPage, mainPageViewModel, true);

            Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_PopPage()
        {
            var mainPageViewModel = new MockFreshPageModel();
            var mainPage = new MockContentPage { BindingContext = mainPageViewModel };
            var detailsPage = new MockContentPage { BindingContext = mainPageViewModel };
            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(mainPage);

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
            var mainPageViewModel = new MockFreshPageModel();
            var mainPage = new MockContentPage { BindingContext = mainPageViewModel };
            var detailsPage = new MockContentPage { BindingContext = mainPageViewModel };
            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(mainPage);

            await navigation.PushPage(detailsPage, mainPageViewModel, true);

            Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(1));

            await navigation.PopPage(true);

            Assert.That(navigation.Navigation.ModalStack.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Test_PopToRoot()
        {
            var mainPage = new MockContentPage{BindingContext = new MockContentPageModel()};
            var navigation = new FreshMvvm.NavigationContainers.FreshNavigationContainer(mainPage);

            await navigation.PushPage(new MockContentPage{BindingContext = new MockContentPageModel()}, new MockFreshPageModel());
            await navigation.PushPage(new MockContentPage{BindingContext = new MockContentPageModel()}, new MockFreshPageModel());
            await navigation.PopToRoot();

            var firstPage = navigation.Navigation.NavigationStack.FirstOrDefault();

            Assert.IsNotNull(firstPage);
            Assert.AreSame(mainPage, firstPage);
        }
    }
}
