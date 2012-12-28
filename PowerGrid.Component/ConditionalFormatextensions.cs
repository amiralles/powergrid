namespace PowerGrid.Component {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class ConditionalFormatextensions {
        private static readonly List<string> _operators = new List<string> {
            "+","-","*","/",">","<","==","!=",">=","<=","&&", "||",
        };

        public static string ToCSharp(this string target, Dictionary<string, object> args = null) {

            var tokens = target.ToUpperInvariant().Replace("'", "\"").ToCharArray();

            var result = new StringBuilder();
            var variables = new StringBuilder();

            if (args != null) {
                variables.AppendLine("using System.Collections.Generic;");
                variables.AppendLine("Dictionary<string, object> variables = new Dictionary<string, object>{");
                foreach (var key in args.Keys) {
                    if (args[key].IsNumber()) {
                        variables.AppendLine("{" + "\"" + key + "\"," + Convert.ToDouble(args[key]).ToString(CultureInfo.InvariantCulture) + "},");
                    }
                    else
                        variables.AppendLine("{" + "\"" + key + "\",\"" + args[key] + "\"},");
                }
                variables.AppendLine("};");
            }

            for (var i = 0; i < tokens.Length; i++) {

                if (tokens[i] == '<' && tokens[i + 1] == '>') {
                    result.Append(" != ");
                    i++;
                    continue;
                }
                if (tokens[i] == '=') {
                    result.Append(" == ");
                    continue;
                }
                if (tokens[i] == 'Y' && tokens[i - 1] == ' ' && tokens[i - 1] == ' ') {
                    result.Append(" && ");
                    continue;
                }
                if (tokens[i] == 'O' && tokens[i - 1] == ' ' && tokens[i - 1] == ' ') {
                    result.Append(" || ");
                    continue;
                }
                if (tokens[i] == '\'') {
                    result.Append("\"");
                    continue;
                }

                result.Append(string.Format("{0}", tokens[i]));
            }

            return TransformVariablesToEngineCalls(result.ToString(), variables);
        }

        private static string TransformVariablesToEngineCalls(string src, StringBuilder variables) {
            var tokens = src.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();
            for (var i = 0; i < tokens.Length; i++) {
                if (IsKeyword(tokens[i]) || IsOperator(tokens[i]) || IsNumberOrDate(tokens[i])) {
                    result.Append(tokens[i]);
                    continue;
                }

                if (tokens[i].StartsWith("\""))/*string literal*/ {
                    result.AppendFormat(tokens[i]);
                    continue;
                }
                //Si llego hasta aca es el nombre de un campo.
                result.AppendFormat("GetValue(\"{0}\",variables)", tokens[i]);
            }

            result.Insert(0, variables.ToString());
            return result.ToString();
        }

        private static bool IsOperator(string token) {
            return _operators.Any(o => o == token);
        }

        private static bool IsNumberOrDate(string token) {
            double d;
            if (double.TryParse(token, out d))
                return true;
            DateTime date;
            return DateTime.TryParse(token, out date);
        }

        private static bool IsKeyword(string token) {
            return _operators.Contains(token);
        }

        private static bool IsNumber(this object target) {
            if (target == null)
                return false;
            double d;
            return double.TryParse(target.ToString(), out d);
        }
    }
}