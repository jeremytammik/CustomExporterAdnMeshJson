using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// The data format specifying one solid for the 
  /// WebGL viewer, defining its centre, colour, id,
  /// triangular facets, their vertex coordinates, 
  /// indices and normals.
  /// </summary>
  class AdnMeshData
  {
    int FacetCount { get; set; } // optional
    int VertexCount { get; set; } // optional
    int[] VertexCoords { get; set; }
    int[] VertexIndices { get; set; } // triangles
    double[] Normals { get; set; }
    int[] NormalIndices { get; set; } // not optional, one normal per vertex
    int[] Center { get; set; }
    int Color { get; set; }
    string Id { get; set; }

    /// <summary>
    /// Apply this factor to all point data when 
    /// saving to JSON to accomodate the expected
    /// scaling.
    /// </summary>
    const double _export_factor = 0.002;

    public AdnMeshData(
      VertexLookupInt vertices,
      List<int> vertexIndices,
      NormalLookupXyz normals,
      List<int> normalIndices,
      PointInt center,
      Color color,
      double transparency,
      string id )
    {
      int n = vertexIndices.Count;

      Debug.Assert( 0 == (n % 3), 
        "expected triples of 3D point vertex indices" );

      Debug.Assert( normalIndices.Count == n, 
        "expected a normal for each vertex" );

      FacetCount = n / 3;

      n = vertices.Count;
      VertexCount = n;
      VertexCoords = new int[n * 3];
      int i = 0;
      foreach( PointInt p in vertices.Keys )
      {
        VertexCoords[i++] = p.X;
        VertexCoords[i++] = p.Y;
        VertexCoords[i++] = p.Z;
      }
      VertexIndices = vertexIndices.ToArray();

      n = normals.Count;
      Normals = new double[n * 3];
      i = 0;
      foreach( XYZ v in normals.Keys )
      {
        Normals[i++] = v.X;
        Normals[i++] = v.Y;
        Normals[i++] = v.Z;
      }
      NormalIndices = normalIndices.ToArray();

      Center = new int[3];
      i = 0;
      Center[i++] = center.X;
      Center[i++] = center.Y;
      Center[i] = center.Z;

      byte alpha = (byte) ( 
        ( 100 - transparency ) * 2.55555555 );

      Color = ConvertClr( 
        color.Red, color.Green, color.Blue, alpha );

      Id = id;
    }

    /// <summary>
    /// Convert colour and transparency to 
    /// the required integer format.
    /// </summary>
    static int ConvertClr( byte r, byte g, byte b, byte a )
    {
      return ( r << 24 ) + ( g << 16 ) + ( b << 8 ) + a;
    }

    public string ToJson()
    {
      // I did think of using a JSON serialiser, 
      // either one of these two provided by the
      // .NET framework or one of the other libraries:
      // System.Runtime.Serialization.Json.DataContractJsonSerializer 
      // System.Web.Script.Serialization.JavaScriptSerializer
      // However, reading this comparison and alternative 
      // implementation, I decided to just write the couple 
      // of lines myself.
      // http://procbits.com/2011/08/11/fridaythe13th-the-best-json-parser-for-silverlight-and-net

      string s = string.Format
        ( "\n \"FacetCount\":{0},"
        + "\n \"VertexCount\":{1},"
        + "\n \"VertexCoords\":[{2}],"
        + "\n \"VertexIndices\":[{3}],"
        + "\n \"Normals\":[{4}],"
        + "\n \"NormalIndices\":[{5}],"
        + "\n \"Center\":[{6}],"
        + "\n \"Color\":[{7}],"
        + "\n \"Id\":\"{8}\"",
        FacetCount, 
        VertexCount,
        string.Join( ",", VertexCoords.Select<int, string>( i => ( _export_factor * i ).ToString( "0.#" ) ).ToArray() ),
        string.Join( ",", VertexIndices.Select<int, string>( i => i.ToString() ).ToArray() ),
        string.Join( ",", Normals.Select<double, string>( a => a.ToString( "0.####" ) ).ToArray() ),
        string.Join( ",", NormalIndices.Select<int, string>( i => i.ToString() ) ),
        string.Join( ",", Center.Select<int, string>( i => ( _export_factor * i ).ToString( "0.#" ) ) ),
        Color,
        Id );

      return "\n{" + s + "\n}";
    }
  }
}
