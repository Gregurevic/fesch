using fesch.Services.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace fesch.Services.Storage.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Neptun : Attribute
    {
        private string code;
        public virtual string Code
        {
            get { return code; }
            set
            {
                if (!Regex.Match(value, @"\b([A-Z]|[0-9]){6}\b").Success)
                    throw new NeptunException("Invalid Neptun code construction attempt!");
                code = value;
            }
        }
        public Neptun(string code)
        {
            Code = code;
        }
        public bool Match(Neptun anotherNeptun)
        {
            return code.Equals(anotherNeptun.Code);
        }
    }
}
