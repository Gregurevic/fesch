using fesch.Services.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace fesch.Services.Storage.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CourseNeptun : Attribute
    {
        private string code;
        public virtual string Code
        {
            get { return code; }
            set
            {
                /// tanszékek kódjai a https://www.vik.bme.hu/page/43/ alapján
                if (!Regex.Match(value, @"\bBMEVI(AU|ET|EE|EV|FO|HI|MM|NF|MA|HV|EV|MH|TT|VE|VG|VM)([A-Z]|[0-9]){4}\b").Success)
                    throw new NeptunException("Invalid Neptun code construction attempt!");
                code = value;
            }
        }
        public CourseNeptun(string code)
        {
            Code = code;
        }
        public bool Match(CourseNeptun anotherNeptun)
        {
            return Code.Equals(anotherNeptun);
        }
    }
}
