using System;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace BCMMUtilityAudit___AMAMETER.Services
{
    public static class PdfGenerator
    {
        public static string GenerateSection95DisputePdf(
            string accountNo,
            string userName,
            string userEmail,
            string userPhone,
            string address,
            string region,
            double billedReading,
            double actualReading,
            string gpsCoords,
            string timestamp)
        {
            var document = new PdfDocument();
            document.Info.Title = "BCMM Section 95 Dispute";

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Arial", 14, XFontStyle.Bold);
            var normalFont = new XFont("Arial", 12, XFontStyle.Regular);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);

            int yPosition = 40;

            // Header
            gfx.DrawString("BUFFALO CITY METROPOLITAN MUNICIPALITY", titleFont, XBrushes.Black, new XRect(0, yPosition, page.Width, page.Height), XStringFormats.TopCenter);
            yPosition += 25;
            gfx.DrawString("SECTION 95 MUNICIPAL SYSTEMS ACT FORMAL DISPUTE", titleFont, XBrushes.Black, new XRect(0, yPosition, page.Width, page.Height), XStringFormats.TopCenter);
            yPosition += 50;

            // User Info
            gfx.DrawString($"Date Generated: {timestamp}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Account Number: {accountNo}", boldFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Account Holder: {userName}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Contact Email: {userEmail}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Contact Phone: {userPhone}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Property Address: {address}, {region}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 40;

            // Readings
            gfx.DrawString("AUDIT READINGS SUMMARY", boldFont, XBrushes.DarkBlue, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Municipal Billed Reading (Estimated): {billedReading}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Physical Verified Reading (Scanned): {actualReading}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"Discrepancy (Over-billed units): {billedReading - actualReading}", boldFont, XBrushes.DarkRed, 40, yPosition);
            yPosition += 40;

            // Evidence
            gfx.DrawString("VERIFICATION EVIDENCE", boldFont, XBrushes.DarkBlue, 40, yPosition);
            yPosition += 20;
            gfx.DrawString($"GPS Coordinates Lock: {gpsCoords}", normalFont, XBrushes.Black, 40, yPosition);
            yPosition += 20;
            gfx.DrawString("Verification Status: Physical Reading Captured via AMAMETER App", normalFont, XBrushes.Black, 40, yPosition);

            // Save File
            string targetFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fileName = $"Section95_Dispute_{accountNo}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(targetFolder, fileName);

            document.Save(filePath);
            return filePath;
        }
    }
}