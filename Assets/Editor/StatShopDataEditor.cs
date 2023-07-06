using System.Collections;
using System.Collections.Generic;
using ScriptableObjectData;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatShopData))]
public class StatShopDataEditor : Editor
{
    private Vector2 scroll;
    private bool showStatsSection = true; // Toggle to show or hide the stats section

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        
        StatShopData _shopData = target as StatShopData;
        if(_shopData == null) return;

        EditorGUIUtility.labelWidth = 200;
        
        // Toggle for showing or hiding the stats section
        showStatsSection = EditorGUILayout.Toggle("Show Stats Per Level", showStatsSection);
        
        EditorGUIUtility.labelWidth = 100;
        
        if (showStatsSection)
        {
            // Calculate the total width required for the stats section
            float totalWidth = EditorGUIUtility.labelWidth * 2;
            float totalHeight = EditorGUIUtility.singleLineHeight * 100;

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MaxHeight(200), GUILayout.Width(EditorGUIUtility.currentViewWidth - 20));
            
            EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));
            EditorGUILayout.BeginVertical(GUILayout.Height(totalHeight));
            GUILayout.Space(10);

            for (int i = 1; i <= 100; i++)
            {
                float width = EditorStyles.label.CalcSize(new GUIContent("Label")).x;

                EditorGUILayout.BeginHorizontal();
                int _cost = _shopData.GetCost(i);
                EditorGUILayout.LabelField("Level " + (i), GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField("Cost: " + _cost);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }
    }
}
