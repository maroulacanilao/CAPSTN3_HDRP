using UnityEditor;
using UnityEngine;

public class GroupObjects : EditorWindow
{
    [MenuItem("Tools/Group Selected %g")]
    public static void Group()
    {
        GameObject group = new GameObject("Group");
        Undo.RegisterCreatedObjectUndo(group, "Group Selected");

        foreach (Transform transform in Selection.transforms)
        {
            Undo.SetTransformParent(transform, group.transform, "Group Selected");
        }

        Selection.activeGameObject = group;
    }
}
