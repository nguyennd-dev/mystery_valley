using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StormStudio.Common;
using StormStudio.Common.UI;
using UnityEngine;
using static StormStudio.Common.GSMachine;

public partial class GameFlow : MonoBehaviour
{
    [SerializeField] WorldDataSO _worldData;
    [SerializeField] ResourceDataSO _resourceData;

    public bool IsFirstTimeLogin { get { return PlayerPrefs.GetInt(C.KeyConstants.KEY_FIRST_LOGIN, 0) == 0; } set { PlayerPrefs.SetInt(C.KeyConstants.KEY_FIRST_LOGIN, value ? 1 : 0); } }

    SplashUI _splashUI = null;

    void GameState_Init(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            _controller = GameObject.Instantiate(_prefabWorld, Vector3.zero, Quaternion.identity).GetComponent<GameController>();
            _controller.Setup();

            if (IsFirstTimeLogin)
            {
                IsFirstTimeLogin = true;
                _worldData.LoadWorld("");
                _resourceData.LoadResource(InitResourceData());
            }
            else
            {
                var worldData = File.ReadAllText(C.PathConstants.GetSavePath(C.PathConstants.WORLD_DATA_PATH));
                _worldData.LoadWorld(worldData);

                var resourceData = File.ReadAllText(C.PathConstants.GetSavePath(C.PathConstants.RESOURCE_DATA_PATH));
                _resourceData.LoadResource(resourceData);
            }

            _splashUI = UIManager.Instance.ShowUIOnTop<SplashUI>("SplashUI");
            _splashUI.Setup(() => ChangeToGameplay());
        }
        else if (stateEvent == StateEvent.Exit)
        {
            UIManager.Instance.ReleaseUI(_splashUI, true);
        }
    }

    void ChangeToGameplay()
    {
        SceneTransition(() => _gsMachine.ChangeState(GameState.Gameplay));
    }

    public GameData LoadGameData()
    {
        var data = new GameData();
        return data;
    }

    public string InitResourceData()
    {
        List<ItemData> items = new List<ItemData>();
        foreach (var itemId in ConfigManager.Instance.GetAllItemIDs())
            items.Add(new ItemData(itemId, C.ResourceConstants.INIT_VALUE_RESOURCE));
        return JsonConvert.SerializeObject(items);
    }

    void OnApplicationQuit()
    {
        File.WriteAllText(C.PathConstants.GetSavePath(C.PathConstants.WORLD_DATA_PATH), JsonConvert.SerializeObject(_worldData.Tiles));
        File.WriteAllText(C.PathConstants.GetSavePath(C.PathConstants.RESOURCE_DATA_PATH), JsonConvert.SerializeObject(_resourceData.Items));
    }
}
