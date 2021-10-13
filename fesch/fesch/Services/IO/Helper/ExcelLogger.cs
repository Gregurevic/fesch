using fesch.Services.Storage;
using fesch.Services.Storage.Scheduler;
using fesch.Services.Storage.Scheduler.StructureModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace fesch.Services.IO.Helper
{
    static class ExcelLogger
    {
        public static void LogExamStructure(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage())
            {
                ExcelWorksheet w = excelPackage.Workbook.Worksheets.Add("Elnökök");
                LogData(w);
                w.Cells.AutoFitColumns();
                FileInfo excelLogFile = new FileInfo(path);
                excelPackage.SaveAs(excelLogFile);
            }
        }

        private static void LogData(ExcelWorksheet w) {

            /// headline
            w.Cells[1, 1].Value = "DayIndex";
            w.Cells[2, 1].Value = "ChamberIndex";
            w.Cells[3, 1].Value = "President";
            w.Cells[4, 1].Value = "Secretary";
            w.Cells[5, 1].Value = "Tution";
            for (int i = 1; i <= 14; i++)
            {
                w.Cells[i, 1].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                w.Cells[i, 1].Style.Font.Bold = true;
                w.Cells[i, 1].Style.Font.Size = 14;
            }
            /// data
            int j = 2;
            foreach (Fragment f in Structures.Service.Fragments)
            {
                w.Cells[1, j].Value = f.DayIndex;
                w.Cells[2, j].Value = f.ChamberIndex;
                w.Cells[3, j].Value = DataModels.Service.getInstructor(f.PresidentId).Name;
                w.Cells[4, j].Value = DataModels.Service.getInstructor(f.SecretaryId).Name;
                w.Cells[5, j].Value = f.Tution.ToString();
                j++;
            }
        }
    }
}
