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

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {

            //if (!GameController.Instance.currentGameMode.Contains(position)) GameController.Instance.currentGameMode.itemBoxPositions.Add(position);

            tileData.colliderType = Tile.ColliderType.None;

#if UNITY_EDITOR
            if (EditorApplication.isPlaying) tileData.sprite = null;
            else tileData.sprite = sprite;
#else
            tileData.sprite = null;
#endif
        }
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
