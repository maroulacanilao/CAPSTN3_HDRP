using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class TestShake : MonoBehaviour
{
    public float duration = 1f, strength = 10;
    [Button("Shake")]
    private void Shake()
    {
        transform.Co_DoHitEffect();
    }
}
