using System.Collections.Generic;
using System.IO;
using AspNetiTextSharp.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;

namespace AspNetiTextSharp.Reports
{
    public class StudentReport
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        public StudentReport(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
        }

        #region Declaration
            int _maxColumn = 3;
            Document _document; 
            Font _fontStyle;
            PdfPTable _pdfTable = new PdfPTable(3);
            PdfPCell _pdfCell;
            MemoryStream _memoryStream = new MemoryStream();
            List<Student> _students = new List<Student>();
        #endregion

        public byte[] Report(List<Student> students)
        {
            _students = students;

            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f,5f, 20f, 5f);
        
            _pdfTable.WidthPercentage = 90;
            _pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter docWrite = PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();

            float[] sizes = new float[_maxColumn];

            for (int i = 0; i < _maxColumn; i++)
            {
                if (i == 0)
                {
                    sizes[i] = 20;
                } else 
                {
                    sizes[i] = 100;
                }
            }

            _pdfTable.SetWidths(sizes);

            this.ReportHeader();
            this.EmpthyRow(2);
            this.ReportBody();

            _pdfTable.HeaderRows = 2;
            
            _document.Add(_pdfTable);
            _document.Close();

            return _memoryStream.ToArray();
        }

        private void ReportHeader()
        {
            _pdfCell = new PdfPCell(this.AddLogo());
            _pdfCell.Colspan = 1;
            _pdfCell.Border = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(this.AddPageTitle());
            _pdfCell.Colspan = 2;
            _pdfCell.Border = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 1;
            PdfPTable pdfTable = new PdfPTable(maxColumn);

            string path = _webHostEnviroment.WebRootPath + "/images";

            string imagePath = Path.Combine(path, "logo.png");

            Image img = Image.GetInstance(imagePath);
            img.ScaleAbsolute(120f, 120f);

            _pdfCell = new PdfPCell(img);
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;

            pdfTable.AddCell(_pdfCell);
            pdfTable.CompleteRow();

            return pdfTable;
        }
    
        private PdfPTable AddPageTitle()
        {
            int maxColumn = 3;
            PdfPTable pdfTable = new PdfPTable(maxColumn);

            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase("Student Information", _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            
            pdfTable.AddCell(_pdfCell);
            pdfTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase("School Information", _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            
            pdfTable.AddCell(_pdfCell);
            pdfTable.CompleteRow();

            return pdfTable;

        }

        private void EmpthyRow(int nCount)
        {
            for (int i = 0; i < nCount; i++)
            {
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            }
        }
    
        private void ReportBody()
        {
            var fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            #region Detail Tab Header
            _pdfCell = new PdfPCell(new Phrase("Id", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Name", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Address", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);
            #endregion

            #region Detail Table Body
            foreach (var student in _students)
            {
                _pdfCell = new PdfPCell(new Phrase(student.Id.ToString(), _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(student.Name, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(student.Address, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfTable.AddCell(_pdfCell);   

                _pdfTable.CompleteRow();
            }
            #endregion

        }
    }
}