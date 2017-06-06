using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Vulner
{
    class AutorunReg
    {
        public RegistryKey r = null;
        public string path = null;
        public string key = null;
        public string val = null;
        public int id = 0;
        public AutorunReg( int i, RegistryKey reg, string p, string k, string v )
        {
            id = i;
            r = reg;
            path = p;
            key = k;
            val = v;
        }

        public bool Delete()
        {
            try
            {
                RegistryKey a = r.OpenSubKey(path, true);
                a.DeleteValue(key);
                a.Close();
            } catch(Exception)
            {
                return false;
            }
            return true;
        }

        public bool Make()
        {
            try
            {
                RegistryKey a = r.OpenSubKey(path, true);
                a.SetValue(key, val);
                a.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Set(string v)
        {
            try
            {
                RegistryKey a = r.OpenSubKey(path, true);
                a.SetValue(key, v);
                val = v;
                a.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static int AutorunList(List<AutorunReg> a, RegistryKey b, string c, int id)
        {
            RegistryKey r = b.OpenSubKey(c);
            foreach (string s in r.GetValueNames())
            {
                AutorunReg y = new AutorunReg(id++, b, c, s, r.GetValue(s).ToString());
                a.Add(y);
            }
            return id;
        }
    }
}
