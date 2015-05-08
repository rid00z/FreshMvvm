using FreshTinyIoC;

namespace FreshMvvm
{
	public class FreshIOC
	{
        public static FreshTinyIoCContainer Container 
        { 
            get
            {
				return FreshTinyIoCContainer.Current;
            }
        }

		public FreshIOC ()
		{
            
		}
	}
}

