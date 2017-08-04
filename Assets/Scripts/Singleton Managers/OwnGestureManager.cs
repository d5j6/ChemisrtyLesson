using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using System;
using TMPro;

public class OwnGestureManager : Singleton<OwnGestureManager>
{
    #region Strategy implementation
    private interface IGestureStrategy
    {
        void Alghoritm();
    }

    private class GestureNoneStrategy : IGestureStrategy
    {
        public void Alghoritm() {  }
    }

    private class GestureStandartStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            

            if(Instance.onTapEvent != null)
            {
                
                Instance.onTapEvent.Invoke(OwnGazeManager.Instance.currentFocused);
            }

            //if (Instance.navStart != null)
            //{
            //    Instance.navStart.Invoke(OwnGazeManager.Instance.currentFocused);
            //}

            //if (Instance.navUpdate != null)
            //{
            //    Instance.navUpdate.Invoke(OwnGazeManager.Instance.currentFocused);
            //}
        }
    }

    private class GestureResizeStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if (OwnGazeManager.Instance.hitObjectType != OwnGazeManager.HitObjectType.Interactive)
            {
                return;
            }

            if (Instance.onTapEvent != null)
            {
                Instance.onTapEvent.Invoke(OwnGazeManager.Instance.currentFocused);
            }

            if (Instance.navStart != null)
            {
                Debug.Log("Nav Start");
                Instance.navStart.Invoke(OwnGazeManager.Instance.currentFocused);
            }

            if (Instance.navUpdate != null)
            {
                Debug.Log("Nav Update");
                Instance.navUpdate.Invoke(OwnGazeManager.Instance.currentFocused);
            }
            else
            {
                Debug.Log("NavUpdate empty");
            }
        }
    }

    private class GestureDragAndDropStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if(OwnGazeManager.Instance.hitObjectType != OwnGazeManager.HitObjectType.Spatial)
            {
                return;
            }

            if(Instance.onTapEvent != null)
            {
                Instance.onTapEvent.Invoke(null);
            }
        }
    }

    private class GestureDemonstrationStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if(Instance.onTapEvent != null)
            {                
                if (OwnGazeManager.Instance.currentFocusedReset != null)
                {
                    
                    Instance.onTapEvent.Invoke(OwnGazeManager.Instance.currentFocusedReset);
                    return;
                }

                Debug.Log("!GestureDemonstrationStrategy. OnTapEvent with " + OwnGazeManager.Instance.currentFocusedChapter.name);
                Instance.onTapEvent.Invoke(OwnGazeManager.Instance.currentFocusedChapter);
            }
        }
    }

    #endregion

    #region Properties
    private bool _isInitialized;

    private IGestureStrategy _strategy;

    public KeyCode editorTapKey = KeyCode.F;
    private GestureRecognizer _tapGestureRecognizer;

    public event Action<IInteractive> onTapEvent;
    public event Action<IInteractive> navStart;
    public event Action<IInteractive> navUpdate;
    public event Action<IInteractive> navComplete;
    public event Action<IInteractive> navCancel;

    private string _strategyName;
    #endregion

    public void ChangeStrategyToNone()
    {
        if (_tapGestureRecognizer.IsCapturingGestures())
            _tapGestureRecognizer.StopCapturingGestures();

        _strategy = new GestureNoneStrategy();
        _strategyName = _strategy.GetType().ToString();

        _tapGestureRecognizer.StartCapturingGestures();
    }                                       

    public void ChangeStrategyToStandart()
    {
        if (_tapGestureRecognizer.IsCapturingGestures())
            _tapGestureRecognizer.StopCapturingGestures();

        _strategy = new GestureStandartStrategy();
        _strategyName = _strategy.GetType().ToString();

        _tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void ChangeStrategyToResize()
    {
        if (_tapGestureRecognizer.IsCapturingGestures())
            _tapGestureRecognizer.StopCapturingGestures();

        _strategy = new GestureResizeStrategy();
        _strategyName = _strategy.GetType().ToString();

        _tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void ChangeStrategyToDragAndDrop()
    {
        if(_tapGestureRecognizer.IsCapturingGestures())
            _tapGestureRecognizer.StopCapturingGestures();

        _strategy = new GestureDragAndDropStrategy();
        _strategyName = _strategy.GetType().ToString();

        _tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void ChangeStrategyToDemonstration()
    {
        if (_tapGestureRecognizer.IsCapturingGestures())
            _tapGestureRecognizer.StopCapturingGestures();

        _strategy = new GestureDemonstrationStrategy();
        _strategyName = _strategy.GetType().ToString();

        _tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        _tapGestureRecognizer = new GestureRecognizer();

        _tapGestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX);
        _tapGestureRecognizer.TappedEvent += OnTap;
        _tapGestureRecognizer.NavigationStartedEvent += NavigationStart;
        _tapGestureRecognizer.NavigationUpdatedEvent += NavigationUpdate;
        _tapGestureRecognizer.NavigationCompletedEvent += NavigationComplete;
        _tapGestureRecognizer.NavigationCanceledEvent += NavigationCansel;

        //Debug.Log("Events initialized");

        ChangeStrategyToNone();

        _isInitialized = true;
    }

    void OnTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        
        
        _strategy.Alghoritm();
    }

    void NavigationStart(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        Debug.Log("Works navStart");
        _strategy.Alghoritm();
    }

    void NavigationUpdate(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        _strategy.Alghoritm();
    }

    void NavigationComplete(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        _strategy.Alghoritm();
    }

    void NavigationCansel(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        _strategy.Alghoritm();
    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(editorTapKey))
        {
            Debug.Log("!" + _strategyName);
            _strategy.Alghoritm();
        }
    }
#endif
}
