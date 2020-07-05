// Annotate each edge with a noisy path, to make maps look more interesting.
// Author: amitp@cs.stanford.edu
// License: MIT

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Map;
using Random = UnityEngine.Random;

public class NoisyEdges
{
    private static readonly float NOISY_LINE_TRADEOFF = 0.5f;// low: jagged vedge; high: jagged dedge
    public Dictionary<int, List<Vector2>> path0 = new Dictionary<int, List<Vector2>>();// edge index -> Vector.<Point>
    public Dictionary<int, List<Vector2>> path1 = new Dictionary<int, List<Vector2>>();// edge index -> Vector.<Point>

    private const float SizeScale = 0.1f;
    // Build noisy line paths for each of the Voronoi edges. There are
    // two noisy line paths for each edge, each covering half the
    // distance: path0 is from v0 to the midpoint and path1 is from v1
    // to the midpoint. When drawing the polygons, one or the other
    // must be drawn in reverse order.
    public void BuildNoisyEdges(Map map)
    {
        foreach (Center p in map.Graph.centers)
        {
            foreach (Edge edge in p.borders)
            {
                if (edge.d0 != null && edge.d1 != null && edge.v0 != null && edge.v1 != null
                    && !path0.ContainsKey(edge.index))
                {
                    float f = NOISY_LINE_TRADEOFF;
                    Vector2 t = Vector2Extensions.Interpolate(edge.v0.point, edge.d0.point, f);
                    Vector2 q = Vector2Extensions.Interpolate(edge.v0.point, edge.d1.point, f);
                    Vector2 r = Vector2Extensions.Interpolate(edge.v1.point, edge.d0.point, f);
                    Vector2 s = Vector2Extensions.Interpolate(edge.v1.point, edge.d1.point, f);

                    float minLength = 10 * SizeScale;
                    if (edge.d0.biome != edge.d1.biome) minLength = 3 * SizeScale;
                    if (edge.d0.ocean && edge.d1.ocean) minLength = 100 * SizeScale;
                    if (edge.d0.coast || edge.d1.coast) minLength = 1 * SizeScale;
                    if (edge.river > 0) minLength = 1 * SizeScale;

                    path0[edge.index] = buildNoisyLineSegments(edge.v0.point, t, edge.midpoint, q, minLength);
                    path1[edge.index] = buildNoisyLineSegments(edge.v1.point, s, edge.midpoint, r, minLength);
                }
            }
        }
    }

    // Helper function: build a single noisy line in a quadrilateral A-B-C-D,
    // and store the output points in a Vector.
    private List<Vector2> buildNoisyLineSegments(Vector2 A, Vector2 B, Vector2 C, Vector2 D, float minLength)
    {
        List<Vector2> points = new List<Vector2>();
        
        points.Add(A);
        subdivide(A, B, C, D,points,minLength);
        points.Add(C);

        return points;
    }

    private void subdivide(Vector2 A, Vector2 B, Vector2 C, Vector2 D, List<Vector2> points, float minLength)
    {
        if (Vector2.Distance(A,C) < minLength || Vector2.Distance(B,D)<minLength)
            return;

        // Subdivide the quadrilateral
        float p = Random.Range(0.2f, 0.8f);// vertical (along A-D and B-C)
        float q = Random.Range(0.2f, 0.8f);// horizontal (along A-B and D-C)

        // Midpoints
        Vector2 E = Vector2Extensions.Interpolate(A, D, p);
        Vector2 F = Vector2Extensions.Interpolate(B, C, p);
        Vector2 G = Vector2Extensions.Interpolate(A, B, q);
        Vector2 I = Vector2Extensions.Interpolate(D, C, q);

        // Central point
        Vector2 H = Vector2Extensions.Interpolate(E, F, q);

        // Divide the quad into subquads, but meet at H
        float s = 1 - Random.Range(-0.4f, 0.4f);
        float t = 1 - Random.Range(-0.4f, 0.4f);

        subdivide(A, Vector2Extensions.Interpolate(G, B, s), H, Vector2Extensions.Interpolate(E, D, t), points,
            minLength);
        points.Add(H);
        subdivide(H, Vector2Extensions.Interpolate(F, C, s), C, Vector2Extensions.Interpolate(I, D, t), points,
            minLength);
    }
}
