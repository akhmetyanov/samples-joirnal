using CsvHelper;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Services;
using SamplesJournal_v2.Views;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels
{
    public class TemplateViewModel: BaseViewModel
    {
        Template _template;
        TemplatesViewModel _parentViewModel;
        public ICommand DeleteTemplateCommand { get; set; }
        public ICommand EditeTemplateCommand { get; set; }
        public ICommand AddHeaderCommand { get; set; }
        public ICommand AddDictCommand { get; set; }
        public ICommand AddHeaderFromFileCommand { get; set; }
        public ICommand SaveTemplateCommand { get; set; }
        public ICommand SaveTemplateToFileCommand { get; set; }
        public ICommand SaveTemplateToCsvFileCommand { get; set; }

        public Page Page { get; set; } // for DisplayAlert
        public TemplateViewModel(TemplatesViewModel parentViewModel, Template template, INavigation navigation, Page page = null)
            :base(navigation)
        {
            _parentViewModel = parentViewModel;
            _template = template;

            DeleteTemplateCommand = new Command(() => { _parentViewModel.DeleteTemplate(Id); });
            EditeTemplateCommand = new Command(() => { _navigation.PushAsync(new TemplatePage(this)); });
            AddHeaderCommand = new Command(() => {
                var name = "Header " + _template.Headers.Count.ToString();
                _template.Headers.Add(new TemplateHeader(_template.Id) { Name = name });
                OnPropertyChanged("Headers");
            });
            AddDictCommand = new Command(() =>
            {
                var name = "Dict " + _template.Dicts.Count.ToString();
                _template.Dicts.Add(new TemplateDict(Id) { Name = name });
                OnPropertyChanged("Dicts");
            });
            AddHeaderFromFileCommand = new Command(AddHeaderFromFile);
            SaveTemplateCommand = new Command(async () =>
            {
                // проверить на наличие полей для отображения
                var hasFieldToEdit = false;
                foreach (var header in _template.Headers)
                {
                    if (header.ShowInEditor)
                    {
                        hasFieldToEdit = true;
                    }
                }

                if (hasFieldToEdit == false) 
                {
                    Page?.DisplayAlert("Сохранение", "Не выбрано поле для отображения в редакторе", "OK");
                    return;
                }

                await DataBaseFactory.DataBase.InsertOrReplaceTemplate(_template);
                Page?.DisplayAlert("Сохранение", "Шаблон сохранен", "OK");
                _navigation.PopAsync();
            });

            SaveTemplateToFileCommand = new Command(SaveThisToFile);
        }

        public Guid Id { get { return _template.Id; }}

        public string Name 
        { 
            get
            {
                return _template.Name;
            }
            set
            {
                _template.Name = value;
                OnPropertyChanged();
            }
        }

        public Template Template { get { return _template; } }
        public ObservableCollection<TemplateHeaderViewModel> Headers 
        { 
            get
            {
                var ret = new ObservableCollection<TemplateHeaderViewModel>();
                foreach (var header in _template.Headers)
                {
                    ret.Add(new TemplateHeaderViewModel(_navigation, header, this));
                }

                return ret;
            }
        }

        async void AddHeaderFromFile()
        {
            var file = await FilePicker.PickAsync();

            try
            {
                string[] lines = System.IO.File.ReadAllLines(file.FullPath);
                var headers = lines[0].Split(';');

                foreach (var header in headers)
                {
                    _template.Headers.Add(new TemplateHeader(_template.Id) { Name = header });
                }

                OnPropertyChanged("Headers");
            }
            catch (Exception ex)
            {
                string[] msq = { ex.Message };
                _navigation.PushAsync(new MessagePage(msq));
            }
            
        }

        public void SortHeaderListUpHeader(Guid headerID)
        {
            var header = _template.Headers.Where(h => h.Id == headerID).FirstOrDefault();
            var idx = _template.Headers.IndexOf(header);

            if (idx == -1 || idx == 0) { return; }

            var headerToDow = _template.Headers[idx -1];
            _template.Headers[idx] = headerToDow;
            _template.Headers[idx - 1] = header;

            OnPropertyChanged("Headers");
        }

        public void SortHeaderListDownHeader(Guid headerID)
        {
            var header = _template.Headers.Where(h => h.Id == headerID).FirstOrDefault();
            var idx = _template.Headers.IndexOf(header);

            if (idx == -1 || idx == _template.Headers.Count - 1) { return; }

            var headerToUp = _template.Headers[idx + 1];
            _template.Headers[idx] = headerToUp;
            _template.Headers[idx + 1] = header;

            OnPropertyChanged("Headers");
        }

        public ObservableCollection<TemplateDictViewModel> Dicts
        {
            get
            {
                var ret = new ObservableCollection<TemplateDictViewModel>();
                foreach (var dict in _template.Dicts)
                {
                    ret.Add(new TemplateDictViewModel(_navigation, dict, this));
                }

                return ret;
            }
        }

        public void DeleteHeader(Guid headerId)
        {
            var header = _template.Headers.Where(h => h.Id == headerId).FirstOrDefault();
            _template.Headers.Remove(header);
            OnPropertyChanged("Headers");
        }

        public void DeleteDict(Guid dictId)
        {
            var dict = _template.Dicts.Where(d => d.Id == dictId).FirstOrDefault();
            _template.Dicts.Remove(dict);
            OnPropertyChanged("Dicts");
        }
    
        void SaveThisToFile()
        {
            var saveService = DependencyService.Get<IFileSaverService>();
            var r = saveService.SaveTemplate(_template);

            string[] msq = new string[1];
            if (r != "")
            {
                msq[0] = r;
                
            }
            else
            {
                msq[0] = "Ошибка при сохранении";
            }

            _navigation.PushAsync(new MessagePage(msq));
        }
    }
}
