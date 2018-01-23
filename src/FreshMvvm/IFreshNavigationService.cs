using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace FreshMvvm
{
    public interface IFreshNavigationService
    {
        Task PopToRoot(bool animate = true);

		Task PushPage (Page page, FreshBasePageModel model, bool modal = false, bool animate = true);

        Task PopPage (bool modal = false, bool animate = true);

        /// <summary>
        /// This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom pages also
        /// </summary>
        /// <returns>The BagePageModel, allows you to PopToRoot, Pass Data</returns>
        /// <param name="newSelected">The pagemodel of the root you want to change</param>
        Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel;

        void NotifyChildrenPageWasPopped();

        string NavigationServiceName { get; }
    }
}

