using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class SkipGidButton : MonoBehaviour, IInteractive
{
    [SerializeField]
    private List<ActionType> _allowedTypes;

    private Tween _colorTween;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private Color _selfColor;

	[SerializeField]
	private TextMeshPro _textPro;

    [SerializeField]
    private Color _highlightColor = Color.white;

    [SerializeField]
    private float _duration = 1f;

	private bool isTextPro;
	public bool playNextSection = false;

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

	private void Start()
	{
		if (_textPro != null) {
			isTextPro = true;
		}

		_text = GetComponentInChildren<Text>();

        if (_text != null) _text.color = Color.white;		
	}

    public void OnGazeEnter()
    {
		Debug.Log ("!SKIP ENTER: " + gameObject.name);
            
		if (!isTextPro) {
			_colorTween.Kill();
			_colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _highlightColor, _duration).Play();
		}

		if (isTextPro) {
			_colorTween.Kill();
			_colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _highlightColor, _duration).Play();
		}
        
    }

    public void OnGazeLeave()
    {
		if (!isTextPro) {
			_colorTween.Kill();
			_colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
		}

		if (isTextPro) {
			_colorTween.Kill();
			_colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _selfColor, _duration).Play();
		} 
    }

    public void OnGestureTap()
    {
        Debug.Log("!Skip Gid Button");
        if (gameObject.tag == "Free Mode")
        {
            Debug.Log("!Free Mode " + Environment.StackTrace);
            PlayerManager.Instance.ChangeStateToStandart();
            CutsceneManager.Instance.StopCutscene();
        }
        else
        {
			if (playNextSection) {
				CutsceneManager.Instance.NextChapter ();
			} else {
                Debug.Log("!Skip Cutscene");
				CutsceneManager.Instance.SkipCutscene();
			}
        }

        //GetComponentInParent<PeriodicTable>().SelectElement(GetComponentInChildren<TableElement>());
    }

    public void StopDrag() { }

    public bool TryToDrag() { return false; }
    public bool TryToResize() { return false; }
}
