using FreshTinyIoc;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class FreshIocTests
	{
		[Test]
		public void Test_Get_Ioc_Container()
		{
			var container = FreshIoc.Container;

			Assert.That(container, Is.Not.Null);
            Assert.That(container, Is.TypeOf<FreshTinyIocBuiltIn>());
		}
	}
}
