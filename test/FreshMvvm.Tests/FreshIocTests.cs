using FluentAssertions;
using FreshMvvm.Ioc;
using FreshTinyIoc;
using Xunit;

namespace FreshMvvm.Tests
{
    public class FreshIocTests
    {
        [Fact]
        public void Test_Get_Ioc_Container()
        {
            var container = FreshIoc.Container;

            Assert.That(container, Is.Not.Null);
            Assert.That(container, Is.TypeOf<FreshTinyIocBuiltIn>());
        }
    }
}
