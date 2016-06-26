using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// A facet normal vector lookup class to avoid
  /// duplicate normal vector definitions.
  /// </summary>
  public class NormalLookupXyz : Dictionary<XYZ, int>
  {
    #region XyzVectorEqualityComparer
    /// <summary>
    /// Define equality for Revit XYZ vectors.
    /// </summary>
    class XyzVectorEqualityComparer : IEqualityComparer<XYZ>
    {
      const double _eps = 1.0e-9;

      public bool Equals( XYZ v, XYZ w )
      {
        return v.IsAlmostEqualTo( w,
          _eps );
      }

      public int GetHashCode( XYZ v )
      {
        return Util.PointString( v ).GetHashCode();
      }
    }
    #endregion // XyzVectorEqualityComparer

    public NormalLookupXyz()
      : base( new XyzVectorEqualityComparer() )
    {
    }

    /// <summary>
    /// Return the index of the given normal vector,
    /// adding a new entry if required.
    /// </summary>
    public int AddNormal( XYZ v )
    {
      return ContainsKey( v )
        ? this[v]
        : this[v] = Count;
    }
  }
}
