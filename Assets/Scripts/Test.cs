using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Test:MonoBehaviour
    {
        [SerializeField]
        private float perlinNoise;
        [SerializeField]
        private Vector2 refinement;

        private void Update()
        {
            perlinNoise = Mathf.PerlinNoise(refinement.x, refinement.y);
        }
    }
}
