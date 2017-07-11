using System;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class FreshPageModelResolverTests
	{
		[TestCase]
		public void Test_ResolvePageModel_Not_Found()
		{
			Assert.Throws<Exception>(() =>
			{
				FreshPageModelResolver.ResolvePageModel<MockFreshPageModel>();
			});
		}

		[TestCase]
		public void Test_ResolvePageModel()
		{
			var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.Navigation);
		}

		[TestCase("test data")]
		public void Test_ResolvePageModel_With_Init(object data)
		{
			var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>(data);
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.Navigation);
			Assert.AreSame(data, context.Data);
		}
	}
}

