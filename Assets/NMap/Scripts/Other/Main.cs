using UnityEngine;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using System.Linq;
using Assets.Map;

public class Main : MonoBehaviour
{
    Map _map;
    const int _textureScale = 10;
    GameObject _selector;

//    void Update()
//    {
//        if (_map != null && _map.SelectedCenter != null)
//        {
//            _selector.transform.localPosition = new Vector3(_map.SelectedCenter.point.x, _map.SelectedCenter.point.y, 1);
//        }
//    }

	void Awake ()
	{
        _selector = GameObject.Find("Selector");

        //Random.seed = 1;
            
        _map = new Map();
        _map.Init();

        GameObject.Find("Main MyCamera").GetComponentInChildren<MyCamera>().Map = _map;

        //»≈¬“±ﬂ‘µ
        NoisyEdges noisyEdge = new NoisyEdges();
        noisyEdge.BuildNoisyEdges(_map);

        new MapTexture(_textureScale).AttachTexture(GameObject.Find("Map"), _map,noisyEdge);
	}
}