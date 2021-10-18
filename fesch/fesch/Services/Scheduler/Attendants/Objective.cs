using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        private static int S = Variables.S;
        private static int F = Variables.F;
        private static int OS = Variables.OS;
        private static int OL = Variables.OL;

        public static void Set(GRBModel model)
        {
            model.SetObjective(
                InstructorCount() +
                InstructorUnavailability() +
                MELoads() + 
                LanguageBlocks() +
                TutionBlocks(),
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

        private static GRBQuadExpr InstructorUnavailability()
        {
            int penaltyScore = 100;
            GRBQuadExpr penalty = 0;
            for (int s = 0; s < S; s++)
            {
                for (int f = 0; f < F; f++)
                {
                    for (int o = 0; o < Variables.sor[s].Length; o++)
                    {
                        for(int me = 0; me < Variables.sme[s].Length; me++)
                        {
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

        private static GRBLinExpr MELoads()
        {
            int penaltyScore = 10;
            GRBLinExpr penalty = 0;
            for (int fl = 0; fl < Attendants.Service.SMEFlattenedLength; fl++)
            {
                penalty.AddTerm(1.0 / (30 * S), Variables._objective_ME_PositiveDeviation[fl]);
                penalty.AddTerm(1.0 / (30 * S), Variables._objective_ME_NegativeDeviation[fl]);
            }
            return penaltyScore * penalty;
        }

        private static GRBLinExpr LanguageBlocks()
        {
            int penaltyScore = 1;
            GRBLinExpr penalty = 0;
            for (int f = 0; f < F; f++)
            {
                penalty.AddTerm(1, Variables._objective_LanguageBlock[f]);
            }
            return penaltyScore * penalty;
        }

        private static GRBLinExpr TutionBlocks()
        {
            int penaltyScore = 1;
            GRBLinExpr penalty = 0;
            for (int f = 0; f < F; f++)
            {
                penalty.AddTerm(1, Variables._objective_TutionBlock[f]);
            }
            return penaltyScore * penalty;
        }
    }
}
