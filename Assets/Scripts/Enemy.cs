using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Enemy : MonoBehaviour
{
    // Engine in SF
    [SerializeField] private Popup destroyPopup;
    [SerializeField] private float moveForce;
    [SerializeField] private float moveDelay;
    [SerializeField] private int size;
    [SerializeField] private int points;
    private Rigidbody _rigidbody;
    private float _delay;

    public int GetPoints() => points;
    public int GetSize() => size;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _delay = 0;
    }

    private void Update()
    {
        if (_delay <= moveDelay)
        {
            _delay += Time.deltaTime;
        }
        if (_delay >= moveDelay)
        {
            var angle = Random.Range(0, 360);
            var moveVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(moveVector * moveForce, ForceMode.Impulse);
            _delay = 0;
        }
    }

    public void PopUp()
    {
        Instantiate(destroyPopup, transform.position + Vector3.up * 2, Quaternion.AngleAxis(90, Vector3.right));
    }
}
