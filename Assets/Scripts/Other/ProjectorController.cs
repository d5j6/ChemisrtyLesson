using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

// AbstractFactory used for atom creation
public class ProjectorController : MonoBehaviour, IInteractive
{
    [SerializeField]
    private List<ActionType> allowedActions;

    private int _oldLayer;

    private Coroutine _dragCoroutine;

    private Tween _brightnessTween;

    [SerializeField]
    private float _duration = 1f;

    private Material _material;

    private AtomFactory atomFactory;

    private Atom _currentAtom;

    [SerializeField]
    private Transform _spawnPlace;

    [SerializeField]
    private AtomUIStateController _atomUIState;

    public bool IsProjectingAtom { get; private set; }

    void Awake()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
        atomFactory = new AtomFactory();
        IsProjectingAtom = false;
    }

    public List<ActionType> GetAllowedActions()
    {
        return allowedActions;
    }

    public void OnGazeEnter()
    {
        
        _brightnessTween.Kill();
        _brightnessTween = DOTween.To(() => { return _material.GetFloat("_Brightness"); }, (b) => { _material.SetFloat("_Brightness", b); }, 2f, _duration).Play();
    }

    public void OnGazeLeave()
    {
        _brightnessTween.Kill();
        _brightnessTween = DOTween.To(() => { return _material.GetFloat("_Brightness"); }, (b) => { _material.SetFloat("_Brightness", b); }, 1f, _duration).Play();
    }

    public void OnGestureTap() { }

    public void StopDrag()
    {
        StopCoroutine(_dragCoroutine);

        gameObject.transform.position = OwnGazeManager.Instance.hitPoint;
        gameObject.transform.rotation = Quaternion.LookRotation(OwnGazeManager.Instance.pointNormal);

        ChangeLayerRecursively(gameObject, _oldLayer);
    }

    public bool TryToDrag()
    {
        _oldLayer = gameObject.layer;

        ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("Ignore Raycast"));

        _dragCoroutine = StartCoroutine(DragCoroutine());

        return true;
    }

    IEnumerator DragCoroutine()
    {
        while(true)
        {
            transform.position = Vector3.Lerp(gameObject.transform.position, OwnCursorManager.Instance.cursor.position, Time.deltaTime * 8f);
            transform.rotation = Quaternion.Slerp(transform.rotation, OwnCursorManager.Instance.cursor.rotation, Time.deltaTime * 8f) ;

            yield return null;
        }
    }

    private void ChangeLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;

        foreach(Transform child in go.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }

    void OnDestroy()
    {
        _material.color = Color.gray;
    }

    public void CreateAtomProjection(TableElement element)
    {
        DestroyAtomProjection();

        _currentAtom = atomFactory.CreateAtom(element.atomName);

        _currentAtom.transform.SetParent(_spawnPlace);
        _currentAtom.transform.localPosition = Vector3.zero;
        _currentAtom.transform.localRotation = Quaternion.identity;

        _atomUIState.atomNameText.text = _currentAtom.atomInformation.name;
        _atomUIState.atomFormulaText.text = _currentAtom.atomInformation.formula;
        _atomUIState.gameObject.SetActive(true);
        _atomUIState.ChangeViewToClassic3D();

        IsProjectingAtom = true;
    }

    public Atom GetProjectedAtom()
    {
        return _currentAtom;
    }

    public void DestroyAtomProjection()
    {
        _atomUIState.gameObject.SetActive(false);

        if(_currentAtom != null)
        {
            Destroy(_currentAtom.gameObject);
        }

        IsProjectingAtom = false;
    }

    public bool TryToResize() { return false; }
}
