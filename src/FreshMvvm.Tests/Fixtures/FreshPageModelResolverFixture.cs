using System;
using System.Threading.Tasks;
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
			Assert.ThrowsAsync<Exception>(async () =>
			{
				await FreshPageModelResolver.ResolvePageModel<MockFreshBasePageModel>();
			});
		}

		[TestCase]
		public async Task Test_ResolvePageModel()
		{
			var page = await  FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.CoreMethods);
		}

		[TestCase("test data")]
		public async Task Test_ResolvePageModel_With_Init(object data)
		{
			var page = await FreshPageModelResolver.ResolvePageModel<MockContentPageModel>(data);
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.CoreMethods);
			Assert.AreSame(data, context.Data);
		}
	}
}

