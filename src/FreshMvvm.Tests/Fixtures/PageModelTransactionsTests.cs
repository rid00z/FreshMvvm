using FreshMvvm.CoreMethods;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
    [TestFixture]
    public class PageModelTransactionsTests
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

        private PageModelTransactions CreateSut()
        {
            return new PageModelTransactions(page);
        }
    }
}