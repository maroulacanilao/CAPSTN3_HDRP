using BaseCore;
using Character;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    private Vector2 scroll;
    private bool showStatsSection = true;
    private bool showLevelSection = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        PlayerData _playerData = target as PlayerData;
        if (_playerData == null) return;

        PlayerLevel _levelData = _playerData.LevelData;
        StatsGrowth _statsGrowthData = _playerData.statsData;

        EditorGUIUtility.labelWidth = 200;

        showLevelSection = EditorGUILayout.Toggle("Show Experience Per Level", showLevelSection);
        showStatsSection = EditorGUILayout.Toggle("Show Stats Per Level", showStatsSection);
        
        // Exp
        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.LabelField("Total EXP ");
        EditorGUILayout.LabelField(_levelData.TotalExperience + " XP");
        EditorGUILayout.EndHorizontal();
        
        // Level
        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.LabelField("Current Level ");
        EditorGUILayout.LabelField(_levelData.CurrentLevel.ToString());
        EditorGUILayout.EndHorizontal();
        
        // Stats
        
        EditorGUIUtility.labelWidth = 100;

        if (showLevelSection)
        {
            float totalWidth = EditorGUIUtility.labelWidth * 9f;

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MaxHeight(300), GUILayout.Width(EditorGUIUtility.currentViewWidth - 20));

            EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            for (int i = 1; i <= _levelData.LevelCap; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Level " + i);
                EditorGUILayout.LabelField(_levelData.EvaluateExperience(i) + " XP");
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }

        EditorGUIUtility.labelWidth = 75;
        if (showStatsSection)
        {
            float totalWidth = EditorGUIUtility.labelWidth * 10f;

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MaxHeight(300), GUILayout.Width(EditorGUIUtility.currentViewWidth - 20));

            EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            for (int i = 1; i <= 10; i++)
            {
                CombatStats _lvlStats = _statsGrowthData.GetLeveledStats(i, 10);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level " + i, GUILayout.Width(EditorGUIUtility.labelWidth));
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
