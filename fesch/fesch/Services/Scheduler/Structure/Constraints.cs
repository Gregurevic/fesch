using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.Structure
{
    static class Constraints
    {
        private static int I = Variables.iGRB.GetLength(0);
        private static int D = Variables.iGRB.GetLength(1);
        private static int C = Variables.iGRB.GetLength(2);

        public static void Set(GRBModel model)
        {
            /// objective constraints
            PresidentLoads(model);
            SecretaryLoads(model);
            /// regular constraints
            NoDuplicateInstructors(model);
            DailyInstructorCount(model);
            FragmentCount(model);
            BothRolesArePresent(model);
            ValidFragmentTutions(model);
            EnoughFragmentsPerTution(model);
        }

        /// két új változó (tömböt) vezetünk be,
        /// amiknek értékül adjuk elnökönként hogy hányszor osztottuk be minusz az optimális értéket
        /// a constraint-ben fel vannak szorozva az összes elnök számával a változók, hogy egész értékeket kapjunk
        private static void PresidentLoads(GRBModel model)
        {
            GRBLinExpr required = D;
            int P = Structures.Service.Instructors.FindAll(i => i.President).Count;
            GRBLinExpr[] sums = new GRBLinExpr[P];
            for (int p = 0; p < P; p++) { sums[p] = 0; }
            int idx = 0;
            for (int i = 0; i < I; i++)
            {
                if (Structures.Service.Instructors[i].President)
                {
                    for (int d = 0; d < D; d++)
                    {
                        for (int c = 0; c < C; c++)
                        {
                            sums[idx].AddTerm(1, Variables.iGRB[i, d, c]);
                        }
                    }
                    model.AddConstr(
                        P * (Variables._objective_PresidentPositiveDeviation[idx] - Variables._objective_PresidentNegativeDeviation[idx]) 
                        == P * sums[idx] - D, "PresidentLoads_" + idx
                    );
                    idx++;
                }
            }
        }

        /// ugyan az a logika, mint az elnököknél, lásd feljebb
        private static void SecretaryLoads(GRBModel model)
        {
            GRBLinExpr required = D;
            int S = Structures.Service.Instructors.FindAll(i => i.Secretary).Count;
            GRBLinExpr[] sums = new GRBLinExpr[S];
            for (int s = 0; s < S; s++) { sums[s] = 0; }
            int idx = 0;
            for (int i = 0; i < I; i++)
            {
                if (Structures.Service.Instructors[i].Secretary)
                {
                    for (int d = 0; d < D; d++)
                    {
                        for (int c = 0; c < C; c++)
                        {
                            sums[idx].AddTerm(1, Variables.iGRB[i, d, c]);
                        }
                    }
                    model.AddConstr(
                        S * (Variables._objective_SecretaryPositiveDeviation[idx] - Variables._objective_SecretaryNegativeDeviation[idx])
                        == S * sums[idx] - D, "SecretaryLoads_" + idx
                    );
                    idx++;
                }
            }
        }

        /// ugyan azon instruktor egy napon belül csak egy terembe lehet beosztva
        private static void NoDuplicateInstructors(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[I * D];
            for (int id = 0; id < I * D; id++) { sums[id] = 0; }
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        sums[i * d + d].AddTerm(1, Variables.iGRB[i, d, c]);
                    }
                    model.AddConstr(sums[i * d + d] <= 1, "NoDuplicateInstructors_" + i + "_" + d);
                }
            }
        }

        /// minden napra 2 vagy 0 instruktort osztunk
        /// definiálunk szabad, bináris változókat: minden összeg értéke legyen egy külön bináris változó értéke szorozva kettővel
        /// ergo minden összeg értéke legyen 0 vagy 2
        private static void DailyInstructorCount(GRBModel model)
        {
            GRBLinExpr[] sums = new GRBLinExpr[D * C];
            for (int dc = 0; dc < D * C; dc++) { sums[dc] = 0; }
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        sums[d * c + c].AddTerm(1, Variables.iGRB[i, d, c]);
                    }
                    model.AddConstr(sums[d * c + c] == 2 * Variables._constr_DailyInstructorCount[d * c + c], "DailyInstructorCount_" + d + "_" + c);
                }
            }
        }

        /// a beosztott instruktorok száma a szükséges fragmentek számának kétszerese kell legyen
        private static void FragmentCount(GRBModel model)
        {
            GRBLinExpr sum = 0;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        sum.AddTerm(1, Variables.iGRB[i, d, c]);
                    }
                }
            }
            model.AddConstr(sum == 2 * Structures.Service.FragmentCount, "FragmentCount");
        }

        /// legyen elnök és titkár is beosztva, ezek diszjunkt halmazok, ez biztosítva van a nyers adatokban
        private static void BothRolesArePresent(GRBModel model)
        {
            GRBLinExpr sumP = 0;
            GRBLinExpr sumS = 0;
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        sumP.AddTerm(Structures.Service.Instructors[i].President ? 1 : 0, Variables.iGRB[i, d, c]);
                        sumS.AddTerm(Structures.Service.Instructors[i].Secretary ? 1 : 0, Variables.iGRB[i, d, c]);
                    }
                    model.AddConstr(sumP == 1, "BothRolesArePresentP_" + d + "_" + c);
                    model.AddConstr(sumS == 1, "BothRolesArePresentS_" + d + "_" + c);
                }
            }
        }

        /// valid infós és villanyos Fragment-eket hozzunk létre
        /// itt is constraint variable-t használunk, lásd fentebb
        private static void ValidFragmentTutions(GRBModel model)
        {
            GRBLinExpr sumI = 0;
            GRBLinExpr sumV = 0;
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        sumI.AddTerm(Structures.Service.Instructors[i].Tutions.Contains(Tution.mérnökinformatikus) ? 1 : 0, Variables.iGRB[i, d, c]);
                        sumV.AddTerm(Structures.Service.Instructors[i].Tutions.Contains(Tution.villamosmérnöki) ? 1 : 0, Variables.iGRB[i, d, c]);
                    }
                    /// sumP x sumS igazságtábla:
                    /// 
                    /// X 0 1 2
                    /// 0 I H I
                    /// 1 H H I
                    /// 2 I I I
                    /// 
                    /// kiszűri: (1;0), (0;1)
                    model.AddConstr(sumI + sumV == 2 * Variables._constr_ValidFragmentTutions[d * c + c], "ValidFragmentTutions_" + d + "_" + c + "+");
                    /// kiszűri: (1;1)
                    model.AddQConstr(sumI * sumV == 2 * Variables._constr_ValidFragmentTutions[d * c + c], "ValidFragmentTutions_" + d + "_" + c + "*");
                }
            }
        }

        /// elég infós és villanyos Fragmentet kell létrehozni
        private static void EnoughFragmentsPerTution(GRBModel model)
        {
            GRBLinExpr sumI = 0;
            GRBLinExpr sumV = 0;
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        sumI.AddTerm(Structures.Service.Instructors[i].Tutions.Contains(Tution.mérnökinformatikus) ? 1 : 0, Variables.iGRB[i, d, c]);
                        sumV.AddTerm(Structures.Service.Instructors[i].Tutions.Contains(Tution.villamosmérnöki) ? 1 : 0, Variables.iGRB[i, d, c]);
                    }
                }
            }
            model.AddConstr(sumI - Structures.Service.InfoFragmentCount >= 0, "EnoughFragmentsPerTution_I");
            model.AddConstr(sumI - Structures.Service.VillFragmentCount >= 0, "EnoughFragmentsPerTution_V");
        }
    }
}
