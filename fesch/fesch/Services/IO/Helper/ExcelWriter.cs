using fesch.Services.Storage;
using fesch.Services.Storage.FinalExam;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace fesch.Services.IO.Helper
{
    static class ExcelWriter
    {
        public static void Write(string destination)
        {
            /// document
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet fe = package.Workbook.Worksheets.Add("final exam");

            /// headline
            fe.Cells[1, 1].Value = "Summary";
            fe.Cells[1, 2].Value = "Date";
            fe.Cells[1, 3].Value = "Room";
            fe.Cells[1, 4].Value = "Seq";
            fe.Cells[1, 5].Value = "Time";
            fe.Cells[1, 6].Value = "Student";
            fe.Cells[1, 7].Value = "Supervisor";
            fe.Cells[1, 8].Value = "Course";
            fe.Cells[1, 9].Value = "Faculty";
            fe.Cells[1,10].Value = "Examiner";
            fe.Cells[1,11].Value = "President";
            fe.Cells[1,12].Value = "Member";
            fe.Cells[1,13].Value = "External";
            fe.Cells[1,14].Value = "Secretary";
            for (int h = 1; h <= 14; h++)
            {
                fe.Cells[1, h].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                fe.Cells[1, h].Style.Font.Bold = true;
                fe.Cells[1, h].Style.Font.Size = 14;
            }

            /// data
            int i = 2;
            foreach(FinalExam e in FinalExams.Service.getExams())
            {
                /// fill row
                fe.Cells[i, 1].Value = e.Summary;
                fe.Cells[i, 2].Value = e.Date;
                fe.Cells[i, 3].Value = e.Room;
                fe.Cells[i, 4].Value = e.Seq;
                fe.Cells[i, 5].Value = e.Time;
                fe.Cells[i, 6].Value = e.Student;
                fe.Cells[i, 7].Value = e.Supervisor;
                fe.Cells[i, 8].Value = e.Course;
                fe.Cells[i, 9].Value = e.Faculty;
                fe.Cells[i, 10].Value = e.Examiner;
                fe.Cells[i, 11].Value = e.President;
                fe.Cells[i, 12].Value = e.Member;
                fe.Cells[i, 13].Value = e.External;
                fe.Cells[i, 14].Value = e.Secretary;
                /// format cells
                if ((i - 1) % 5 == 1)
                {
                    for (int j = 1; j <= 14; j++)
                    {
                        fe.Cells[i, j].Style.Border.Bottom.Style = ((i - 1) % 5 == 2) ? ExcelBorderStyle.Medium : ExcelBorderStyle.Dotted;
                    }
                }
                i++;
            }

            /// auto-fit
            fe.Cells.AutoFitColumns();

            /// save workbook
            FileInfo excelFile = new FileInfo(destination);
            package.SaveAs(excelFile);
        }
    }
}
