using UnityEngine;

namespace GenerateMap
{
    internal class TileUnit : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        public eTileType TileType { get; internal set; }
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            internal set
            {
                position = value;
                transform.position = value;
            }
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }
    }
}