﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map
{
    public static class BiomeProperties
    {
        public static Dictionary<Biome, Color> Colors = new Dictionary<Biome, Color>
        {
            { Biome.Ocean, new Color(1,0,0)/* HexToColor("44447a")*/ },
            //{ COAST, HexToColor("33335a") },
            //{ LAKESHORE, HexToColor("225588") },
            { Biome.Lake, HexToColor("336699") },
            //{ RIVER, HexToColor("225588") },
            { Biome.Marsh, HexToColor("2f6666") },
            { Biome.Ice, HexToColor("99ffff") },
            { Biome.Beach, HexToColor("a09077") },
            //{ ROAD1, HexToColor("442211") },
            //{ ROAD2, HexToColor("553322") },
            //{ ROAD3, HexToColor("664433") },
            //{ BRIDGE, HexToColor("686860") },
            //{ LAVA, HexToColor("cc3333") },
            { Biome.Snow, HexToColor("ffffff") },
            { Biome.Tundra, HexToColor("bbbbaa") },
            { Biome.Bare, HexToColor("888888") },
            { Biome.Scorched, HexToColor("555555") },
            { Biome.Taiga, HexToColor("99aa77") },
            { Biome.Shrubland, HexToColor("889977") },
            { Biome.TemperateDesert, HexToColor("c9d29b") },
            { Biome.TemperateRainForest, HexToColor("448855") },
            { Biome.TemperateDeciduousForest, HexToColor("679459") },
            { Biome.Grassland, HexToColor("88aa55") },
            { Biome.SubtropicalDesert, HexToColor("d2b98b") },
            { Biome.TropicalRainForest, HexToColor("337755") },
            { Biome.TropicalSeasonalForest, HexToColor("559944") }
        };

        public static Dictionary<Biome, string> Chinese = new Dictionary<Biome, string>
        {
            { Biome.Ocean,"海洋"},
            //{ COAST, HexToColor("33335a") },
            //{ LAKESHORE, HexToColor("225588") },
            { Biome.Lake, "湖泊"},
            //{ RIVER, HexToColor("225588") },
            { Biome.Marsh, "沼泽"},
            { Biome.Ice, "冰原"},
            { Biome.Beach, "海滩"},
            //{ ROAD1, HexToColor("442211") },
            //{ ROAD2, HexToColor("553322") },
            //{ ROAD3, HexToColor("664433") },
            //{ BRIDGE, HexToColor("686860") },
            //{ LAVA, HexToColor("cc3333") },
            { Biome.Snow, "雪山"},
            { Biome.Tundra, "冻原"},
            { Biome.Bare, "荒原"},
            { Biome.Scorched, "焦土"},
            { Biome.Taiga, "针叶林"},
            { Biome.Shrubland,"灌木丛"},
            { Biome.TemperateDesert, "温带沙漠"},
            { Biome.TemperateRainForest, "温带雨林"},
            { Biome.TemperateDeciduousForest, "温带落叶林"},
            { Biome.Grassland, "草原"},
            { Biome.SubtropicalDesert, "亚热带沙漠"},
            { Biome.TropicalRainForest, "热带雨林"},
            { Biome.TropicalSeasonalForest, "热带季雨林"},
        };
        static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
    }

    public enum Biome
    {
        Ocean,
        Marsh,
        Ice,
        Lake,
        Beach,
        Snow,
        Tundra,
        Bare,
        Scorched,
        Taiga,
        Shrubland,
        TemperateDesert,
        TemperateRainForest,
        TemperateDeciduousForest,
        Grassland,
        TropicalRainForest,
        TropicalSeasonalForest,
        SubtropicalDesert
    }
}
