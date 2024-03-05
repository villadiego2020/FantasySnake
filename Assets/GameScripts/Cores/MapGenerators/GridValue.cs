using UnityEngine;

namespace FS.Cores.MapGenerators
{
    public class GridValue
    {
        public Vector2Int Coordinate;
        public Vector3 Position;
        public IBehavior Member;

        public GridValue(Vector2Int coordinate, Vector3 position)
        {
            Coordinate = coordinate;
            Position = position;
            Member = null;
        }

        public bool HasMember
        {
            get => Member != null;
        }
    }
}