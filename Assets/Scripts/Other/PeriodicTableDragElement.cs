using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class PeriodicTableDragElement : MonoBehaviour, IInteractive
{
    [SerializeField]
    private GameObject _periodicTable;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private Color _selfColor;

    [SerializeField]
    private Color _highlightColor = Color.white;

    [SerializeField]
    private float _duration = 1f;

    private Tween _colorTween;

    private int _oldLayer;

    private Coroutine _dragCoroutine;

    [SerializeField]
    private List<ActionType> _allowedTypes;

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

    public void OnGazeEnter()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _sprite.color; }, (c) => { _sprite.color = c; }, _highlightColor, _duration).Play();
    }

    public void OnGazeLeave()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _sprite.color; }, (c) => { _sprite.color = c; }, _selfColor, _duration).Play();
    }

    public void OnGestureTap() { }

    public bool TryToDrag()
    {
        Debug.Log("sd");
        _oldLayer = _periodicTable.layer;

        ChangeLayerRecursively(_periodicTable, LayerMask.NameToLayer("Ignore Raycast"));

        _dragCoroutine = StartCoroutine(DragCoroutine());

        return true;
    }

    IEnumerator DragCoroutine()
    {
        while(true)
        {
            _periodicTable.transform.position = Vector3.Lerp(_periodicTable.transform.position, OwnCursorManager.Instance.cursor.position, Time.deltaTime * 8f);
            _periodicTable.transform.rotation = Quaternion.Slerp(_periodicTable.transform.rotation, OwnCursorManager.Instance.cursor.rotation, Time.deltaTime * 8f) ;

            yield return null;
        }
    }

    public void StopDrag()
    {
        StopCoroutine(_dragCoroutine);

        _periodicTable.transform.position = OwnGazeManager.Instance.hitPoint;
        _periodicTable.transform.rotation = Quaternion.LookRotation(OwnGazeManager.Instance.pointNormal);
        
        ChangeLayerRecursively(_periodicTable, _oldLayer);
    }

    private void ChangeLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;

        foreach(Transform child in go.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }

    public bool TryToResize() { return false; }
}
