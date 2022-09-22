using System;
using System.Linq;
using TemplateCommandLine.Helper;
using TemplateCommandLine.Resources;

namespace CommandNilo
{
    internal class Program
    {
        static AppSettings appSettings;

        static void Main(string[] args)
        {
            //Carrega os comandos da aplicação
            ConfigurationHelper.CreateDefaultConfig(ref appSettings);

            //Texto inicial
            ConsoleHelper.Write(Resource.Title);
            ConsoleHelper.Write(Resource.Description);

            //Inicia a captura do comando
            InitCommand();
        }

        static void InitCommand()
        {
            ConsoleHelper.Write($"{Environment.MachineName}: ", false);

            if (!HandleCommand(Console.ReadLine(), out Command command))
            {
                ConsoleHelper.Write(string.Format(Resource.CommandNotExists, command?.CommandName));
                InitCommand();
            }
            else
            {
                //Procura o método 
                var method = typeof(Program).GetMethod(command.MethodName);

                if(method != null)
                {
                    var resultMethod = (string)method.Invoke(null, new object[] { command.Params.ToArray() });
                    ConsoleHelper.Write(resultMethod);
                }
                else
                {
                    ConsoleHelper.Write(string.Format(Resource.MethodNotExists, command.MethodName));
                }
            }

            InitCommand();
        }

        /// <summary>
        /// Método que obtém o comando digitado pelo usuário e chama o método configurado no appsettings
        /// </summary>
        /// <param name="text"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        static bool HandleCommand(string text, out Command command)
        {
            command = null;

            var commandName = text.Split(' ');

            if (string.IsNullOrWhiteSpace(text)) return false;

            //Busca do comando digitado
            command = appSettings.Commands
                .Select(x => new Command
                {
                    CommandName = x.CommandName,
                    Description = x.Description,
                    MethodName = x.MethodName,
                    Params = x.Params.Select(p => new Param
                    {
                        Description = p.Description,
                        ParamName = p.ParamName
                    }).AsEnumerable()
                })
                .FirstOrDefault(x => x.CommandName.Equals(commandName.FirstOrDefault()));

            if (command == null)
            {
                command = new Command { CommandName = commandName.FirstOrDefault() };
                return false;
            }
            
            //Localiza os parametros
            var parameters = commandName.Where(x => x.Contains('-')).AsEnumerable();

            //Prepara o parametros para enviar para o método
            command.Params = command.Params
                .Where(x => parameters.Contains($"-{x.ParamName}"))
                .Select(x =>
                {
                    var index = commandName.ToList().FindIndex(s => s.Equals($"-{x.ParamName}"));

                    x.Value = commandName.Count() > index + 1 ? 
                        commandName.ToArray()[index + 1] : string.Empty;

                    x.Value = x.Value.StartsWith('-') ? string.Empty : x.Value;
                    return x;
                }).AsEnumerable();

            return true;
        }

        #region [Commands]
        public static string CommandHelp(params Param[] parameters)
        {
            var commands = string.Empty;
            var n = Environment.NewLine;
            var paramFull = parameters.Any(x => x.ParamName.Equals("f"));

            appSettings.Commands.ToList().ForEach(x =>
            {
                commands += $"{Resource.LabelCommand}{x.CommandName}{n}";

                if (paramFull)
                    commands += $"{Resource.LabelDescription}{x.Description}{n}";


                commands += $"{Resource.LabelParams}{(x.Params.Any() ? string.Empty : Resource.None)}";
                foreach (var par in x.Params)
                {
                    if (paramFull)
                    {
                        commands += $"{n}   [-{par.ParamName}]        - {par.Description}";
                    }
                    else
                    {
                        commands += $"[-{par.ParamName}] ";
                    }
                }

                commands += n;
                commands += n;
            });

            return commands;
        }

        public static string CommandClear(params Param[] parameters)
        {
            Console.Clear();
            return string.Empty;
        }

        public static void CommandExit(params Param[] parameters)
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
