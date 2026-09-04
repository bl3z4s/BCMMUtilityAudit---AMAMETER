using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Web;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    [QueryProperty(nameof(PassedAccountNo), "accountno")]
    [QueryProperty(nameof(PassedLatitude), "lat")]
    [QueryProperty(nameof(PassedLongitude), "lon")]
    [QueryProperty(nameof(PassedReading), "reading")]
    public partial class AuditPage : ContentPage, IQueryAttributable
    {
        public string? PassedAccountNo { get; set; }
        public string? PassedLatitude { get; set; }
        public string? PassedLongitude { get; set; }
        public string? PassedReading { get; set; }

        public AuditPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Auto-fill profile fields from saved device preferences if empty
            if (string.IsNullOrWhiteSpace(UserNameEntry?.Text))
                UserNameEntry.Text = Preferences.Default.Get("UserName", string.Empty);

            if (string.IsNullOrWhiteSpace(UserEmailEntry?.Text))
                UserEmailEntry.Text = Preferences.Default.Get("UserEmail", string.Empty);

            if (string.IsNullOrWhiteSpace(UserPhoneEntry?.Text))
                UserPhoneEntry.Text = Preferences.Default.Get("UserPhone", string.Empty);

            if (string.IsNullOrWhiteSpace(AccountNoEntry?.Text))
                AccountNoEntry.Text = Preferences.Default.Get("DefaultAccount", string.Empty);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("accountno"))
            {
                PassedAccountNo = HttpUtility.UrlDecode(query["accountno"].ToString());
                if (AccountNoEntry != null && PassedAccountNo != null)
                    AccountNoEntry.Text = PassedAccountNo;
            }

            if (query.ContainsKey("reading"))
            {
                PassedReading = HttpUtility.UrlDecode(query["reading"].ToString());
                if (ActualReadingEntry != null && PassedReading != null)
                    ActualReadingEntry.Text = PassedReading;
            }

            if (query.ContainsKey("lat"))
                PassedLatitude = HttpUtility.UrlDecode(query["lat"].ToString());

            if (query.ContainsKey("lon"))
                PassedLongitude = HttpUtility.UrlDecode(query["lon"].ToString());
        }

        private async void OnGenerateDisputeClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AccountNoEntry?.Text) ||
                string.IsNullOrWhiteSpace(BilledReadingEntry?.Text) ||
                string.IsNullOrWhiteSpace(ActualReadingEntry?.Text))
            {
                await DisplayAlertAsync("Missing Info", "Please fill in the Account Number and both Readings.", "OK");
                return;
            }

            if (!double.TryParse(BilledReadingEntry.Text, out double billed) ||
                !double.TryParse(ActualReadingEntry.Text, out double actual))
            {
                await DisplayAlertAsync("Error", "Please enter valid numeric readings.", "OK");
                return;
            }

            try
            {
                string gpsCoords = (!string.IsNullOrEmpty(PassedLatitude) && !string.IsNullOrEmpty(PassedLongitude))
                    ? $"{PassedLatitude}, {PassedLongitude}"
                    : "-33.0153, 27.8927";

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                string pdfPath = PdfGenerator.GenerateSection95DisputePdf(
                    accountNo: AccountNoEntry.Text,
                    userName: string.IsNullOrWhiteSpace(UserNameEntry?.Text) ? "Resident" : UserNameEntry.Text,
                    userEmail: UserEmailEntry?.Text ?? "",
                    userPhone: UserPhoneEntry?.Text ?? "",
                    address: AddressEntry?.Text ?? "Buffalo City Metropolitan Municipality",
                    region: "East London",
                    billedReading: billed,
                    actualReading: actual,
                    gpsCoords: gpsCoords,
                    timestamp: timestamp
                );

                // Create the audit record object
                var record = new AuditRecord
                {
                    AccountNo = AccountNoEntry.Text,
                    UserName = UserNameEntry?.Text ?? "Resident",
                    BilledReading = billed,
                    ActualReading = actual,
                    GpsCoords = gpsCoords,
                    Timestamp = timestamp,
                    PdfPath = pdfPath
                };

                // Save record using your existing DatabaseService
                await DatabaseService.SaveRecordAsync(record);

                bool shareNow = await DisplayAlertAsync(
                    "Dispute Ready! 📄",
                    $"Your Section 95 dispute document has been compiled and saved successfully.\n\nWould you like to share or email it now?",
                    "Share PDF",
                    "Close"
                );

                if (shareNow)
                {
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = "Section 95 BCMM Dispute",
                        File = new ShareFile(pdfPath)
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("PDF Error", $"Failed to generate document: {ex.Message}", "OK");
            }
        }
    }
}