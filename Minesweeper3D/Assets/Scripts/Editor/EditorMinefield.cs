using UnityEngine;
using UnityEditor;
using Minesweeper;

namespace Minesweeper { 
    [CustomEditor(typeof(Minefield))]
    public class EditorMinefield : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Minefield field = (Minefield)target;

            EditorGUILayout.Space();
            
            var centered = GUI.skin.GetStyle("Label");
            centered.alignment = TextAnchor.MiddleCenter;
            centered.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Minefield Size", centered);

            EditorGUILayout.BeginVertical();
            field.SetMinefieldWidth(EditorGUILayout.IntField("Width", field.GetMinefieldWidth()));
            field.SetMinefieldHeight(EditorGUILayout.IntField("Height", field.GetMinefieldHeight()));
            field.SetMinefieldDepth(EditorGUILayout.IntField("Depth", field.GetMinefieldDepth()));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Minefield"))
                field.CreateMinefield();
            if (GUILayout.Button("Reset Minefield"))
                field.DestroyMinefield();
            EditorGUILayout.EndHorizontal();
        }

    }
}