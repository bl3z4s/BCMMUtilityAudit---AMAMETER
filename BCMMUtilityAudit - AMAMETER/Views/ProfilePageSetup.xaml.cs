using Microsoft.Maui.Controls;
using Supabase.Gotrue.Mfa;
using System;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    public partial class ProfileSetupPage : ContentPage
    {
        public ProfileSetupPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(AccountEntry.Text))
            {
                await DisplayAlertAsync("Required", "Please fill in your name and account number.", "OK");
                return;
            }

            // Save details persistently on device
            Preferences.Default.Set("UserName", NameEntry.Text);
            Preferences.Default.Set("UserEmail", EmailEntry.Text ?? "");
            Preferences.Default.Set("UserPhone", PhoneEntry.Text ?? "");
            Preferences.Default.Set("DefaultAccount", AccountEntry.Text);
            Preferences.Default.Set("IsUserRegistered", true);

            // Launch the main application shell
            Application.Current.MainPage = new AppShell();
        }
    }
}