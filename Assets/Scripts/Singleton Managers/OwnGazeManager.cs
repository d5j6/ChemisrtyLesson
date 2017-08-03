using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            //Additional by HSE
            //Clearing fields to avoid saving data about objects from Demonstration mode
            _ownGaze.currentFocusedChapter = null;
            _ownGaze.currentFocusedReset = null;

            RaycastHit hitInfo = _ownGaze.hitInfo;
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);
            _ownGaze.IsGazingAtObject = isHit;

            if (isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                //ADDITIONAL BY HSE
                _ownGaze.HitObject = hitInfo.collider.gameObject;

                IInteractive newFocused = hitInfo.transform.GetComponent<IInteractive>();
                if (newFocused != null)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if (_ownGaze._currentFocused == newFocused)
                    {
                        return;
                    }

                    if (_ownGaze._currentFocused != null)
                    {
                        if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        {
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocused);
                        }
                    }

                    _ownGaze._currentFocused = newFocused;

                    if (_ownGaze.onGazeEnterToInteractiveEvent != null)
                    {
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocused);
                    }
                }
                else
                {
                    if (hitInfo.transform.gameObject.layer == _spartialMeshLayer)
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

            if (_ownGaze.hitObjectType != HitObjectType.Interactive)
            {
                if (_ownGaze._currentFocused != null)
                {
                    if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
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

            if (isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                _ownGaze._hitObjectType = HitObjectType.Default;

                if (hitInfo.transform.gameObject.layer == _spartialMeshLayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Spatial;
                }
                else
                {
                    if (hitInfo.transform.GetComponent<IInteractive>() != null)
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
            //Additional by HSE
            //Clearing currentFocused to avoid saving data about objects from Standart mode
            _ownGaze.currentFocused = null;

            RaycastHit hitInfo = _ownGaze.hitInfo;
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            _ownGaze.IsGazingAtObject = isHit;

            if (isHit)
            {
                _ownGaze._hitPoint = hitInfo.point;
                _ownGaze._pointNormal = hitInfo.normal;

                //ADDITIONAL BY HSE
                _ownGaze.HitObject = hitInfo.collider.gameObject;

                BtnTap newFocused = hitInfo.transform.GetComponent<BtnTap>();
                SkipGidButton newFocusedReset = hitInfo.transform.GetComponent<SkipGidButton>();

                bool objectOnDemostrationlayer = hitInfo.transform.gameObject.layer == _demonstrationLayer;

                //Gaze detected a chapter in menu
                if (newFocused != null &&
                    objectOnDemostrationlayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if (_ownGaze._currentFocusedChapter == newFocused)
                        return;

                    if (_ownGaze._currentFocusedChapter != null)
                        if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);

                    _ownGaze._currentFocusedChapter = newFocused;

                    if (_ownGaze.onGazeEnterToInteractiveEvent != null)
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);
                }
                //Gaze detected a skipGidButton
                else if (newFocusedReset != null &&
                    objectOnDemostrationlayer)
                {
                    _ownGaze._hitObjectType = HitObjectType.Interactive;

                    if (_ownGaze._currentFocusedReset == newFocusedReset)
                        return;

                    if (_ownGaze._currentFocusedReset != null)
                        if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                            _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);

                    _ownGaze._currentFocusedReset = newFocusedReset;

                    if (_ownGaze.onGazeEnterToInteractiveEvent != null)
                        _ownGaze.onGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);
                }

                //Gaze could not detect neither chapterMenu, nor skipGidButton
                else
                {
                    //Defining hitObjectType
                    if (hitInfo.transform.gameObject.layer == _spatialMeshLayer)
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
                _ownGaze._hitObjectType = HitObjectType.None;

            if (_ownGaze.hitObjectType != HitObjectType.Interactive)
            {
                //Gaze lost the skipGidButton, but it has been detected before
                if (_ownGaze._currentFocusedReset != null)
                {
                    if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedReset);

                    _ownGaze._currentFocusedReset = null;
                }

                //Gaze lost the chapter, but it has been detected before
                if (_ownGaze._currentFocusedChapter != null)
                {
                    if (_ownGaze.onGazeLeaveFromInteractiveEvent != null)
                        _ownGaze.onGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);

                    _ownGaze._currentFocusedChapter = null;
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
    ////Basic gaze methods to identify physical and ui objects
    //private void RayCastUnityUI()
    //{
    //    if (UnityUIPointerEvent == null)
    //    {
    //        UnityUIPointerEvent = new PointerEventData(EventSystem.current);
    //    }

    //    // 2D cursor position
    //    Vector2 cursorScreenPos = Camera.main.WorldToScreenPoint(hitPoint);
    //    UnityUIPointerEvent.delta = cursorScreenPos - UnityUIPointerEvent.position;
    //    UnityUIPointerEvent.position = cursorScreenPos;

    //    // Graphics raycast
    //    rayCastResults.Clear();
    //    EventSystem.current.RaycastAll(UnityUIPointerEvent, rayCastResults);
    //    RaycastResult uiRaycastResult = FindClosestRaycastHitInLayermasks(rayCastResults, RaycastLayerMasks);
    //    UnityUIPointerEvent.pointerCurrentRaycast = uiRaycastResult;

    //    // If we have a raycast result, check if we need to overwrite the 3D raycast info
    //    if (uiRaycastResult.gameObject != null)
    //    {
    //        // Add the near clip distance since this is where the raycast is from
    //        float uiRaycastDistance = uiRaycastResult.distance + Camera.main.nearClipPlane;

    //        bool superseded3DObject = false;
    //        if (IsGazingAtObject)
    //        {
    //            // Check layer prioritization
    //            if (RaycastLayerMasks.Length > 1)
    //            {
    //                // Get the index in the prioritized layer masks
    //                int uiLayerIndex = FindLayerListIndex(uiRaycastResult.gameObject.layer, RaycastLayerMasks);
    //                int threeDLayerIndex = FindLayerListIndex(hitInfo.collider.gameObject.layer, RaycastLayerMasks);

    //                if (threeDLayerIndex > uiLayerIndex)
    //                {
    //                    superseded3DObject = true;
    //                }
    //                else if (threeDLayerIndex == uiLayerIndex)
    //                {
    //                    if (hitInfo.distance > uiRaycastDistance)
    //                    {
    //                        superseded3DObject = true;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (hitInfo.distance > uiRaycastDistance)
    //                {
    //                    superseded3DObject = true;
    //                }
    //            }
    //        }

    //        if (!IsGazingAtObject || superseded3DObject)
    //        {
    //            Debug.Log("UI object has been detected!");

    //            IsGazingAtObject = true;
    //            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(uiRaycastResult.screenPosition.x, uiRaycastResult.screenPosition.y, uiRaycastDistance));
    //            hitInfo = new RaycastHit()
    //            {
    //                distance = uiRaycastDistance,
    //                normal = -Camera.main.transform.forward,
    //                point = worldPos
    //            };

    //            HitObject = uiRaycastResult.gameObject;
    //        }
    //    }
    //}
    //private RaycastHit? PrioritizeHits(RaycastHit[] hits)
    //{
    //    if (hits.Length == 0)
    //        return null;

    //    for (int layerMaskIdx = 0; layerMaskIdx < RaycastLayerMasks.Length; layerMaskIdx++)
    //    {
    //        RaycastHit? minHit = null;

    //        for (int hitIdx = 0; hitIdx < hits.Length; hitIdx++)
    //        {
    //            RaycastHit hit = hits[hitIdx];
    //            if (IsLayerInLayerMask(hit.transform.gameObject.layer, RaycastLayerMasks[layerMaskIdx]) &&
    //                (minHit == null || hit.distance < minHit.Value.distance))
    //                minHit = hit;
    //        }

    //        if (minHit != null)
    //            return minHit;
    //    }

    //    return null;
    //}

    //private bool IsLayerInLayerMask(int layer, int layerMask)
    //{
    //    return ((1 << layer) & layerMask) != 0;
    //}

    //private RaycastResult FindClosestRaycastHitInLayermasks(List<RaycastResult> candidates, LayerMask[] layerMaskList)
    //{
    //    int combinedLayerMask = 0;

    //    for (int i = 0; i < layerMaskList.Length; i++)
    //        combinedLayerMask = combinedLayerMask | layerMaskList[i].value;

    //    RaycastResult? minHit = null;
    //    for (var i = 0; i < candidates.Count; ++i)
    //    {
    //        if (candidates[i].gameObject == null || !IsLayerInLayerMask(candidates[i].gameObject.layer, combinedLayerMask))
    //            continue;
    //        if (minHit == null || candidates[i].distance < minHit.Value.distance)
    //            minHit = candidates[i];
    //    }

    //    return minHit ?? new RaycastResult();
    //}

    //private int FindLayerListIndex(int layer, LayerMask[] layerMaskList)
    //{
    //    for (int i = 0; i < layerMaskList.Length; i++)
    //        if (IsLayerInLayerMask(layer, layerMaskList[i].value))
    //            return i;

    //    return -1;
    //}

    private IGazeStrategy _strategy;

    //Additional by HSE
    //Neccessary attributes for correct work of UI ray casting
    RaycastHit hitInfo;
    bool IsGazingAtObject;
    public PointerEventData UnityUIPointerEvent { get; private set; }
    public LayerMask[] RaycastLayerMasks = new LayerMask[] { Physics.DefaultRaycastLayers };
    private List<RaycastResult> rayCastResults = new List<RaycastResult>();

    private IInteractive _currentFocused;
    private BtnTap _currentFocusedChapter;
    private SkipGidButton _currentFocusedReset;

    public IInteractive currentFocused
    {
        get
        {
            return _currentFocused;
        }
        protected set
        {
            _currentFocused = value;
        }
    }
    public BtnTap currentFocusedChapter
    {
        get
        {
            return _currentFocusedChapter;
        }
        protected set
        {
            _currentFocusedChapter = value;
        }
    }
    public SkipGidButton currentFocusedReset
    {
        get
        {
            return _currentFocusedReset;
        }
        protected set
        {
            _currentFocusedReset = value;
        }
    }

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
        if (_currentFocused != null)
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
        if (_currentFocusedChapter != null)
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
        if (_isInitialized)
        {
            return;
        }

        ChangeStrategyToStandart();

        _isInitialized = true;
    }

    void Update()
    {
        Debug.Log(_strategyName);
        if (_strategy != null)
            _strategy.Alghorithm();
    }
}