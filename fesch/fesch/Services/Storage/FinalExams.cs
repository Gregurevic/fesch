using fesch.Services.Storage.FinalExam;
using System.Collections.Generic;

namespace fesch.Services.Storage
{
    class FinalExams
    {
        private static FinalExams instance = null;
        public static FinalExams Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new FinalExams();
                }
                return instance;
            }
        }
        private List<FinalExam.FinalExam> finalExams;
        private FinalExams()
        {
            finalExams = new List<FinalExam.FinalExam>();
        }
        public List<FinalExam.FinalExam> getExams()
        {
            return finalExams;
        }
        public void addExam(FinalExam.FinalExam exam)
        {
            finalExams.Add(exam);
        }
    }
}
