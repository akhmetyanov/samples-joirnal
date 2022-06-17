using SamplesJournal_v2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemplatePage : ContentPage
    {
        public TemplatePage(TemplateViewModel templateViewModel)
        {
            InitializeComponent();
            templateViewModel.Page = this;
            BindingContext = templateViewModel;
        }
    }
}