
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", 
    Justification = "")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Do not directly await a Task", 
    Justification = "No synchronization contexts in .NET Core")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", 
    Justification = "Only used in a mock class")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", 
    Justification = "Need context parameter in this case", Scope = "member", Target = "~M:SecretSanta.Api.Tests.Controllers.GiftControllerTests.ConfigureAutoMapper(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext)")]