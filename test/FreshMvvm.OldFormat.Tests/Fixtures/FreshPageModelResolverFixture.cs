using System;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class FreshPageModelResolverFixture
	{
		[TestCase]
		public void Test_ResolvePageModel_Not_Found()
		{
			Assert.Throws<Exception>(() =>
			{
				FreshPageModelResolver.ResolvePageModel<MockFreshBasePageModel>();
			});
		}

		[TestCase]
		public void Test_ResolvePageModel()
		{
			var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.CoreMethods);
		}

		[TestCase("test data")]
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

