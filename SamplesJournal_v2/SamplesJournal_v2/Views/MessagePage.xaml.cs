using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SamplesJournal_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePage : ContentPage
    {
        string[] _msqs;
        public MessagePage(string[] msqs)
        {
            InitializeComponent();
            BindingContext = this;
            _msqs = msqs;
            setValues();
        }

        void setValues()
        {
            var stackLayout = new StackLayout();

            foreach (var msq in _msqs)
            {
                stackLayout.Children.Add(new Label() { Text = msq});
            }

           scrollView.Content = stackLayout;
        }
    }
}