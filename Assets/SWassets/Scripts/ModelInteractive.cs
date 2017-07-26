using HoloTools.Unity.Input;
using UnityEngine;
using UnityEngine.Events;

public class ModelInteractive : MonoBehaviour
{
    #region Public Fields

    public bool MoveOnStart = false;

    public enum CurrentState { None, Move, Rotate, Scale }

    public CurrentState currentState = CurrentState.None;

    public UnityEvent StateChange;

    #endregion

    #region Private Fields

    private HandDraggable _handDraggable;

    private HandRotatable _handRotatable;

    private HandScalable _handScalable;

    #endregion

    #region Private Properties

    private HandDraggable HandDraggable
    {
        get
        {
            if (!_handDraggable)
            {
                _handDraggable = GetComponent<HandDraggable>();
            }

            return _handDraggable;
        }
    }

    private HandRotatable HandRotatable
    {
        get
        {
            if (!_handRotatable)
            {
                _handRotatable = GetComponent<HandRotatable>();
            }

            return _handRotatable;
        }
    }

    private HandScalable HandScalable
    {
        get
        {
            if (!_handScalable)
            {
                _handScalable = GetComponent<HandScalable>();
            }

            return _handScalable;
        }
    }

    #endregion

    #region Main Methods

    private void Start()
    {
        DisableInteractivity(); // Disable all interactivity onStart

        if (MoveOnStart)
        {
            ToggleMove();
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Toggle Model Move State
    /// </summary>
    public void ToggleMove()
    {
        if (HandDraggable)
        {
            DisableInteractivity();
            HandDraggable.IsEnabled = !HandDraggable.IsEnabled;
            SetState();
        }
    }

    /// <summary>
    /// Toggle Model Rotate State
    /// </summary>
    public void ToggleRotate()
    {
        if (HandRotatable)
        {
            DisableInteractivity();
            HandRotatable.IsEnabled = !HandRotatable.IsEnabled;
            SetState();
        }
    }
    
    /// <summary>
    /// Toggle Model Scale State
    /// </summary>
    public void ToggleScale()
    {
        if (HandScalable)
        {
            DisableInteractivity();
            HandScalable.IsEnabled = !HandScalable.IsEnabled;
            SetState();
        }
    }

    /// <summary>
    /// Disable Model Interactivity
    /// </summary>
    public void DisableInteractivity()
    {
        if (HandDraggable)
        {
            HandDraggable.IsEnabled = false;
        }

        if (HandRotatable)
        {
            HandRotatable.IsEnabled = false;
        }

        if (HandScalable)
        {
            HandScalable.IsEnabled = false;
        }

        SetState();
    }

    private void SetState()
    {
        currentState = CurrentState.None;

        if (HandDraggable)
        {
            if (HandDraggable.IsEnabled)
            {
                currentState = CurrentState.Move;
            }
        }

        if (HandRotatable)
        {
            if (HandRotatable.IsEnabled)
            {
                currentState = CurrentState.Rotate;
            }
        }

        if (HandScalable)
        {
            if (HandScalable.IsEnabled)
            {
                currentState = CurrentState.Scale;
            }
        }

        StateChange.Invoke();
    }

    #endregion
}
