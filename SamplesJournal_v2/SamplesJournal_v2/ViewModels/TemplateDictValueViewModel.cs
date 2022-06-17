using SamplesJournal_v2.Models.Template;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels
{
    public class TemplateDictValueViewModel: BaseViewModel
    {
        TemplateDictValue _value;
        public TemplateDictValueViewModel(INavigation navigatiom, TemplateDictValue value)
            : base(navigatiom)
        {
            _value = value;
        }

        public Guid Id { get { return _value.Id; } }

        public string Value 
        { 
            get
            {
                return _value.Value;
            } 
            set
            {
                _value.Value = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get
            {
                return _value.Code;
            }
            set
            {
                _value.Code = value;
                OnPropertyChanged();
            }
        }
    }
}
