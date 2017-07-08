using FreshMvvm.CoreMethods;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
    [TestFixture]
    public class PageModelNotificationsTests
    {
        private Page page = Substitute.For<Page>();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_When_Then()
        {
            var sut = CreateSut();
            
        }

        private PageModelNotifications CreateSut()
        {
            return new PageModelNotifications(page);
        }
    }
}