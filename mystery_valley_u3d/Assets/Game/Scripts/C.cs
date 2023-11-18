using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class C
{
    public enum BtnType
    {
        Close = 0,
        Resume = 1,
        Quit = 2,
        Restart = 3,
        None = 4
    }

    public static class PathConstants
    {
        public const string WORLD_DATA_PATH = "world_data";
        public const string RESOURCE_DATA_PATH = "resource_data";

        public static string GetSavePath(string fileName)
        {
            return System.IO.Path.Combine(Application.persistentDataPath, fileName);
        }
    }

    public static class KeyConstants
    {
        public const string KEY_FIRST_LOGIN = "FIRST_LOGIN";
    }

    public static class ResourceConstants
    {
        public const int INIT_VALUE_RESOURCE = 10;
    }

    public static class LayerConstants
    {
        public const string LAYER_BLOCK = "Block";
        public const string LAYER_DRAG_BLOCK = "DragBlock";
    }

    public static class BlockConstants
    {
        public const float HALF_SIZE_BLOCK = 0.5f;
    }
}
