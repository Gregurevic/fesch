using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.Structure
{
    static class Variables
    {
        /// dimensions
        private static int I = Structures.Service.Instructors.Count;
        private static int D = Structures.Service.DimensionD;
        private static int C = Structures.Service.DimensionC;
        /// variables
        public static GRBVar[,,] iGRB;
        /// constraint helper variables
        public static GRBVar[] _constr_FragmentTutions_s2;
        public static GRBVar[] _constr_FragmentTutions_s3;
        public static GRBVar[] _constr_FragmentTutions_s4;
        public static GRBVar[] _constr_FragmentTutions_p2;
        public static GRBVar[] _constr_FragmentTutions_p4;
        /// objective helper variables
        public static GRBVar[] _objective_PresidentPositiveDeviation;
        public static GRBVar[] _objective_PresidentNegativeDeviation;
        public static GRBVar[] _objective_SecretaryPositiveDeviation;
        public static GRBVar[] _objective_SecretaryNegativeDeviation;
        public static void Set(GRBModel model)
        {
            /// variables
            iGRB = new GRBVar[I, D, C];
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        iGRB[i, d, c] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "i_" + i + "_" + d + "_" + c);
                    }
                }
            }
            /// constraint helper variables
            _constr_FragmentTutions_s2 = new GRBVar[D * C];
            _constr_FragmentTutions_s3 = new GRBVar[D * C];
            _constr_FragmentTutions_s4 = new GRBVar[D * C];
            _constr_FragmentTutions_p2 = new GRBVar[D * C];
            _constr_FragmentTutions_p4 = new GRBVar[D * C];
            for (int dc = 0; dc < D * C; dc++)
            {
                _constr_FragmentTutions_s2[dc] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constr_FragmentTutions_s2_" + dc);
                _constr_FragmentTutions_s3[dc] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constr_FragmentTutions_s3_" + dc);
                _constr_FragmentTutions_s4[dc] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constr_FragmentTutions_s4_" + dc);
                _constr_FragmentTutions_p2[dc] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constr_FragmentTutions_p2_" + dc);
                _constr_FragmentTutions_p4[dc] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "_constr_FragmentTutions_p4_" + dc);
            }
            /// objective helper variables
            int P = Structures.Service.Instructors.FindAll(i => i.President).Count;
            _objective_PresidentPositiveDeviation = new GRBVar[P];
            _objective_PresidentNegativeDeviation = new GRBVar[P];
            for (int p = 0; p < P; p++)
            {
                _objective_PresidentPositiveDeviation[p] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_PresidentPositiveDeviation_" + p);
                _objective_PresidentNegativeDeviation[p] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_PresidentNegativeDeviation_" + p);
            }
            int S = Structures.Service.Instructors.FindAll(i => i.Secretary).Count;
            _objective_SecretaryPositiveDeviation = new GRBVar[S];
            _objective_SecretaryNegativeDeviation = new GRBVar[S];
            for (int s = 0; s < S; s++)
            {
                _objective_SecretaryPositiveDeviation[s] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_SecretaryPositiveDeviation_" + s);
                _objective_SecretaryNegativeDeviation[s] = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "_objective_SecretaryNegativeDeviation_" + s);
            }
        }
    }
}
