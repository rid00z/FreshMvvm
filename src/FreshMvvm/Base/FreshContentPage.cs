using System.Collections.Specialized;
using Xamarin.Forms;

namespace FreshMvvm.Base
{
    public class FreshContentPage : ContentPage
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var pageModel = BindingContext as FreshPageModel;

            if (pageModel?.ToolbarItems == null || pageModel.ToolbarItems.Count <= 0) return;

            pageModel.ToolbarItems.CollectionChanged += PageModel_ToolbarItems_CollectionChanged;

            foreach (var toolBarItem in pageModel.ToolbarItems)
            {
                if (!ToolbarItems.Contains(toolBarItem))
                {
                    ToolbarItems.Add(toolBarItem);
                }
            }
        }

        private void PageModel_ToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ToolbarItem toolBarItem in e.NewItems)
            {
                if (!ToolbarItems.Contains(toolBarItem))
                {
                    ToolbarItems.Add(toolBarItem);
                }
            }

            if (e.Action != NotifyCollectionChangedAction.Remove &&
                e.Action != NotifyCollectionChangedAction.Replace) return;

            foreach (ToolbarItem toolBarItem in e.OldItems)
            {
                if (!ToolbarItems.Contains(toolBarItem))
                {
                    ToolbarItems.Add(toolBarItem);
                }
            }

        }
    }
}

