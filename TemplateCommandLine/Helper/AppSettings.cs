using System.Collections.Generic;

namespace TemplateCommandLine.Helper
{
    public class Command
    {
        public string CommandName { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public IEnumerable<Param> Params { get; set; }
    }

    public class Param
    {
        public bool CheckParam(string param)
        {
            if (param.Replace("-",string.Empty) == ParamName) return true;

            return false;
        }

        public string ParamName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class AppSettings
    {
        public IEnumerable<Command> Commands { get; set; }
    }
}
