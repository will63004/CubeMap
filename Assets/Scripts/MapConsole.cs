using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapConsole : MonoBehaviour
{
    [SerializeField]
    private int x = 50;
    [SerializeField]
    private int y = 100;
    [SerializeField]
    private int z = 50;
    [SerializeField]
    private float refinement = 0f;
    [SerializeField]
    private float multiplier = 0f;
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;

    [SerializeField]
    private float densityRefinement = 0.1f;
    [SerializeField]
    private float densityRefinementDiminishing = 0.9f;
    [SerializeField]
    private float stoneDensity = 0.5f;

    [SerializeField]
    private int altitude = 6;

    private TileDetail[,,] tileDetails;

    private MapGenerate mapGenerate = new MapGenerate();
    private MapDrawer mapDrawer = new MapDrawer();



    // Update is called once per frame
    async void Start()
    {
        //土
        tileDetails = new TileDetail[x, y, z];
        IList<(Vector3 pos, float perlinNoise)> surfacePoints = mapGenerate.CreateSurfacePositions(x, z, refinement, offsetX, offsetY, multiplier);
        foreach (var v in surfacePoints)
        {
            for (int i = 0; i < v.pos.y; ++i)
            {
                tileDetails[(int)v.pos.x, i, (int)v.pos.z].tileType = eTileType.Earth;
            }
        }

        //礦物
        for (int i = 0; i < y; ++i)
        {
            IList<(Vector3 pos, float perlinNoise)> stonePoints = mapGenerate.CreateSurfacePositions(x, z, densityRefinement, 0, 0, 1);
            foreach (var stonePoint in stonePoints)
            {
                if (stonePoint.perlinNoise * Mathf.Pow(densityRefinementDiminishing, i + 1) >= stoneDensity)
                {
                    TileDetail tileDetail = tileDetails[(int)stonePoint.pos.x, i, (int)stonePoint.pos.z];
                    if (tileDetail.tileType != eTileType.None && i + 1 < y && tileDetails[(int)stonePoint.pos.x, i + 1, (int)stonePoint.pos.z].tileType != eTileType.None)
                        tileDetails[(int)stonePoint.pos.x, i, (int)stonePoint.pos.z].tileType = eTileType.Stone;
                }
            }
        }

        //水
        for (int i = 0; i < x; ++i)
            for (int j = 0; j < y; ++j)
                for (int k = 0; k < z; ++k)
                {
                    //海拔以下，正上方為空
                    if (j <= altitude && tileDetails[i, j, k].tileType != eTileType.None && j + 1 < y && tileDetails[i, j + 1, k].tileType == eTileType.None)
                        tileDetails[i, j, k].tileType = eTileType.Water;
                }

        await mapDrawer.DrawMap(tileDetails);
        //for (int i = 0; i < x * z; ++i)
        //    gos.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));

        //IList<Vector3> surfacePoints = mapGenerate.CreateSurfacePoints(x, z, refinement, offsetX, offsetY, multiplier);
        //foreach (var v in surfacePoints)
        //{
        //    for (int i = 0; i < v.y; ++i)
        //    {
        //        Vector3 pos = new Vector3(v.x, i, v.z);
        //        float density = mapGenerate.GernateDensity((int)v.x, (int)v.z, new NoiseDetail(densityRefinementX, densityOffsetX), new NoiseDetail(densityRefinementY, densityOffsetY));
        //        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        TileUnit tileUnit = go.AddComponent<TileUnit>();
        //        tileDetails[(int)v.x, i, (int)v.z] = tileUnit;
        //        tileUnit.SetMaterial(await GetMaterialAsync(density));
        //        go.transform.position = new Vector3(v.x, i, v.z);
        //    }
        //}
    }

    private async Task<Material> GetMaterialAsync(float density)
    {
        if (density < 0.5f)
        {
            return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grey Stones.mat").Task;
        }
        else
            return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grass.mat").Task;
    }
}
