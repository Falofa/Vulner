using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Vulner
{
    class Argumenter
    {
        public String Escapes = "\'\"`´~";
        public String ParamStr = "-";
        public String SwitcStr = "/";
        public String[] Full = new string[80];
        public String[] Parsed = new string[80];
        public String RawCmd;
        public String Cmd;
        public bool CaseSensitive = true;

        public Dictionary<int,string[]> FormatStr = new Dictionary<int, string[]>();
        public bool RunCommand = false;
        public string Output = null;
        Main m = null;

        public String[] Switches = new string[0];
        public String[] Params = new string[0];
        public Dictionary<string, bool> Sw = new Dictionary<string, bool>();
        public Dictionary<string, string> Pr = new Dictionary<string, string>();

        public void SetM(Main M) { m = M; }

        public Argumenter( string s, bool commands = false )
        {
            RunCommand = commands;
            RawCmd = s;
            Cmd = Environment.ExpandEnvironmentVariables(s);
            int k = 0;
            string Cur = "";
            string Fcur = "";
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
                            FormatStr[k] = new string[] { "", string.Format(Fcur, Cur) };
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
                        Fcur = string.Format("{0}{1}{0}", EscChar, "{0}");
                    }
                    else
                    {
                        if (Ch.ToString().Trim().Length == 0)
                        {
                            if ( Cur.Length > 0 )
                            {
                                FormatStr[k] = new string[] { "", Cur };
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
                        FormatStr[k] = new string[] { "", Cur };
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
            bool skip = false;
            for (int i = 0; i < Full.Length; i++)
            {
                bool skiponce = false;
                string Cur = Full[i];
                string[] Fcur = FormatStr[i];
                if (i == 0) { Fcur[0] = "Command"; } else if(string.IsNullOrEmpty(Fcur[0])) { Fcur[0] = "Argument"; }
                if (Fcur[1] == ">")
                {
                    Fcur[0] = "Operator";
                    if (FormatStr.Count >= i + 2)
                    {
                        Output = FormatStr[i + 1][1];
                        FormatStr[i + 1][0] = "Output";
                    } else
                    {
                        Output = "";
                        FormatStr.Add(FormatStr.Count, new string[] { "Output", "" });
                    }
                    skip = true;
                    continue;
                }
                if ( Fcur[1].StartsWith("`") && Fcur[1].EndsWith("`"))
                {
                    string C = Fcur[1];
                    if (RunCommand && !Equals(m, null)) {
                        m.t.StartBuffer(true);
                        string cm = Fcur[1].Substring(1, Fcur[1].Length-2);
                        m.RunCommand(cm);
                        Parsed[j++] = m.t.EndBuffer();
                    } else
                    {
                        Parsed[j++] = Fcur[1].Substring(1, Fcur.Length - 2);
                    }
                    skiponce = true;
                }
                if (!skiponce && !skip)
                {
                    if (Cur.StartsWith(SwitcStr) && ParseSW)
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
                    }
                    else if (Cur.StartsWith(ParamStr) && ParsePR)
                    {
                        try
                        {
                            string s = Cur.Substring(ParamStr.Length).ToLower();
                            if (Pr[s] == string.Empty)
                            {
                                if (FormatStr[i + 1][1] == ">")
                                {
                                    return false;
                                }
                                Pr[s] = Full[i + 1];
                                i = i + 1;
                                continue;
                            }
                        }
                        catch (Exception) { return false; }
                    }
                    Parsed[j++] = Cur;
                }
            }
            Parsed = Parsed.Select(t => Environment.ExpandEnvironmentVariables(!Equals(t, null) ? t : "")).Take(j).ToArray();
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
