using SamplesJournal_v2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemplateHeaderSelectDictPage : ContentPage
    {
        public TemplateHeaderSelectDictPage(TemplateHeaderViewModel headerViewModel)
        {
            InitializeComponent();
            BindingContext = headerViewModel;
        }
    }
}