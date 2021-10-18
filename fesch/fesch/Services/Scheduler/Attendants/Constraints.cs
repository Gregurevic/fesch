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
            /// objective constraints
            MELoads(model);
            LanguageBlocks(model);
            TutionBlocks(model);
            /// regular constraints
            sfx_StudentCount(model);
            sfx_EveryStudentIsPresent(model);
            sfx_MatchingTutions(model);
            sor_OneTrueOrdinal(model);
            sor_NoDuplicateOrdinals(model);
            sor_PreferLesserOrdinals(model);
            sme_MEPresence(model);
        }

        /// objective constraints
        private static void MELoads(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[Attendants.Service.SMEFlattenedLength];
            GRBLinExpr sum = 0;
            for (int fl = 0; fl < Attendants.Service.SMEFlattenedLength; fl++) { sums[fl] = 0; }
            for (int s = 0; s < S; s++)
            {
                for (int me = 0; me < Variables.sme[s].Length; me++)
                {
                    sums[Attendants.Service.SME[s][me].FlattenedId].AddTerm(1, Variables.sme[s][me]);
                    sum.AddTerm(1, Variables.sme[s][me]);
                }
            }
            for (int fl = 0; fl < Attendants.Service.SMEFlattenedLength; fl++)
            {
                model.AddConstr(
                    Variables._objective_ME_PositiveDeviation[fl] - Variables._objective_ME_NegativeDeviation[fl] == sum - sums[fl], 
                    "MELoads_" + fl);
            }
        }

        private static void LanguageBlocks(GRBModel model)
        {
            for (int f = 0; f < F; f++)
            {
                GRBLinExpr sum = 0;
                for (int s = 0; s < S; s++)
                {
                    sum.AddTerm(
                        Attendants.Service.Students[s].English ? 1.0 : 0.0,
                        Variables.sfx[s, f]);
                }
                /// if an English student exists in Fragment, set indicator to true
                /// [later]: minimize indicator count
                /// hence
                /// create language blocks
                model.AddGenConstrIndicator(
                    Variables._objective_LanguageBlock[f], 
                    1, 
                    sum >= 1,
                    "LanguageBlocks_" + f);
            }
        }

        private static void TutionBlocks(GRBModel model)
        {
            for (int f = 0; f < F; f++)
            {
                GRBLinExpr sum = 0;
                for (int s = 0; s < S; s++)
                {
                    sum.AddTerm(
                        Attendants.Service.Students[s].BPRO ? 1.0 : 0.0,
                        Variables.sfx[s, f]);
                }
                /// if an English student exists in Fragment, set indicator to true
                /// [later]: minimize indicator count
                /// hence
                /// create language blocks
                model.AddGenConstrIndicator(
                    Variables._objective_TutionBlock[f],
                    1,
                    sum >= 1,
                    "TutionBlocks_" + f);
            }
        }

        /// regular constraints
        private static void sfx_StudentCount(GRBModel model)
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
                model.AddConstr(sumsS[f] <= 11 * Variables._constraint_sfx_s_count_S[f], "sfx_StudentCount_S_" + f);
                model.AddConstr(sumsL[f] <= 9 * Variables._constraint_sfx_s_count_L[f], "sfx_StudentCount_L_" + f);
                model.AddConstr(Variables._constraint_sfx_s_count_S[f] + Variables._constraint_sfx_s_count_L[f] == 1, "sfx_StudentCount_H_" + f);
            }
        }

        private static void sfx_EveryStudentIsPresent(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                sums[s] = 0;
                for (int f = 0; f < F; f++)
                {
                    sums[s].AddTerm(1, Variables.sfx[s, f]);
                }
                model.AddConstr(sums[s] == 1, "sfx_EveryStudentIsPresent_" + s);
            }
        }

        private static void sfx_MatchingTutions(GRBModel model)
        {
            for (int s = 0; s < S; s++)
            {
                for (int f = 0; f < F; f++)
                {
                    if (Attendants.Service.Students[s].Tution != Attendants.Service.Fragments[f].Tution)
                        model.AddConstr(Variables.sfx[s, f] == 0, "sfx_MatchingTutions_" + s + "_" + f);
                }
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
            /// ME instructors who can be Member(M), First Examiner (EF) or Secondary Examiner (ES)
            GRBLinExpr[] sumsM = new GRBLinExpr[S];
            GRBLinExpr[] sumsEF = new GRBLinExpr[S];
            GRBLinExpr[] sumsES = new GRBLinExpr[S];
            /// president or secretary can be examiner in given Fragment
            GRBLinExpr[] _Structure_is_EF = new GRBLinExpr[S];
            GRBLinExpr[] _Structure_is_ES = new GRBLinExpr[S];
            for (int s = 0; s < S; s++)
            {
                /// init local expressions
                sumsM[s] = 0;
                sumsEF[s] = 0;
                sumsES[s] = 0;
                _Structure_is_EF[s] = 0;
                _Structure_is_ES[s] = 0;
                /// fill expressions with correlating data
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
                /// CONSTRAINTS
                /// Member
                model.AddConstr(sumsM[s] >= 1, "sme_MEPresence_M_" + s);
                /// First Examiner
                for (int f = 0; f < F; f++)
                {
                    _Structure_is_EF[s].AddTerm(
                        Attendants.Service.Fragments[f].Courses.Contains(Attendants.Service.Students[s].Courses[0]) ? 1.0 : 0.0, 
                        Variables.sfx[s, f]
                    );
                }
                model.AddConstr(sumsEF[s] >= 1 - _Structure_is_EF[s], "sme_MEPresence_EF_" + s);
                /// Second Examiner
                if (!Attendants.Service.Students[s].Short)
                {
                    for (int f = 0; f < F; f++)
                    {
                        _Structure_is_ES[s].AddTerm(
                            Attendants.Service.Fragments[f].Courses.Contains(Attendants.Service.Students[s].Courses[1]) ? 1.0 : 0.0, 
                            Variables.sfx[s, f]
                        );
                    }
                    model.AddConstr(sumsES[s] >= 1 - _Structure_is_ES[s], "sme_MEPresence_ES_" + s);
                }
            }
        }
    }
}
