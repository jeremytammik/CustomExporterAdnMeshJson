using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "CustomExporterAdnMeshJson" )]
[assembly: AssemblyDescription( "C# .NET Revit API add-in geometry CustomExporter to ADN mesh JSON format" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "Autodesk Inc." )]
[assembly: AssemblyProduct( "CustomExporterAdnMeshJson" )]
[assembly: AssemblyCopyright( "Copyright 2013-2016 © Jeremy Tammik Autodesk Inc." )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "321044f7-b0b2-4b1c-af18-e71a19252be0" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//
// History:
//
// 2013-07-11 2014.0.0.0 initial version for Revit 2014
// 2016-06-26 2017.0.0.0 flat migrtion to Revit 2017
// 2016-06-26 2017.0.0.1 set IncludeGeometricObjects and successful test
// 2016-06-26 2017.0.0.2 removed throw NotImplementedException in OnLinkBegin and OnLinkEnd
// 2016-06-26 2017.0.0.3 wrap call to exporter.Export in an exception handler and all is well
//
[assembly: AssemblyVersion( "2017.0.0.3" )]
[assembly: AssemblyFileVersion( "2017.0.0.3" )]
