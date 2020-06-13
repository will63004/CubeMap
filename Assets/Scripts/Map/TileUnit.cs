using UnityEngine;

namespace Map
{
    internal class TileUnit : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

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