﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vulner.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Vulner.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?php
        ///	$name = &quot;{0}&quot;;
        ///	include &apos;sys/header.php&apos;;
        ///?&gt;
        ///		&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;css/{0}.css&quot;&gt;
        ///		&lt;!--/CSS--&gt;
        ///		&lt;script src=&quot;js/{0}.js&quot;&gt;&lt;/script&gt;
        ///		&lt;!--/JS--&gt;
        ///	&lt;/head&gt;
        ///	&lt;body&gt;
        ///		&lt;div class=&quot;container&quot;&gt;
        ///			Template
        ///		&lt;/div&gt;
        ///	&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string headertemplate {
            get {
                return ResourceManager.GetString("headertemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon ICO {
            get {
                object obj = ResourceManager.GetObject("ICO", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?php
        ///	if (!isset($title)) $title = &apos;&apos;;
        ///?&gt;
        ///&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
        ///	&lt;head&gt;
        ///		&lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=utf-8&quot; /&gt;
        ///		&lt;title&gt;&lt;?php echo $title; ?&gt;&lt;/title&gt;
        ///		&lt;link rel=&quot;stylesheet&quot; href=&quot;https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css&quot;&gt;
        ///		&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;css/main.css&quot;&gt;
        ///		&lt;!--/CS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string phptemplate {
            get {
                return ResourceManager.GetString("phptemplate", resourceCulture);
            }
        }
    }
}
