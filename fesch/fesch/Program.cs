using fesch.Services.IO;
using fesch.Services.Scheduler;

namespace fesch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /// külső adatforrás
            string pathFrom = "C://Munka//bme//onlab//fesch//input//input_001.xlsx";
            string pathTo = "C://Munka//bme//onlab//fesch//output//output_001.xlsx";
            bool timeLimit = false;
            int dayCount = 4;
            /// funkcionalitás
            Excel.Service.Read(pathFrom);
            if (timeLimit) { Scheduler.Service.ScheduleExamStructure(dayCount); }
                else { Scheduler.Service.ScheduleExamStructure(); }
            Excel.Service.LogExamStructure(pathTo);
            Scheduler.Service.ScheduleExamAttendants();
            Excel.Service.Write(pathTo);
        }
    }
}
