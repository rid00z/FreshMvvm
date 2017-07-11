using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm.Base;
using FreshMvvm.CoreMethods;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class PageModelNavigationFixture
	{
	    private Page page = Substitute.For<Page>();
	    private FreshPageModel pageModel = Substitute.For<FreshPageModel>();

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
