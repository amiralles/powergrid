namespace PowerGrid.Component {
    using System;
    using System.Reflection;

    //Wrappers around Roslyn APIs. (not merged with this expample yet)
    internal class ScriptEngine {
        public Session CreateSession(ConditionalFormatEngine engine, Type type) {
            return new Session();
        }
    }

    internal class Session {
        public void AddReference(Assembly assembly) {

        }

        public bool Execute(string toCSharp) {
            return false;
        }
    }
    //
}