using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private float lifetime;
    private TextMeshPro _mesh;
    private float _currentTime = 100000;
    private bool _isDisappearing = false;
    private void Start()
    {
        _mesh = GetComponent<TextMeshPro>();
        _mesh.text = text;
        transform.DOScale(transform.localScale * 1.5f, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            _currentTime = lifetime;
            _isDisappearing = true;
        });
    }

    private void Update()
    {
        if (_isDisappearing)
        {
            _currentTime -= Time.deltaTime;
            _mesh.color = new Color(_mesh.color.r, _mesh.color.g, _mesh.color.b, _currentTime / lifetime);
            // _meshColor = new Color(_meshColor.r, _meshColor.g, _meshColor.b, 0.5f);
        }

        if (_currentTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
