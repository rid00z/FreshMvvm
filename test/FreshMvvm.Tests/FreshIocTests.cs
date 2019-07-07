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

            container.Should().NotBeNull();
            container.Should().BeAssignableTo<FreshTinyIocBuiltIn>();
        }
    }
}
