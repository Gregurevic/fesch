using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.StructureModel;
using System;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler
{
    public class Structures
    {
        private static Structures instance = null;
        public static Structures Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Structures();
                }
                return instance;
            }
        }
        public int DimensionD { get; set; }
        public int DimensionC { get; set; }
        public int FragmentCount { get; set; }
        public int InfoFragmentCount { get; set; }
        public int VillFragmentCount { get; set; }
        public List<StructureInstructor> Instructors { get; set; }
        public Structures()
        {
            double longExamInfoCount = DataModels.Service.getStudents().FindAll(
                s => (s.Language == Language.angol || s.Level == Level.MSc) && s.Tution == Tution.mérnökinformatikus).Count;
            double longExamVillCount = DataModels.Service.getStudents().FindAll(
                s => (s.Language == Language.angol || s.Level == Level.MSc) && s.Tution == Tution.villamosmérnöki).Count;
            double shortExamInfoCount = DataModels.Service.getStudents().FindAll(s => s.Tution == Tution.mérnökinformatikus).Count - longExamInfoCount;
            double shortExamVillCount = DataModels.Service.getStudents().FindAll(s => s.Tution == Tution.villamosmérnöki).Count - longExamVillCount;
            double infoCount = Math.Ceiling(longExamInfoCount / 8) + Math.Ceiling(shortExamInfoCount / 10);
            double villCount = Math.Ceiling(longExamVillCount / 8) + Math.Ceiling(shortExamVillCount / 10);
            double fragmentCount = infoCount + villCount;
            double days = (fragmentCount >= 10) ? 10 : (int)(fragmentCount);
            double chambers = Math.Ceiling(fragmentCount / 10);
            /// calculated attributes
            DimensionD = (int)(days);
            DimensionC = (int)(chambers);
            FragmentCount = (int)(fragmentCount);
            InfoFragmentCount = (int)(infoCount);
            VillFragmentCount = (int)(villCount);
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
                    for (int a = (i.Availability.Count - DimensionD * timeslotsPerDay); a < i.Availability.Count ; a+= timeslotsPerDay)
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
