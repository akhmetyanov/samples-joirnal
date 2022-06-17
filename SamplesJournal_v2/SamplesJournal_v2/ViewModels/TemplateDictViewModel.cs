using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels
{
    public class TemplateDictViewModel: BaseViewModel
    {
        TemplateDict _templateDict;
        public ICommand EditDictCommand { get; set; }
        public ICommand AddNewValueCommand { get; set; }
        public ICommand DeleteDictCommand { get; set; }
        public ICommand DeleteSelectedValue { get; set; }
        public TemplateDictViewModel(INavigation navigation, TemplateDict templateDict, TemplateViewModel parentViewModel)
            : base(navigation)
        {
            _templateDict = templateDict;

            //selectedValue = new TemplateDictValueViewModel(_navigation, _templateDict.Values[0]);

            EditDictCommand = new Command(() => _navigation.PushAsync(new TemplateDictPage(this)));
            DeleteDictCommand = new Command(() => { parentViewModel.DeleteDict(_templateDict.Id); _navigation.PopAsync(); });
            AddNewValueCommand = new Command(() => {
                templateDict.Values.Add(new TemplateDictValue());
                OnPropertyChanged("Values");
            });
            DeleteSelectedValue = new Command(() =>
            {
                if (selectedValue == null) { return; }
                var dictToDel = _templateDict.Values.Where(d => d.Id == selectedValue.Id).FirstOrDefault();
                _templateDict.Values.Remove(dictToDel);
                OnPropertyChanged("Values");
            });
        }

        public TemplateDict Dict { get { return _templateDict; }}
        public string Name 
        { 
            get
            {
                return _templateDict.Name;
            }
            set
            {
                _templateDict.Name = value;
                OnPropertyChanged();
            }
        }

        public Guid Id
        {
            get
            {
                return _templateDict.Id;
            }
        }

        public ObservableCollection<TemplateDictValueViewModel> Values
        {
            get
            {
                var ret = new ObservableCollection<TemplateDictValueViewModel>();
                foreach (var val in _templateDict.Values)
                {
                    ret.Add(new TemplateDictValueViewModel(_navigation, val));
                }
                return ret;
            }
        }

        TemplateDictValueViewModel selectedValue;

        public TemplateDictValueViewModel SelectedValue 
        { 
            get 
            {
                return selectedValue; 
            }    
            set 
            { 
                selectedValue = value; 
                OnPropertyChanged();
            }
        }
    }
}
