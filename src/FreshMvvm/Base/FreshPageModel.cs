﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FreshMvvm.CoreMethods;
using Xamarin.Forms;

namespace FreshMvvm.Base
{
    public abstract class FreshPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        public FreshPageModel PreviousPageModel { get; set; }

        /// <summary>
        /// A reference to the current page, that's automatically filled, on push
        /// </summary>
        public Page CurrentPage { get; set; }

        /// <summary>
        /// Core methods are basic built in methods for the App including Pushing, Pop and Alert
        /// </summary>
        public IPageModelNavigation Navigation { get; set; }

        public IPageModelNotifications Notifications { get; set; }

        public IPageModelTransactions Transactions { get; set; }

        /// <summary>
        /// This method is called when a page is Pop'd, it also allows for data to be returned.
        /// </summary>
        /// <param name="model">This data that's returned from </param>
        public virtual void PoppedData (object model)
        {
        }

        /// <summary>
        /// This method is called when the PageModel is loaded, the model is the data that's sent from pagemodel before
        /// </summary>
        /// <param name="model">Data that's sent to this PageModel from the pusher</param>
        public virtual void PushedData (object model)
        {            
        }

        protected void RaisePropertyChanged ([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke (this, new PropertyChangedEventArgs (propertyName));
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
        public string PreviousNavigationServiceName;

        /// <summary>
        /// Used when a page is shown modal and wants a new Navigation Stack
        /// </summary>
        public string CurrentNavigationServiceName = Constants.DefaultNavigationServiceName;

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

        private bool _alreadyAttached;
        /// <summary>
        /// This is used to attach the page was popped method to a NavigationPage if available
        /// </summary>
        private void AttachPageWasPoppedEvent()
        {
            var navPage = CurrentPage.Parent as NavigationPage;
            if (navPage == null) return;
            _alreadyAttached = true;
            navPage.Popped += HandleNavPagePopped;
        }

        private void HandleNavPagePopped(object sender, NavigationEventArgs e)
        {
            if (e.Page == CurrentPage)
            {
                RaisePageWasPopped();
            }
        }

        public void RaisePageWasPopped()
        {
            PageWasPopped?.Invoke(this, EventArgs.Empty);

            var navPage = CurrentPage.Parent as NavigationPage;
            if (navPage != null)
                navPage.Popped -= HandleNavPagePopped;

            CurrentPage.Appearing -= ViewIsAppearing;
            CurrentPage.Disappearing -= ViewIsDisappearing;
            CurrentPage.BindingContext = null;
        }
    }
}

