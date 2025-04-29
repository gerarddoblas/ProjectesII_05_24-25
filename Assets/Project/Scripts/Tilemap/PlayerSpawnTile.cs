using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using static UnityEditor.VersionControl.Asset;
#endif

namespace UnityEngine.Tilemaps
{
    //Several parts of the code are dependent on the UNITY_EDITOR flag
    //This is to allow for easier use during development

    [Serializable]
    public class PlayerSpawnTile : TileBase
    {

        public List<ValueTuple<Vector3Int, string>> spawnInfo;
        public Sprite sprite;

        //        [ExecuteAlways]
        //        private void Awake()
        //        {
        //            //spawnInfo = new List<ValueTuple<Vector3Int, string>>();
        //            //SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
        //            //{

        //            //    Debug.Log("PlayerSpawn");
        //            //    string sceneName = loadedScene.name;
        //            //    PlayersManager.Instance.playerSpawnPositions.Clear();
        //            //    foreach (var info in spawnInfo)
        //            //    {
        //            //        if (sceneName == info.Item2) PlayersManager.Instance.playerSpawnPositions.Add(info.Item1);
        //            //    }
        //            //    PlayersManager.Instance.SetPlayersPosition();
        //            //    Debug.Assert(PlayersManager.Instance.playerSpawnPositions.Count == 4);
        //            //};
        //        }
        //        private void OnEnable()
        //        {
        //            spawnInfo = new List<ValueTuple<Vector3Int, string>>();
        //            SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
        //            {

        //                Debug.Log("PlayerSpawn");
        //                string sceneName = loadedScene.name;
        //                PlayersManager.Instance.playerSpawnPositions.Clear();
        //                foreach (var info in spawnInfo)
        //                {
        //                    if (sceneName == info.Item2) PlayersManager.Instance.playerSpawnPositions.Add(info.Item1);
        //                }
        //                PlayersManager.Instance.SetPlayersPosition();
        //            };
        //        }

        //        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        //        {

        //            string sceneName = SceneManager.GetActiveScene().name;
        //            if (!spawnInfo.Contains((position, sceneName))) spawnInfo.Add((position, sceneName));

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

    [CustomEditor(typeof(PlayerSpawnTile))]
    public class PlayerSpawnTileEditor : Editor
    {
        private PlayerSpawnTile tile { get { return (target as PlayerSpawnTile); } }

        public void OnEnable()
        {


        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprite.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            tile.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", tile.sprite, typeof(Sprite), false, null);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);
        }

        [MenuItem("Assets/Create/PlayerSpawnTile")]
        public static void CreatePlayerSpawnTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save PlayerSpawn Tile", "New PlayerSpawn Tile", "Asset", "Save PlayerSpawn Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PlayerSpawnTile>(), path);
        }
    }

#endif

}
