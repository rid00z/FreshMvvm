﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FreshMvvm
{
    public abstract class FreshBasePageModel : FreshNotifyPropertyChanged
	{
        /// <summary>
        /// This event is raise when a page is Popped, this might not be raise everytime a page is Popped. 
        /// Note* this might be raised multiple times. 
        /// </summary>
        public event EventHandler PageWasPopped; 

        /// <summary>
        /// This property is used by the FreshBaseContentPage and allows you to set the toolbar items on the page.
        /// </summary>
        public ObservableCollection<ToolbarItem> ToolbarItems { get; set; }

        /// <summary>
        /// The previous page model, that's automatically filled, on push
        /// </summary>
        public FreshBasePageModel PreviousPageModel { get; set; }

        /// <summary>
        /// A reference to the current page, that's automatically filled, on push
        /// </summary>
        public Page CurrentPage { get; set; }

        /// <summary>
        /// Core methods are basic built in methods for the App including Pushing, Pop and Alert
        /// </summary>
        public IPageModelCoreMethods CoreMethods { get; set; }

        /// <summary>
        /// This method is called when a page is Pop'd, it also allows for data to be returned.
        /// </summary>
        /// <param name="returndData">This data that's returned from </param>
        public virtual void ReverseInit (object returndData)
        {
        }

        /// <summary>
        /// This method is called when the PageModel is loaded, the initData is the data that's sent from pagemodel before
        /// </summary>
        /// <param name="initData">Data that's sent to this PageModel from the pusher</param>
        public virtual void Init (object initData)
        {            
        }

        internal void WireEvents (Page page)
        {
            page.Appearing += ViewIsAppearing;
            page.Disappearing += ViewIsDisappearing;
        }

        /// <summary>
        /// Is true when this model is the first of a new navigation stack
        /// </summary>
        internal bool IsModalFirstChild;

        /// <summary>
        /// Used when a page is shown modal and wants a new Navigation Stack
        /// </summary>
        internal string PreviousNavigationServiceName;

        /// <summary>
        /// Used when a page is shown modal and wants a new Navigation Stack
        /// </summary>
        internal string CurrentNavigationServiceName = Constants.DefaultNavigationServiceName;

        /// <summary>
        /// This means the current PageModel is shown modally and can be pop'd modally
        /// </summary>
        public bool IsModalAndHasPreviousNavigationStack()
        {
            return !string.IsNullOrWhiteSpace (PreviousNavigationServiceName) && PreviousNavigationServiceName != CurrentNavigationServiceName;
        }

        /// <summary>
        /// This method is called when the view is disappearing. 
        /// </summary>
        protected virtual void ViewIsDisappearing (object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This methods is called when the View is appearing
        /// </summary>
        protected virtual void ViewIsAppearing (object sender, EventArgs e)
        {
            if (!_alreadyAttached)
                AttachPageWasPoppedEvent();
        }

        bool _alreadyAttached = false;
        /// <summary>
        /// This is used to attach the page was popped method to a NavigationPage if available
        /// </summary>
        void AttachPageWasPoppedEvent()
        {
            var navPage = (this.CurrentPage.Parent as NavigationPage);
            if (navPage != null)
            {
                _alreadyAttached = true;
                navPage.Popped += HandleNavPagePopped;
            }
        }

        void HandleNavPagePopped(object sender, NavigationEventArgs e)
        {
            if (e.Page == this.CurrentPage)
            {
                if (PageWasPopped != null)
                    PageWasPopped(this, EventArgs.Empty);

                var navPage = (this.CurrentPage.Parent as NavigationPage);
                if (navPage != null)
                    navPage.Popped -= HandleNavPagePopped;

                CurrentPage.Appearing -= ViewIsAppearing;
                CurrentPage.Disappearing -= ViewIsDisappearing;
            }
        }

        public void RaisePageWasPopped()
        {
            if (PageWasPopped != null)
                PageWasPopped(this, EventArgs.Empty);
        }
    }
}

