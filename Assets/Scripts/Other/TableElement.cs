using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;

public class TableElement : MonoBehaviour, IInteractive
{
    private PeriodicTable _periodicTable;

    [SerializeField]
    private string _atomName;
    public string atomName { get { return _atomName; } }

    [SerializeField]
    private List<ActionType> _allowedActions = new List<ActionType>();

    private Color _selfColor = Color.black;
    private Color _highlightColor = Color.gray;
    private Color _selectColor = Color.white;

    [SerializeField]
    private TextMeshPro _elementText;

    private bool _isSelected;

    void Awake()
    {        
        _atomName = name;
        _periodicTable = GetComponentInParent<PeriodicTable>();
        _elementText = GetComponentInChildren<TextMeshPro>();
  
        _isSelected = false;

        Debug.Log("!Table element " + _atomName + " awaken");
    }

    public List<ActionType> GetAllowedActions() { return _allowedActions; }

    public void OnGazeEnter()
    {
        Debug.Log("Gaze entered at " + _atomName);
        if(_isSelected)
        {
            return;
        }

        _elementText.color = _highlightColor;
    }

    public void OnGazeLeave()
    {
        Debug.Log("Gaze left at " + _atomName);
        CanselHighlighting();
    }

    private void CanselHighlighting()
    {
        if(_isSelected)
        {
            return;
        }

        _elementText.color = _selfColor;
    }

    public void OnGestureTap()
    {
        Debug.Log("!Tapped the element " + _atomName);

        if(!_isSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }


    public void StopDrag() { }

    public bool TryToDrag() { return false; }

    public void Select()
    {
        Debug.Log("!Selected");

        _isSelected = true;
        _elementText.color = _selectColor;

        _periodicTable.SelectElement(this);
    }

    public void Deselect()
    {
        _isSelected = false;
        _elementText.color = _highlightColor;

        _periodicTable.DeselectElement(this);
    }

    public void CanselSelect()
    {
        Debug.Log("!Cancelling select");
        Deselect();
        CanselHighlighting();
    }

    public void SimpleDeselect()
    {
        _isSelected = false;
        _elementText.color = _selfColor;
    }

    public bool TryToResize() { return false; }
}
