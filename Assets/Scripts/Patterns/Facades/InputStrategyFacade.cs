using UnityEngine;
using System.Collections;
using System;

public class InputStrategyFacade
{
    public enum Strategies { None, Standart, Demonstration, Resize, DragAndDrop}

    public Strategies Strategy { get; private set; }

    public void SetListeners(Action<IInteractive> gazeEnterHandler, Action<IInteractive> gazeLeaveHandler, Action<IInteractive> gestureTapHandler)
    {
        OwnGazeManager.Instance.onGazeEnterToInteractiveEvent += gazeEnterHandler;
        OwnGazeManager.Instance.onGazeLeaveFromInteractiveEvent += gazeLeaveHandler;
        OwnGestureManager.Instance.onTapEvent += gestureTapHandler;
    }

    public void SetListeneresForNavigation(Action<IInteractive> startNavigation, Action<IInteractive> updateNavigation/*, Action<IInteractive> completeNavigation, Action<IInteractive> cancelNavigation*/)
    {
        OwnGestureManager.Instance.navStart += startNavigation;
        OwnGestureManager.Instance.navUpdate += updateNavigation;
        //OwnGestureManager.Instance.navComplete += completeNavigation;
        //OwnGestureManager.Instance.navCancel += cancelNavigation;
    }

    public void ChangeStrategyToStandart()
    {
        Debug.Log("!Facade strategy changed to Standart Strategy");
        Strategy = Strategies.Standart;
        OwnGazeManager.Instance.ChangeStrategyToStandart();
        OwnGestureManager.Instance.ChangeStrategyToStandart();
    }

    public void ChangeStrategyToDragAndDrop()
    {
        Debug.Log("!Facade strategy changed to Drag'n'drop Strategy");
        Strategy = Strategies.DragAndDrop;
        OwnGazeManager.Instance.ChangeStrategyToDragAndDrop();
        OwnGestureManager.Instance.ChangeStrategyToDragAndDrop();
    }

    public void ChangeStrategyToDemonstration()
    {
        Debug.Log("!Facade strategy changed to Demonstration Strategy");
        Strategy = Strategies.Demonstration;
        OwnGazeManager.Instance.ChangeStrategyDemonstration();
        OwnGestureManager.Instance.ChangeStrategyToDemonstration();
    }

    public void ChangeStrategyToResize()
    {
        Debug.Log("!Facade strategy changed to Resize Strategy");
        Strategy = Strategies.Resize;
        OwnGazeManager.Instance.ChangeStrategyToStandart();
        OwnGestureManager.Instance.ChangeStrategyToResize();
    }
}
