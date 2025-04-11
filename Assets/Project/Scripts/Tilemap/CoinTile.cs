using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEngine.Tilemaps
{

    [Serializable]
    public class CoinTile : PrefabTile
    {

//        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
//        {
//            Debug.Log(positions.Count);
//            if (!positions.Contains(position)) positions.Add(position);

//            tileData.colliderType = Tile.ColliderType.None;

//#if UNITY_EDITOR
//            if (EditorApplication.isPlaying) tileData.sprite = null;
//            else tileData.sprite = sprite;
//#else
//            tileData.sprite = null;
//#endif
//        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CoinTile))]
    public class CoinTileEditor : Editor
    {
        private CoinTile tile { get { return (target as CoinTile); } }
        [MenuItem("Assets/Create/CoinTile")]
        public static void CreatePrefabTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Coin Tile", "New Coin Tile", "Asset", "Save Coin Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CoinTile>(), path);
        }
    }

#endif

}
