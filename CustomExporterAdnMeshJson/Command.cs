#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.IO;
#endregion

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// ADN mesh data custom exporter 
  /// external command mainline.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Command : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      // This command requires an active document

      if( null == uidoc )
      {
        message = "Please run this command in an active project document.";
        return Result.Failed;
      }

      View3D view = doc.ActiveView as View3D;

      if( null == view )
      {
        message = "Please run this command in a 3D view.";
        return Result.Failed;
      }

      // Instantiate our custom context

      ExportContextAdnMesh context 
        = new ExportContextAdnMesh( doc );

      // Instantiate a custom exporter with it

      using( CustomExporter exporter
        = new CustomExporter( doc, context ) )
      {
        // Tell the exporter whether we need face info.
        // If not, it is better to exclude them, since 
        // processing faces takes significant time and 
        // memory. In any case, tessellated polymeshes
        // can be exported (and will be sent to the 
        // context). Excluding faces just excludes the calls, 
        // not the actual processing of face tessellation. 
        // Meshes of the faces will still be received by 
        // the context.

        exporter.IncludeFaces = false;

        exporter.Export( view );
      }

      // Save ADN mesh data in JSON format

      StreamWriter s = new StreamWriter( 
        "C:/tmp/test.json" );

      s.Write( "[" );

      int i = 0;

      foreach( AdnMeshData d in context.MeshData )
      {
        if( 0 < i ) { s.Write( ',' ); }

        s.Write( d.ToJson() );

        ++i;
      }

      s.Write( "\n]\n" );
      s.Close();

      return Result.Succeeded;
    }
  }
}
