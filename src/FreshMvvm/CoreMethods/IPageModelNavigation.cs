using System;
using System.Threading.Tasks;
using FreshMvvm.Base;
using FreshMvvm.NavigationContainers;
using Xamarin.Forms;

namespace FreshMvvm
{
    public interface IPageModelNavigation
    {
        Task PushPageModel<T> (object data, bool modal = false, bool animate = true) where T : FreshPageModel;

        Task PushPageModel<T, TPage> (object data, bool modal = false, bool animate = true) where T : FreshPageModel where TPage : Page;

        Task PopPageModel (bool modal = false, bool animate = true);

        Task PopPageModel (object data, bool modal = false, bool animate = true);

        Task PushPageModel<T> (bool animate = true) where T : FreshPageModel;

        Task PushPageModel<T, TPage> (bool animate = true) where T : FreshPageModel where TPage : Page;

        Task PushPageModel (Type pageModelType, bool animate = true);

        /// <summary>
        /// Removes current page/pagemodel from navigation
        /// </summary>
        void RemoveFromNavigation ();

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        /// <param name="removeAll">Will remove all, otherwise it will just remove first on from the top of the stack</param>
        /// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
        void RemoveFromNavigation<TPageModel> (bool removeAll = false) where TPageModel : FreshPageModel;

        /// <summary>
        /// This method pushes a new PageModel modally with a new NavigationContainer
        /// </summary>
        /// <returns>Returns the name of the new service</returns>
        Task<string> PushPageModelWithNewNavigation<T> (object data, bool animate = true) where T : FreshPageModel;

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshPageModel[] basePageModels, bool animate = true);

        Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer, FreshPageModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer, FreshPageModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshPageModel basePageModels, bool animate = true);

        Task PopModalNavigationService(bool animate = true);

        void SwitchOutRootNavigation(string navigationServiceName);

        /// <summary>
        /// This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom pages also
        /// </summary>
        /// <returns>The BagePageModel, allows you to PopToRoot, Pass Data</returns>
        Task<FreshPageModel> SwitchSelectedRootPageModel<T>() where T : FreshPageModel;

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        /// <returns>The BagePageModel, allows you to PopToRoot, Pass Data</returns>
        Task<FreshPageModel> SwitchSelectedTab<T>() where T : FreshPageModel;

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        /// <returns>The BagePageModel, allows you to PopToRoot, Pass Data</returns>
        Task<FreshPageModel> SwitchSelectedMaster<T>()where T : FreshPageModel;

        Task PopToRoot(bool animate);
    }
}

