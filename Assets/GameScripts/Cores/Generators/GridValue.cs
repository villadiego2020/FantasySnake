using System.Collections.Generic;
using UnityEngine;

namespace FS.Cores.Generators
{
    [System.Serializable]
    public class GridValue
    {
        public Vector2Int Coordinate;
        public Vector3 Position;
        public List<IBehavior> Members;

        public GridValue(Vector2Int coordinate, Vector3 position)
        {
            Coordinate = coordinate;
            Position = position;
            Members = new List<IBehavior>();
        }

        public bool HasMember
        {
            get => Members.Count > 0;
        }
    }
}