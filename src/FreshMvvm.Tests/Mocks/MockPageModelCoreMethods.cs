using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Mocks
{
    class MockPageModelCoreMethods : IPageModelCoreMethods
	{
        public void SwitchOutRootNavigation(string navigationServiceName)
        {
            throw new NotImplementedException();
        }

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

        public Task PushPageModel<T>(object data, bool modal = false, bool animate = true) where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task PushPageModel<T, TPage>(object data, bool modal = false, bool animate = true)
            where T : FreshBasePageModel
            where TPage : Page
        {
            throw new NotImplementedException();
        }

        public Task PopPageModel(bool modal = false, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PopPageModel(object data, bool modal = false, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PushPageModel<T>(bool animate = true) where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task PushPageModel<T, TPage>(bool animate = true)
            where T : FreshBasePageModel
            where TPage : Page
        {
            throw new NotImplementedException();
        }

        public Task PushPageModel(Type pageModelType, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromNavigation()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromNavigation<TPageModel>(bool removeAll = false) where TPageModel : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task<string> PushPageModelWithNewNavigation<T>(object data, bool animate = true) where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PushNewNavigationServiceModal(IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PopModalNavigationService(bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task<FreshBasePageModel> SwitchSelectedTab<T>() where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public Task<FreshBasePageModel> SwitchSelectedMaster<T>() where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }
    }
}
