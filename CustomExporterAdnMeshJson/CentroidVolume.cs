using System;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// Calculate and store the centroid and volume 
  /// from a set of triangular facets.
  /// </summary>
  class CentroidVolume
  {
    XYZ _centroid;
    double _volume;

    public CentroidVolume()
    {
      Init();
    }

    public void Init()
    {
      _centroid = XYZ.Zero;
      _volume = 0.0;
    }

    public void AddTriangle( XYZ[] p )
    {
      double vol
        = p[0].X * ( p[1].Y * p[2].Z - p[2].Y * p[1].Z )
        + p[0].Y * ( p[1].Z * p[2].X - p[2].Z * p[1].X )
        + p[0].Z * ( p[1].X * p[2].Y - p[2].X * p[1].Y );

      _centroid += vol * ( p[0] + p[1] + p[2] );
      _volume += vol;
    }

    /// <summary>
    /// Set centroid coordinates and volume 
    /// to their final values when completed.
    /// </summary>
    public void Complete()
    {
      _centroid /= 4 * _volume;
      _volume /= 6;
    }

    public XYZ Centroid
    {
      get
      {
        return _centroid;
      }
    }

    public double Volume
    {
      get
      {
        return _volume;
      }
    }

    override public string ToString()
    {
      return Util.RealString( _volume ) + "@"
        + Util.PointString( _centroid );
    }
  }
}
