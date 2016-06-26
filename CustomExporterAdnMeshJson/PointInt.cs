using System;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// An integer-based 3D point class.
  /// </summary>
  class PointInt : IComparable<PointInt>
  {
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    //public PointInt( int x, int y, int z )
    //{
    //  X = x;
    //  Y = y;
    //  Z = z;
    //}

    const double _feet_to_mm = 25.4 * 12;

    static int ConvertFeetToMillimetres( double d )
    {
      return (int) ( _feet_to_mm * d + 0.5 );
    }

    /// <summary>
    /// Create an integer-based point in millimetres
    /// from a given point in imperial coordinates.
    /// </summary>
    public PointInt( XYZ p )
    {
      X = ConvertFeetToMillimetres( p.X );
      Y = ConvertFeetToMillimetres( p.Y );
      Z = ConvertFeetToMillimetres( p.Z );
    }

    public int CompareTo( PointInt a )
    {
      int d = X - a.X;

      if( 0 == d )
      {
        d = Y - a.Y;

        if( 0 == d )
        {
          d = Z - a.Z;
        }
      }
      return d;
    }
  }
}
