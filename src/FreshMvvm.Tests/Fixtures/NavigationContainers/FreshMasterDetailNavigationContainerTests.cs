using FreshMvvm.CoreMethods;
using FreshMvvm.NavigationContainers;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures.NavigationContainers
{
	[TestFixture]
	public class FreshMasterDetailNavigationContainerTests
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

	    private FreshMasterDetailNavigationContainer CreateSut()
	    {
	        return new FreshMasterDetailNavigationContainer();
	    }
    }
}
