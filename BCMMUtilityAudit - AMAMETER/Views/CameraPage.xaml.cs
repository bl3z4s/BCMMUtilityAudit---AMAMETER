using Plugin.Maui.OCR;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices.Sensors;
using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Models;

namespace BCMMUtilityAudit___AMAMETER.Views
{
    public partial class CameraPage : ContentPage
    {
        private readonly IOcrService _ocrService;
        private string _capturedLatitude = string.Empty;
        private string _capturedLongitude = string.Empty;

        public CameraPage(IOcrService ocrService)
        {
            InitializeComponent();
            _ocrService = ocrService;
        }

        private async void OnTakePhotoClicked(object? sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        {
                            using Stream sourceStream = await photo.OpenReadAsync();
                            using FileStream localFileStream = File.Create(localFilePath);
                            await sourceStream.CopyToAsync(localFileStream);
                        }

                        MeterPhoto.Source = ImageSource.FromFile(localFilePath);

                        await GetCurrentLocationAsync();

                        byte[] imageBytes = File.ReadAllBytes(localFilePath);
                        OcrResult ocrResult = await _ocrService.RecognizeTextAsync(imageBytes);

                        if (ocrResult.Success)
                        {
                            ReadingResultEntry.Text = ocrResult.AllText;
                        }
                        else
                        {
                            await DisplayAlert("OCR Alert", "Could not detect clear text on meter. Please enter reading manually.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Camera error: {ex.Message}", "OK");
            }
        }

        private async Task GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    _capturedLatitude = location.Latitude.ToString("F6");
                    _capturedLongitude = location.Longitude.ToString("F6");
                    GpsLocationLabel.Text = $"Lat: {_capturedLatitude}, Long: {_capturedLongitude}";
                }
                else
                {
                    GpsLocationLabel.Text = "Unable to detect location.";
                }
            }
            catch (FeatureNotSupportedException)
            {
                GpsLocationLabel.Text = "GPS not supported on device.";
            }
            catch (PermissionException)
            {
                GpsLocationLabel.Text = "Location permission denied.";
            }
            catch (Exception ex)
            {
                GpsLocationLabel.Text = $"Error: {ex.Message}";
            }
        }

        private async void OnProceedClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReadingResultEntry.Text))
            {
                await DisplayAlert("Required", "Please scan or enter a meter reading.", "OK");
                return;
            }

            var record = new AuditRecord
            {
                MeterReading = ReadingResultEntry.Text,
                Latitude = _capturedLatitude,
                Longitude = _capturedLongitude,
                GpsCoords = $"{_capturedLatitude}, {_capturedLongitude}",
                LocalImagePath = MeterPhoto.Source is FileImageSource fileSource ? fileSource.File : string.Empty,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await DatabaseService.SaveRecordAsync(record);

            await DisplayAlert("Success", "Audit record successfully saved to local database!", "OK");
        }

        private async void OnViewHistoryClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HistoryPage));
        }
    }
}