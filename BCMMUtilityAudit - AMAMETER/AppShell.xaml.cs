using BCMMUtilityAudit___AMAMETER.Views;

namespace BCMMUtilityAudit___AMAMETER
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register all navigation routes here once
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(AuditPage), typeof(AuditPage));
        }
    }
}