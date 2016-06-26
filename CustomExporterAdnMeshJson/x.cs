#region Assembly RevitAPI.dll, v4.0.30319
// C:\Program Files\Autodesk\Revit 2014\RevitAPI.dll
#endregion

using System;

namespace Autodesk.Revit.DB
{
  // Summary:
  //     An interface that is used in custom export to process a Revit model.  An
  //     instance of this class is passed in as a parameter of a CustomExporter. 
  //     The methods herein are then called at times of exporting entities of the
  //     model.
  public interface IExportContext
  {
    // Summary:
    //     This method is called at the very end of the export proces, after all entities
    //     were processed (or after the process was cancelled).
    void Finish();
    //
    // Summary:
    //     This method is queried at the begining of every element.
    //
    // Returns:
    //     Return True if you wish to cancel the exporting process, or False otherwise.
    bool IsCanceled();
    //
    // Summary:
    //     This method marks the beginning of export of a daylight portal.
    //
    // Parameters:
    //   node:
    //     A node describing the daylight portal object.
    void OnDaylightPortal( DaylightPortalNode node );
    //
    // Summary:
    //     This method marks the beginning of an element to be exported
    //
    // Parameters:
    //   elementId:
    //     The Id of the element that is about to be processed
    //
    // Returns:
    //     Return RenderNodeAction.Skip if you wish to skip exporting this element,
    //     or return RenderNodeAction.Proceed otherwise.
    RenderNodeAction OnElementBegin( ElementId elementId );
    //
    // Summary:
    //     This method marks the end of an element being exported
    //
    // Parameters:
    //   elementId:
    //     The Id of the element that has just been processed
    void OnElementEnd( ElementId elementId );
    //
    // Summary:
    //     This method marks the beginning of a Face to be exported
    //
    // Parameters:
    //   node:
    //     An output node that represents a Face.
    //
    // Returns:
    //     Return RenderNodeAction.Proceed if you wish to receive geometry (polymesh)
    //     for this face, or return RenderNodeAction.Skip otherwise.
    //
    // Remarks:
    //     Note that OnFaceBeging (as well as OnFaceEnd) is called only if the custom
    //     exporter was set up to include faces in the output stream.  See CustomExporter.IncudeFaces
    //     for mode details.
    RenderNodeAction OnFaceBegin( FaceNode node );
    //
    // Summary:
    //     This method marks the end of the current face being exported.
    //
    // Parameters:
    //   node:
    //     An output node that represents a Face.
    void OnFaceEnd( FaceNode node );
    //
    // Summary:
    //     This method marks the beginning of a family instance to be exported
    //
    // Returns:
    //     Return RenderNodeAction.Skip if you wish to skip processing this family instance,
    //     or return RenderNodeAction.Proceed otherwise.
    RenderNodeAction OnInstanceBegin( InstanceNode node );
    //
    // Summary:
    //     This method marks the end of a family instance being exported
    //
    // Parameters:
    //   node:
    //     An output node that represents a family instance.
    void OnInstanceEnd( InstanceNode node );
    //
    // Summary:
    //     This method marks the beginning of export of a light object.
    //
    // Parameters:
    //   node:
    //     A node describing the light object.
    void OnLight( LightNode node );
    //
    // Summary:
    //     This method marks the beginning of a link instance to be exported.
    //
    // Returns:
    //     Return RenderNodeAction.Skip if you wish to skip processing this link instance,
    //     or return RenderNodeAction.Proceed otherwise.
    RenderNodeAction OnLinkBegin( LinkNode node );
    //
    // Summary:
    //     This method marks the end of a link instance being exported.
    //
    // Parameters:
    //   node:
    //     An output node that represents a Revit link.
    void OnLinkEnd( LinkNode node );
    //
    // Summary:
    //     This method marks a change of the material.
    //
    // Parameters:
    //   node:
    //     A node describing the current material.
    void OnMaterial( MaterialNode node );
    //
    // Summary:
    //     This method is called when a tessellated polymesh of a 3d face is being output.
    //
    // Parameters:
    //   node:
    //     A node representing topology of the polymesh
    void OnPolymesh( PolymeshTopology node );
    //
    // Summary:
    //     This method marks the beginning of export of an RPC object.
    //
    // Parameters:
    //   node:
    //     A node with asset information about the RPC object.
    void OnRPC( RPCNode node );
    //
    // Summary:
    //     This method marks the beginning of a 3D view to be exported
    //
    // Parameters:
    //   node:
    //     Geometry node associated with the view
    //
    // Returns:
    //     Return RenderNodeAction.Skip if you wish to skip exporting this view, or
    //     return RenderNodeAction.Proceed otherwise.
    RenderNodeAction OnViewBegin( ViewNode node );
    //
    // Summary:
    //     This method marks the end of a 3D view being exported
    //
    // Parameters:
    //   elementId:
    //     The Id of the 3D view that has just been processed
    void OnViewEnd( ElementId elementId );
    //
    // Summary:
    //     This method is called at the very start of the export proces, still before
    //     the first entity of the model was send out.
    //
    // Returns:
    //     Return True if you are ready to proceed with processing the export.
    bool Start();
  }
}
