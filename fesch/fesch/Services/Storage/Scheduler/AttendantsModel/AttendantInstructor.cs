using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantInstructor
    {
        public int Id { get; set; }
        public bool Member { get; set; }
        public List<bool> Availability { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        private AttendantInstructor() { }
        public AttendantInstructor(int id, bool member, List<bool> availability, List<CourseNeptun> courses)
        {
            Id = id;
            Member = member;
            Availability = availability;
            Courses = courses;
        }

        public bool Available(Level level, Language language,  int day, int ordinal)
        {
            /// number of Availability indexes per day
            int idxCountPerDay = 10;
            /// ordinal values: 1..11
            int start;
            int fromIdx;
            int toIdx;
            if (level == Level.BSC && language == Language.HUN) { /// shortExam
                start = 60 + (ordinal - 1) * 40; /// am
                if (ordinal >= 6) { start += 50; } /// pm
                fromIdx = (int)Math.Floor((double)start / 60);
                toIdx = (int)Math.Floor((double)(start + 40) / 60);
            }
            else { /// longExam
                start = 50 + (ordinal - 1) * 50; /// am
                if (ordinal >= 5) { start += 50; } /// pm
                fromIdx = (int)Math.Floor((double)start / 60);
                toIdx = (int)Math.Floor((double)(start + 50) / 60);
            }
            for (int i = fromIdx; i <= toIdx; i++)
            {
                if (!Availability[idxCountPerDay * day + i]) return false;
            }
            return true;
        }
    }
}
