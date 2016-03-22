using System;
using System.Threading.Tasks;

namespace FreshMvvm.Tests.Mocks
{
    class MockPageModelCoreMethods : IPageModelCoreMethods
	{
		public void BatchBegin()
		{
			throw new NotImplementedException();
		}

		public void BatchCommit()
		{
			throw new NotImplementedException();
		}

		public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
		{
			throw new NotImplementedException();
		}

		public Task DisplayAlert(string title, string message, string cancel)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			throw new NotImplementedException();
		}

        public Task PushPageModel<T, TPage>(object data, bool modal = false) where T : FreshBasePageModel where TPage : Xamarin.Forms.Page
        {
            throw new NotImplementedException();
        }

        public Task PushPageModel<T, TPage>() where T : FreshBasePageModel where TPage : Xamarin.Forms.Page
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null)
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null)
        {
            throw new NotImplementedException();
        }

		public Task PopPageModel(bool modal = false)
		{
			throw new NotImplementedException();
		}

		public Task PopPageModel(object data, bool modal = false)
		{
			throw new NotImplementedException();
		}

		public Task PopToRoot(bool animate)
		{
			throw new NotImplementedException();
		}

		public Task PushPageModel(Type pageModelType)
		{
			throw new NotImplementedException();
		}

		public Task PushPageModel(Type pageModelType, object data, bool modal = false)
		{
			throw new NotImplementedException();
		}

		public Task PushPageModel<T>() where T : FreshBasePageModel
		{
			throw new NotImplementedException();
		}

		public Task PushPageModel<T>(object data, bool modal = false) where T : FreshBasePageModel
		{
			throw new NotImplementedException();
		}

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels)
        {
            throw new NotImplementedException ();
        }

        public Task PopModalNavigationService ()
        {
            throw new NotImplementedException ();
        }

        public Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer)
        {
            throw new NotImplementedException ();
        }

        public Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer)
        {
            throw new NotImplementedException ();
        }

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels)
        {
            throw new NotImplementedException ();
        }
	}
}
