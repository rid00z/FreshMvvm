using System;
using FreshMvvm.TestHelpers.Mocks;
using Xunit;
using FluentAssertions;

namespace FreshMvvm.Tests
{
    public class FreshPageModelResolverTests
    {
        [Fact]
        public void Test_ResolvePageModel_Not_Found()
        {
            Assert.Throws<Exception>(() =>
            {
                FreshPageModelResolver.ResolvePageModel<MockFreshBasePageModel>();
            });
        }

        [Fact]
        public void Test_ResolvePageModel()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
            var context = page.BindingContext as MockContentPageModel;

            context.Should().NotBeNull();
            context.CurrentPage.Should().NotBeNull();
            context.CoreMethods.Should().NotBeNull();
        }

        [Theory]
        [InlineData("test data")]
        public void Test_ResolvePageModel_With_Init(object data)
        {
            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>(data);
            var context = page.BindingContext as MockContentPageModel;

            context.Should().NotBeNull();
            context.CurrentPage.Should().NotBeNull();
            context.CoreMethods.Should().NotBeNull();
            data.Should().BeSameAs(context.Data);
        }
    }
}

