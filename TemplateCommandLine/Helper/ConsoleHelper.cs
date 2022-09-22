using System;

namespace TemplateCommandLine.Helper
{
    public static class ConsoleHelper
    {
        public static void Write(string text, bool newLine = true)
        {
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
        }
    }
}
