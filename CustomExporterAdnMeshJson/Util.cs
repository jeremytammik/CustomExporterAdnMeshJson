using System;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// Utility methods.
  /// </summary>
  class Util
  {
    /// <summary>
    /// Return a string for a real number
    /// formatted to two decimal places.
    /// </summary>
    public static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    /// <summary>
    /// Return a string for an XYZ point
    /// or vector with its coordinates
    /// formatted to two decimal places.
    /// </summary>
    public static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})",
        RealString( p.X ),
        RealString( p.Y ),
        RealString( p.Z ) );
    }

    /// <summary>
    /// Return the signed volume of the paralleliped 
    /// spanned by the vectors a, b and c. In German, 
    /// this is also known as Spatprodukt.
    /// </summary>
    public static double SignedParallelipedVolume(
      XYZ a,
      XYZ b,
      XYZ c )
    {
      return a.CrossProduct( b ).DotProduct( c );
    }

    /// <summary>
    /// Return true if the three vectors a, b and c 
    /// form a right handed coordinate system, i.e.
    /// the signed volume of the paralleliped spanned 
    /// by them is positive.
    /// </summary>
    public static bool IsRightHanded( XYZ a, XYZ b, XYZ c )
    {
      return 0 < SignedParallelipedVolume( a, b, c );
    }
  }
}
