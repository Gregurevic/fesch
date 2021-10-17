using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Constraints
    {
        private static int S = Variables.S;
        private static int F = Variables.F;
        private static int OS = Variables.OS;
        private static int OL = Variables.OL;

        public static void Set(GRBModel model)
        {
            sfx_studentCountPerFragment(model);
            sfx_everyStudentIsPresent(model);
            sor_OneTrueOrdinalPerStudent(model);
            sor_NoDuplicateOrdinalsPerFragment(model);
        }

        private static void sfx_studentCountPerFragment(GRBModel model)
        {
            GRBLinExpr[] sumsS = new GRBLinExpr[F];
            GRBLinExpr[] sumsL = new GRBLinExpr[F];
            for (int f = 0; f < F; f++)
            {
                sumsS[f] = 0;
                sumsL[f] = 0;
                for (int s = 0; s < S; s++)
                {
                    if (Attendants.Service.Students[s].Short) {
                        sumsS[f].AddTerm(1, Variables.sfx[s, f]);
                    } else {
                        sumsL[f].AddTerm(1, Variables.sfx[s, f]);
                    }
                }
                model.AddConstr(sumsS[f] <= 11 * Variables._constraint_sfx_s_count_S[f], "sfx_studentCountPerFragment_S_" + f);
                model.AddConstr(sumsL[f] <= 9 * Variables._constraint_sfx_s_count_L[f], "sfx_studentCountPerFragment_L_" + f);
                model.AddConstr(Variables._constraint_sfx_s_count_S[f] + Variables._constraint_sfx_s_count_L[f] == 1, "sfx_studentCountPerFragment_H_" + f);
            }
        }

        private static void sfx_everyStudentIsPresent(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                sums[s] = 0;
                for (int f = 0; f < F; f++)
                {
                    sums[s].AddTerm(1, Variables.sfx[s, f]);
                }
                model.AddConstr(sums[s] == 1, "sfx_everyStudentIsPresent_" + s);
            }
        }

        private static void sor_OneTrueOrdinalPerStudent(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                sums[s] = 0;
                for (int o = 0; o < Variables.sor[s].Length; o++)
                {
                    sums[s].AddTerm(1, Variables.sor[s][o]);
                }
                model.AddConstr(sums[s] == 1, "sor_OneTrueOrdinalPerStudent_" + s);
            }
        }

        private static void sor_NoDuplicateOrdinalsPerFragment(GRBModel model)
        {
            GRBQuadExpr[] sums = new GRBQuadExpr[F * OS];
            for (int fos = 0; fos < F * OS; fos++) { sums[fos] = 0; }
            int idx = 0;
            for (int f = 0; f < F; f++)
            {
                for (int os = 0; os < OS; os++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        if (os < Variables.sor[s].Length)
                        {
                            sums[idx].Add(Variables.sfx[s, f] * Variables.sor[s][os]);
                        }
                    }
                    model.AddQConstr(sums[idx] <= 1, "sor_NoDuplicateOrdinalsPerFragment_" + f + "_" + os);
                    idx++;
                }
            }
        }
    }
}
