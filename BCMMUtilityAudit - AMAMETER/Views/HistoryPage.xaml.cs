using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Models;
using Microsoft.Maui.Controls;
using System.Linq;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Fetch saved records using your static DatabaseService method
            AuditCollectionView.ItemsSource = await DatabaseService.GetHistoryAsync();
        }

        private async void OnAuditSelected(object? sender, SelectionChangedEventArgs e)
        {
            var selectedRecord = e.CurrentSelection.FirstOrDefault() as AuditRecord;
            if (selectedRecord == null) return;

            AuditCollectionView.SelectedItem = null;

            string route = $"{nameof(AuditPage)}?reading={selectedRecord.MeterReading}&lat={selectedRecord.Latitude}&lon={selectedRecord.Longitude}&accountno={selectedRecord.AccountNo}";
            await Shell.Current.GoToAsync(route);
        }
    }
}