using BaseCore;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    private Vector2 scroll;
    private bool showStatsSection = true; // Toggle to show or hide the stats section

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EnemyData _enemyData = target as EnemyData;
        
        if(_enemyData == null) return;
        
        StatsGrowth _statsGrowthDataData = _enemyData.statsData;
        
        EditorGUIUtility.labelWidth = 200;
        
        // Expandable text box for the 'description' property
        EditorGUILayout.LabelField("Encyclopedia Description");
        _enemyData.encyclopediaInfo.description = EditorGUILayout.TextArea(_enemyData.encyclopediaInfo.description, GUILayout.Height(100));
        
        // Toggle for showing or hiding the stats section
        showStatsSection = EditorGUILayout.Toggle("Show Stats Per Level", showStatsSection);
        
        EditorGUIUtility.labelWidth = 50;
        
        if (showStatsSection)
        {
            // Calculate the total width required for the stats section
            float totalWidth = EditorGUIUtility.labelWidth * 9f;

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MaxHeight(200), GUILayout.Width(EditorGUIUtility.currentViewWidth - 20));
            
            EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));

            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            for (int i = 1; i <= 10; i++)
            {
                float width = EditorStyles.label.CalcSize(new GUIContent("Label")).x;

                EditorGUILayout.BeginHorizontal();
                CombatStats _lvlStats = _statsGrowthDataData.GetLeveledStats(i, 10);
                EditorGUILayout.LabelField("Level " + (i), GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField("HP: " + _lvlStats.vitality);
                EditorGUILayout.LabelField("Str: " + _lvlStats.strength);
                EditorGUILayout.LabelField("Int: " + _lvlStats.intelligence);
                EditorGUILayout.LabelField("Def: " + _lvlStats.defense);
                EditorGUILayout.LabelField("Spd: " + _lvlStats.speed);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }
    }
}

