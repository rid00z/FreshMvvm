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

        Task PushPageModel<T> (object data, bool modal = false, bool animate = true) where T : FreshBasePageModel;

        Task PushPageModel<T, TPage> (object data, bool modal = false, bool animate = true) where T : FreshBasePageModel where TPage : Page;

        Task PopPageModel (bool modal = false, bool animate = true);

        Task PopPageModel (object data, bool modal = false, bool animate = true);

        Task PushPageModel<T> (bool animate = true) where T : FreshBasePageModel;

        Task PushPageModel<T, TPage> (bool animate = true) where T : FreshBasePageModel where TPage : Page;

        Task PushPageModel (Type pageModelType, bool animate = true);

        /// <summary>
        /// This method pushes a new PageModel modally with a new NavigationContainer
        /// </summary>
        /// <returns>Returns the name of the new service</returns>
        Task<string> PushPageModelWithNewNavigation<T> (object data, bool animate = true) where T : FreshBasePageModel;

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels, bool animate = true);

        Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels, bool animate = true);

        Task PopModalNavigationService(bool animate = true);

        void SwitchOutRootNavigation(string navigationServiceName);

        Task PopToRoot(bool animate);

		void BatchBegin();

		void BatchCommit();
    }
}

