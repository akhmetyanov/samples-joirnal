using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.ViewModels.File;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views.File
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileEditPage : ContentPage
    {
        List<FileRowViewModel> _rowsVm;
        public FileEditPage(EditorFileRowsNode rowsNode, Template template)
        {
            _rowsVm = new List<FileRowViewModel>();
            foreach (var rowNode in rowsNode.Rows)
            {
                _rowsVm.Add(new FileRowViewModel(Navigation, rowNode, template));
            }

            InitializeComponent();
            BindingContext = this;
        }

        public ObservableCollection<FileRowViewModel> Rows 
        {
            get 
            {
                return new ObservableCollection<FileRowViewModel>(_rowsVm);
            } 
        }
    }
}