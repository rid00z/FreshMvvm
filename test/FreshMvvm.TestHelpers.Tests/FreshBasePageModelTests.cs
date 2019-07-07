using System.ComponentModel;
using FreshMvvm.TestHelpers.Mocks;
using Xunit;

namespace FreshMvvm.Tests
{
	public class FreshBasePageModelTests
	{
		[Fact]
		public void Test_ImplementationINotifyPropertyChanged()
		{
			var viewModel = new MockFreshBasePageModel();

			Assert.IsInstanceOf<INotifyPropertyChanged>(viewModel);
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

			Assert.IsNotNull(actual);
			Assert.AreEqual("Name", actual);
			Assert.AreEqual(name, viewModel.Name);
		}

		[Theory]
        [InlineData("default name", "new name")]
		public void Test_Init(string defaultValue, string newValue)
		{
			var viewModel = new MockFreshBasePageModel { Name = defaultValue };

			viewModel.Init(newValue);

			Assert.AreEqual(newValue, viewModel.Name);
		}

		[Theory]
        [InlineData("default name", "new name")]
		public void Test_ReverseInit(string defaultValue, string newValue)
		{
			var viewModel = new MockFreshBasePageModel { Name = defaultValue };

			viewModel.ReverseInit(newValue);

			Assert.AreEqual(newValue, viewModel.Name);
		}

		[Fact]
		public void Test_CurrentPage_Property()
		{
			var viewModel = new MockFreshBasePageModel();
            var page = new MockContentPage();
            page.BindingContext = viewModel;

			viewModel.CurrentPage = page;

			Assert.IsNotNull(viewModel.CurrentPage);
			Assert.AreEqual(page, viewModel.CurrentPage);
		}

		[Fact]
		public void Test_PreviousPageModel_Property()
		{
			var viewModel = new MockFreshBasePageModel();
			var prevViewModel = new MockFreshBasePageModel();

			viewModel.PreviousPageModel = prevViewModel;

			Assert.IsNotNull(viewModel.PreviousPageModel);
			Assert.AreEqual(prevViewModel, viewModel.PreviousPageModel);
		}

		[Fact]
		public void Test_CoreMethods_Property()
		{
			var viewModel = new MockFreshBasePageModel();
			var methods = new MockPageModelCoreMethods();

			viewModel.CoreMethods = methods;

			Assert.IsNotNull(viewModel.CoreMethods);
			Assert.AreEqual(methods, viewModel.CoreMethods);
		}
	}
}

