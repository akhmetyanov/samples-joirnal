using SamplesJournal_v2.Models.Template;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Text.Json;
using SamplesJournal_v2.Views;

namespace SamplesJournal_v2.ViewModels
{
    public class TemplatesViewModel: BaseViewModel
    {
        ObservableCollection<Template> _templates;
        public ICommand AddTemplateCommand { get; set; }

        public ICommand AddTemplateFromFileCommand { get; set; }

        public TemplatesViewModel(INavigation navigation):
            base(navigation)
        {
            _templates = new ObservableCollection<Template>(DataBaseFactory.DataBase.GetTemplates());
            AddTemplateCommand = new Command(AddTemplate);
            AddTemplateFromFileCommand = new Command(AddTemplateFromFile);
        }

        public ObservableCollection<TemplateViewModel> Templates 
        { 
            get
            {
                var ret = new ObservableCollection<TemplateViewModel>();
                foreach (var tempplate in _templates)
                {
                    ret.Add(new TemplateViewModel(this, tempplate, _navigation));
                }
                return ret;
            } 
            set
            {
                _templates.Clear();
                foreach (var templateViewModel in value)
                {
                    _templates.Add(templateViewModel.Template);
                }
                OnPropertyChanged();
            }
        }

        void AddTemplate()
        {
            var template = new Template();
            _templates.Add(template);
            OnPropertyChanged("Templates");
        }

        async void AddTemplateFromFile()
        {
            var file = await FilePicker.PickAsync();
            var json = System.IO.File.ReadAllText(file.FullPath);

            try
            {
                var template = JsonSerializer.Deserialize<Template>(json);
                await DataBaseFactory.DataBase.InsertOrReplaceTemplate(template);
                _templates.Add(template);
                OnPropertyChanged("Templates");
            }
            catch (Exception ex)
            {
                string[] msq = { ex.ToString() };
                _navigation.PushAsync(new MessagePage(msq));
            }
        }



        public void DeleteTemplate(Guid templateId)
        {
            var template = Templates.Where(t => t.Template.Id == templateId).FirstOrDefault().Template;
            if (template != null)
            {
                _templates.Remove(template);
                DataBaseFactory.DataBase.DeleteTemplate(template);
                OnPropertyChanged("Templates");
            }
        }

    }
}
