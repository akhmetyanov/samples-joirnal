using SamplesJournal_v2.Models.Template;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SamplesJournal_v2.Services
{
    public class CreateInputViewService
    {
        public Frame CreateTextInputView(
            string fieldName, 
            Action<object> onOkAction, 
            Action onBackAction, 
            bool notEmpty, 
            Func<string, string, string, Task> onAlert, 
            string currentValOrDefault)
        {
            var frame = new Frame();
            var frameStackLayout = new StackLayout();

            frameStackLayout.Children.Add(new Label() { Text = fieldName });
            var textEntry = new Entry() { Text = currentValOrDefault };
            frameStackLayout.Children.Add(textEntry);

            var nextButton = new Button() { Text = "Далее" };
            nextButton.Clicked += (object sender, EventArgs e) =>
            {
                if (notEmpty == true && (textEntry.Text == "" | textEntry.Text == null))
                {
                    onAlert.Invoke("Пустое поле", "Пустое значение запрещено", "Ок");
                    return;
                }
                onOkAction?.Invoke(textEntry.Text);
            };
            var backButtom = new Button() { Text = "Назад" };
            backButtom.Clicked += (object sender, EventArgs e) =>
            {
                onBackAction?.Invoke();
            };

            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(backButtom, 0, 0);
            grid.Children.Add(nextButton, 1, 0);

            frameStackLayout.Children.Add(grid);

            frame.Content = frameStackLayout;

            return frame;
        }

        public Frame CreateNumberInputView(
            string fieldName, 
            Action<object> onOkAction, 
            Action onBackAction, 
            bool notEmpty, 
            Func<string, string, string, Task> onAlert, 
            string currentValOrDefault)
        {
            var frame = new Frame();
            var frameStackLayout = new StackLayout();

            frameStackLayout.Children.Add(new Label() { Text = fieldName });
            var numberEntery = new Entry() { Text = currentValOrDefault };
            frameStackLayout.Children.Add(numberEntery);

            var nextButton = new Button() { Text = "Далее" };
            nextButton.Clicked += (object sender, EventArgs e) =>
            {
                if (notEmpty == true && (numberEntery.Text == "" | numberEntery.Text == null))
                {
                    onAlert.Invoke("Пустое поле", "Пустое значение запрещено", "Ок");
                    return;
                }

                double output;
                if (double.TryParse(numberEntery.Text, out output))
                {
                    onOkAction?.Invoke(output);
                }
                else
                {
                    onAlert?.Invoke("Ошибка преобразования", "Введено некорректное значение", "Ок");
                }
                
            };
            var backButtom = new Button() { Text = "Назад" };
            backButtom.Clicked += (object sender, EventArgs e) =>
            {
                onBackAction?.Invoke();
            };

            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(backButtom, 0, 0);
            grid.Children.Add(nextButton, 1, 0);

            frameStackLayout.Children.Add(grid);

            frame.Content = frameStackLayout;

            return frame;
        }

        public Frame CreateDictInputView(
            string fieldName,
            Action<object> onOkAction,
            Action onBackAction,
            TemplateDict dict,
            string currentValOrDefault)
        {
            var frame = new Frame();
            var scrollView = new ScrollView();
            var stackLayout = new StackLayout();

            stackLayout.Children.Add(new Label() { Text = fieldName });

            var dictFrame = new Frame();
            var dictStackLayout = new StackLayout();
            foreach (var dictValue in dict.Values)
            {
                var button = new Button() { Text = dictValue.Value };

                if (dictValue.Code == currentValOrDefault)
                {
                    button.Background = new SolidColorBrush(Color.Red);
                }

                button.Clicked += (object sender, EventArgs e) =>
                {
                    onOkAction?.Invoke(dictValue.Code);
                };
                dictStackLayout.Children.Add(button);
            }
            dictFrame.Content = dictStackLayout;

            stackLayout.Children.Add(dictFrame);

            var backButtom = new Button() { Text = "Назад" };
            backButtom.Clicked += (object sender, EventArgs e) =>
            {
                onBackAction?.Invoke();
            };

            stackLayout.Children.Add(backButtom);

            scrollView.Content = stackLayout;
            frame.Content = scrollView;
            return frame;
        }
    }
}
