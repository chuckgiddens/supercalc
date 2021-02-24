using SuperCalc.ViewModels;
using SuperCalc.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SuperCalc
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NormalKeyLayoutPage), typeof(NormalKeyLayoutPage));
            Routing.RegisterRoute(nameof(TenKeyLayoutPage), typeof(TenKeyLayoutPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
