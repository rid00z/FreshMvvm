﻿using FreshMvvm.IoC;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class FreshIocTests
	{
		[Test]
		public void Test_Get_IoC_Container()
		{
			var container = FreshIoC.Container;

			Assert.That(container, Is.Not.Null);
            Assert.That(container, Is.TypeOf<FreshTinyIoCBuiltIn>());
		}
	}
}
