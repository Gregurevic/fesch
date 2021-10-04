using fesch.Services.IO.Helper;

namespace fesch.Services.IO
{
    public class Excel
    {
        private static Excel instance = null;
        public static Excel Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Excel();
                }
                return instance;
            }
        }
        private Excel() { }
        public void Read(string source = "default", bool secondaryExaminers = false)
        {
            ExcelReader.Read(source, secondaryExaminers);
        }
        public void LogExamStructure(string destination = "default")
        {
            ExcelLogger.LogExamStructure(destination);
        }
        public void Write(string destination = "default")
        {
            ExcelWriter.Write(destination);
        }
    }
}
