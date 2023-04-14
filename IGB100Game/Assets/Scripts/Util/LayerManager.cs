using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask interactLayer;

    public LayerMask GroundLayer => groundLayer;
    public LayerMask InteractLayer => interactLayer;

    public static LayerManager i;

    private void Awake()
    {
        i = this;
    }
}
