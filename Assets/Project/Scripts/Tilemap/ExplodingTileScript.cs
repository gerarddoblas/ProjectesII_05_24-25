using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps
{
    //Several parts of the code are dependent on the UNITY_EDITOR flag
    //This is to allow for easier use during development

    [Serializable]
    public class ExplodingTile : TileBase
    {
        public Sprite[] sprites;
        public GameObject particles;
        public Dictionary<Vector3Int, int> states = new Dictionary<Vector3Int, int>();

        void OnEnable()
        {
            states = new Dictionary<Vector3Int, int>();

            SceneManager.sceneUnloaded += delegate (Scene unloadedScene)
            {
                states = new Dictionary<Vector3Int, int>();
            };
        }
        [ExecuteInEditMode]
        private void Awake()
        {
            states = new Dictionary<Vector3Int, int>();
            SceneManager.sceneUnloaded += delegate (Scene unloadedScene)
            {
                states = new Dictionary<Vector3Int, int>();
            };
        }
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                UpdateTile(position, tilemap, ref tileData, 0);
                return;
            }
#endif

            if(!states.ContainsKey(position))
            {
                states.Add(position, 0);
                UpdateTile(position, tilemap, ref tileData, 0);
            } else
            {
                UpdateTile(position, tilemap, ref tileData, states[position]);
            }
        }

        private void UpdateTile(Vector3Int position, ITilemap tilemap, ref TileData tileData, int state)
        {
            tileData.sprite = sprites[state];
            tileData.colliderType = Tile.ColliderType.Sprite;
        }

        public void ExplodeTile(Vector3Int position, ITilemap tilemap)
        {

            TileData tileData = new TileData();
            tilemap.GetTile(position).GetTileData(position, tilemap, ref tileData);

            if (states[position] == 1) return;

            states[position] = 1;
            UpdateTile(position, tilemap, ref tileData, 1);
            RefreshTile(position, tilemap);

            Instantiate(particles, position, Quaternion.identity);
        }
    }

#if UNITY_EDITOR

[CustomEditor(typeof(ExplodingTile))]
public class ExplodingTileEditor : Editor 
{
    private ExplodingTile tile { get { return (target as ExplodingTile); } }

    public void OnEnable()
    {
        if(tile.sprites == null || tile.sprites.Length != 2) tile.sprites = new Sprite[2];
    }
    
    public override void OnInspectorGUI() 
    {
        EditorGUILayout.LabelField("Place sprites shown based on the state.");
        EditorGUILayout.Space();
          
        EditorGUI.BeginChangeCheck();
        tile.sprites[0] = (Sprite) EditorGUILayout.ObjectField("Before", tile.sprites[0], typeof(Sprite), false, null);
        tile.sprites[1] = (Sprite) EditorGUILayout.ObjectField("After", tile.sprites[1], typeof(Sprite), false, null);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);

        EditorGUILayout.LabelField("Place particle system");
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        tile.particles = (GameObject) EditorGUILayout.ObjectField("Particles", tile.particles, typeof(GameObject), false, null);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);
    }

    [MenuItem("Assets/Create/ExplodingTile")]
    public static void CreateExplodingTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Exploding Tile", "New Exploding Tile", "Asset", "Save Exploding Tile", "Assets");
        if (path == "")
            return;                           
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ExplodingTile>(), path);
    }
}

#endif

}