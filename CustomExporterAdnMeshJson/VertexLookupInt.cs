using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomExporterAdnMeshJson
{
  /// <summary>
  /// A vertex lookup class to avoid 
  /// duplicate vertex definitions.
  /// </summary>
  class VertexLookupInt : Dictionary<PointInt, int>
  {
    #region PointIntEqualityComparer
    /// <summary>
    /// Define equality for integer-based PointInt.
    /// </summary>
    class PointIntEqualityComparer : IEqualityComparer<PointInt>
    {
      public bool Equals( PointInt p, PointInt q )
      {
        return 0 == p.CompareTo( q );
      }

      public int GetHashCode( PointInt p )
      {
        return ( p.X.ToString()
          + "," + p.Y.ToString()
          + "," + p.Z.ToString() )
          .GetHashCode();
      }
    }
    #endregion // PointIntEqualityComparer

    public VertexLookupInt()
      : base( new PointIntEqualityComparer() )
    {
    }

    /// <summary>
    /// Return the index of the given vertex,
    /// adding a new entry if required.
    /// </summary>
    public int AddVertex( PointInt p )
    {
      return ContainsKey( p )
        ? this[p]
        : this[p] = Count;
    }
  }
}
