using Microsoft.Maui.Controls;
using System;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void OnGetStartedClicked(object sender, EventArgs e)
        {
            Preferences.Default.Set("HasConsented", true);
            await Navigation.PushAsync(new ProfileSetupPage());
        }
    }
}