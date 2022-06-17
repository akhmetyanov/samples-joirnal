using SamplesJournal_v2.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemplatesPage : ContentPage
    {
        public TemplatesPage()
        {
            InitializeComponent();
            BindingContext = TemplatesViewModelFactory.GetViewModel(Navigation);
        }
    }
}