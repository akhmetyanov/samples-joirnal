using SamplesJournal_v2.ViewModels.File;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views.File
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePage : ContentPage
    {
        public FilePage(FileViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}