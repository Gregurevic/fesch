using fesch.Services.IO;
using fesch.Services.Scheduler;
using fesch.Services.Storage;

namespace fesch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /// data, acquired from external source
            string inputPath = "C://Munka//bme//onlab//fesch//input//input_001.xlsx";
            string logPath = "C://Munka//bme//onlab//fesch//output//log_001.xlsx";
            string outputPath = "C://Munka//bme//onlab//fesch//output//output_001.xlsx";
            /// Program functionality
            ExcelActions.Service.Read(inputPath);
            if (!DataModels.Service.TimeLimit)
            {
                Scheduler.Service.ScheduleExamStructure();
                ExcelActions.Service.LogExamStructure(logPath);
                Scheduler.Service.ScheduleExamAttendants();
                ExcelActions.Service.Write(outputPath);
            }
        }
    }
}
