using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation _navigation;
        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
