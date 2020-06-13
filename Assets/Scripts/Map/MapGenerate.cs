using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace Map
{
    class MapGenerate
    {
        internal float GerneateNoise(NoiseDetail x, NoiseDetail y)
        {
            float perlinNoise = Mathf.PerlinNoise(x.refinement + x.offset, y.refinement + y.offset);
            return perlinNoise;
        }

        internal IList<(Vector3, float)> CreateSurfacePositions(int x, int y, float refinement, float offsetX, float offsetY, float multiplier)
        {
            List<(Vector3, float)> points = new List<(Vector3, float)>();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    float perlinNoise = GerneateNoise(new NoiseDetail(i * refinement, offsetX), new NoiseDetail(j * refinement, offsetY));
                    points.Add((new Vector3(i, Mathf.RoundToInt(perlinNoise * multiplier), j), perlinNoise));
                }
            }

            return points;
        }

        internal float GernateDensity(int x, int y, NoiseDetail xDetail, NoiseDetail yDetail)
        {
            float perlinNoise = GerneateNoise(new NoiseDetail(x * xDetail.refinement, xDetail.offset), new NoiseDetail(y * yDetail.refinement, yDetail.offset));

            return perlinNoise;
        }
    }
}