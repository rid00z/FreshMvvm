using System.ComponentModel;
using FreshMvvm.Tests.Mocks;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
    [TestFixture]
    public class FreshPageModelTests
    {
        [Test]
        public void Test_ImplementationINotifyPropertyChanged()
        {
            var viewModel = new MockFreshPageModel();

            Assert.IsInstanceOf<INotifyPropertyChanged>(viewModel);
        }

        [TestCase("test name")]
        public void Test_RaisePropertyChanged(string name)
        {
            string actual = null;
            var viewModel = new MockFreshPageModel();
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
            var viewModel = new MockFreshPageModel { Name = defaultValue };

            viewModel.PushedData(newValue);

            Assert.AreEqual(newValue, viewModel.Name);
        }

        [TestCase("default name", "new name")]
        public void Test_ReverseInit(string defaultValue, string newValue)
        {
            var viewModel = new MockFreshPageModel { Name = defaultValue };

            viewModel.PoppedData(newValue);

            Assert.AreEqual(newValue, viewModel.Name);
        }

        [Test]
        public void Test_CurrentPage_Property()
        {
            var viewModel = new MockFreshPageModel();
            var page = new MockContentPage();
            page.BindingContext = viewModel;

            viewModel.CurrentPage = page;

            Assert.IsNotNull(viewModel.CurrentPage);
            Assert.AreEqual(page, viewModel.CurrentPage);
        }

        [Test]
        public void Test_PreviousPageModel_Property()
        {
            var viewModel = new MockFreshPageModel();
            var prevViewModel = new MockFreshPageModel();

            viewModel.PreviousPageModel = prevViewModel;

            Assert.IsNotNull(viewModel.PreviousPageModel);
            Assert.AreEqual(prevViewModel, viewModel.PreviousPageModel);
        }

        [Test]
        public void Test_CoreMethods_Property()
        {
            var viewModel = new MockFreshPageModel();
            var methods = new MockPageModelNavigation();

            viewModel.Navigation = methods;

            Assert.IsNotNull(viewModel.Navigation);
            Assert.AreEqual(methods, viewModel.Navigation);
        }
    }
}

