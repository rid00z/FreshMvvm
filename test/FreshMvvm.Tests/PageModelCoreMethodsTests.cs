using System.Threading.Tasks;
using FreshMvvm.TestHelpers.Mocks;
using NSubstitute;
using Xunit;
using Xamarin.Forms;
using FreshMvvm.Ioc;

namespace FreshMvvm.Tests
{
    public class PageModelCoreMethodsTests
    {
        public (PageModelCoreMethods, IFreshNavigationService) SetupFirstNavigationAndPage()
        {
            var navigationMock = Substitute.For<IFreshNavigationService>();
            FreshIoc.Container.Register<IFreshNavigationService>(navigationMock, "firstNav");

            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            var pageModel = (MockContentPageModel)page.BindingContext;
            pageModel.CurrentNavigationServiceName = "firstNav";

            var coreMethods = new PageModelCoreMethods(page, pageModel);

            return (coreMethods, navigationMock);
        }

        [Fact]

        public async Task model_property_populated_by_action()
        {
            var (coreMethods, navigationMock) = SetupFirstNavigationAndPage();

            const string item = "asj";
            await coreMethods.PushPageModel<MockItemPageModel>(pm => pm.Item = item);

            await navigationMock.Received().PushPage(Arg.Any<Page>(), Arg.Is<MockItemPageModel>(o => o.Item.Equals(item)), Arg.Any<bool>(), Arg.Any<bool>());
        }
    }
}
