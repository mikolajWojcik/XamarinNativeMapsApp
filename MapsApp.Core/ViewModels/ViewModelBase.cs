using MvvmCross.ViewModels;

namespace MapsApp.ViewModels
{
    public class ViewModelBase : MvxViewModel
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

    }
}
