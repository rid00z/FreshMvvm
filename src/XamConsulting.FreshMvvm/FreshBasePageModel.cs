using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace FreshMvvm
{
    public abstract class FreshBasePageModel : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;
        
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
		public virtual void ReverseInit(object returndData) { }

		/// <summary>
		/// This method is called when the PageModel is loaded, the initData is the data that's sent from pagemodel before
		/// </summary>
		/// <param name="initData">Data that's sent to this PageModel from the pusher</param>
		public virtual void Init(object initData) { }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

		internal void WireEvents(Page page)
		{
			page.Appearing += ViewIsAppearing;
			page.Disappearing += ViewIsDisappearing;
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
        }
    }
}

