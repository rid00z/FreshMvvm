using System.Threading.Tasks;
using FreshMvvm.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class PageModelCoreMethodsFixture
	{
	    PageModelCoreMethods _coreMethods;
	    IFreshNavigationService _navigationMock;
	    Page _page;
	    FreshBasePageModel _pageModel;

        void SetupFirstNavigationAndPage()
	    {
	        _navigationMock = Substitute.For<IFreshNavigationService>();
	        FreshIOC.Container.Register<IFreshNavigationService>(_navigationMock, "firstNav");

	        _page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
	        _pageModel = _page.BindingContext as MockContentPageModel;
	        _pageModel.CurrentNavigationServiceName = "firstNav";


	        _coreMethods = new PageModelCoreMethods(_page, _pageModel);
        }

        [Test]
	    public async Task model_property_populated_by_action()
	    {
            SetupFirstNavigationAndPage();

	        const string item = "asj";
	        await _coreMethods.PushPageModel<MockItemPageModel>(pm => pm.Item = item);

	        _navigationMock.Received().PushPage(Arg.Any<Page>(), Arg.Is<MockItemPageModel>(o => o.Item == item), Arg.Any<bool>(), Arg.Any<bool>());
        }
    }
}
