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
            model.Optimize();
            if (model.Status == GRB.Status.OPTIMAL) {
                /// Visualizer.VisualizeStructures();
                Structure.Reader.Get();
                model.Dispose();
                env.Dispose();
            } else {
                model.ComputeIIS();
                throw new StructureSchedulerException("The structure model proved to be infeasible.");
            }
        }

        public void ScheduleExamAttendants()
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            Attendant.Variables.Set(model);
            Attendant.Constraints.Set(model);
            Attendant.Objective.Set(model);
            model.Optimize();
            if (model.Status == GRB.Status.OPTIMAL) {
                /// Visualizer.VisualizeAttendants();
                Attendant.Reader.Get();
                model.Dispose();
                env.Dispose();
            } else {
                model.ComputeIIS();
                throw new AttendantSchedulerException("The attendant model proved to be infeasible.");
            }
        }
    }
}
