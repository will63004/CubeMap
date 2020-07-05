using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace GenerateMap
{
    class SurfaceGenerate
    {
        internal float GerneateNoise(NoiseDetail x, NoiseDetail y)
        {
            float perlinNoise = Mathf.PerlinNoise(x.refinement + x.offset, y.refinement + y.offset);
            return perlinNoise;
        }

        internal IList<(Vector3, float)> CreateSurfacePositions(int width, int length, float refinement, float offsetX, float offsetY, float multiplier, float perlinWeight = 0)
        {
            List<(Vector3, float)> points = new List<(Vector3, float)>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    float perlinNoise = GerneateNoise(new NoiseDetail(i * refinement, offsetX), new NoiseDetail(j * refinement, offsetY));
                    if (perlinNoise >= perlinWeight)
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