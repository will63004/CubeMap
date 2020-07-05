using GenerateMap;
using UnityEngine;

public class MapConsole : MonoBehaviour
{
    [SerializeField]
    private int width = 50;
    [SerializeField]
    private int heigh = 100;
    [SerializeField]
    private int length = 50;
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

    private MapDrawer mapDrawer = new MapDrawer();

    // Update is called once per frame
    async void Start()
    {
        MapGenerate mapGenerate = new MapGenerate(width, length, heigh);

        ////礦物
        //for (int i = 0; i < y; ++i)
        //{
        //    IList<(Vector3 pos, float perlinNoise)> stonePoints = mapGenerate.CreateSurfacePositions(x, z, densityRefinement, 0, 0, 1);
        //    foreach (var stonePoint in stonePoints)
        //    {
        //        if (stonePoint.perlinNoise * Mathf.Pow(densityRefinementDiminishing, i + 1) >= stoneDensity)
        //        {
        //            TileDetail tileDetail = tileDetails[(int)stonePoint.pos.x, i, (int)stonePoint.pos.z];
        //            if (tileDetail.tileType != eTileType.None && i + 1 < y && tileDetails[(int)stonePoint.pos.x, i + 1, (int)stonePoint.pos.z].tileType != eTileType.None)
        //                tileDetails[(int)stonePoint.pos.x, i, (int)stonePoint.pos.z].tileType = eTileType.Stone;
        //        }
        //    }
        //}

        ////水
        //for (int i = 0; i < x; ++i)
        //    for (int j = 0; j < y; ++j)
        //        for (int k = 0; k < z; ++k)
        //        {
        //            //海拔以下，正上方為空
        //            if (j <= altitude && tileDetails[i, j, k].tileType != eTileType.None && j + 1 < y && tileDetails[i, j + 1, k].tileType == eTileType.None)
        //                tileDetails[i, j, k].tileType = eTileType.Water;
        //        }

        await mapDrawer.DrawMap(mapGenerate.Generate());
    }
}
