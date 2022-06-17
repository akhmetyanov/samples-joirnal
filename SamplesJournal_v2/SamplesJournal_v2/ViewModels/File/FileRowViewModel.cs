using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Views.File;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels.File
{
    public class FileRowViewModel: BaseViewModel
    {
        EditorFileRow _row;
        Template _template;

        public ICommand OnSelectThis { get; set; }

        public FileRowViewModel(INavigation navigation, EditorFileRow row, Template template) 
            : base(navigation)
        {
            _row = row;
            _template = template;

            OnSelectThis = new Command(() =>
            {
                navigation.PushAsync(new FileRowEditPage(this, row, template));
            });
        }

        public string Name 
        {
            get
            {
                var toShow = _template.Headers.Find(t => t.ShowInEditor == true);
                var value = _row.Values.Find(v => v.TemplateHeaderId == toShow.Id);

                return value.Value.ToString();
            } 
        }

        public bool Edited 
        { 
            get { return _row.Edited; }
            set 
            { 
                _row.Edited = value;
                _row.EditedDate = DateTime.Now;
                OnPropertyChanged("TextColor");
            }
        }

        public string TextColor
        {
            get
            {
                if (_row.Edited)
                {
                    return "Green";
                }
                else
                {
                    return "Red";
                }
            }
        }
    }
}
