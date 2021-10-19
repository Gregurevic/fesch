using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.Attendant
{
    static class Variables
    {
        ///dimension
        public static int S = Attendants.Service.DimensionS;
        public static int F = Attendants.Service.DimensionF;
        public static int OS = Attendants.Service.DimensionOS;
        public static int OL = Attendants.Service.DimensionOL;
        /// variables
        public static GRBVar[,] sfx;
        public static GRBVar[][] sor;
        public static GRBVar[][] sme;
        /// objective variables 
        public static GRBVar[] _objective_ME_PositiveDeviation;
        public static GRBVar[] _objective_ME_NegativeDeviation;
        public static GRBVar[] _objective_LanguageBlock;
        public static GRBVar[] _objective_TutionBlock;

        public static void Set(GRBModel model)
        {
            /// variables
            InitVariables(model);
            /// constraint variables
            InitConstraintVariables(model);
            /// objective variables
            InitObjectiveVariables(model);
            InitUnavailabilityMatrix();
        }

        /// VARIABLES
        /// sfx ~ Student-Fragment-matriX
        /// sor ~ Student-ORDinal-matrix
        /// sme ~ Student-(Member-or-Examiner)-matrix
        private static void InitVariables(GRBModel model)
        {
            sfx = new GRBVar[S, F];
            for (int s = 0; s < S; s++)
            {
                for (int f = 0; f < F; f++)
                {
                    sfx[s, f] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "sfx_" + s + "_" + f);
                }
            }
            sor = new GRBVar[S][];
            for (int s = 0; s < S; s++)
            {
                sor[s] = new GRBVar[Attendants.Service.Students[s].Short ? OS : OL];
                for (int o = 0; o < sor[s].Length; o++)
                {
                    sor[s][o] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "sor_" + s + "_" + o);
                }
            }
            sme = new GRBVar[S][];
            for (int s = 0; s < S; s++)
            {
                sme[s] = new GRBVar[Attendants.Service.SME[s].Length];
                for (int me = 0; me < sme[s].Length; me++)
                {
                    sme[s][me] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "sme_" + s + "_" + me);
                }
            }
        }

        /// CONSTRAINT VARIABLES
        public static GRBVar[] _constraint_sfx_s_count_S;
        public static GRBVar[] _constraint_sfx_s_count_L;
        public static GRBVar[] _constraint_no_duplicate_ordinals;

        private static void InitConstraintVariables(GRBModel model)
        {
            _constraint_sfx_s_count_S = new GRBVar[F];
            _constraint_sfx_s_count_L = new GRBVar[F];
            for (int f = 0; f < F; f++)
            {
                _constraint_sfx_s_count_S[f] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constraint_sfx_s_count_S_" + f);
            }
            for (int f = 0; f < F; f++)
            {
                _constraint_sfx_s_count_L[f] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constraint_sfx_s_count_L_" + f);
            }
            _constraint_no_duplicate_ordinals = new GRBVar[S * OS * F];
            for (int f = 0; f < F; f++)
            {
                for (int o = 0; o < OS; o++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        _constraint_no_duplicate_ordinals[f * OS * S + o * S + s] 
                            = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constraint_no_duplicate_ordinals_" + f + "_" + o + "_" + "_" + s);
                    }
                }
            }
        }

        /// OBJECTIVE VARIABLES - ME_DEVIATION
        /// OBJECTIVE VARIABLES - LANGUAGE_BLOCKS
        /// OBJECTIVE VARIABLES - TUTION_BLOCKS
        private static void InitObjectiveVariables(GRBModel model)
        {
            _objective_ME_PositiveDeviation = new GRBVar[Attendants.Service.SMEFlattenedLength];
            _objective_ME_NegativeDeviation = new GRBVar[Attendants.Service.SMEFlattenedLength];
            for (int fl = 0; fl < Attendants.Service.SMEFlattenedLength; fl++)
            {
                _objective_ME_PositiveDeviation[fl] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_ME_PositiveDeviation_" + fl);
            }
            for (int fl = 0; fl < Attendants.Service.SMEFlattenedLength; fl++)
            {
                _objective_ME_NegativeDeviation[fl] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_ME_NegativeDeviation_" + fl);
            }
            _objective_LanguageBlock = new GRBVar[F];
            for (int f = 0; f < F; f++)
            {
                _objective_LanguageBlock[f] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_LanguageBlock_" + f);
            }
            _objective_TutionBlock = new GRBVar[F];
            for (int f = 0; f < F; f++)
            {
                _objective_TutionBlock[f] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_TutionBlock_" + f);
            }
        }

        /// OBJECTIVE VARIABLES - UNAVAILABILITY MATRICES
        private static double[,,] SAM;
        private static double[,,] LAM;

        private static void InitUnavailabilityMatrix()
        {
            int I = Attendants.Service.GetI();
            int D = Attendants.Service.GetD();
            SAM = new double[I, D, OS];
            int time;
            int fromIdx;
            int toIdx;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int os = 0; os < OS; os++)
                    {
                        time = 60; /// 9:00 -> 60;
                        time += os * 40; /// previous exams' length
                        time += ((os + 1) / 6) * 50; /// add lunch duration (50) from 6th daily exam onward
                        fromIdx = time / 60; /// convert to instructor availability index from the above calculated 'time'
                        toIdx = (time + 40) / 60; /// current exam's finishing availability index
                        bool available = Attendants.Service.Availability[i, fromIdx] && Attendants.Service.Availability[i, toIdx];
                        SAM[i, d, os] = available ? 0.0 : 1.0;
                    }
                }
            }
            LAM = new double[I, D, OL];
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int ol = 0; ol < OL; ol++)
                    {
                        time = 50; /// 8:50 -> 50;
                        time += ol * 50; /// previous exams' length
                        time += ((ol + 1) / 5) * 50; /// add lunch duration (50) from 5th daily exam onward
                        fromIdx = time / 60; /// convert to instructor availability index from the above calculated 'time'
                        toIdx = (time + 50) / 60; /// current exam's finishing availability index
                        bool available = Attendants.Service.Availability[i, fromIdx] && Attendants.Service.Availability[i, toIdx];
                        LAM[i, d, ol] = available ? 0.0 : 1.0;
                    }
                }
            }
        }

        public static GRBQuadExpr Unavailable(GRBVar match, GRBVar[] ordinal, int i, int d)
        {
            double[,,] AM = (ordinal.Length == OS) ? SAM : LAM;
            GRBQuadExpr product = 0;
            for (int o = 0; o < ordinal.Length; o++)
            {
                product.Add(AM[i, d, o] * match * ordinal[o]);
            }
            return product;
        }
    }
}
