using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StormStudio.Common.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IGameController
{
    Camera Camera { get; }
    GBlock GetItemByID(int id);
    void StoreItem(int id, GBlock itemStore);
}

public class GameController : MonoBehaviour, IGameController
{
    [Header("Reference")]
    [SerializeField] Camera _camera;
    [SerializeField] DragController _dragController;
    [SerializeField] GridController _gridController;
    [SerializeField] Transform _tfITems;

    [Header("Data")]
    [SerializeField] WorldDataSO _worldData;
    [SerializeField] ResourceDataSO _resourceData;

    [Header("Events")]
    [SerializeField] PlaceItemEvent _placeItemEvent;
    [SerializeField] TapBlockEvent _tapBlockEvent;

    PlayUI _playUI;

    Dictionary<int, ObjectPool<GBlock>> _poolItems = new Dictionary<int, ObjectPool<GBlock>>();
    Dictionary<Vector3Int, GBlock> _worldItems = new Dictionary<Vector3Int, GBlock>();

    public Camera Camera { get { return _camera; } }

    GameInput _gameInput = null;

    void Awake()
    {
        _placeItemEvent.OnEvent += OnPlaceItem;
        _tapBlockEvent.OnEvent += OnTapBlock;
        _gameInput = new GameInput();
        _gameInput.Game.Quit.performed += OnTapECS;
        _gameInput.Enable();
    }

    void OnDestroy()
    {
        _placeItemEvent.OnEvent -= OnPlaceItem;
        _gameInput.Game.Quit.performed -= OnTapECS;
        _gameInput.Disable();
    }

    public void Setup()
    {
        GeneratePools();
        _dragController.Setup(this);
    }

    public void OnStart()
    {
        SoundManager.Instance.PlayBgmGameplay();
        _playUI = UIManager.Instance.ShowUIOnTop<PlayUI>("PlayUI");
        _playUI.Setup(onPause);
        LoadWorld();
    }

    public void Exit()
    {
        UIManager.Instance.ReleaseUI(_playUI, true);
    }

    void onPause()
    {
        var pauseUI = UIManager.Instance.ShowUIOnTop<PauseUI>("PauseUI");
        pauseUI.Setup(SoundManager.Instance.IsSfxOn, SoundManager.Instance.IsBgmOn);
        pauseUI.OnQuit = () => Application.Quit();
        pauseUI.OnResume = () => ResumeGame();
        pauseUI.OnRestart = () => RestartGame();
    }

    public void PauseGame()
    {
    }

    public void ResumeGame()
    {
    }

    void RestartGame()
    {
        _worldData.LoadWorld("");
        _resourceData.LoadResource(GameFlow.Instance.InitResourceData());
        LoadWorld();
        ResumeGame();
    }

    public void OnTapECS(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsActiveOnTop(_playUI))
        {
            PauseGame();
            SoundManager.Instance.PauseBGMs();
            var quitUI = UIManager.Instance.ShowUIOnTop<QuitUI>("QuitUI");
            quitUI.Setup((cancel) =>
            {
                if (!cancel)
                {
                    Debug.Log("QUIT GAME");
                    Application.Quit();
                }
                else
                {
                    ResumeGame();
                    SoundManager.Instance.ResumeBGMs();
                }
            });
        }
    }

    void GeneratePools()
    {
        _poolItems.Clear();
        foreach (var itemID in ConfigManager.Instance.GetAllItemIDs())
        {
            var itemPool = GameObject.Instantiate<GameObject>(ConfigManager.Instance.GetItemConfig(itemID).Prefab).GetComponent<GBlock>();
            itemPool.transform.SetParent(this.transform);
            _poolItems.Add(itemID, new ObjectPool<GBlock>(itemPool));
        }
    }

    void LoadWorld()
    {
        foreach (var item in _worldItems)
        {
            _poolItems[item.Value.ID].Store(item.Value);
        }

        _worldItems.Clear();
        foreach (var tile in _worldData.Tiles)
        {
            var item = GetItemByID(tile.Value);
            item.Setup(tile.Value);
            item.transform.position = _gridController.CellToWorld(tile.Position) + new Vector3(0f, 0.5f, 0f);
            _worldItems.Add(tile.Position, item);
        }
    }

    bool AddBlock(Vector3Int point, GBlock block)
    {
        if (_worldItems.ContainsKey(point) && _worldItems[point] != null) return false;
        if (!_worldItems.ContainsKey(point)) _worldItems.Add(point, null);
        _worldItems[point] = block;
        _worldData.AddTile(new TileData(block.ID, point));
        return true;
    }

    bool RemoveBlock(Vector3Int point)
    {
        if (_worldItems.ContainsKey(point))
        {
            _worldItems.Remove(point);
            _worldData.RemoveTile(point);
            return true;
        }
        return false;
    }

    private void OnPlaceItem(int itemId, Vector3Int point)
    {
        if (_worldItems.ContainsKey(point) && _worldItems[point] != null) return;
        var item = GetItemByID(itemId);
        item.Setup(itemId);
        item.transform.position = _gridController.CellToWorld(point) + new Vector3(0f, 0.5f, 0f);
        _resourceData.ConsumeResource(itemId, 1);
        AddBlock(point, item);

        SoundManager.Instance.PlaySfxPlaceBlock();
    }

    void OnTapBlock(GBlock block)
    {
        var gridPoint = _gridController.WorldToCell(block.transform.position + new Vector3(C.BlockConstants.HALF_SIZE_BLOCK, 0f, C.BlockConstants.HALF_SIZE_BLOCK));
        if (_worldItems.ContainsKey(gridPoint))
        {
            SoundManager.Instance.PlaySfxTakeBlock();
            _resourceData.AddResource(block.ID, 1);
            RemoveBlock(gridPoint);
            StoreItem(block.ID, block);
            UpdateGrid(gridPoint);
        }
    }

    void UpdateGrid(Vector3Int point)
    {
        var blockOnGrids = new List<GBlock>();
        var lastBlockPoints = new List<Vector3Int>();
        foreach (var gribPoint in _worldItems.Keys)
        {
            if (gribPoint.x != point.x || gribPoint.z != point.z) continue;
            blockOnGrids.Add(_worldItems[gribPoint]);
            lastBlockPoints.Add(gribPoint);
        }

        foreach (var blockPoint in lastBlockPoints)
        {
            DOTween.Kill(_worldItems[blockPoint].transform);
            RemoveBlock(blockPoint);
        }

        if (blockOnGrids.Count > 0)
        {
            blockOnGrids.Sort((a, b) =>
            {
                if (b.transform.position.y > a.transform.position.y) return -1;
                if (b.transform.position.y < a.transform.position.y) return 1;
                return 0;
            });

            int count = 0;
            for (int i = 0; i < blockOnGrids.Count; i++)
            {
                var blockPoint = new Vector3Int(point.x, i, point.z);
                AddBlock(blockPoint, blockOnGrids[i]);
                blockOnGrids[i].transform.DOMoveY(i * C.BlockConstants.HALF_SIZE_BLOCK * 2 + C.BlockConstants.HALF_SIZE_BLOCK, 0.3f).OnComplete(() =>
                {
                    count++;
                    if (count == blockOnGrids.Count)
                    {
                    }
                });
            }
        }
    }

    public GBlock GetItemByID(int id)
    {
        return _poolItems[id].Get();
    }

    public void StoreItem(int id, GBlock itemStore)
    {
        itemStore.transform.position = new Vector3(0, -100, 0);
        itemStore.transform.SetParent(this.transform);
        _poolItems[id].Store(itemStore);
    }
}
