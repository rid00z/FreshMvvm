using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace FreshMvvmApp
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
            list.ItemTapped += async (object sender, ItemTappedEventArgs e) => {
                if ((string)e.Item == "Basic Navigation")
                    await app.LoadBasicNav ();
                else if ((string)e.Item == "Master Detail")
                    app.LoadMasterDetail ();
                else if ((string)e.Item == "Tabbed Navigation")
                    app.LoadTabbedNav ();
                else if ((string)e.Item == "Tabbed (FO) Navigation")
                    app.LoadFOTabbedNav ();
                else if ((string)e.Item == "Custom Navigation")
                    await app.LoadCustomNav ();
                else if ((string)e.Item == "Multiple Navigation")
                    await app.LoadMultipleNavigation ();
            };
            this.Content = list;
        }
    }
}

