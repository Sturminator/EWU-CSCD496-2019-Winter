
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1060:Move pinvokes to native methods class", 
    Justification = "Unmanaged code. Not concerned with calls to it.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", 
    Justification = "Unmanaged code. Not concerned with calls to it.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Do not directly await a Task", 
    Justification = "No synchronization contexts in .NET Core")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", 
    Justification = "Only meant to match HTTP verbs with the method name")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", 
    Justification = "Need EF to be able to Map, won't work with URI")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", 
    Justification = "Automapper needs property to be mutable")]
