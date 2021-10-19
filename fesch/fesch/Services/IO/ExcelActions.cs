using fesch.Services.IO.ExcelAction;

namespace fesch.Services.IO
{
    public class ExcelActions
    {
        private static ExcelActions instance = null;
        public static ExcelActions Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExcelActions();
                }
                return instance;
            }
        }
        private ExcelActions() { }
        public void Read(string source, bool secondaryExaminers = false)
        {
            ExcelReader.Read(source, secondaryExaminers);
        }
        public void LogExamStructure(string destination)
        {
            ExcelLogger.LogExamStructure(destination);
        }
        public void Write(string destination)
        {
            ExcelWriter.Write(destination);
        }
    }
}
