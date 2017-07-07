using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class PageModelNavigationFixture
	{
	    private Page page = Substitute.For<Page>();
	    private FreshBasePageModel pageModel = Substitute.For<FreshBasePageModel>();

	    [SetUp]
	    public void Setup()
	    {
	    }

	    [Test]
	    public void Given_When_Then()
	    {
	        var sut = CreateSut();
	    }

	    private PageModelNavigation CreateSut()
	    {
	        return new PageModelNavigation(page, pageModel);
	    }
    }
}
