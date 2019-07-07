using System;
using FreshMvvm.TestHelpers.Mocks;
using Xunit;

namespace FreshMvvm.Tests
{
    public class FreshPageModelResolverTests
    {
        [Theory]
        [InlineData]
        public void Test_ResolvePageModel_Not_Found()
        {
            Assert.Throws<Exception>(() =>
            {
                FreshPageModelResolver.ResolvePageModel<MockFreshBasePageModel>();
            });
        }

        [Theory]
        [InlineData]
        public void Test_ResolvePageModel()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            var context = page.BindingContext as MockContentPageModel;

            Assert.IsNotNull(context);
            Assert.IsNotNull(context.CurrentPage);
            Assert.IsNotNull(context.CoreMethods);
        }

        [Theory]
        [InlineData("test data")]
        public void Test_ResolvePageModel_With_Init(object data)
        {
            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>(data);
            var context = page.BindingContext as MockContentPageModel;

            Assert.IsNotNull(context);
            Assert.IsNotNull(context.CurrentPage);
            Assert.IsNotNull(context.CoreMethods);
            Assert.AreSame(data, context.Data);
        }
    }
}

