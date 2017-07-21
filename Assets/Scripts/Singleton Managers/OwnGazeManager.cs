using UnityEngine;
using System.Collections;
using System;

public class OwnGazeManager : Singleton<OwnGazeManager>
{
    #region Strategy implementation
    private interface IGazeStrategy
    {
        void Alghorithm();
    }

    private class GazeNoneStrategy : IGazeStrategy
    {
        public void Alghorithm() { }
    }

    private class GazeStandartStrategy : IGazeStrategy
    {
        private OwnGazeManager _ownGaze;

        private int _spartialMeshLayer = LayerMask.NameToLayer("SpartialMesh");

        public GazeStandartStrategy(OwnGazeManager gaze)
        {
            _ownGaze = gaze;
        }

        public void Alghorithm()
        {
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            if(isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                //ADDITIONAL BY HSE
                _ownGaze.HitObject = hitInfo.collider.gameObject;

                IInteractive newFocused = hitInfo.transform.GetComponent<IInteractive>();
                if(newFocused != null)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if(_ownGaze._currentFocused == newFocused)
                    {
                        return;
                    }

                    if(_ownGaze._currentFocused != null)
                    {
                        if(_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        {
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocused);
                        }
                    }

                    _ownGaze._currentFocused = newFocused;

                    if(_ownGaze.onGazeEnterToInteractiveEvent != null)
                    {
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocused);
                    }
                }
                else
                {
                    if(hitInfo.transform.gameObject.layer == _spartialMeshLayer)
                    {
                        _ownGaze._hitObjectType = HitObjectType.Spatial;
                    }
                    else
                    {
                        _ownGaze._hitObjectType = HitObjectType.Default;
                    }
                }
            }
            else
            {
                _ownGaze._hitObjectType = HitObjectType.None;
            }

