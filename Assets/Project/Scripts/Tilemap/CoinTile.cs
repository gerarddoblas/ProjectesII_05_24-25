using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEngine.Tilemaps
{

    [Serializable]
    public class CoinTile : PrefabTile
    {
        public override void OnEnable()
        {
            //SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadSceneMode)
            //{
            //    if (!positions.ContainsKey(loadedScene.buildIndex)) positions.Add(loadedScene.buildIndex, new List<Vector3Int>());
            //    GameObject parent = Instantiate(new GameObject("---" + prefab.name.ToUpper() + "---"));
            //    if (GameController.Instance.currentGameMode != null && GameController.Instance.currentGameMode.GetType().Equals(typeof(CoinCollectGame)))
            //        foreach (Vector3Int position in positions[loadedScene.buildIndex]) Instantiate(prefab, position, Quaternion.identity, parent.transform);
            //};
        }
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
