using PdfSharpCore.Fonts;
using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Views;

namespace BCMMUtilityAudit___AMAMETER
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Register custom font resolver for PDF creation on mobile
            GlobalFontSettings.FontResolver = new TableFontResolver();

            // Check if the user has completed registration before
            bool isRegistered = Preferences.Default.Get("IsUserRegistered", false);

            if (isRegistered)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new WelcomePage());
            }
        }
    }
}