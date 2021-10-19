using fesch.Services.Storage;
using fesch.Services.Storage.Result;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace fesch.Services.IO.ExcelAction
{
    static class ExcelWriter
    {
        public static void Write(string destination)
        {
            /// document
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet fe = package.Workbook.Worksheets.Add("1. kör");

            /// headline
            fe.Cells[1, 1].Value = "Összesítő";
            fe.Cells[1, 2].Value = "Sorsz";
            fe.Cells[1, 3].Value = "Idő";
            fe.Cells[1, 4].Value = "Név";
            fe.Cells[1, 5].Value = "Neptun";
            fe.Cells[1, 6].Value = "Képzéskód";
            fe.Cells[1, 7].Value = "Konzulens";
            fe.Cells[1, 8].Value = "Vizsgatárgyak";
            fe.Cells[1, 9].Value = "Tanszék";
            fe.Cells[1,10].Value = "Vizsgáztatók";
            fe.Cells[1,11].Value = "Elnök";
            fe.Cells[1,12].Value = "Tag";
            fe.Cells[1,13].Value = "Tag";
            fe.Cells[1,14].Value = "Titkár";
            for (int h = 1; h <= 14; h++)
            {
                fe.Cells[1, h].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                fe.Cells[1, h].Style.Font.Bold = true;
                fe.Cells[1, h].Style.Font.Size = 14;
            }

            /// data
            int i = 2;
            foreach(Finalexam e in Results.Service.Finalexams)
            {
                /// fill row
                fe.Cells[i, 1].Value = e.Summary;
                fe.Cells[i, 2].Value = e.Sequence;
                fe.Cells[i, 3].Value = e.Time;
                fe.Cells[i, 4].Value = e.StudentName;
                fe.Cells[i, 5].Value = e.StudentNeptun;
                fe.Cells[i, 6].Value = e.TutionLevelLanguage;
                fe.Cells[i, 7].Value = e.SupervisorName;
                fe.Cells[i, 8].Value = e.Courses;
                fe.Cells[i, 9].Value = e.Faculty;
                fe.Cells[i, 10].Value = e.Examiners;
                fe.Cells[i, 11].Value = e.President;
                fe.Cells[i, 12].Value = e.Member;
                fe.Cells[i, 13].Value = e.External;
                fe.Cells[i, 14].Value = e.Secretary;
                i++;
            }

            /// borders
            for (int c = 1; c <= fe.Dimension.End.Column; c++)
            {
                for (int r = 2; r <= fe.Dimension.End.Row; r++)
                {
                    if (r % 11 == 1)
                    {
                        fe.Cells[r, c].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    }
                    else
                    {
                        fe.Cells[r, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    fe.Cells[r, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    fe.Cells[r, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            /// merge cells
            int[] merge_columns = new int[] { 1, 11, 12, 13, 14 };
            for (int mc = 0; mc < merge_columns.Length; mc++)
            {
                int c = merge_columns[mc];
                int merge_r_start = 0;
                int merge_r_finish = 0;
                bool merge = false;
                for (int r = 2; r <= fe.Dimension.End.Row; r++)
                {
                    if (fe.Cells[(r - 1), c].Value.ToString().Equals(fe.Cells[r, c].Value.ToString()) && r != fe.Dimension.End.Row && (r % 11 != 2 && r != 2))
                    {
                        if (merge)
                        {
                            merge_r_finish++;
                        }
                        else
                        {
                            merge = true;
                            merge_r_start = r - 1;
                            merge_r_finish = r;
                        }
                    }
                    else
                    {
                        if (r == fe.Dimension.End.Row) merge_r_finish++;
                        if (merge)
                        {
                            fe.Cells[merge_r_start, c, merge_r_finish, c].Merge = true;
                            merge = false;
                            merge_r_start = 0;
                            merge_r_finish = 0;
                        }
                    }
                }
            }

            /// align data center
            for (int c = 1; c <= fe.Dimension.End.Column; c++)
            {
                for (int r = 1; r <= fe.Dimension.End.Row; r++)
                {
                    fe.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fe.Cells[r, c].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            /// colours
            bool fill = false;
            for (int c = 1; c <= fe.Dimension.End.Column; c++)
            {
                for (int r = 2; r < fe.Dimension.End.Row; r++)
                {
                    if (fill)
                    {
                        fe.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        fe.Cells[r, c].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFCC"));
                    }
                    if (r % 11 == 1)
                    {
                        fill = !fill;
                    }
                }
            }

            /// auto-fit
            fe.Cells[fe.Dimension.Address].AutoFitColumns();

            /// save and export
            FileInfo excelFile = new FileInfo(destination);
            package.SaveAs(excelFile);
        }
    }
}
