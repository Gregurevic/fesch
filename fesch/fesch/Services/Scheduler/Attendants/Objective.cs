using fesch.Services.Storage;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        public static void Set(GRBModel model)
        {
            model.SetObjective(
                Dummy(), 
                GRB.MINIMIZE);
        }

        private static GRBLinExpr Dummy() { return 0; }
    }
}
