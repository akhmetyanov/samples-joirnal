using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Services;
using SamplesJournal_v2.Views;
using SamplesJournal_v2.Views.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels.File
{
    public class FileViewModel: BaseViewModel
    {
        EditorFile _file;
        public ICommand SelectThisCommand { get; set; }
        public ICommand ThisToEditeCommand { get; set; }
        public ICommand DeleteThisCommand { get; set; }
        public ICommand UpdateTemplateCommand { get; set; }
        public ICommand SaveThisCommand { get; set; }
        public ICommand SaveThisToFileCommand { get; set; }
        public ICommand SaveThisToCsvFileCommand { get; set; }

        public FileViewModel(INavigation navigation, EditorFile file, Action<Guid> deleteThisAction)
            : base(navigation)
        {
            _file = file;
            SelectThisCommand = new Command(() => navigation.PushAsync(new FilePage(this)));
            ThisToEditeCommand = new Command(onToEdit);
            UpdateTemplateCommand = new Command(updateTemplate);

            SaveThisCommand = new Command(() => 
            {
                DataBaseFactory.DataBase.InsertOrReplaceFile(_file);
                navigation.PopAsync();
            });

            DeleteThisCommand = new Command(() =>
            {
                deleteThisAction?.Invoke(_file.Id);
                navigation.PopAsync();
            });

            SaveThisToFileCommand = new Command(SaveThisToFile);
            SaveThisToCsvFileCommand = new Command(SaveThisToScvFile);
        }

        void onToEdit()
        {
            if (_file.Node.Childs.Count != 0)
            {
                _navigation.PushAsync(new FileGroupPage(_file.Node.Childs, _file.Template));
            }
        }

        void updateTemplate()
        {
            var template = DataBaseFactory.DataBase.GetTemplates().Where(t => t.Id == _file.Template.Id).FirstOrDefault();
            if (template != null) 
            { 
                _file.Template = template;
                string[] msq = { "Шаблон обновлен" };
                _navigation.PushAsync(new MessagePage(msq));
            }
        }

        public EditorFile File { get { return _file; } }
        public string Name { get { return _file.Name; } }

        void SaveThisToFile()
        {
            var saveService = DependencyService.Get<IFileSaverService>();
            var r = saveService.SaveFile(_file);

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

        void SaveThisToScvFile()
        {
            var saveService = DependencyService.Get<IFileSaverService>();
            var r = saveService.SaveFileToScv(_file);

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
