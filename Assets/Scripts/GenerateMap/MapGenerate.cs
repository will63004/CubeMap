using Assets.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TreeEditor;
using UnityEngine;

namespace GenerateMap
{
    public class MapGenerate
    {
        private int width;
        private readonly int length;
        private int height;
        private float refinement = 0.1f;
        private float multiplier = 10;
        private float offsetX;
        private float offsetY;
        private int altitude = 3;
        private int _textureScale = 1;

        private float treeRefinement = 0.2f;
        private float treeWeights = 0.7f;

        private int geologyPointCount = 10;
        private float geologyRefinement = 0.1f;

        public MapGenerate(int width, int length, int height)
        {
            this.width = width;
            this.length = length;
            this.height = height;
        }

        public TileDetail[,,] Generate()
        {
            SurfaceGenerate mapGenerate = new SurfaceGenerate();

            Map2 voronioMap = new Map2((float)width, (float)length);
            voronioMap.Init();

            var terrain = ConvertToTerrain(voronioMap);

            TileDetail[,,] tileDetails = new TileDetail[width, height, length];

            Map1 geologyMap = new Map1((float)width, (float)length, geologyPointCount);
            var geologyTerrain = ConvertToGeology(geologyMap, geologyRefinement);

            //土
            IList<(Vector3 pos, float perlinNoise)> surfacePoints = mapGenerate.CreateSurfacePositions(width, length, refinement, offsetX, offsetY, multiplier);
            foreach (var v in surfacePoints)
            {
                if (terrain[(int)v.pos.x, (int)v.pos.z].tileType == eTileType.Ocean)
                {
                    for (int i = 0; i < altitude; ++i)
                        tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Ocean;

                    continue;
                }

                for (int i = 0; i < v.pos.y; ++i)
                {
                    switch (geologyTerrain[(int)v.pos.x, (int)v.pos.z].geology)
                    {
                        case eGeology.None:
                            tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.None;
                            break;
                        case eGeology.Grass:
                            tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Earth;
                            break;
                        case eGeology.Desert:
                            tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Sand;
                            break;
                        case eGeology.RockyMountain:
                            tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Stone;
                            break;
                        default:
                            tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Earth;
                            break;
                    }
                }
            }

            //水
            for (int i = 0; i < width; ++i)
                for (int k = 0; k < length; ++k)
                {
                    if (tileDetails[i, altitude - 1, k].tileType == eTileType.None)
                    {
                        for (int j = altitude - 1; j >= 0; --j)
                        {
                            if (tileDetails[i, j, k].tileType == eTileType.None)
                                tileDetails[i, j, k].tileType = eTileType.Lake;
                        }
                    }
                }

            //種樹
            IList<(Vector3 pos, float perlinNoise)> trees = mapGenerate.CreateSurfacePositions(width, length, treeRefinement, offsetX, offsetY, 1, treeWeights);
            foreach (var tree in trees)
            {
                for (int i = 0; i < height; ++i)
                {
                    if (tileDetails[(int)tree.pos.x, i, (int)tree.pos.z].tileType == eTileType.None &&
                        i > 0 &&
                        tileDetails[(int)tree.pos.x, i - 1, (int)tree.pos.z].tileType == eTileType.Earth)
                    {
                        tileDetails[(int)tree.pos.x, i, (int)tree.pos.z].tileType = eTileType.Tree;
                        break;
                    }
                }
            }

            return tileDetails;
        }

        private TerrainDistribution[,] ConvertToTerrain(Map2 map)
        {
            TerrainDistribution[,] terrainDistributions = new TerrainDistribution[(int)map.Width, (int)map.Height];

            for (int i = 0; i < map.Width; ++i)
                for (int j = 0; j < map.Height; ++j)
                    terrainDistributions[i, j] = new TerrainDistribution();

            foreach (var td in terrainDistributions)
            {
                td.tileType = eTileType.Ocean;
            }

            //陸地
            var oceanConors = map.Graph.centers.Where(p => !p.water).Select(p => p.corners);
            foreach (var conors in oceanConors)
            {
                terrainDistributions.FillPolygon(conors.Select(p => p.point * _textureScale).ToArray(), eTileType.Earth);
            }

            //湖泊
            var lakeConors = map.Graph.centers.Where(p => p.water && !p.ocean).Select(p => p.corners);
            foreach (var conors in lakeConors)
                terrainDistributions.FillPolygon(conors.Select(p => p.point * _textureScale).ToArray(), eTileType.Lake);

            return terrainDistributions;
        }

        private GeologyTerrain[,] ConvertToGeology(Map1 map, float refinement)
        {
            GeologyTerrain[,] geologyTerrains = new GeologyTerrain[(int)map.Width, (int)map.Height];
            for (int i = 0; i < map.Width; ++i)
                for (int j = 0; j < map.Height; ++j)
                    geologyTerrains[i, j] = new GeologyTerrain();

            foreach (var center in map.Graph.centers)
            {
                float perlinNoise = Mathf.PerlinNoise(refinement + (int)center.point.x, refinement + (int)center.point.y);
                eGeology geology = GetGeology(perlinNoise);
                geologyTerrains.FillPolygon(center.corners.Select(p => p.point * _textureScale).ToArray(), geology);
            }

            return geologyTerrains;
        }

        private eGeology GetGeology(float perlinNoise)
        {
            return (eGeology)UnityEngine.Random.Range(1, Enum.GetValues(typeof(eGeology)).Length);

            float percent = 1.0f / (Enum.GetValues(typeof(eGeology)).Length - 1);
            for (int i = 1; i < Enum.GetValues(typeof(eGeology)).Length; ++i)
            {
                if (perlinNoise < i * percent)
                    return (eGeology)i;
            }

            return eGeology.Grass;
        }
    }
}
