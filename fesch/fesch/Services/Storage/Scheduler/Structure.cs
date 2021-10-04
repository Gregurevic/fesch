﻿using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.StructureModel;
using System;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler
{
    public class Structure
    {
        private static Structure instance = null;
        public static Structure Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Structure();
                }
                return instance;
            }
        }
        public int DimensionD { get; set; }
        public int DimensionC { get; set; }
        public int TutionInfoCount { get; set; }
        public List<StructureInstructor> Instructors { get; set; }
        public Structure()
        {
            /// calculate Days' Dimension
            double longExamInfoCount = DataModels.Service.getStudents().FindAll(
                s => (s.Language == Language.angol || s.Level == Level.MSc) && s.Tution == Tution.mérnökinformatikus).Count;
            double longExamVillanyCount = DataModels.Service.getStudents().FindAll(
                s => (s.Language == Language.angol || s.Level == Level.MSc) && s.Tution == Tution.villamosmérnöki).Count;
            double shortExamInfoCount = DataModels.Service.getStudents().FindAll(s => s.Tution == Tution.mérnökinformatikus).Count - longExamInfoCount;
            double shortExamVillanyCount = DataModels.Service.getStudents().FindAll(s => s.Tution == Tution.villamosmérnöki).Count - longExamVillanyCount;
            double days = Math.Ceiling(longExamInfoCount / 8) + Math.Ceiling(longExamVillanyCount / 8) + 
                Math.Ceiling(shortExamInfoCount / 10) + Math.Ceiling(shortExamVillanyCount / 10);
            DimensionD = (days >= 10) ? 10 : (int)(days);
            /// calculate Chambers' Dimension
            double chambers = Math.Ceiling(days / 10);
            DimensionC = (int)(chambers);
            /// calculate the number of "mérnökinformatikus
            double infoCount = Math.Ceiling(longExamInfoCount / 8) + Math.Ceiling(shortExamInfoCount / 10);
            TutionInfoCount = (int)(infoCount);
            /// generate Structure instructors from DataModel instructors
            Instructors = new List<StructureInstructor>();
            int timeslotsPerDay = 10;
            foreach (Instructor i in DataModels.Service.getInstructors())
            {
                if (i.President || i.Secretary)
                {
                    List<bool> presence = new List<bool>();
                    /// for every day from the beginning of the "day" dimension untill the end of instructor availability
                    /// check every day if the instructor is available
                    /// [NOTE] a day consists of 10 timeslots -> timeslotsPerDay = 10
                    /// [NOTE] an istructor is available if he is available from 9:00 'till 17:00
                    for (int a = ((i.Availability.Count - 1) - DimensionD * timeslotsPerDay); a <= (i.Availability.Count - 1); a+= timeslotsPerDay)
                    {
                        presence.Add(
                            i.Availability[a + 1] &&
                            i.Availability[a + 2] &&
                            i.Availability[a + 3] &&
                            i.Availability[a + 4] &&
                            i.Availability[a + 5] &&
                            i.Availability[a + 6] &&
                            i.Availability[a + 7] &&
                            i.Availability[a + 8]
                           );
                    }
                    Instructors.Add(new StructureInstructor(i.Id, i.President, i.Secretary, i.Tutions, presence));
                }
            }
        }
    }
}
