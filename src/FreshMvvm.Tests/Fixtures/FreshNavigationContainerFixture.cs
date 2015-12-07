using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class FreshNavigationContainerFixture
	{
		[Test]
		public void Test_Register_IFreshNavigationService()
		{
			var page = new MockContentPage();

			var navigation = new FreshNavigationContainer(page);
			var navigationService = FreshIOC.Container.Resolve<IFreshNavigationService>();

			Assert.IsNotNull(navigationService);
			Assert.AreEqual(navigation, navigationService);
		}

		[Test]
		public async Task Test_PushPage_By_Default()
		{
			var isDisappeared = false;
			var isAppeared = false;
			var mainPageViewModel = new MockFreshBasePageModel();
			var mainPage = new MockContentPage();
			var detailsPage = new MockContentPage();
			var navigation = new FreshNavigationContainer(mainPage);

			mainPage.Disappearing += (s, e) =>
			{
				isDisappeared = true;
			};
			detailsPage.Appearing += (s, e) =>
			{
				isAppeared = true;
			};

			await navigation.PushPage(detailsPage, mainPageViewModel).ContinueWith(t =>
			{
				Assert.IsTrue(isAppeared);
				Assert.IsTrue(isDisappeared);
			});
		}
	}
}
