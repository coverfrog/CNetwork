using System;
using UnityEngine;

public class DestroyStart : MonoBehaviour
{
    private void Start()
    {
        DestroyImmediate(gameObject);
    }
}
