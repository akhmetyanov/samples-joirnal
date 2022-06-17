using SamplesJournal_v2.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemplateHeaderPage : ContentPage
    {
        public TemplateHeaderPage(TemplateHeaderViewModel templateHeaderViewModel)
        {
            InitializeComponent();
            BindingContext = templateHeaderViewModel;
        }
    }
}