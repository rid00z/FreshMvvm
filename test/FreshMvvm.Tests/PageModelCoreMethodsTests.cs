﻿using System.Threading.Tasks;
using FreshMvvm.TestHelpers.Mocks;
using NSubstitute;
using Xunit;
using Xamarin.Forms;
using FreshMvvm.Ioc;

namespace FreshMvvm.Tests
{
    public class PageModelCoreMethodsTests
    {
        PageModelCoreMethods _coreMethods;
        IFreshNavigationService _navigationMock;
        Page _page;
        FreshBasePageModel _pageModel;

        void SetupFirstNavigationAndPage()
        {
            _navigationMock = Substitute.For<IFreshNavigationService>();
            FreshIoc.Container.Register<IFreshNavigationService>(_navigationMock, "firstNav");

            _page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            _pageModel = _page.BindingContext as MockContentPageModel;
            _pageModel.CurrentNavigationServiceName = "firstNav";


            _coreMethods = new PageModelCoreMethods(_page, _pageModel);
        }

        [Fact]
        
        public async Task model_property_populated_by_action()
        {
            SetupFirstNavigationAndPage();

            const string item = "asj";
            await _coreMethods.PushPageModel<MockItemPageModel>(pm => pm.Item = item);

            _navigationMock.Received().PushPage(Arg.Any<Page>(), Arg.Is<MockItemPageModel>(o => o.Item == item), Arg.Any<bool>(), Arg.Any<bool>());
        }
    }
}
