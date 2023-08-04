using System;
using Fungus;
using UnityEngine;

namespace Others
{
    public class Windmill : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 50f;

        private void Update()
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }
}
