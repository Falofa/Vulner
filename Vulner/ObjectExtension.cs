using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vulner
{
    static class ObjectExtension
    {
        public static IEnumerable<T> Each<T>(this T[] o, Action<T> f)
        {
            return o.Select(g => { f(g); return g; });
        }
        public static IEnumerable<T> Each<T>(this IEnumerable<T> o, Action<T> f)
        {
            return o.Select(g => { f(g); return g; });
        }
        public static string Format(this string s, params object[] o)
        {
            return string.Format(s, o);
        }
        public static string Format(this string s, IEnumerable<object> o)
        {
            return string.Format(s, o);
        }
        public static bool IsNull(object o)
        {
            return Equals(o, null);
        }
        public static T To<T>( this object o )
        {
            return (T)Convert.ChangeType(o, typeof(T));
        }
        public static bool TryOpen(this FileInfo fi, FileMode fm, out FileStream f)
        {
            f = null;
            try
            {
                f = fi.Open(fm);
                return true;
            }
            catch (Exception) { }
            return false;
        }
        public static String Get(this string[] a, int i)
        {
            return a.Length >= i - 1 ? a[i] : "";
        }
        public static T[] ToA<T>(this IEnumerable<T> a)
        {
            return a.ToArray();
        }
        public static string Join(this string s, params object[] o)
        {
            try
            {
                return string.Join(s, o.Select(b => Funcs.ToType<string>(b)).ToA());
            }
            catch (Exception) { }
            return string.Join(s, o.Select(b => b.ToString()).ToA());
        }
        public static string Join(this string s, IEnumerable<object> o)
        {
            try
            {
                return string.Join(s, o.Select(b => Funcs.ToType<string>(b)).ToA());
            }
            catch (Exception) { }
            return string.Join(s, o.Select(b => b.ToString()).ToA());
        }
        public static string Escape(this string a)
        {
            Dictionary<string, string> d = new Dictionary<string, string> {
                { "\\", "\\\\" },
                { "\"", "\\\"" },
                { "\'", "\\\'" },
                { "\n", "\\n" },
            };
            string r = a;
            d.Each(b => r = r.Replace(b.Key, b.Value));
            return r;
        }
        public static string Printable( this IEnumerable<object> o )
        {
            string[] Escaped = o.Select(b => ((string)b).Escape()).ToA();
            string Joined = ", ".Join(o.Select(a => "\"{0}\"".Format(a)).ToArray());
            return Format("[ {0} ]", Joined);
        }
        public static string Replace( this string s, string[] a, string f )
        {
            foreach( string b in a )
            {
                s = s.Replace(b, f);
            }
            return s;
        }
    }
    public class Util
    {
        public static T[] Array<T>(params T[] a)
        {
            return a;
        }
        public static string[] Eq(string s)
        {
            try
            {
                bool wm = s.Split('=').Length > 1;
                string k = s.Substring(0, s.IndexOf('='));
                string v = s.Substring(k.Length + 1);
                if (wm)
                    return Array(k, v);
            } catch(Exception) { }
            return Array<string>();
        }
        public static T Or<T>( T a, T b, bool IfEmpty = false )
        {
            if (Equals(a, null))
            {
                return b;
            }
            if (IfEmpty)
            {
                if ( a.ToString() == string.Empty )
                {
                    return b;
                }
            }
            return a;
        }
    }
}
