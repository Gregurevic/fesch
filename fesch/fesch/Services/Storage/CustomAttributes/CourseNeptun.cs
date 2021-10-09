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
                /// tanszékek kódjai a https://www.vik.bme.hu/page/43/ alapján -> (AU|ET|EE|EV|FO|HI|MM|NF|MA|HV|EV|MH|TT|VE|VG|VM|MI|II|TM) VISZONT! ez helytelen, egyelőre cserélve bármilyen karakterre
                if (!Regex.Match(value, @"\bBME(VI|TE)([A-Z]|[0-9]){6}\b").Success)
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
            return code.Equals(anotherNeptun.Code);
        }
    }
}
