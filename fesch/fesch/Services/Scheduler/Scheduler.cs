using fesch.Services.Exceptions;
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

            Structure.Variables.Set(model);
            Structure.Constraints.Set(model);
            Structure.Objective.Set(model);
            model.ComputeIIS();
            model.Optimize();
            if (model.Status == GRB.Status.OPTIMAL) { Structure.Reader.Get(); }
                else { throw new StructureSchedulerException("The structure model proved to be infeasible."); }
        }

        public void ScheduleExamStructure(int numberOfDaysAvailableToConductTheExam)
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            Structure.Variables.Set(model);
            Structure.Constraints.Set(model);
            Structure.Objective.Set(model);
            model.Optimize();
            Structure.Reader.Get();
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