            if(_ownGaze.hitObjectType != HitObjectType.Interactive)
            {
                if(_ownGaze._currentFocused != null)
                {
                    if(_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                    {
                        _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocused);
                    }

                    _ownGaze._currentFocused = null; 
                }
            }
        }
    }

    private class GazeDragAndDropStrategy : IGazeStrategy
    {
        private OwnGazeManager _ownGaze;

        private int _spartialMeshLayer = LayerMask.NameToLayer("SpartialMesh");

        RaycastHit hitInfo;

        public GazeDragAndDropStrategy(OwnGazeManager gaze)
        {
            _ownGaze = gaze;
        }

        public void Alghorithm()
        {
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            if(isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                _ownGaze._hitObjectType = HitObjectType.Default;

                if(hitInfo.transform.gameObject.layer == _spartialMeshLayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Spatial;
                }
                else
                {
                    if(hitInfo.transform.GetComponent<IInteractive>() != null)
                    {
                        _ownGaze._hitObjectType = HitObjectType.Interactive;
                    }
                }
            }
            else
            {
                _ownGaze._hitObjectType = HitObjectType.None;
            }
        }
    }

    private class GazeDemostrationStrategy : IGazeStrategy
    {
        private OwnGazeManager _ownGaze;

        private int _spatialMeshLayer = LayerMask.NameToLayer("SpatialMesh");
        private int _demonstrationLayer = LayerMask.NameToLayer("Demonstration");

        RaycastHit hitInfo;

        public GazeDemostrationStrategy(OwnGazeManager gaze)
        {
            _ownGaze = gaze;
        }

        public void Alghorithm()
        {
            //bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            //if(isHit)
            //{
            //    _ownGaze._hitPoint = hitInfo.point;
            //    _ownGaze._pointNormal = hitInfo.normal;

            //    _ownGaze._hitObjectType = HitObjectType.Default;

            //    if(hitInfo.transform.gameObject.layer == _spatialMeshLayer)
            //    {
            //        _ownGaze._hitObjectType = HitObjectType.Spatial;
            //    }
            //    else
            //    {
            //        if(hitInfo.transform.gameObject.layer == _demonstrationLayer)
            //        {
            //            if(hitInfo.transform.GetComponent<IInteractive>() != null)
            //            {
            //                _ownGaze._hitObjectType = HitObjectType.Interactive;
            //                _ownGaze._currentFocused = hitInfo.transform.GetComponent<IInteractive>();
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    _ownGaze._hitObjectType = HitObjectType.None;
            //}

            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            if(isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                //ADDITIONAL BY HSE
                _ownGaze.HitObject = hitInfo.collider.gameObject;

                BtnTap newFocused = hitInfo.transform.GetComponent<BtnTap>();
                SkipGidButton newFocusedReset = hitInfo.transform.GetComponent<SkipGidButton>();

                Debug.Log("Standart Gaze");

                bool objectOnDemostrationlayer = hitInfo.transform.gameObject.layer == _demonstrationLayer;

                if(newFocused != null && objectOnDemostrationlayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if(_ownGaze._currentFocusedChapter == newFocused)
                    {
                        return;
                    }

                    if(_ownGaze._currentFocusedChapter != null)
                    {
                        if(_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        {
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);
                        }
                    }

                    _ownGaze._currentFocusedChapter = newFocused;

                    if(_ownGaze.onGazeEnterToInteractiveEvent != null)
                    {
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);
                    }
                }
                else if(newFocusedReset != null && objectOnDemostrationlayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if (_ownGaze._currentFocusedReset == newFocusedReset)
                    {
                        return;
                    }

                    if (_ownGaze._currentFocusedReset != null)
                    {
                        if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        {
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);
                        }
                    }

                    _ownGaze._currentFocusedReset = newFocusedReset;

                    if (_ownGaze.onGazeEnterToInteractiveEvent != null)
                    {
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);
                    }
                }
                else
                {
                    if(hitInfo.transform.gameObject.layer == _spatialMeshLayer)
                    {
                        _ownGaze._hitObjectType = HitObjectType.Spatial;
                    }
                    else
                    {
                        _ownGaze._hitObjectType = HitObjectType.Default;
                    }
                }
            }
            else
            {
                _ownGaze._hitObjectType = HitObjectType.None;
            }

            if(_ownGaze.hitObjectType != HitObjectType.Interactive)
            {
                if(_ownGaze._currentFocusedReset != null)
                {
                    if(_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                    {
                        _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);
                    }

                    _ownGaze._currentFocusedReset = null;
                }
            }
        }
    }
    #endregion

    #region Properties
    private bool _isInitialized;

    public enum HitObjectType
    {
        None,
        Default,
        Interactive,
        Spatial
    }

    //ADDITIONAL BY HSE
    public GameObject HitObject { get; set; }

    private IGazeStrategy _strategy;

    private IInteractive _currentFocused;
    private BtnTap _currentFocusedChapter;
    private SkipGidButton _currentFocusedReset;

    public IInteractive currentFocused { get { return _currentFocused; } }
    public BtnTap currentFocusedChapter { get { return _currentFocusedChapter; } }
    public SkipGidButton currentFocusedReset { get { return _currentFocusedReset; } }

    private HitObjectType _hitObjectType;
    public HitObjectType hitObjectType { get { return _hitObjectType; } }

    private Vector3 _hitPoint;
    public Vector3 hitPoint { get { return _hitPoint; } }

    private Vector3 _pointNormal;
    public Vector3 pointNormal { get { return _pointNormal; } }

    public event Action<IInteractive> onGazeEnterToInteractiveEvent;
    public event Action<IInteractive> onGazeLeaveFromInteractiveEvent;

    private string _strategyName;
    #endregion

    public void ChangeStrategyToNone()
    {
        Reset();
        _strategy = new GazeNoneStrategy();
        _strategyName = _strategy.GetType().ToString();
    }

    public void ChangeStrategyToStandart()
    {
        Reset();
        _strategy = new GazeStandartStrategy(this);
        _strategyName = _strategy.GetType().ToString();

        Debug.Log("!Change GazeStratedy To Standart");
    }

    public void ChangeStrategyToDragAndDrop()
    {
        if(_currentFocused != null)
        {
            onGazeLeaveFromInteractiveEvent.Invoke(_currentFocused);
        }

        Reset();
        _strategy = new GazeDragAndDropStrategy(this);
        _strategyName = _strategy.GetType().ToString();

        Debug.Log("!Change GazeStratedy To Drag'n'Drop");
    }

    public void ChangeStrategyDemonstration()
    {
        if(_currentFocusedChapter != null)
        {
            onGazeLeaveFromInteractiveEvent.Invoke(_currentFocusedChapter);
        }

        Reset();
        _strategy = new GazeDemostrationStrategy(this);
        Debug.Log("!Change GazeStratedy To Demo");
        _strategyName = _strategy.GetType().ToString();
    }

    private void Reset()
    {
        _currentFocusedChapter = null;
        _hitObjectType = HitObjectType.None;
    }

    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        ChangeStrategyToStandart();

        _isInitialized = true;
    }

    void Update()
    {
        Debug.Log(_strategyName);
        _strategy.Alghorithm();
    }
}