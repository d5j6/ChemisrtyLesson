using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerManager : Singleton<PlayerManager>
{
    #region State implementation
    private interface IPlayerState
    {
        void OnGazeEnterHandler(IInteractive interactive);
        void OnGazeLeaveHandler(IInteractive interactive);
        void OnGestureTapHandler(IInteractive interactive);
        void TryToDragInteractive(IInteractive interactive);
        void StopDraggingInteractive(IInteractive draggingInteractive);
        void ChangeStateToDemonstration();
        void ChangeStateToStandart();
    }

    private interface IResizable : IPlayerState
    {
        
    }

    private class PlayerStandartState : IPlayerState
    {
        public void OnGazeEnterHandler(IInteractive interactive)
        {
            interactive.OnGazeEnter();
        }

        public void OnGazeLeaveHandler(IInteractive interactive)
        {
            interactive.OnGazeLeave();
        }

        public void OnGestureTapHandler(IInteractive interactive)
        {
            //Debug.Log(interactive);
            List<ActionType> allowedActionTypes = interactive.GetAllowedActions();

            //TODO: заменить работу исключительно с одним типом на работу со множеством в дальнейшем
            //ActionType allowedActionType = allowedActionTypes[0];

            foreach(ActionType allowedActionType in allowedActionTypes)
                switch (allowedActionType)
                {
                    case ActionType.TapOnly:
                        Debug.Log("!!!Player Standart Tap works" + Environment.StackTrace);
                        new TapCommand(interactive).Execute();
                        break;
                    case ActionType.DragAndDrop:
                        TryToDragInteractive(interactive);
                        break;
                    case ActionType.Resize:
                        TryToResizeInteractive(interactive);
                        break;
                    default:
                        break;
                }
        }

        public void TryToResizeInteractive (IInteractive interactive)
        {
            new ResizeCommand(interactive).Execute();
        }

        public void TryToDragInteractive(IInteractive interactive)
        {
            new DragCommand(interactive).Execute();
        }

        public void StopDraggingInteractive(IInteractive draggingInteractive) { }

        public void ChangeStateToDemonstration()
        {
            Instance._inputFacade.ChangeStrategyToDemonstration();
            Instance._state = new PlayerDemonstrationState();
            Instance._stateName = Instance._state.GetType().ToString();
        }

        public void ChangeStateToStandart() { }
    }

    private class PlayerDragAndDropState : IPlayerState
    {
        private IInteractive _draggingInteractive;

        public PlayerDragAndDropState(IInteractive interactive)
        {
            _draggingInteractive = interactive;
        }

        public void ChangeStateToDemonstration() { }

        public void ChangeStateToStandart() { }

        public void OnGazeEnterHandler(IInteractive interactive) { }

        public void OnGazeLeaveHandler(IInteractive interactive) { }

        public void OnGestureTapHandler(IInteractive interactive)
        {
            StopDraggingInteractive(_draggingInteractive);
        }

        public void StopDraggingInteractive(IInteractive draggingInteractive)
        {
            if(draggingInteractive == null)
            {
                return;
            }

            draggingInteractive.StopDrag();

            Instance._state = new PlayerStandartState();
            Instance._stateName = Instance._state.GetType().ToString();

            //Instance._inputFacade.ChangeStrategyToStandart();
            Instance._inputFacade.ChangeStrategyToDemonstration();
        }

        public void TryToDragInteractive(IInteractive interactive) { }
    }

    private class PlayerDemonstrationState : IPlayerState
    {
        public void ChangeStateToDemonstration() { }

        public void ChangeStateToStandart()
        {
            Instance._inputFacade.ChangeStrategyToStandart();
            Instance._state = new PlayerStandartState();
            Instance._stateName = Instance._state.GetType().ToString();
        }

        public void OnGazeEnterHandler(IInteractive interactive)
        {
            interactive.OnGazeEnter();
            //SkipGidButton test = GameObject.FindObjectOfType<SkipGidButton>();
            //if (test != null)
            //{
            //    test.OnGazeEnter();
            //}
        }

        public void OnGazeLeaveHandler(IInteractive interactive)
        {
              interactive.OnGazeLeave();
            //SkipGidButton test = GameObject.FindObjectOfType<SkipGidButton>();
            //if (test != null)
            //{
            //    test.OnGazeLeave();
            //}
        }

        public void OnGestureTapHandler(IInteractive interactive)
        {
            Debug.Log("!!!Player Demonstration Tap works");
            new TapCommand(interactive).Execute();
        }

        public void StopDraggingInteractive(IInteractive draggingInteractive) { }

        public void TryToDragInteractive(IInteractive interactive) { }
    }
    #endregion

    #region Command implementation
    private interface ICommand
    {
        void Execute();
    }

    private class TapCommand : ICommand
    {
        private IInteractive _interactive;

        public TapCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            _interactive.OnGestureTap();
        }
    }

    private class DragCommand : ICommand
    {
        private IInteractive _interactive;

        public DragCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            if(_interactive.TryToDrag())
            {
                Instance._inputFacade.ChangeStrategyToDragAndDrop();

                Instance._state = new PlayerDragAndDropState(_interactive);
                Instance._stateName = Instance._state.GetType().ToString();
            }
        }
    }

    private class ResizeCommand : ICommand
    {
        private IInteractive _interactive;

        public ResizeCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            if (_interactive.TryToResize())
            {
                Instance._inputFacade.ChangeStrategyToResize();

                Instance._state = new PlayerStandartState();
                Instance._stateName = Instance._state.GetType().ToString();
            }
        }
    }

    #endregion

    #region Properties
    private bool _isInitialized;

    private IPlayerState _state;

    private InputStrategyFacade _inputFacade;

    private string _stateName;
    #endregion

    #region Initialize and Start methods
    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        _state = new PlayerStandartState();
        _stateName = _state.GetType().ToString();

        _inputFacade = new InputStrategyFacade();

        _inputFacade.SetListeners(OnGazeEnterHandler, OnGazeLeaveHandler, OnGestureTapHandler);
        _inputFacade.SetListeneresForNavigation(OnNavigationStart, OnNavigationUpdate);
        //_inputFacade.ChangeStrategyToResize();
        //_inputFacade.ChangeStrategyToDemonstration();
        _inputFacade.ChangeStrategyToStandart();
        _isInitialized = true;
    }
    #endregion

    #region Input events handlers
    void OnGazeEnterHandler(IInteractive interactive)
    {
        _state.OnGazeEnterHandler(interactive);
    }

    void OnGazeLeaveHandler(IInteractive interactive)
    {
        _state.OnGazeLeaveHandler(interactive);
    }

    void OnGestureTapHandler(IInteractive interactive)
    {
        _state.OnGestureTapHandler(interactive);
    }

    void OnNavigationStart(IInteractive interactive)
    {
        _state.OnGestureTapHandler(interactive);
    }

    void OnNavigationUpdate(IInteractive interactive)
    {
        _state.OnGestureTapHandler(interactive);
    }
    #endregion

    public void TapOnInteractive(IInteractive interactive)
    {
        _state.OnGestureTapHandler(interactive);
    }

    public void TryToDragInteractive(IInteractive interactive)
    {
        _state.TryToDragInteractive(interactive);
    }

    public void StopDraggingInteractive(IInteractive draggingInteractive)
    {
        _state.StopDraggingInteractive(draggingInteractive);
    }

    public void ChangeStateToDemonstration()
    {
        _state.ChangeStateToDemonstration();
    }

    public void ChangeStateToStandart()
    {
        _state.ChangeStateToStandart();
    }
}
