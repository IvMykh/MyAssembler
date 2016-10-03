﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAssembler.Core.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyAssembler.Core.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &quot;{0}&quot;: such command does not exist (line {1}, index {2})..
        /// </summary>
        internal static string CommandNotExistErrorMsgFormat {
            get {
                return ResourceManager.GetString("CommandNotExistErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot;: command is not implemented yet (line {1}, index {2})..
        /// </summary>
        internal static string CommandNotImplementedErrorMsgFormat {
            get {
                return ResourceManager.GetString("CommandNotImplementedErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;.
        /// </summary>
        internal static string CommentStartSymbol {
            get {
                return ResourceManager.GetString("CommentStartSymbol", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot;: such directive does not exist (line {1}, index {2})..
        /// </summary>
        internal static string DirectiveNotExistErrorMsgFormat {
            get {
                return ResourceManager.GetString("DirectiveNotExistErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot;: directive is not implemented yet (line {1}, index {2})..
        /// </summary>
        internal static string DirectiveNotImplementedErrorMsgFormat {
            get {
                return ResourceManager.GetString("DirectiveNotImplementedErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Line {0}: the line is neither command, nor directive..
        /// </summary>
        internal static string IncorrectLineErrorMsgFormat {
            get {
                return ResourceManager.GetString("IncorrectLineErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Line {0}: undefined parse error occurred..
        /// </summary>
        internal static string UndefinedParseErrorMsgFormat {
            get {
                return ResourceManager.GetString("UndefinedParseErrorMsgFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot;: unexpected token (line {1}, index {2})..
        /// </summary>
        internal static string UnexpectedTokenErrorMsgFormat {
            get {
                return ResourceManager.GetString("UnexpectedTokenErrorMsgFormat", resourceCulture);
            }
        }
    }
}
