using FreshMvvm.Base;

namespace FreshMvvm.Tests.Mocks
{
    public class MockFreshPageModel : FreshPageModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public override void PushedData(object initData)
        {
            base.PushedData(initData);

            Name = initData as string;
        }

        public override void PoppedData(object returndData)
        {
            base.PoppedData(returndData);

            Name = returndData as string;
        }
    }
}

