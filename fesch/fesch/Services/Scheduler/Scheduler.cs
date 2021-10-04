using Gurobi;

namespace fesch.Services.Scheduler
{
    public class Scheduler
    {
        private static Scheduler instance = null;
        public static Scheduler Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Scheduler();
                }
                return instance;
            }
        }
        private GRBModel model;
        private Scheduler() {}
        public void ScheduleExamStructure()
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            ExamStructure.Variables.Set(model);
            ExamStructure.Constraints.Set(model);
            ExamStructure.Objective.Set(model);
            model.Optimize();
            ExamStructure.Reader.Get();
        }
        public void ScheduleExamStructure(int numberOfDaysAvailableToConductTheExam)
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            ExamStructure.Variables.Set(model);
            ExamStructure.Constraints.Set(model);
            ExamStructure.Objective.Set(model);
            model.Optimize();
            ExamStructure.Reader.Get();
        }
        public void ScheduleExamAttendants()
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            ExamAttendants.Variables.Set(model);
            ExamAttendants.Constraints.Set(model);
            ExamAttendants.Objective.Set(model);
            model.Optimize();
            ExamAttendants.Reader.Get();
        }
    }
}
