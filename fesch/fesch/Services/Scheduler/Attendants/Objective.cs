using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        /// dimensions
        private static int S = Variables.S;
        private static int F = Variables.F;

        public static void Set(GRBModel model)
        {
            model.SetObjective(
                InstructorCount() +
                InstructorAvailability(), 
                GRB.MINIMIZE);
        }

        private static GRBLinExpr InstructorCount()
        {
            int penaltyScore = 10;
            GRBLinExpr penalty = 0;
            for (int s = 0; s < S; s++)
            {
                for (int me = 0; me < Variables.sme[s].Length; me++)
                {
                    penalty.AddTerm(1, Variables.sme[s][me]);
                }
            }
            return penaltyScore * penalty;
        }

        private static GRBQuadExpr InstructorAvailability()
        {
            int penaltyScore = 1000;
            GRBQuadExpr penalty = 0;
            for (int s = 0; s < S; s++)
            {
                for (int f = 0; f < F; f++)
                {
                    for (int o = 0; o < Variables.sor[s].Length; o++)
                    {
                        for(int me = 0; me < Variables.sme[s].Length; me++)
                        {
                            GRBVar temp = Variables.sme[s][me];
                            GRBVar[] tempeki = Variables.sor[s];
                            int temporary = Attendants.Service.SME[s][me].DataModelsId;
                            temporary = Attendants.Service.Fragments[f].Day;
                            penalty.Add(Variables.Unavailable(
                                Variables.sme[s][me],
                                Variables.sor[s],
                                Attendants.Service.SME[s][me].DataModelsId,
                                Attendants.Service.Fragments[f].Day));
                        }
                    }
                }
            }
            return penaltyScore * penalty;
        }
    }
}
