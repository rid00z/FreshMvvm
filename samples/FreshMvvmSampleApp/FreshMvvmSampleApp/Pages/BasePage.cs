using System;
using Xamarin.Forms;
using System.ComponentModel;

namespace FreshMvvmSampleApp
{
    public class BasePage : ContentPage
    {
        public BasePage ()
        {
            ToolbarItems.Add (new ToolbarItem ("", "Home.png", () => {                
                Application.Current.MainPage = new NavigationPage (new LaunchPage ((App)Application.Current));
            }));
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            var basePageModel = this.BindingContext as FreshMvvm.FreshBasePageModel;
            if (basePageModel != null) {
                if (basePageModel.IsModalAndHasPreviousNavigationStack ()) {
                    if (ToolbarItems.Count < 2)
                    {
                        var closeModal = new ToolbarItem ("Close Modal", "", () => {                
                            basePageModel.CoreMethods.PopModalNavigationService(); 
                        });

                        ToolbarItems.Add (closeModal);
                    }
                }
            }
        }
    }
}

