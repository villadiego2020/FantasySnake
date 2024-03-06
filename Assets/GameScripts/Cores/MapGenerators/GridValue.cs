using System.Collections.Generic;
using UnityEngine;

namespace FS.Cores.MapGenerators
{
    [System.Serializable]
    public class GridValue
    {
        public Vector2Int Coordinate;
        public Vector3 Position;
        //public IBehavior Member;
        public List<IBehavior> Members;

        public GridValue(Vector2Int coordinate, Vector3 position)
        {
            Coordinate = coordinate;
            Position = position;
            //Member = null;
            Members = new List<IBehavior>();
        }

        public bool HasMember
        {
            //get => Member != null;
            get => Members.Count > 0;
        }
    }
}