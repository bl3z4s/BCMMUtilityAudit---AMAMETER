using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Models;
using Microsoft.Maui.Controls;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    public partial class HistoryPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public HistoryPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Fetch saved records from SQLite and bind them to the list UI
            AuditCollectionView.ItemsSource = await _databaseService.GetAuditRecordsAsync();
        }

        private async void OnAuditSelected(object? sender, SelectionChangedEventArgs e)
        {
            // 1. Get the selected item
            var selectedRecord = e.CurrentSelection.FirstOrDefault() as Models.AuditRecord;
            if (selectedRecord == null) return;

            // 2. Clear selection so they can tap it again later
            AuditCollectionView.SelectedItem = null;

            // 3. Navigate to AuditPage passing the data as a query string
            // This looks like: AuditPage?reading=1234&lat=-33.01&lon=27.89
            string route = $"{nameof(AuditPage)}?reading={selectedRecord.MeterReading}&lat={selectedRecord.Latitude}&lon={selectedRecord.Longitude}";
            await Shell.Current.GoToAsync(route);
        }
    }
}