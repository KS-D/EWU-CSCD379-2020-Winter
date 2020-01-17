// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppression either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

//API
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Not Relevant because we are writing general purpose code", Scope = "member", Target = "~M:SecretSanta.Api.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Not relevant for class assignment",Scope = "member", Target = "~M:SecretSanta.Api.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
//Business
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "The Api laid out specify url as a string", Scope = "member", Target = "~M:SecretSanta.Business.Gift.#ctor(System.Int32,System.String,System.String,System.String,SecretSanta.Business.User)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "The Api Laid out specify url as a string", Scope = "member", Target = "~P:SecretSanta.Business.Gift.Url")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "The Assignment specified gifts as a get, set property", Scope = "member", Target = "~P:SecretSanta.Business.User.Gifts")]
//Web
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Not relevant because we are writing general purpose code", Scope = "member", Target = "~M:SecretSanta.Web.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Not Relevant for assignment", Scope = "member", Target = "~M:SecretSanta.Web.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
//Tests
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "The Naming convention for Test methods is to include underscores", Scope = "namespaceanddescendants", Target ="SecretSanta.Business.Tests")]

