using System;
using System.Collections.Generic;
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
        public Dictionary<Vector3Int, int> states;

        void OnEnable()
        {
            states = new Dictionary<Vector3Int, int>();
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

            try
            {
                if (states[position] < sprites.Length - 1)
                {
                    states[position]++;
                    UpdateTile(position, tilemap, ref tileData, states[position]);
                }
            }
            catch (KeyNotFoundException)
            {
                states.Add(position, 0);
                UpdateTile(position, tilemap, ref tileData, states[position]);
            }
        }

        private void UpdateTile(Vector3Int position, ITilemap tilemap, ref TileData tileData, int state)
        {
            tileData.sprite = sprites[state];
            tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.colliderType = Tile.ColliderType.Sprite;

#if UNITY_EDITOR
            if (EditorApplication.isPlaying && state > 0)
                Instantiate(particles, position, Quaternion.identity);
#else
            Instantiate(particles, position, Quaternion.identity);
#endif

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
            tile.particles = (GameObject) EditorGUILayout.ObjectField("Before", tile.particles, typeof(GameObject), false, null);
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