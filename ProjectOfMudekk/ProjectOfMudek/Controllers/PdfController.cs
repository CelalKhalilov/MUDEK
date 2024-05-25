using Microsoft.AspNetCore.Mvc;
using ProjectOfMudek.Context;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font;


namespace ProjectOfMudek.Controllers
{
    public class PdfController : Controller
    {
        private readonly MudekContext _context;

        public PdfController(MudekContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GeneratePdf()
        {
            var users = _context.learningOutcomess.ToList();

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "FreeSans.ttf");
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, true);

                 // Logo ekleme
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jpg", "Samsun.png");
                ImageData imageData = ImageDataFactory.Create(logoPath);
                Image logo = new Image(imageData).ScaleToFit(100, 100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(logo);

                var header = new Paragraph("Öğrenim Çıktıları")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetFont(font);
                document.Add(header);

                document.Add(new Paragraph("\n"));

                var table = new Table(new float[] { 1, 1,1 }).UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Oğrenim Çıktıları").SetBold().SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Anahtar Kelimeler").SetBold().SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tanım").SetBold().SetFont(font)));
                var i = 1;
                foreach (var user in users)
                {
                    table.AddCell(new Cell().Add(new Paragraph("Ö - "+i.ToString()).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph(user.Keywords).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph(user.Definition).SetFont(font)));
                    i++;
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "OgrenimCiktilari.pdf");
            }
        }

    }
}