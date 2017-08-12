using FreshMvvm.CoreMethods;
using FreshMvvm.NavigationContainers;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures.NavigationContainers
{
    [TestFixture]
    public class FreshTabbedNavigationContainerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_When_Then()
        {
            var sut = CreateSut();

        }

        private FreshTabbedNavigationContainer CreateSut()
        {
            return new FreshTabbedNavigationContainer();
        }
    }
}
