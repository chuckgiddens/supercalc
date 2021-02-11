using SuperCalc.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SuperCalc.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}