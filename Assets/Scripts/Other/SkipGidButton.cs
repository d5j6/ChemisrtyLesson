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
    private Color _highlightFreeModeColor = Color.cyan;

    [SerializeField]
    private float _duration = 1f;

	private bool isTextPro;
    private bool isFreeMode;
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

        isFreeMode = true;
        HighlightFreeModeText();
    }

    private void HighlightFreeModeText()
    {
        if (!isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _highlightFreeModeColor, _duration).Play();
        }

        if (isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _highlightFreeModeColor, _duration).Play();
        }
    }

    private void DeHighlightFreeModeText()
    {
        if (!isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
        }

        if (isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _selfColor, _duration).Play();
        }
    }

    public void OnGazeEnter()
    {
		Debug.Log ("!SKIP ENTER: " + gameObject.name);

        if (isFreeMode)
            return;

        if (!isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _highlightColor, _duration).Play();
        }

        if (isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _highlightColor, _duration).Play();
        }
    }

    public void OnGazeLeave()
    {
        if (isFreeMode)
            return;

        if (!isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
        }

        if (isTextPro)
        {
            _colorTween.Kill();
            _colorTween = DOTween.To(() => { return _textPro.color; }, (c) => { _textPro.color = c; }, _selfColor, _duration).Play();
        }
    }

    public void OnGestureTap()
    {
        Debug.Log("!Skip Gid Button");
        if(PlayerManager.Instance.Strategy == InputStrategyFacade.Strategies.Standart)
        {
            DeHighlightFreeModeText();
            isFreeMode = false;

            //Destroying current projection of an atom before changing to demonstration mode
            PeriodicTable periodicTable = GetComponentInParent<PeriodicTable>();
            if (periodicTable.SelectedElement != null)
                periodicTable.SelectedElement.CanselSelect();

            PlayerManager.Instance.ChangeStateToDemonstration();
            return;
        }

        if (PlayerManager.Instance.Strategy == InputStrategyFacade.Strategies.Demonstration)
        {
            //if (gameObject.tag == "Free Mode")
            //{
            //    Debug.Log("!Free Mode");
            //    CutsceneManager.Instance.StopCutscene();
            //}
            //else
            //{
            //    if (playNextSection)
            //    {
            //        CutsceneManager.Instance.NextChapter();
            //    }
            //    else
            //    {
            //        Debug.Log("!Skip Cutscene");
            //        CutsceneManager.Instance.SkipCutscene();
            //    }
            //}

            HighlightFreeModeText();
            isFreeMode = true;

            if (!CutsceneManager.Instance.isStop)
            {
                CutsceneManager.Instance.StopCutscene();
                //CutsceneManager.Instance.StopAllCoroutines();
                CutsceneManager.Instance.DeactivateButton();
            }
            //CutsceneManager.Instance.
            PlayerManager.Instance.ChangeStateToStandart();
        }

        //GetComponentInParent<PeriodicTable>().SelectElement(GetComponentInChildren<TableElement>());
    }

    public void StopDrag() { }

    public bool TryToDrag() { return false; }
    public bool TryToResize() { return false; }
}
