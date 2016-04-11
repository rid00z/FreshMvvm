using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public interface IPageModelCoreMethods
    {
        Task DisplayAlert (string title, string message, string cancel);

        Task<string> DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons);

        Task<bool> DisplayAlert (string title, string message, string accept, string cancel);

        Task PushPageModel<T> (object data, bool modal = false) where T : FreshBasePageModel;

        Task PushPageModel<T, TPage> (object data, bool modal = false) where T : FreshBasePageModel where TPage : Page;

        Task PopPageModel (bool modal = false);

        Task PopPageModel (object data, bool modal = false);

        Task PushPageModel<T> () where T : FreshBasePageModel;

        Task PushPageModel<T, TPage> () where T : FreshBasePageModel where TPage : Page;

        Task PushPageModel (Type pageModelType);

        Task PushPageModel (Type pageModelType, object data, bool modal = false);

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels);

        Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null);

        Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null);

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels);

        Task PopModalNavigationService();

        Task SwitchOutRootNavigation(string navigationServiceName);

        Task PopToRoot(bool animate);

		void BatchBegin();

		void BatchCommit();
    }
}

