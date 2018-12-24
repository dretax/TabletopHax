using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tabletopatcher.Patcher
{
    public sealed class CommandLine
    {
        public readonly string Parameter;

        public readonly string Value;

        private CommandLine(string key, string value)
        {
            Parameter = key;
            Value = value;
        }

        private static readonly List<CommandLine> CommandLines;

        public static bool HasCommandLine => CommandLines.Count != 0;

        static CommandLine()
        {
            CommandLines = new List<CommandLine>();

            var cmd = Environment.GetCommandLineArgs().Skip(1).ToArray();

            for (var i = 0; i < cmd.Length; i++)
            {

                var key = CleanVariableKey(cmd[i]);
                var next = i + 1;
                var value = (cmd.Length == next ? null : cmd[next]);
                if (!string.IsNullOrEmpty(value) && !value.StartsWith("-"))
                {
                    value = value.Trim(' ', '"');
                    CommandLines.Add(new CommandLine(key, value));
                    i = i + 1;
                    continue;
                }

                CommandLines.Add(new CommandLine(key, null));
            }
        }

        public static bool HasSwitch(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            name = CleanVariableKey(name);
            return CommandLines.Any(c => c.Parameter == name);
        }

        public static bool HasSwitch(params string[] names)
        {
            if (names == null || names.Length == 0)
                return false;

            return names.Any(HasSwitch);
        }

        public static string GetSwitch(string name, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            name = CleanVariableKey(name);
            var s = CommandLines.FirstOrDefault(c => c.Parameter == name);

            return s?.Value ?? defaultValue;
        }

        public static string GetSwitch(string[] names, string defaultValue = null)
        {
            if (names == null || names.Length == 0)
                return defaultValue;

            var result = names.Select(n => GetSwitch(n))
                .FirstOrDefault(v => v != null);

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        public static string CleanVariableKey(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            var key = input.ToLower();

            while (true)
            {
                var f = key[0];

                if (!f.Equals(Path.DirectorySeparatorChar) && !f.Equals(Path.AltDirectorySeparatorChar) && !f.Equals('-'))
                    return key.Trim();

                key = key.Remove(0, 1);
            }
        }
    }
}
