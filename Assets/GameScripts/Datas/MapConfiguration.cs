using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Map", menuName = "FS/Configiration/Map", order = 1)]
    public class MapConfiguration : ScriptableObject
    {
        public int SizeX;
        public int SizeY;
        [Range(0,1f)] public float Margin;
        public GameObject GridPrefab;
    }
}