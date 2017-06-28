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
    class UserVar
    {
        Object Value = null;
        Type ValueType = null;
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
            return Funcs.ToType<T>(Value);
        }
        public object Get()
        {
            return Funcs.ToType<ValueType>(Value);
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
