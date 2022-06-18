using CsvHelper;
using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.ViewModels.FIle;
using SamplesJournal_v2.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SamplesJournal_v2.ViewModels.File
{
    public class FileCreateViewModel : BaseViewModel
    {
        readonly FilesViewModel _parentViewModel;
        public ICommand SelectFileCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        FileResult _result;
        List<Template> _templates;

        public FileCreateViewModel(INavigation navigation, FilesViewModel parentViewModel)
            : base(navigation)
        {
            _parentViewModel = parentViewModel;
            SelectFileCommand = new Command(SelectFile);
            CreateCommand = new Command(CerateFile);
            _templates = DataBaseFactory.DataBase.GetTemplates();
        }
        async void SelectFile()
        {
            _result = await FilePicker.PickAsync();
            OnPropertyChanged("SelectedFileName");
        }
        public string SelectedFileName
        {
            get
            {
                string name;
                if (_result == null)
                {
                    name = "Файл не выбран";
                } else
                {
                    name = _result.FileName;
                }

                return name;
            }
        }
        public List<Template> Templates
        {
            get
            {
                return _templates;
            }
        }
        public Template SelectedTemplate { get; set; }
        void CerateFile()
        {
            if (_result == null || SelectedTemplate == null) { return; }

            if (SelectedTemplate.Headers.Count == 0)
            {
                string[] msq = { "Не заданы поля в шаблоне" };
                _navigation.PushAsync(new MessagePage(msq));
                return;
            }

            // 1 - прочитать CSV файл
            var fileRows = new List<EditorFileRow>();
            string[] lines = System.IO.File.ReadAllLines(_result.FullPath);
            var fileHeaders = lines[0].Split(';');

            if (fileHeaders.Length== 0)
            {
                string[] msq = { "Не удалось прочитать заголовки из файла CSV" };
                _navigation.PushAsync(new MessagePage(msq));
                return;
            }

            // На основе полей из шаблона определить индексы полей которые нужно прочитать
            var headerIndexToRead = new Dictionary<int, Guid>();
            for (int i = 0; i < fileHeaders.Length; i++)
            {
                var finded = SelectedTemplate.Headers.Where(h => h.Name == fileHeaders[i]).FirstOrDefault();

                if (finded != null)
                {
                    headerIndexToRead.Add(i, finded.Id);
                }
            }

            // Пробегаемся по файлику
            for (int i = 1; i < lines.Length; i++)
            {
                var row = lines[i].Split(';');
                var fileRow = new EditorFileRow();

                foreach (var headerIndex in headerIndexToRead)
                {
                    fileRow.Values.Add(
                        new EditorFileRowValue()
                        {
                            TemplateHeaderId = headerIndex.Value,
                            Value = row[headerIndex.Key]
                        });
                }

                fileRows.Add(fileRow);
            }

            // 2 - Составить ноды группировок
            var headersToGroup = SelectedTemplate.Headers.Where(h => h.GrupBy == true).ToList();

            var mainRowNode = new EditorFileRowsNode();
            if (headersToGroup.Count != 0)
            {
                MakeNode(headersToGroup, 0, fileRows, mainRowNode);
            }
            else
            {
                mainRowNode.Rows = fileRows;
            }

            // 3 - Создаем файл
            var file = new EditorFile();
            file.Name = _result.FileName;
            file.Template = SelectedTemplate;
            file.Node = mainRowNode;

            _parentViewModel.AddFile(file);
            _navigation.PopAsync();
        }
        void MakeNode(List<TemplateHeader> headers, int headerStartPos, List<EditorFileRow> rows, EditorFileRowsNode rowNode)
        {
            // получаем количество уникальных значений по текущему заголовку и текущему набору строк
            var uniqueRowValues = new HashSet<string>();
            foreach (var row in rows)
            {
                var v = row.Values.Where(r => r.TemplateHeaderId == headers[headerStartPos].Id).FirstOrDefault();
                if (v != null)
                {
                    uniqueRowValues.Add(v.Value.ToString());
                }
            }

            // выбираем все строки с уникальнымы значениями по текущему заголовку
            foreach (var uniqueValue in uniqueRowValues)
            {
                var uniqueValNode = new EditorFileRowsNode();
                uniqueValNode.Name = headers[headerStartPos].Name;
                uniqueValNode.Value = uniqueValue;

                var uniqValueRows = rows
                    .Where(r => r.Values.Any(v => v.Value.ToString() == uniqueValue && v.TemplateHeaderId == headers[headerStartPos].Id))
                    .ToList();
                if (headerStartPos == headers.Count - 1)
                {
                    uniqueValNode.Rows = uniqValueRows;
                }
                else
                {
                    MakeNode(headers, headerStartPos + 1, uniqValueRows, uniqueValNode);
                }

                rowNode.Childs.Add(uniqueValNode);
            }
        }
    }
}
