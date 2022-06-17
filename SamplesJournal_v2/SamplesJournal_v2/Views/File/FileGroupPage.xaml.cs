using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views.File
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileGroupPage : ContentPage
    {
        List<EditorFileRowsNode> nodes;
        Template template;
        public FileGroupPage(List<EditorFileRowsNode> nodes, Template template)
        {
            InitializeComponent();
            this.nodes = nodes;
            BindingContext = this;
            Title = nodes.FirstOrDefault().Name;
            this.template = template;
        }

        public ObservableCollection<EditorFileRowsNode> Nodes { 
            get 
            { 
                return new ObservableCollection<EditorFileRowsNode>(nodes); 
            } 
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var node = (EditorFileRowsNode) e.Item;
            
            if (node.Childs.Count != 0)
            {
                Navigation.PushAsync(new FileGroupPage(node.Childs, template));
            }
            else
            {
                Navigation.PushAsync(new FileEditPage(node, template));
            }
        }
    }
}