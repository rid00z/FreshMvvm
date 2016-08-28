using System.ComponentModel;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class FreshBasePageModelTests
	{
		[Test]
		public void Test_ImplementationINotifyPropertyChanged()
		{
			var viewModel = new MockFreshBasePageModel();

			Assert.IsInstanceOf<INotifyPropertyChanged>(viewModel);
		}

		[TestCase("test name")]
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

		[TestCase("default name", "new name")]
		public void Test_Init(string defaultValue, string newValue)
		{
			var viewModel = new MockFreshBasePageModel { Name = defaultValue };

			viewModel.Init(newValue);

			Assert.AreEqual(newValue, viewModel.Name);
		}

		[TestCase("default name", "new name")]
		public void Test_ReverseInit(string defaultValue, string newValue)
		{
			var viewModel = new MockFreshBasePageModel { Name = defaultValue };

			viewModel.ReverseInit(newValue);

			Assert.AreEqual(newValue, viewModel.Name);
		}

		[Test]
		public void Test_CurrentPage_Property()
		{
			var viewModel = new MockFreshBasePageModel();
            var page = new MockContentPage(viewModel);

			viewModel.CurrentPage = page;

			Assert.IsNotNull(viewModel.CurrentPage);
			Assert.AreEqual(page, viewModel.CurrentPage);
		}

		[Test]
		public void Test_PreviousPageModel_Property()
		{
			var viewModel = new MockFreshBasePageModel();
			var prevViewModel = new MockFreshBasePageModel();

			viewModel.PreviousPageModel = prevViewModel;

			Assert.IsNotNull(viewModel.PreviousPageModel);
			Assert.AreEqual(prevViewModel, viewModel.PreviousPageModel);
		}

		[Test]
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

