using UnityEngine;

namespace FS.Cores.MapGenerators
{
    public class GridValue
    {
        public Vector2 Coordinate;
        public Vector3 Position;
        public IBehavior Member;

        public GridValue(Vector2 coordinate, Vector3 position)
        {
            Coordinate = coordinate;
            Position = position;
            Member = null;
        }
    }
}