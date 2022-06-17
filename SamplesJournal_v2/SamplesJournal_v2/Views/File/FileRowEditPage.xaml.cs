using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Services;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System;
using SamplesJournal_v2.ViewModels.File;

namespace SamplesJournal_v2.Views.File
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileRowEditPage : ContentPage
    {
        EditorFileRow _row;
        Template _template;
        List<TemplateHeader> _headersToEdit;
        int _currentHeaderIndex;
        CreateInputViewService _createInputViewService;
        FileRowViewModel _rowViewModel;

        public FileRowEditPage(FileRowViewModel rowViewModel, EditorFileRow row, Template template)
        {
            InitializeComponent();
            _row = row;
            _rowViewModel = rowViewModel;
            _template = template;

            SetTitle();
            SetValueToEdit();

            _currentHeaderIndex = 0;
            _createInputViewService = new CreateInputViewService();

            stackLayout.Children.Add(InitPageForValue(_currentHeaderIndex));
        }

        void SetTitle()
        {
            var headerToShow = _template.Headers.Where(h => h.ShowInEditor == true).FirstOrDefault();
            var valToTitle = _row.Values.Where(v => v.TemplateHeaderId == headerToShow.Id).FirstOrDefault();
            Title = $"{headerToShow.Name}: {valToTitle.Value}";
        }

        void SetValueToEdit()
        {
            _headersToEdit = _template.Headers.Where(h => h.ToEdit == true).ToList();
        }

        Frame InitPageForValue(int headerIndex)
        {
            var retFrame = new Frame();

            var header = _headersToEdit[headerIndex];
            var value = _row.Values.Where(v => v.TemplateHeaderId == header.Id).FirstOrDefault();
            value.Value = header.DefaultValue != null ? header.DefaultValue : value.Value;

            switch (header.InputTool)
            {
                case Models.InputTypeEnum.TextInput:
                    retFrame = _createInputViewService.CreateTextInputView(header.Name, OnAction, OnBackAction, header.NullAble, DisplayAlert, value.Value.ToString());
                    break;

                case Models.InputTypeEnum.NumberInput:
                    retFrame = _createInputViewService.CreateNumberInputView(header.Name, OnAction, OnBackAction, header.NullAble, DisplayAlert, value.Value.ToString());
                    break;

                case Models.InputTypeEnum.DictInput:
                    var dict = _template.Dicts.Where(d => d.Id == header.DictId).FirstOrDefault();

                    if (dict == null)
                    {
                        DisplayAlert("Ошибка", $"Словарь для поля {header.Name} не найден", "Ок");
                        break;
                    }

                    
                    retFrame = _createInputViewService.CreateDictInputView(header.Name, OnAction, OnBackAction, dict, value.Value.ToString());
                    break;

                default:
                    DisplayAlert("Ошибка", "Что то пошло не так", "Ок");
                    break;
            }

            return retFrame;
        }

        void OnAction(object value)
        {
            _rowViewModel.Edited = false;
            stackLayout.Children.Clear();

            var _value = _row.Values
                .Where(v => v.TemplateHeaderId == _headersToEdit[_currentHeaderIndex].Id)
                .FirstOrDefault();

            if (_value != null) { _value.Value = value; };

            if (_currentHeaderIndex != _headersToEdit.Count - 1)
            {
                _currentHeaderIndex++;
                stackLayout.Children.Add(InitPageForValue(_currentHeaderIndex));
            }

            if (_currentHeaderIndex == _headersToEdit.Count - 1)
            {
                _rowViewModel.Edited = true;
                Navigation.PopAsync();
            }
        }

        void OnBackAction()
        {
            if (_currentHeaderIndex > 0)
            {
                stackLayout.Children.Clear();
                _currentHeaderIndex--;
                stackLayout.Children.Add(InitPageForValue(_currentHeaderIndex));
            }
        }
    }
}