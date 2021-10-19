using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.StructureModel;
using System;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler
{
    public class Structures : IDisposable
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
        public List<StructureFragment> Fragments { get; set; }
        private Structures()
        {
            /// check if DataModels is valid
            DataModels.Service.Validate();
            /// partials
            double longExamInfoCount = DataModels.Service.Students.FindAll(
                s => (s.Language == Language.ENG || s.Level == Level.MSC) && (s.Tution == Tution.INFO || s.Tution == Tution.BPRO)).Count;
            double longExamVillCount = DataModels.Service.Students.FindAll(
                s => (s.Language == Language.ENG || s.Level == Level.MSC) && s.Tution == Tution.VILL).Count;
            double shortExamInfoCount = DataModels.Service.Students.FindAll(s => s.Tution == Tution.INFO || s.Tution == Tution.BPRO).Count - longExamInfoCount;
            double shortExamVillCount = DataModels.Service.Students.FindAll(s => s.Tution == Tution.VILL).Count - longExamVillCount;
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
            foreach (Instructor i in DataModels.Service.Instructors)
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
            Fragments = new List<StructureFragment>();
            /// correct FirstDay according to days needed
            DataModels.Service.FirstDay = DataModels.Service.FirstDay.AddDays(- days);
        }

        /// IDISPOSEABLE
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Instructors = null;
                Fragments = null;
            }
            _disposed = true;
        }
        ~Structures()
        {
            Dispose(false);
        }
    }
}
