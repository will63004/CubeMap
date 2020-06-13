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
        private float refinement = 0.1f;

        private void Update()
        {
            float perlinNoise = Mathf.PerlinNoise(refinement, Time.time);
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(perlinNoise, 0, Time.time);
        }
    }
}
