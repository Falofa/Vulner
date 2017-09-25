using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Vulner
{
    abstract internal class Autorun
    {
        public virtual string Name { get; set; }
        public virtual string Lt { get; set; }
        public virtual string Key { get; set; }
        public virtual string Data { get; set; }
        public virtual bool CanModify { get; set; }
        public virtual bool CanDelete { get; set; }
        public virtual bool Invalid { get; set; }
        public abstract bool Remove();
        public abstract bool Modify(string New);
        //public abstract Autorun();
    }
    #region Registry Auto Run
    class AutorunFetch
    {
        public static List<RegAutorun> Fetch( RegistryKey Reg, string Path, string Letters )
        {
            RegistryKey r = Reg.OpenSubKey(Path);
            List<RegAutorun> l = new List<RegAutorun>();
            foreach( string s in r.GetValueNames() )
            {
                bool cd = true;
                if (Registry.LocalMachine == Reg && !Funcs.IsAdmin()) cd = false;
                RegAutorun a = new RegAutorun()
                {
                    Reg = r,
                    BaseReg = Reg,
                    KeyPath = Path,
                    Key = s,
                    Data = (string)r.GetValue(s),
                    CanDelete = cd,
                    CanModify = cd,
                    Invalid = false,
                    Lt = Letters,
                    Name = s
                };
                l.Add(a);
            }
            return l;
        }
        public static List<RegAutorun> Fetch(RegDef r)
        {
            return Fetch(r.Reg, r.Path, r.Letters);
        }
    }
    class RegAutorun : Autorun
    {
        public RegistryKey Reg { get; set; }
        public string KeyPath { get; set; }
        public RegistryKey BaseReg { get; set; }
        public override bool Remove()
        {
            try
            {
                BaseReg.OpenSubKey(KeyPath, true).DeleteValue(Key);
                Invalid = true;
                return true;
            }
            catch (Exception) { }
            return false;
        }
        public override bool Modify(string New)
        {
            try
            {
                BaseReg.OpenSubKey(KeyPath, true).SetValue(Key, New);
                Data = New;
                return true;
            }
            catch (Exception) { }
            return false;
        }
    }
    struct RegDef
    {
        public RegistryKey Reg;
        public string Path;
        public string Letters;
    }
    #endregion
}
