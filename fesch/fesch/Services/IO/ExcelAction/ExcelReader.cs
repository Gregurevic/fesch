﻿using fesch.Services.Storage;
using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace fesch.Services.IO.ExcelAction
{
    static class ExcelReader
    {
        public static void Read(string source, bool secondaryExaminersNeeded = false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(new System.IO.FileInfo(source)))
            {
                /// fill DataModels with atomic data
                ReadAtomicData(excelPackage);
                /// fill DataModels with parttakers of finals
                UploadStudents(excelPackage);
                UploadInstructors(excelPackage);
                UploadCourses(excelPackage);
                UploadExaminers(excelPackage);
                if (secondaryExaminersNeeded)
                    UploadSecondaryExaminers(excelPackage);
            }
        }

        private static void ReadAtomicData(ExcelPackage excelPackage)
        {
            ExcelWorksheet atomicData = excelPackage.Workbook.Worksheets["Útmutató"];
            DataModels.Service.TimeLimit = atomicData.Cells[3, 4].Value.ToString().Equals("igen");
            DataModels.Service.DayCount = atomicData.Cells[5, 4].GetValue<int>();
            /// the attribute is called FirstDay because Storage/Structure.cs will calculate that
            /// from its current value, which is: the final day of finals minus two days
            /// two days are deducted, because we schedule no exams on those days
            DataModels.Service.FirstDay = new DateTime(atomicData.Cells[7, 5].GetValue<int>(), atomicData.Cells[7, 6].GetValue<int>(), atomicData.Cells[7, 7].GetValue<int>() - 2);
        }

        private static void UploadExaminers(ExcelPackage excelPackage, string worksheetName = "Vizsgáztatók")
        {
            ExcelWorksheet examiners = excelPackage.Workbook.Worksheets[worksheetName];
            /// examiners
            int rowInit = 3;
            int col = 2;
            int row = rowInit;
            while (col <= examiners.Dimension.End.Column)
            {
                DataModels.Service.Instructors.Find(i => i.Neptun.Match(new Neptun(examiners.Cells[row, col].Text)))
                    .AddCourse(new CourseNeptun(examiners.Cells[2, col - 1].Text));
                if (examiners.Cells[(row + 1), col].Value != null)
                {
                    row++;
                }
                else
                {
                    row = rowInit;
                    /// merged cells above
                    col+=2;
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
            int colInit = 1;
            /// merged cells
            for (int col = colInit; col <= courses.Dimension.End.Column; col+=2)
            {
                DataModels.Service.Courses.Add(new Course(
                    col - colInit,
                    courses.Cells[1, col].Text,
                    courses.Cells[2, col].Text
                ));
            }
        }

        private static void UploadInstructors(ExcelPackage excelPackage)
        {
            ExcelWorksheet instructor = excelPackage.Workbook.Worksheets["Elérhetőségek"];
            /// instructors
            int rowInit = 3;
            for (int row = rowInit; row <= instructor.Dimension.End.Row; row++)
            {
                /// tution
                List<Tution> tutions = new List<Tution>();
                if (instructor.Cells[row, 8].Value != null) tutions.Add(Tution.INFO);
                if (instructor.Cells[row, 9].Value != null) tutions.Add(Tution.VILL);
                /// availability
                List<bool> availability = new List<bool>();
                for (int col = 12; col <= instructor.Dimension.End.Column; col++)
                {
                    availability.Add(instructor.Cells[row, col].Value != null);
                }
                /// general information
                DataModels.Service.Instructors.Add(new Instructor(
                    // Id
                    row - rowInit,
                    /// Name
                    instructor.Cells[row, 1].Text,
                    ///Neptun
                    instructor.Cells[row, 3].Text,
                    /// President
                    instructor.Cells[row, 5].Value != null,
                    /// Secretary
                    instructor.Cells[row, 7].Value != null,
                    /// Member
                    instructor.Cells[row, 6].Value != null,
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
            int rowInit = 2;
            for (int row = rowInit; row <= student.Dimension.End.Row; row++)
            {
                DataModels.Service.Students.Add(new Student(
                    /// Id
                    row - rowInit,
                    /// Name
                    student.Cells[row, 9].Text,
                    /// Neptun
                    student.Cells[row, 10].Text,
                    /// Level
                    student.Cells[row, 5].Text,
                    /// Language
                    student.Cells[row, 6].Text,
                    /// Tution
                    student.Cells[row, 7].Text,
                    /// Supervisor
                    student.Cells[row, 13].Text,
                    /// FirstCourse
                    student.Cells[row, 18].Text,
                    /// SecondCourse
                    student.Cells[row, 15].Text
                ));
            }
        }
    }
}
