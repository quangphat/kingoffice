﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Entity.errors", typeof(errors).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to error_login_expected.
        /// </summary>
        public static string error_login_expected {
            get {
                return ResourceManager.GetString("error_login_expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to invalid_data.
        /// </summary>
        public static string invalid_data {
            get {
                return ResourceManager.GetString("invalid_data", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to invalid_id.
        /// </summary>
        public static string invalid_id {
            get {
                return ResourceManager.GetString("invalid_id", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to invalid_username_or_pass.
        /// </summary>
        public static string invalid_username_or_pass {
            get {
                return ResourceManager.GetString("invalid_username_or_pass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to missing_data.
        /// </summary>
        public static string missing_data {
            get {
                return ResourceManager.GetString("missing_data", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to username_or_password_must_not_be_empty.
        /// </summary>
        public static string username_or_password_must_not_be_empty {
            get {
                return ResourceManager.GetString("username_or_password_must_not_be_empty", resourceCulture);
            }
        }
    }
}
