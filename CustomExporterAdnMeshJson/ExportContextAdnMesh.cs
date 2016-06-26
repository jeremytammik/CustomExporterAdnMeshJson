using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// Custom exporter IExportContext implementation to 
  /// capture ADN mesh data.
  /// </summary>
  class ExportContextAdnMesh : IExportContext
  {
    Document _doc;

    /// <summary>
    /// Stack of transformations for 
    /// link and instance elements.
    /// </summary>
    Stack<Transform> _transformationStack
      = new Stack<Transform>();

    /// <summary>
    /// List of triangle vertices.
    /// </summary>
    VertexLookupInt _vertices = new VertexLookupInt();

    /// <summary>
    /// List of triangles, defined as 
    /// triples of vertex indices.
    /// </summary>
    List<int> _triangles = new List<int>();

    /// <summary>
    /// List of normal vectors, defined by an index 
    /// into the normal lookup for each triangle vertex.
    /// </summary>
    List<int> _normalIndices = new List<int>();

    NormalLookupXyz _normals = new NormalLookupXyz();

    /// <summary>
    /// Calculate center of gravity of current element.
    /// </summary>
    CentroidVolume _centroid_volume
      = new CentroidVolume();

    Color _color;
    double _transparency;
    List<AdnMeshData> _data;

    public ExportContextAdnMesh( Document doc )
    {
      _doc = doc;
      _data = new List<AdnMeshData>();
      _transformationStack.Push( Transform.Identity );
    }

    public AdnMeshData[] MeshData
    {
      get
      {
        return _data.ToArray();
      }
    }

    Transform CurrentTransform
    {
      get
      {
        return _transformationStack.Peek();
      }
    }

    /// <summary>
    /// Store a triangle, adding new vertices for it
    /// to our vertex lookup dictionary if needed and
    /// accumulating its volume and centroid contribution.
    /// </summary>
    void StoreTriangle(
      IList<XYZ> vertices,
      PolymeshFacet triangle,
      XYZ normal )
    {
      // Retrieve the three triangle vertices

      Transform currentTransform = CurrentTransform;

      XYZ[] p = new XYZ[] {
        currentTransform.OfPoint( vertices[triangle.V1] ),
        currentTransform.OfPoint( vertices[triangle.V2] ),
        currentTransform.OfPoint( vertices[triangle.V3] )
      };

      // Ensure the three are ordered counter-clockwise

      //XYZ v = p[1] - p[0];
      //XYZ w = p[2] - p[0];
      
      //Debug.Assert( Util.IsRightHanded( v, w, normal ),
      //  "expected counter-clockwise vertex order" );

      // Centroid and volume calculation

      _centroid_volume.AddTriangle( p );

      // Store vertex, facet and normals

      for( int i = 0; i < 3; ++i )
      {
        PointInt q = new PointInt( p[i] );

        _triangles.Add( _vertices.AddVertex( q ) );

        _normalIndices.Add( _normals.AddNormal(
          currentTransform.OfVector( normal ) ) );
      }
    }

    public void Finish()
    {
      Debug.Print( "Finish" );
    }

    public bool IsCanceled()
    {
      return false;
    }

    public void OnDaylightPortal(
      DaylightPortalNode node )
    {
      throw new NotImplementedException();
    }

    public RenderNodeAction OnElementBegin(
      ElementId elementId )
    {
      string s = elementId.IntegerValue.ToString();

      Debug.Print( "ElementBegin id " + s );

      _vertices.Clear();
      _triangles.Clear();
      _normals.Clear();
      _normalIndices.Clear();
      _centroid_volume.Init();

      return RenderNodeAction.Proceed;
    }

    public void OnElementEnd( ElementId elementId )
    {
      Debug.Print( "ElementEnd" );

      // Set centroid coordinates to their final value

      _centroid_volume.Complete();

      string metadataId = _doc.GetElement(
        elementId ).UniqueId;

      AdnMeshData meshData = new AdnMeshData(
        _vertices, _triangles, _normals, _normalIndices,
        new PointInt( _centroid_volume.Centroid ),
        _color, _transparency, metadataId );

      _data.Add( meshData );
    }

    public RenderNodeAction OnFaceBegin( FaceNode node )
    {
      throw new NotImplementedException();
    }

    public void OnFaceEnd( FaceNode node )
    {
      throw new NotImplementedException();
    }

    public RenderNodeAction OnInstanceBegin(
      InstanceNode node )
    {
      FamilySymbol symbol = _doc.GetElement(
        node.GetSymbolId() ) as FamilySymbol;

      Debug.Assert( null != symbol,
        "expected valid family symbol" );

      Debug.Print( "InstanceBegin "
        + symbol.Category.Name + " : "
        + symbol.Family.Name + " : "
        + symbol.Name );

      _transformationStack.Push( CurrentTransform
        .Multiply( node.GetTransform() ) );

      return RenderNodeAction.Proceed;
    }

    public void OnInstanceEnd( InstanceNode node )
    {
      Debug.Print( "InstanceEnd" );

      _transformationStack.Pop();
    }

    public void OnLight( LightNode node )
    {
      throw new NotImplementedException();
    }

    public RenderNodeAction OnLinkBegin( LinkNode node )
    {
      _transformationStack.Push( CurrentTransform
        .Multiply( node.GetTransform() ) );

      throw new NotImplementedException();
    }

    public void OnLinkEnd( LinkNode node )
    {
      _transformationStack.Pop();

      throw new NotImplementedException();
    }

    public void OnMaterial( MaterialNode node )
    {
      Color c = node.Color;
      double t = node.Transparency;

      string s = string.Format( "({0},{1},{2})",
        c.Red, c.Green, c.Blue );

      Debug.Print( "Colour " + s + ", transparency "
        + t.ToString( "0.##" ) );

      _color = c;
      _transparency = t;
    }

    public void OnPolymesh( PolymeshTopology node )
    {
      int nPts = node.NumberOfPoints;
      int nFacets = node.NumberOfFacets;

      DistributionOfNormals distrib
        = node.DistributionOfNormals;

      Debug.Print( string.Format(
        "Polymesh {0} vertices {1} facets",
        nPts, nFacets ) );

      int iFacet = 0;
      int iPoint = 0;

      IList<XYZ> vertices = node.GetPoints();
      IList<XYZ> normals = node.GetNormals();
      XYZ normal;

      foreach( PolymeshFacet triangle in node.GetFacets() )
      {
        // Just grab one normal per facet; ignore the 
        // three normals per point if they differ.

        if( DistributionOfNormals.OnePerFace == distrib )
        {
          normal = node.GetNormal( 0 );
        }
        else if( DistributionOfNormals.OnEachFacet
          == distrib )
        {
          normal = node.GetNormal( iFacet++ );
        }
        else
        {
          Debug.Assert( DistributionOfNormals
            .AtEachPoint == distrib, "what else?" );

          normal = node.GetNormal( triangle.V1 )
            + node.GetNormal( triangle.V2 )
            + node.GetNormal( triangle.V3 );
          normal /= 3.0;
        }

        StoreTriangle( vertices, triangle, normal );
      }
    }

    public void OnRPC( RPCNode node )
    {
      throw new NotImplementedException();
    }

    public RenderNodeAction OnViewBegin( ViewNode node )
    {
      View3D view = _doc.GetElement( node.ViewId )
        as View3D;

      Debug.Assert( null != view,
        "expected valid 3D view" );

      Debug.Print( "ViewBegin " + view.Name );

      return RenderNodeAction.Proceed;
    }

    public void OnViewEnd( ElementId elementId )
    {
      Debug.Print( "ViewEnd" );
    }

    public bool Start()
    {
      Debug.Print( "Start" );
      return true;
    }
  }
}
