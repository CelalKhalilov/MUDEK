using Microsoft.AspNetCore.Mvc;
using ProjectOfMudek.Context;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;


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

            var header = new Paragraph("Ögrenim Çiktilari")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);
            document.Add(header);

            document.Add(new Paragraph("\n"));

            var table = new Table(new float[] { 1, 1,1 }).UseAllAvailableWidth();
            table.AddHeaderCell(new Cell().Add(new Paragraph("Ogrenim Ciktilari").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Anahtar Kelimeler").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Tanim").SetBold()));
            var i = 1;
            foreach (var user in users)
            {
                table.AddCell(new Cell().Add(new Paragraph("Ö - "+i.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(user.Keywords)));
                table.AddCell(new Cell().Add(new Paragraph(user.Definition)));
                i++;
            }

            document.Add(table);
            document.Close();

            return File(stream.ToArray(), "application/pdf", "OgrenimCiktilari.pdf");
        }
    }

    }
}