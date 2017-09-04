using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulner
{
    class UserVars
    {
        Dictionary<string, UserVar> Vars = new Dictionary<string, UserVar>();
        public UserVars()
        {

        }
        public void Set(string s, object o, Type t = null)
        {
            UserVar Var = new UserVar(o);
            if (!Equals(t, null)) Var.SetType(t);
            Vars[s] = Var;
        }
        public UserVar Get(string s)
        {
            return Vars.ContainsKey(s) ? Vars[s] : new UserVar(new Null());
        }
    }
    static class UserVarExp
    {
        public static string[] StringArray(this UserVar[] O)
        {
            return O.Select(t => t.Get<string>()).ToArray();
        }
        public static string[] StringArray(this IEnumerable<UserVar> O)
        {
            return O.Select(t => t.Get<string>()).ToArray();
        }
    }
    class UserVar
    {
        Object Value = null;
        Type ValueType = null;
        public Object Val() { return Value; }
        public UserVar(Object o)
        {
            if (Equals(o, null))
                o = new Null();
            this.Value = o;
            this.ValueType = o.GetType();
        }
        public bool Is(Type T)
        {
            return T == Type();
        }
        public T Get<T>()
        {
            try
            {
                return Funcs.ToType<T>(Value);
            } catch(Exception) { }
            if ( typeof(T) == typeof(String) ) { return Funcs.ToType<T>(Value.ToString()); }
            throw new Exception();
        }
        public object Get()
        {
            try
            {
                return Funcs.ToType<ValueType>(Value);
            }
            catch (Exception) { }
            if (typeof(ValueType) == typeof(String)) { return Value.ToString(); }
            throw new Exception();
        }
        public Type Type()
        {
            return ValueType;
        }
        public void SetType(Type T)
        {
            ValueType = T;
        }
        public bool IsNull()
        {
            return ValueType == typeof(Null);
        }
    }
    struct Null { };
}
