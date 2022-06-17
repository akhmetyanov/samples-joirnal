using SamplesJournal_v2.Models;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels
{
    public class TemplateHeaderViewModel : BaseViewModel
    {
        TemplateHeader _header;
        TemplateViewModel _parentViewModel;
        public ICommand EditHeaderCommand { get; set; }
        public ICommand UpPositionInListHeaderCommand { get; set; }
        public ICommand DownPositionInListHeaderCommand { get; set; }
        public ICommand DelteHeaderCommand { get; set; }
        public ICommand SetDictPageOpenCommand { get; set; }
        public TemplateHeaderViewModel(INavigation navigation, TemplateHeader header, TemplateViewModel parentViewModel)
            : base(navigation)
        {
            _header = header;
            _parentViewModel = parentViewModel;

            EditHeaderCommand = new Command(() => { _navigation.PushAsync(new TemplateHeaderPage(this)); });
            UpPositionInListHeaderCommand = new Command(() =>
            {
                parentViewModel.SortHeaderListUpHeader(Id);
                _navigation.PopAsync();
            });
            DownPositionInListHeaderCommand = new Command(() =>
            {
                parentViewModel.SortHeaderListDownHeader(Id);
                _navigation.PopAsync();
            });
            DelteHeaderCommand = new Command(() => { parentViewModel.DeleteHeader(_header.Id); _navigation.PopAsync(); });
            SetDictPageOpenCommand = new Command(() =>
            {
                _navigation.PushAsync(new TemplateHeaderSelectDictPage(this));
            });
        }

        public Guid Id { get { return _header.Id; } }

        public string Name
        {
            get
            {
                return _header.Name;
            }
            set
            {
                _header.Name = value;
                OnPropertyChanged();
            }
        }

        public string DefaultValue
        {
            get
            {
                return _header.DefaultValue;
            }
            set
            {
                _header.DefaultValue = value;
                OnPropertyChanged();
            }
        }

        public bool CanSetDefault 
        { 
            get
            {
                switch(_header.InputTool)
                {
                    case InputTypeEnum.NumberInput:
                        return true;
                    case InputTypeEnum.TextInput:
                        return true;
                    default:
                        return false;
                }
            } 
        }

        public string SelectedDictName
        {
            get
            {
                var dict = _parentViewModel.Dicts.Where(d => d.Id == _header.DictId).FirstOrDefault();

                if (dict == null)
                {
                    return "Не определен";
                }

                return dict.Name;
            }
        }

        public ObservableCollection<TemplateDictViewModel> Dicts
        {
            get { return _parentViewModel.Dicts; }
        }

        public TemplateDictViewModel SlectedDict 
        { 
            get
            {
                return _parentViewModel.Dicts.Where(d => d.Dict.Id == _header.DictId).FirstOrDefault();
            } 
            set
            {
                if (value == null) { return; }
                _header.DictId = value.Id;
                OnPropertyChanged("SelectedDictName");
                _navigation.PopAsync();
            }
        }

        public List<string> InputTypes
        {
            get
            {
                var t = Enum.GetNames(typeof(InputTypeEnum)).ToList();
                return t;
            }
        }

        public string SelectedInputTool
        {
            get
            {
                var v = _header.InputTool.ToString();
                return v;
            }
            set
            {
                InputTypeEnum tool = (InputTypeEnum)Enum.Parse(typeof(InputTypeEnum), value);
                _header.InputTool = tool;
                OnPropertyChanged("CanSetDefault");
                OnPropertyChanged("CanSetDict");
            }
        }

        public bool CanSetDict
        {
            get
            {
                switch (_header.InputTool)
                {
                    case InputTypeEnum.DictInput:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool ToEdit
        {
            get
            {
                return _header.ToEdit;
            }
            set
            {
                if (value)
                {
                    _header.ShowInEditor = false;
                    OnPropertyChanged("ShowInEditor");

                    _header.GrupBy = false;
                    OnPropertyChanged("GrupBy");

                    _header.NullAble = false;
                    OnPropertyChanged("NullAble");

                }
                _header.ToEdit = value;
            }
        }
        
        public bool NullAble 
        { 
            get
            {
                return !_header.NullAble;
            } 
            set
            {
                _header.NullAble = !value;
            }
        }

        public bool GrupBy
        {
            get
            {
                return _header.GrupBy;
            }
            set
            {
                // Еси поле установлено как для группировки, то редакетировать это поле нельзя.
                if (value)
                {
                    _header.ToEdit = false;
                    OnPropertyChanged("ToEdit");

                    _header.NullAble = false;
                    OnPropertyChanged("NullAble");
                }

                _header.GrupBy = value;
            }
        }

        public bool ShowInEditor
        {
            get
            {
                return _header.ShowInEditor;
            }
            set
            {
                // Только одно поле может быть установлено как для отображения в Editor
                foreach(var header in _parentViewModel.Headers)
                {
                    header._header.ShowInEditor = false;
                }

                // Еси поле установлено как для отображения в Editor, то редакетировать это поле нельзя.

                if (value)
                {
                    _header.ToEdit = false;
                    OnPropertyChanged("ToEdit");

                    _header.NullAble = false;
                    OnPropertyChanged("NullAble");
                } 

                _header.ShowInEditor = value;
            }
        }
    }
}
