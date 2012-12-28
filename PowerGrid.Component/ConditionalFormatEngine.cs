namespace PowerGrid.Component {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class ConditionalFormatEngine {

        private readonly List<Format> _formats;
        private readonly ScriptEngine _engine;
        private readonly Session _session;

        public ConditionalFormatEngine() {
            _formats = new List<Format>();
            _engine = new ScriptEngine();
            _session = _engine.CreateSession(this, typeof(ConditionalFormatEngine));
            _session.AddReference(Assembly.Load(typeof(ConditionalFormatEngine).Assembly.GetName()));
            _session.AddReference(Assembly.Load(typeof(GenObj).Assembly.GetName()));
            _session.AddReference(Assembly.Load(typeof(Dictionary<string, object>).Assembly.GetName()));
        }

        public bool Eval(string expression) {
            return Eval(expression, (Dictionary<string, object>)null);
        }

        public bool Eval(string expression, DataGridViewRow args) {
            Func<DataGridViewRow, bool> func = row => Eval(expression, row.ToDictionary());
            return Eval(func, args);
        }

        public bool Eval(Func<DataGridViewRow, bool> expression, DataGridViewRow args) {
            return expression(args);
        }

        public bool Eval(string expression, Dictionary<string, object> args) {
            Func<DataGridViewRow, bool> func = row => (bool)_session.Execute(expression.ToCSharp(args));            
            return Eval(func, null);
        }

        //We pass the variables as parameter to avoid global state. (This facilitates concurrent execution)
        public GenObj GetValue(object fieldName, Dictionary<string, object> variables) {
            if (fieldName != null && variables.ContainsKey(fieldName.ToString()))
                return GenObj.GetGenObj(variables[fieldName.ToString()].ToTrimmedUpperStr());

            return GenObj.GetGenObj(null);
        }

        public void RegisterOrUpdateFormat(Format format) {
            if (_formats.Contains(format))
                _formats.Remove(format);

            _formats.Add(format);
        }

        public object GetFormatByName(string name) {
            return _formats.FirstOrDefault(f => String.Compare(f.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public Func<DataGridViewRow, bool> CreateFunc(string expression) {
            Func<DataGridViewRow, bool> func = row => Eval(expression, row);
            return func;
        }
    }

}