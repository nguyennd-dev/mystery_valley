using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] Grid _gridSystem;

    [Header("References")]
    [SerializeField] GameObject _goHighlighBlock;

    [Header("Terrain")]
    [SerializeField] Transform _prefabTile;
    [SerializeField] Vector3 _tileSize;

    ObjectPool<Transform> _poolTiles;
    List<Transform> _tileGrids = new List<Transform>();

    void Start()
    {
        _poolTiles = new ObjectPool<Transform>(_prefabTile);
        BuildTerrain();
    }

    void BuildTerrain()
    {
        var tileSize = new Vector2(5, 5);
        for (var r = -10; r < 10; r++)
        {
            for (var c = -10; c < 10; c++)
            {
                var tile = _poolTiles.Get();
                tile.transform.localScale = _tileSize;
                tile.SetParent(_prefabTile.parent);
                tile.transform.position = new Vector3(_tileSize.x * 0.5f + c * _tileSize.x, 0, _tileSize.z * 0.5f + r * _tileSize.x);
            }
        }
    }

    public void EnableHighlight(bool enabled, Vector3 position)
    {
        _goHighlighBlock.SetActive(enabled);
        _goHighlighBlock.transform.position = _gridSystem.CellToWorld(_gridSystem.WorldToCell(position));
    }

    public Vector3 CellToWorld(Vector3Int point)
    {
        return _gridSystem.CellToWorld(point);
    }

    public Vector3Int WorldToCell(Vector3 pos)
    {
        return _gridSystem.WorldToCell(pos);
    }
}
