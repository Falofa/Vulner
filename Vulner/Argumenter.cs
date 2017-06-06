using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Vulner
{
    public class Argumenter
    {
        public String Escapes = "\'\"`´~";
        public String ParamStr = "-";
        public String SwitcStr = "/";
        public String[] Full = new string[80];
        public String[] Parsed = new string[80];
        public String RawCmd;
        public String Cmd;
        public String FullArg;
        public bool CaseSensitive = true;

        public String[] Switches = new string[0];
        public String[] Params = new string[0];
        public Dictionary<string, bool> Sw = new Dictionary<string, bool>();
        public Dictionary<string, string> Pr = new Dictionary<string, string>();

        public Argumenter( string s )
        {
            RawCmd = s;
            Cmd = Environment.ExpandEnvironmentVariables(s);
            FullArg = Cmd.Substring(new Regex("^[a-z]+", RegexOptions.IgnoreCase).Match(Cmd.Trim()).Value.Length).TrimStart();
            int k = 0;
            string Cur = "";
            bool Escaping = false;
            char EscChar = (char)0;
            for ( int i = 0; i < s.Length; i++ )
            {
                char Ch = s[i];
                if ( Escaping )
                {
                    if (Escapes.Contains(Ch) && Ch == EscChar)
                    {
                        Escaping = false;
                        EscChar = (char)0;
                        {
                            Full[k++] = Cur;
                            Cur = "";
                        }
                    } else
                    {
                        Cur += Ch;
                    }
                }
                else
                {
                    if (Escapes.Contains(Ch) && Cur.Length == 0)
                    {
                        Escaping = true;
                        EscChar = Ch;
                    }
                    else
                    {
                        if (Ch.ToString().Trim().Length == 0)
                        {
                            if ( Cur.Length > 0 )
                            {
                                Full[k++] = Cur;
                                Cur = "";
                            }
                        }
                        else
                        {
                            Cur += Ch;
                        }
                    }
                }
                if ( i == s.Length-1 )
                {
                    if ( Cur.Length != 0 )
                    {
                        Full[k++] = Cur;
                        break;
                    }
                }
            }
            Full = Full.TakeWhile(t => t != null).ToArray();
        }
        public bool Parse(bool ParseSW = true, bool ParsePR = true)
        {
            foreach( string str in Switches )
            {
                Sw[str] = false;
            }
            foreach (string str in Params)
            {
                Pr[str] = string.Empty;
            }
            int j = 0;
            for ( int i = 0; i < Full.Length; i++ )
            {
                string Cur = Full[i];
                if (Cur.StartsWith( SwitcStr ) && ParseSW)
                {
                    try
                    {
                        string s = Cur.Substring(SwitcStr.Length).ToLower();
                        if (Switches.Contains(s))
                        {
                            Sw[s] = true;
                            continue;
                        }
                    }
                    catch (Exception) { return false; }
                } else if ( Cur.StartsWith(ParamStr) && ParsePR)
                {
                    try
                    {
                        string s = Cur.Substring(ParamStr.Length).ToLower();
                        if (Pr[s] == string.Empty)
                        {
                            Pr[s] = Full[i + 1];
                            i = i + 1;
                            continue;
                        }
                    }
                    catch (Exception) { return false; }
                }
                Parsed[j++] = Cur;
            }
            Parsed = Parsed.Where(t => t != string.Empty).ToArray();
            return true;
        }
        public string Get(int i)
        {
            try
            {
                if (CaseSensitive) return Equals( Parsed[i], null ) ? "" : Parsed[i];
                return Equals(Parsed[i], null) ? "" : Parsed[i].ToLower();
            } catch(Exception)
            {
                return "";
            }
        }
        public string GetRaw(int i)
        {
            try
            {
                if ( CaseSensitive ) return Full[i];
                return Full[i].ToLower();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string GetPr(string s, string def = "")
        {
            try
            {
                if (CaseSensitive) return Pr[s];
                return Pr[s].ToLower();
            }
            catch (Exception)
            {
                return def;
            }
        }
        public bool GetSw(string s)
        {
            try
            {
                return Sw[s];
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsSetPr(string s)
        {
            try
            {
                return Pr[s] != string.Empty;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
