﻿using fesch.Services.Storage;
using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using OfficeOpenXml;
using System.Collections.Generic;

namespace fesch.Services.IO.Helper
{
    static class ExcelReader
    {
        public static void Read(string source, bool secondaryExaminersNeeded = false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(new System.IO.FileInfo(source)))
            {
                UploadStudents(excelPackage);
                UploadInstructors(excelPackage);
                UploadCourses(excelPackage);
                UploadExaminers(excelPackage);
                if (secondaryExaminersNeeded)
                    UploadSecondaryExaminers(excelPackage);
            }
        }

        private static void UploadExaminers(ExcelPackage excelPackage, string worksheetName = "Vizsgáztatók")
        {
            ExcelWorksheet examiners = excelPackage.Workbook.Worksheets[worksheetName];
            /// examiners
            int rowInit = 2;
            int col = 0;
            int row = rowInit;
            while (col <= examiners.Dimension.End.Column)
            {
                DataModels.Service.FindInstructor(new Neptun(examiners.Cells[row, col].Text)).AddCourse(new CourseNeptun(examiners.Cells[1, col].Text));
                if (examiners.Cells[(row + 1), col].Value != null)
                {
                    row++;
                }
                else
                {
                    row = rowInit;
                    col++;
                }
            }
        }

        private static void UploadSecondaryExaminers(ExcelPackage excelPackage)
        {
            UploadExaminers(excelPackage, "Másodlagos Vizsgáztatók");
        }

        private static void UploadCourses(ExcelPackage excelPackage)
        {
            ExcelWorksheet courses = excelPackage.Workbook.Worksheets["Vizsgáztatók"];
            /// courses
            int colInit = 0;
            for (int col = colInit; col <= courses.Dimension.End.Column; col++)
            {
                DataModels.Service.addCourse(new Course(
                    col - colInit,
                    courses.Cells[0, col].Text,
                    courses.Cells[1, col].Text
                ));
            }
        }

        private static void UploadInstructors(ExcelPackage excelPackage)
        {
            ExcelWorksheet instructor = excelPackage.Workbook.Worksheets["Elérhetőségek"];
            /// instructors
            int rowInit = 2;
            for (int row = rowInit; row <= instructor.Dimension.End.Row; row++)
            {
                /// tution
                List<Tution> tutions = new List<Tution>();
                if (instructor.Cells[row, 7].Value != null) tutions.Add(Tution.mérnökinformatikus);
                if (instructor.Cells[row, 8].Value != null) tutions.Add(Tution.villamosmérnöki);
                /// availability
                List<bool> availability = new List<bool>();
                for (int col = 11; col <= instructor.Dimension.End.Column; col++)
                {
                    availability.Add(instructor.Cells[row, col].Value != null);
                }
                /// general information
                DataModels.Service.addInstructor(new Instructor(
                    // Id
                    row - rowInit,
                    /// Name
                    instructor.Cells[row, 0].Text,
                    ///Neptun
                    instructor.Cells[row, 2].Text,
                    /// President
                    instructor.Cells[row, 4].Value != null,
                    /// Secretary
                    instructor.Cells[row, 6].Value != null,
                    /// Member
                    instructor.Cells[row, 5].Value != null,
                    /// Tutions
                    tutions,
                    /// Availability
                    availability
                ));
            }
        }

        private static void UploadStudents(ExcelPackage excelPackage)
        {
            ExcelWorksheet student = excelPackage.Workbook.Worksheets["Véd"];
            // students
            int rowInit = 1;
            for (int row = rowInit; row <= student.Dimension.End.Row; row++)
            {
                DataModels.Service.addStudent(new Student(
                    /// Id
                    row - rowInit,
                    /// Name
                    student.Cells[row, 8].Text,
                    /// Neptun
                    student.Cells[row, 9].Text,
                    /// Level
                    student.Cells[row, 4].Text,
                    /// Language
                    student.Cells[row, 5].Text,
                    /// Tution
                    student.Cells[row, 6].Text,
                    /// Supervisor
                    student.Cells[row, 12].Text,
                    /// FirstCourse
                    student.Cells[row, 14].Text,
                    /// SecondCourse
                    student.Cells[row, 17].Text
                ));
            }
        }
    }
}
