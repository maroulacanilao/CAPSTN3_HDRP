#region

using UnityEngine;

#endregion

namespace ScriptableObjectData.WeightedDictionary
{
    [CreateAssetMenu
        (fileName = "WeightedDictionary", menuName = "ScriptableObjects/Weighted Dictionary/GameObjects")]
    public class WDSO_GameObject : WeightedDictionary_ScriptableObject<GameObject>
    {
    }
}