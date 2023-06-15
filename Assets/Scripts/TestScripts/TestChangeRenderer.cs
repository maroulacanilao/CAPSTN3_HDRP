using System.Collections;
using NaughtyAttributes;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

namespace TestScripts
{
    public class TestChangeRenderer : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] private Mesh newMesh;
    }
}
