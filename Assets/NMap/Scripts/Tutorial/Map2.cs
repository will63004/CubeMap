using Delaunay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Map
{
    public class Map2
    {
        private int _pointCount = 500;
        float _lakeThreshold = 0.3f;
        public float Width { get; private set; } = 50;
        public float Height { get; private set; } = 50;
        const int NUM_LLOYD_RELAXATIONS = 2;

        public Graph Graph { get; private set; }
        public Center SelectedCenter { get; private set; }

        public Map2()
        {

        }

        public Map2(float width, float height)
        {
            Width = width;
            Height = height;
        }
        public void Init(Func<Vector2, bool> checkIsland = null)
        {
            List<uint> colors = new List<uint>();
            var points = new List<Vector2>();

            for (int i = 0; i < _pointCount; i++)
            {
                colors.Add(0);
                points.Add(new Vector2(
                        UnityEngine.Random.Range(0, Width),
                        UnityEngine.Random.Range(0, Height))
                );
            }

            for (int i = 0; i < NUM_LLOYD_RELAXATIONS; i++)
                points = Graph.RelaxPoints(points, Width, Height).ToList();

            var voronoi = new Voronoi(points, colors, new Rect(0, 0, Width, Height));

            checkIsland = checkIsland ?? IslandShape.makePerlin();
            Graph = new Graph(checkIsland, points, voronoi, (int)Width, (int)Height, _lakeThreshold);
        }
    }
}
