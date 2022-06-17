using SamplesJournal_v2.ViewModels;
using Xamarin.Forms;

namespace SamplesJournal_v2
{
    public static class TemplatesViewModelFactory
    {
        static TemplatesViewModel vm;

        public static TemplatesViewModel GetViewModel(INavigation navigation)
        { 
            if (vm == null)
            {
                vm = new TemplatesViewModel(navigation);
            }

            return vm;
        }
    }
}
