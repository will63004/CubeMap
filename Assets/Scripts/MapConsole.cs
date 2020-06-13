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
    private float refinement = 0f;
    [SerializeField]
    private float multiplier = 0f;
    [SerializeField]
    private int x = 50;
    [SerializeField]
    private int y = 100;
    [SerializeField]
    private int z = 50;

    private TileUnit[,,] tileUnits;

    private MapGenerate mapGenerate = new MapGenerate();

    private List<GameObject> gos = new List<GameObject>();
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;

    [SerializeField]
    private float densityRefinementX = 0.1f;
    [SerializeField]
    private float densityOffsetX = 0f;
    [SerializeField]
    private float densityRefinementY = 0.1f;
    [SerializeField]
    private float densityOffsetY = 0;
    [SerializeField]
    private float densityPriority = 0.5f;

    // Update is called once per frame
    async void Start()
    {
        tileUnits = new TileUnit[x, y, z];

        for (int i = 0; i < x * z; ++i)
            gos.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));

        IList<Vector3> surfacePoints = mapGenerate.CreateSurfacePoints(x, z, refinement, offsetX, offsetY, multiplier);
        foreach (var v in surfacePoints)
        {
            for (int i = 0; i < v.y; ++i)
            {
                Vector3 pos = new Vector3(v.x, i, v.z);
                float density = mapGenerate.GernateDensity((int)v.x, (int)v.z, new NoiseDetail(densityRefinementX, densityOffsetX), new NoiseDetail(densityRefinementY, densityOffsetY));
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                TileUnit tileUnit = go.AddComponent<TileUnit>();
                tileUnits[(int)v.x, i, (int)v.z] = tileUnit;
                tileUnit.SetMaterial(await GetMaterialAsync(density));
                go.transform.position = new Vector3(v.x, i, v.z);
            }
        }
    }

    private async Task<Material> GetMaterialAsync(float density)
    {
        if(density < 0.5f)
        {
            return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grey Stones.mat").Task;
        }
        else
            return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grass.mat").Task;
    }
}
