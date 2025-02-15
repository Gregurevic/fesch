﻿using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantStudent
    {
        public int Id { get; set; }
        public Level Level { get; set; }
        public bool English { get; set; }
        public Tution Tution { get; set; }
        public bool BPRO { get; set; }
        public bool Short { get; set; }
        public int SupervisorId { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        private AttendantStudent() { }
        public AttendantStudent(int id, Level level, bool english, Tution tution, bool bPRO, bool @short, int supervisorId, List<CourseNeptun> courses)
        {
            Id = id;
            Level = level;
            English = english;
            Tution = tution;
            BPRO = bPRO;
            Short = @short;
            SupervisorId = supervisorId;
            Courses = courses;
        }
    }
}
