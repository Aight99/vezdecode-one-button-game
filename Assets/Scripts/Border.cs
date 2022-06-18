using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private GameObject borderObject;
    [SerializeField] private int borderStep;
    [SerializeField] private int maxBorders;
    private void Awake()
    {
        var distance = transform.localScale.x / 2;
        for (int i = 0; i < maxBorders; i++)
        {
            var angle = Mathf.Lerp(0, 360, i / (float) maxBorders);
            var backgroundTransform = transform;
            var angleVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
            Instantiate(borderObject, angleVector * distance + backgroundTransform.position, Quaternion.identity);
        }
    }
}
