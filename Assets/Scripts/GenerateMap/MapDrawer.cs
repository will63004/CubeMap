using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GenerateMap
{
    class MapDrawer
    {
        private TileUnit[,,] tileUnits;

        public MapDrawer()
        {
        }

        internal async Task DrawMap(TileDetail[,,] tileDetails)
        {
            int xNum = tileDetails.GetLength(0);
            int yNum = tileDetails.GetLength(1);
            int zNum = tileDetails.GetLength(2);

            tileUnits = new TileUnit[xNum, yNum, zNum];

            for (int x = 0; x < xNum; ++x)
                for (int y = 0; y < yNum; ++y)
                    for (int z = 0; z < zNum; ++z)
                    {
                        TileDetail detail = tileDetails[x, y, z];
                        if(detail.tileType != eTileType.None)
                        {
                            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            TileUnit tileUnit = go.AddComponent<TileUnit>();
                            tileUnit.TileType = detail.tileType;
                            tileUnit.Position = new Vector3(x, y, z);
                            tileUnit.SetMaterial(await GetMaterialAsync(detail.tileType));
                        }
                    }
        }

        private async Task<Material> GetMaterialAsync(eTileType tileType)
        {
            switch (tileType)
            {
                case eTileType.Earth:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grass.mat").Task;
                case eTileType.Stone:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Grey Stones.mat").Task;
                case eTileType.Ocean:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Water Deep Blue.mat").Task;                    
                case eTileType.Lake:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Water Light Blue.mat").Task;
                case eTileType.Tree:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Sandy.mat").Task;
                case eTileType.Sand:
                    return await Addressables.LoadAssetAsync<Material>("Assets/Addressible/Cubes/Materials/Sandy Orange.mat").Task;
            }

            return null;
        }
    }
}