using fesch.Services.Exceptions;

namespace fesch.Services.Storage.CustomEnums
{
    public static class CustomEnumConverter
    {
        /// TO
        public static Tution ToTution(string s)
        {
            if (s.Equals("mérnök informatikus")) { return Tution.INFO; }
            if (s.Equals("villamosmérnöki")) { return Tution.VILL; }
            if (s.Equals("üzemmérnök-informatikus")) { return Tution.BPRO; }
            throw new EnumConversionException("The string literal matches no Tution enum.");
        }

        public static Level ToLevel(string s)
        {
            if (s.Equals("BSc")) { return Level.BSC; }
            if (s.Equals("MSc")) { return Level.MSC; }
            throw new EnumConversionException("The string literal matches no Level enum.");
        }

        public static Language ToLanguage(string s)
        {
            if (s.Equals("angol")) { return Language.ENG; }
            if (s.Equals("magyar")) { return Language.HUN; }
            throw new EnumConversionException("The string literal matches no Language enum.");
        }

        /// FROM
        public static string FromTution(Tution tution)
        {
            switch (tution)
            {
                case Tution.INFO:
                    return "mérnök informatikus";
                case Tution.VILL:
                    return "villamosmérnöki";
                case Tution.BPRO:
                    return "üzemmérnök-informatikus";
                default:
                    return "unimplemented Tution";
            }
        }

        public static string FromLevel(Level level)
        {
            switch (level)
            {
                case Level.BSC:
                    return "BSc";
                case Level.MSC:
                    return "MSc";
                default:
                    return "unimplemented Level";
            }
        }

        public static string FromLanguage(Language language)
        {
            switch (language)
            {
                case Language.ENG:
                    return "angol";
                case Language.HUN:
                    return "Magyar";
                default:
                    return "unimplemented Language";
            }
        }
    }
}
