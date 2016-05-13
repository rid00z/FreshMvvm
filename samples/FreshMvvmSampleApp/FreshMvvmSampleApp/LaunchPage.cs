using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace FreshMvvmSampleApp
{
    public class LaunchPage : ContentPage
    {
        public LaunchPage (App app)
        {
            Title = "Select Sample";
            var list = new ListView ();
            list.ItemsSource = new List<string> {
                "Basic Navigation",
                "Master Detail",
                "Tabbed Navigation",
                "Custom Navigation",
                "Tabbed (FO) Navigation",
                "Multiple Navigation"
            };
            list.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if ((string)e.Item == "Basic Navigation")
                    app.LoadBasicNav ();
                else if ((string)e.Item == "Master Detail")
                    app.LoadMasterDetail ();
                else if ((string)e.Item == "Tabbed Navigation")
                    app.LoadTabbedNav ();
                else if ((string)e.Item == "Tabbed (FO) Navigation")
                    app.LoadFOTabbedNav ();
                else if ((string)e.Item == "Custom Navigation")
                    app.LoadCustomNav ();
                else if ((string)e.Item == "Multiple Navigation")
                    app.LoadMultipleNavigation ();
            };
            this.Content = list;
        }
    }
}

