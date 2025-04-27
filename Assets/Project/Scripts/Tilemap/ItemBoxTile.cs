using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEngine.Tilemaps
{

    [Serializable]
    public class ItemBoxTile : PrefabTile
    {

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ItemBoxTile))]
    public class ItemBoxTileEditor : Editor
    {
        private ItemBoxTile tile { get { return (target as ItemBoxTile); } }
        [MenuItem("Assets/Create/ItemBoxTile")]
        public static void CreatePrefabTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save ItemBox Tile", "New ItemBox Tile", "Asset", "Save ItemBox Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ItemBoxTile>(), path);
        }
    }

#endif

}
