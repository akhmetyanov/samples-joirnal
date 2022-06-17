using SamplesJournal_v2.ViewModels.FIle;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views.File
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilesPage : ContentPage
    {
        public FilesPage()
        {
            InitializeComponent();
            BindingContext = new FilesViewModel(Navigation);
        }
    }
}