using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace Vulner
{
    class Argumenter
    {
        public enum OutputType
        {
            None,
            Write,
            Append,
            Ret,
        }
        public String Escapes = "\'\"`´~";
        public String ParamStr = "-";
        public String SwitcStr = "/";
        public Char EscOnceChar = '\\';
        public String[] Full = new string[80];
        public Object[] Parsed = null;
        public String RawCmd;
        public String Cmd;
        public bool CaseSensitive = true;
        public bool ExpectsOutput = false;

        public Dictionary<int,string[]> FormatStr = new Dictionary<int, string[]>();
        public bool RunCommand = false;
        public UserVar Output = null;
        public OutputType OutType = OutputType.None;
        Main m = null;

        public String[] Switches = new string[0];
        public String[] Params = new string[0];
        public Dictionary<string, bool> Sw = new Dictionary<string, bool>();
        public Dictionary<string, string> Pr = new Dictionary<string, string>();
        public UserVar[] InputVars = null;

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
            bool EscOnce = false;
            for ( int i = 0; i < s.Length; i++ )
            {
                char Ch = s[i];
                if ( Ch == EscOnceChar) { EscOnce = true; continue; }
                if (EscOnce) { Cur += Ch; }
                else
                {
                    if (Escaping)
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
                        }
                        else
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
                                if (Cur.Length > 0)
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
        public string Escape(string s)
        {
            string r = s;
            string[] fc = new string[]
            {
                "\"", "\'", "-", "`", "´", "`", "~"
            };
            foreach( string c in fc )
            {
                r = r.Replace(c, "\\" + c);
            }
            return r;
        }
        UserVar Run(string s, bool ExpectsOutput = false)
        {
            if (s.StartsWith("`") && s.EndsWith("`"))
            {
                if (RunCommand && !Equals(m, null))
                {
                    string cm = s.Substring(1, s.Length - 2);
                    m.HideOutput();
                    m.RunCommand(cm, ExpectsOutput);
                    m.ShowOutput();
                    UserVar r = m.Ret;
                    m.t.KillBuffer();
                    if (r != null && !r.IsNull())
                    {
                        return r;
                    }
                    else
                    {
                        return new UserVar(m.Return);
                    }
                }
            }
            return new UserVar(new Null());
        }
        public void ParseOutput(int i, List<object> Ps)
        {
            if ( i < 0 ) { Output = new UserVar(""); return; }
            string Out = FormatStr[i][1];
            if (Out.StartsWith("`") && Out.EndsWith("`"))
            {
                UserVar v = Run(Out, true);
                Ps.Add(v);
                Output = v;
            }
            else
            {
                Output = new UserVar(FormatStr[i][1]);
            }
        }
        public bool Parse(bool ParseSW = true, bool ParsePR = true)
        {
            List<object> Ps = new List<object>();
            foreach ( string str in Switches )
            {
                Sw[str] = false;
            }
            foreach (string str in Params)
            {
                Pr[str] = string.Empty;
            }
            bool skip = false;
            for (int i = 0; i < Full.Length; i++)
            {
                bool skiponce = false;
                string Cur = Full[i];
                string[] Fcur = FormatStr[i];
                if (i == 0) { Fcur[0] = "Command"; } else if(string.IsNullOrEmpty(Fcur[0])) { Fcur[0] = "Argument"; }
                if (Fcur[1] == ">" || Fcur[1] == ">>" || Fcur[1] == "#>")
                {
                    if (Fcur[1] == "#>")
                    {
                        OutType = OutputType.Ret;
                        Fcur[0] = "Operator";
                        ParseOutput(i + 1, Ps);
                        FormatStr[i + 1][0] = "Output";
                        skip = true;
                        continue;
                    }
                    OutType = Fcur[1] == ">" ? OutputType.Write : OutputType.Append;
                    Fcur[0] = "Operator";
                    if (FormatStr.Count >= i + 2)
                    {
                        ParseOutput(i + 1, Ps);
                        FormatStr[i + 1][0] = "Output";
                    } else
                    {
                        ParseOutput(-1, Ps);
                        FormatStr.Add(FormatStr.Count, new string[] { "Output", "" });
                    }
                    skip = true;
                    continue;
                }
                if ( Fcur[1].StartsWith("`") && Fcur[1].EndsWith("`"))
                {
                    string C = Fcur[1];
                    Ps.Add(Run(C, true));
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
                    Ps.Add(Cur);
                }
            }
            Parsed = Ps.ToArray();
            return true;
        }
        public void Write(string f, UserVar v, UserVar ret = null)
        {
            FileStream fil = null;
            UserVar wr = v;
            if (OutType == OutputType.Write)
            {
                fil = new FileInfo(f).OpenWrite();
                fil.SetLength(0);
            }

            if (OutType == OutputType.Append)
                fil = (FileStream)new FileInfo(f).AppendText().BaseStream;

            if (OutType == OutputType.Ret)
            {
                fil = new FileInfo(f).OpenWrite();
                wr = ret == null ? new UserVar(new Null()) : ret;
            }
            if (v.Is(typeof(string)))
            {
                byte[] bt = v.Get<string>().Select(a => (byte)a).ToArray();
                fil.Write(bt, 0, bt.Length);
            }
            else if (v.Is(typeof(string[])))
            {
                string[] s = v.Get<string[]>();
                foreach (string str in s)
                {
                    byte[] bt = str.Select(a => (byte)a).ToArray();
                    fil.Write(bt, 0, bt.Length);
                    fil.WriteByte((byte)'\n');
                }
            } else if (v.Is(typeof(byte[])))
            {
                byte[] bt = v.Get<byte[]>();
                fil.Write(bt, 0, bt.Length);
            } else if (v.Is(typeof(MemoryStream)))
            {
                byte[] bt = v.Get<MemoryStream>().ToArray();
                fil.Write(bt, 0, bt.Length);
            } else
            {
                byte[] bt = v.Get().ToString().Select(a => (byte)a).ToArray();
                fil.Write(bt, 0, bt.Length);
            }
            fil.Flush();
            fil.Close();
            fil.Dispose();
            fil = null;
        }
        public void WriteOutput(Stream o, UserVar u = null)
        {
            UserVar v = Output;
            if (OutType == OutputType.None) { return; }
            UserVar wr = (u != null && !u.IsNull()) ? u : new UserVar(o);
            if ( !wr.IsNull() )
            {
                List<string> Out = new List<string>();
                if (Output.Is(typeof(String[])))
                {
                    foreach (string s in Output.Get<string[]>())
                    {
                        Out.Add(s);
                    }
                }
                else if (Output.Is(typeof(String)))
                {
                    Out.Add(Output.Get<string>());
                }
                foreach( string a in Out )
                {
                    Write(a, wr, u);
                }
            }
        }
        public string Get(int i)
        {
            try
            {
                string s = Funcs.ToType<string>(Parsed[i]);
                if (CaseSensitive) return Equals( s, null ) ? "" : s;
                return Equals(Parsed[i], null) ? "" : s;
            } catch(Exception)
            {
                return "";
            }
        }
        public T Get<T>(int i)
        {
            return Funcs.ToType<T>(Parsed[i]);
        }
        public Type GetType(int i)
        {
            try
            {
                object o = Parsed[i];
                return o.GetType();
            } catch(Exception) { }
            return new Null().GetType();
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
