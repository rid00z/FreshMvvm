using Xamarin.Forms;

namespace FreshMvvm.CoreMethods
{
    public class PageModelTransactions : IPageModelTransactions
    {
        private readonly Page _currentPage;

        public PageModelTransactions(Page currentPage)
        {
            _currentPage = currentPage;
        }

        public void BatchBegin()
        {
            _currentPage.BatchBegin();
        }

        public void BatchCommit()
        {
            _currentPage.BatchCommit();
        }

    }
}