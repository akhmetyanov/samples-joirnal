using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.ViewModels.File;
using SamplesJournal_v2.Views;
using SamplesJournal_v2.Views.File;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels.FIle
{
    public class FilesViewModel: BaseViewModel
    {
        List<EditorFile> _files;
        public ICommand GoToCreateFilePageCommand { get; set; }
        public ICommand CreateFromFileCommand { get; set; }

        public FilesViewModel(INavigation navigation)
            : base(navigation)
        {
            _files = DataBaseFactory.DataBase.GetFiles();
            GoToCreateFilePageCommand = new Command(() =>
            {
                _navigation.PushAsync(new FileCreatePage(new FileCreateViewModel(navigation, this)));
            });
            CreateFromFileCommand = new Command( () => CreateFromFile());
        }

        public void AddFile(EditorFile file)
        {
            _files.Add(file);
            DataBaseFactory.DataBase.InsertOrReplaceFile(file);
            OnPropertyChanged("Files");
        }

        public ObservableCollection<FileViewModel> Files 
        { 
            get
            {
                var ret = new ObservableCollection<FileViewModel>();

                foreach (var file in _files)
                {
                    ret.Add(new FileViewModel(_navigation, file, deleteFile));
                }

                return ret;
            }
            set
            {
                _files = new List<EditorFile>();

                foreach (var fileVm in value)
                {
                    _files.Add(fileVm.File);
                }

                OnPropertyChanged();
            }
        }

        void deleteFile(Guid id)
        {
            var fileTpoDel = _files.Where(f => f.Id == id).FirstOrDefault();

            DataBaseFactory.DataBase.DeleteFile(fileTpoDel);
            _files.Remove(fileTpoDel);

            OnPropertyChanged("Files");
        }
        
        async void CreateFromFile()
        {
            var file = await FilePicker.PickAsync();
            var json = System.IO.File.ReadAllText(file.FullPath);

            try
            {
                var editorFile = JsonSerializer.Deserialize<EditorFile>(json);
                await DataBaseFactory.DataBase.InsertOrReplaceFile(editorFile);
                _files.Add(editorFile);
                OnPropertyChanged("Files");
            }
            catch (Exception ex)
            {
                string[] msq = { ex.ToString() };
                _navigation.PushAsync(new MessagePage(msq));
            }
        }
    }
}
