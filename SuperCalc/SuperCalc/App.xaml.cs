using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SuperCalc.Resources;
using SuperCalc.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SuperCalc
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Xamarin.Essentials.VersionTracking.Track();
            AppCenter.Start(AppResources.AppCenterMetricsKey, typeof(Analytics), typeof(Crashes));
            _ = SetTheme();

            //we only want one so that it maintains our current value for us...
            DependencyService.RegisterSingleton<ICalcEngine>(new CalculatorSimulator());
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //wire up a delegate to handle Theme changes on the OS level
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                Analytics.TrackEvent("Theme - CHANGED");
                _ = SetTheme();
            };
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #region Theme Management
        private async Task SetTheme()
        {
            //default theme is set by the OS
            string themename = "SYSTEM";
            //has the user ever set a preference?
            themename = await Xamarin.Essentials.SecureStorage.GetAsync("Theme");

            //has user allowed the system to determine the mode?
            if (string.IsNullOrEmpty(themename) || themename == "SYSTEM")
            {
                //set the system decide...this is the default
                OSAppTheme currentTheme = Application.Current.RequestedTheme;
                var theme = AppInfo.RequestedTheme;
                SetThemeByName(currentTheme.ToString());
            }
            else
            {
                //user has set to dark or light manually
                SetThemeByName(themename);
            }
        }

        private void SetThemeByName(string currentTheme)
        {
            //get the resource dictionary so that we can setup what we want in it
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                //clear it out
                mergedDictionaries.Clear();
                //based on the theme set by the phone, load the light or dark mode
                switch (currentTheme)
                {
                    case "Dark":
                        Analytics.TrackEvent("Theme - Dark");
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case "Light":
                    default:
                        Analytics.TrackEvent("Theme - Light");
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
                //get all of our standard control themes back into it
                mergedDictionaries.Add(new StandardControlTheme());
            }
        }
        #endregion
    }
}
