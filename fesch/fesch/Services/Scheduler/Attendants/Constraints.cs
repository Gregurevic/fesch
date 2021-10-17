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
            sfx_studentCount(model);
            sfx_everyStudentIsPresent(model);
            sor_OneTrueOrdinal(model);
            sor_NoDuplicateOrdinals(model);
            sor_PreferLesserOrdinals(model);
        }

        private static void sfx_studentCount(GRBModel model)
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
                model.AddConstr(sumsS[f] <= 11 * Variables._constraint_sfx_s_count_S[f], "sfx_studentCount_S_" + f);
                model.AddConstr(sumsL[f] <= 9 * Variables._constraint_sfx_s_count_L[f], "sfx_studentCount_L_" + f);
                model.AddConstr(Variables._constraint_sfx_s_count_S[f] + Variables._constraint_sfx_s_count_L[f] == 1, "sfx_studentCount_H_" + f);
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

        private static void sor_OneTrueOrdinal(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                sums[s] = 0;
                for (int o = 0; o < Variables.sor[s].Length; o++)
                {
                    sums[s].AddTerm(1, Variables.sor[s][o]);
                }
                model.AddConstr(sums[s] == 1, "sor_OneTrueOrdinal_" + s);
            }
        }

        private static void sor_NoDuplicateOrdinals(GRBModel model)
        {
            /// x * y = z
            /// linearized
            /// z <= y
            /// z <= x
            /// z >= x + y - 1
            GRBLinExpr[] sums = new GRBLinExpr[OS * F];
            for (int fo = 0; fo < OS * F; fo++) { sums[fo] = 0; }
            for (int f = 0; f < F; f++)
            {
                for (int o = 0; o < OS; o++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        if (o < Variables.sor[s].Length)
                        {
                            model.AddConstr(Variables._constraint_no_duplicate_ordinals[f * OS * S + o * S + s] <= Variables.sfx[s, f], "");
                            model.AddConstr(Variables._constraint_no_duplicate_ordinals[f * OS * S + o * S + s] <= Variables.sor[s][o], "");
                            model.AddConstr(Variables._constraint_no_duplicate_ordinals[f * OS * S + o * S + s] >= Variables.sfx[s, f] + Variables.sor[s][o] - 1, "");
                            sums[f * OS + o].AddTerm(1, Variables._constraint_no_duplicate_ordinals[f * OS * S + o * S + s]);
                        }
                    }
                    model.AddQConstr(sums[f * OS + o] <= 1, "sor_NoDuplicateOrdinals_" + f + "_" + o);
                }
            }
        }

        private static void sor_PreferLesserOrdinals(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[OS];
            for (int o = 0; o < OS; o++)
            {
                sums[o] = 0;
                for (int s = 0; s < S; s++)
                {
                    if (o < Variables.sor[s].Length)
                    {
                        sums[o].AddTerm(1, Variables.sor[s][o]);
                    }
                }
                if (o >= 1)
                {
                    model.AddConstr(sums[o] <= sums[o - 1], "sor_PreferLesserOrdinals_" + o);
                }
            }
        }

        private static void sme_MEPresence(GRBModel model)
        {
            GRBLinExpr[] sumsM = new GRBLinExpr[S];
            GRBLinExpr[] sumsEF = new GRBLinExpr[S];
            GRBLinExpr[] sumsES = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                sumsM[s] = 0;
                sumsEF[s] = 0;
                sumsES[s] = 0;
                for (int me = 0; me < Variables.sme[s].Length; me++)
                {
                    if (Attendants.Service.SME[s][me].Member)
                    {
                        sumsM[s].AddTerm(1, Variables.sme[s][me]);
                    }
                    if (Attendants.Service.SME[s][me].FirstExaminer)
                    {
                        sumsEF[s].AddTerm(1, Variables.sme[s][me]);
                    }
                    if (!Attendants.Service.Students[s].Short && Attendants.Service.SME[s][me].SecondExaminer)
                    {
                        sumsES[s].AddTerm(1, Variables.sme[s][me]);
                    }
                }
                model.AddConstr(sumsM[s] >= 1, "sme_MEPresence_M_" + s); /// deduct president and secretary
                model.AddConstr(sumsEF[s] >= 1, "sme_MEPresence_EF_" + s); ///e
                model.AddConstr(sumsES[s] >= 1, "sme_MEPresence_ES_" + s);///vrywhere
            }
        }
    }
}
