using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Map", menuName = "FS/Configiration/Map", order = 1)]
    public class MapConfiguration : ScriptableObject
    {
        [Range(1, 100)] public int CellSize;
        public int SizeX;
        public int SizeY;
        public float Margin;
        public GameObject GridPrefab;
    }
}