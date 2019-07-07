using System.ComponentModel;
using Xunit;
using FluentAssertions;
using FreshMvvm.TestHelpers.Mocks;

namespace FreshMvvm.Tests
{
    public class FreshBasePageModelTests
    {
        [Fact]
        public void Test_ImplementationINotifyPropertyChanged()
        {
            var viewModel = new MockFreshBasePageModel();

            viewModel.Should().BeAssignableTo<INotifyPropertyChanged>();
        }

        [Theory]
        [InlineData("test name")]
        public void Test_RaisePropertyChanged(string name)
        {
            string actual = null;
            var viewModel = new MockFreshBasePageModel();
            viewModel.PropertyChanged += (s, e) =>
            {
                actual = e.PropertyName;
            };

            viewModel.Name = name;

            actual.Should().NotBeNull();
            actual.Should().Be("Name");
            name.Should().BeSameAs(viewModel.Name);
        }

        [Theory]
        [InlineData("default name", "new name")]
        public void Test_Init(string defaultValue, string newValue)
        {
            var viewModel = new MockFreshBasePageModel { Name = defaultValue };

            viewModel.Init(newValue);

            newValue.Should().Be(viewModel.Name);
        }

        [Theory]
        [InlineData("default name", "new name")]
        public void Test_ReverseInit(string defaultValue, string newValue)
        {
            var viewModel = new MockFreshBasePageModel { Name = defaultValue };

            viewModel.ReverseInit(newValue);

            newValue.Should().Be(viewModel.Name);
        }

        [Fact]
        public void Test_CurrentPage_Property()
        {
            var viewModel = new MockFreshBasePageModel();
            var page = new MockContentPage();
            page.BindingContext = viewModel;

            viewModel.CurrentPage = page;

            viewModel.CurrentPage.Should().NotBeNull();
            page.Should().Be(viewModel.CurrentPage);
        }

        [Fact]
        public void Test_PreviousPageModel_Property()
        {
            var viewModel = new MockFreshBasePageModel();
            var prevViewModel = new MockFreshBasePageModel();

            viewModel.PreviousPageModel = prevViewModel;

            viewModel.PreviousPageModel.Should().NotBeNull();
            prevViewModel.Should().Be(viewModel.PreviousPageModel);
        }

        [Fact]
        public void Test_CoreMethods_Property()
        {
            var viewModel = new MockFreshBasePageModel();
            var methods = new MockPageModelCoreMethods();

            viewModel.CoreMethods = methods;

            viewModel.CoreMethods.Should().NotBeNull();
            methods.Should().Be(viewModel.CoreMethods);
        }
    }
}

