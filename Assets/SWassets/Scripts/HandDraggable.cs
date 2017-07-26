// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

namespace HoloTools.Unity.Input
{
    /// <summary>
    /// HandDraggable component - using hand coordinates to drag object
    /// </summary>
    public class HandDraggable : MonoBehaviour,
                                 IFocusable,
                                 IInputHandler,
                                 ISourceStateHandler
    {
        #region Public Fields

        /// <summary>
        /// Event triggered when dragging starts.
        /// </summary>
        public event Action StartedDragging;

        /// <summary>
        /// Event triggered when dragging stops.
        /// </summary>
        public event Action StoppedDragging;

        public bool IsEnabled = true;

        public enum MoveAxis { XYZ, X, Y };

        public MoveAxis moveAxis;

        public bool FreezeRotation = false;

        [Tooltip("Transform that will be dragged. Defaults to the object of the component.")]
        public Transform HostTransform;

        [Tooltip("Scale by which hand movement in z is multipled to move the dragged object.")]
        public float DistanceScale = 2f;

        [Tooltip("Should the object be kept upright as it is being dragged?")]
        public bool IsKeepUpright = false;

        [Tooltip("Should the object be oriented towards the user as it is being dragged?")]
        public bool IsOrientTowardsUser = false;

        public string SV_tag;

        #endregion

        #region Private Fields

        private Camera mainCamera;

        private bool isDragging;
        private bool isGazed;

        private Vector3 objRefUp;
        private Vector3 objRefForward;
        private Vector3 objRefGrabPoint;
        private Vector3 draggingPosition;

        private float objRefDistance;
        private float handRefDistance;

        private Quaternion draggingRotation;
        private Quaternion gazeAngularOffset;

        private IInputSource currentInputSource = null;
        private uint currentInputSourceId;

        private Transform popupParent;

        private Vector3 startPosition;

        private Quaternion startRotation;

        private Transform Line;
        private MeshRenderer LineMeshRend;

        #endregion

        #region Main Methods

        private void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }

            mainCamera = Camera.main;

            if (SV_tag == String.Empty)
            {
                SV_tag = gameObject.name;
            }
        }

        private void OnDestroy()
        {
            if (isDragging)
            {
                StopDragging();
            }

            if (isGazed)
            {
                OnFocusExit();
            }
        }

        private void Update()
        {
            if (IsEnabled && isDragging)
            {
                UpdateDragging();
            }
        }

        /// <summary>
        /// Starts dragging the object.
        /// </summary>
        public void StartDragging()
        {
            if (!IsEnabled || isDragging)
            {
                return;
            }

            // Add self as a modal input handler, to get all inputs during the manipulation
            InputManager.Instance.PushModalInputHandler(gameObject);

            isDragging = true;

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;
            Vector3 handPosition;
            currentInputSource.TryGetPosition(currentInputSourceId, out handPosition);

            Vector3 pivotPosition = GetHandPivotPosition();
            handRefDistance = Vector3.Magnitude(handPosition - pivotPosition);
            objRefDistance = Vector3.Magnitude(gazeHitPosition - pivotPosition);

            Vector3 objForward = HostTransform.forward;
            Vector3 objUp = HostTransform.up;

            // Store where the object was grabbed from
            objRefGrabPoint = mainCamera.transform.InverseTransformDirection(HostTransform.position - gazeHitPosition);

            Vector3 objDirection = Vector3.Normalize(gazeHitPosition - pivotPosition);
            Vector3 handDirection = Vector3.Normalize(handPosition - pivotPosition);

            objForward = mainCamera.transform.InverseTransformDirection(objForward);       // in camera space
            objUp = mainCamera.transform.InverseTransformDirection(objUp);                 // in camera space
            objDirection = mainCamera.transform.InverseTransformDirection(objDirection);   // in camera space
            handDirection = mainCamera.transform.InverseTransformDirection(handDirection); // in camera space

            objRefForward = objForward;
            objRefUp = objUp;

            // Store the initial offset between the hand and the object, so that we can consider it when dragging
            gazeAngularOffset = Quaternion.FromToRotation(handDirection, objDirection);
            draggingPosition = gazeHitPosition;

            startPosition = draggingPosition + mainCamera.transform.TransformDirection(objRefGrabPoint);
            startRotation = HostTransform.rotation;

            StartedDragging.RaiseEvent();
        }

        /// <summary>
        /// Gets the pivot position for the hand, which is approximated to the base of the neck.
        /// </summary>
        /// <returns>Pivot position for the hand.</returns>
        private Vector3 GetHandPivotPosition()
        {
            Vector3 pivot = Camera.main.transform.position
                + new Vector3(0, -0.2f, 0)
                - Camera.main.transform.forward * 0.2f; // a bit lower and behind
            return pivot;
        }

        /// <summary>
        /// Enables or disables dragging.
        /// </summary>
        /// <param name="isEnabled">Indicates whether dragging shoudl be enabled or disabled.</param>
        public void SetDragging(bool isEnabled)
        {
            if (IsEnabled == isEnabled)
            {
                return;
            }

            IsEnabled = isEnabled;

            if (isDragging)
            {
                StopDragging();
            }
        }

        /// <summary>
        /// Update the position of the object being dragged.
        /// </summary>
        private void UpdateDragging()
        {
            Vector3 newHandPosition;
            currentInputSource.TryGetPosition(currentInputSourceId, out newHandPosition);

            Vector3 pivotPosition = GetHandPivotPosition();

            Vector3 newHandDirection = Vector3.Normalize(newHandPosition - pivotPosition);

            newHandDirection = mainCamera.transform.InverseTransformDirection(newHandDirection); // in camera space
            Vector3 targetDirection = Vector3.Normalize(gazeAngularOffset * newHandDirection);
            targetDirection = mainCamera.transform.TransformDirection(targetDirection); // back to world space

            float currenthandDistance = Vector3.Magnitude(newHandPosition - pivotPosition);

            float distanceRatio = currenthandDistance / handRefDistance;
            float distanceOffset = distanceRatio > 0 ? (distanceRatio - 1f) * DistanceScale : 0;
            float targetDistance = objRefDistance + distanceOffset;

            draggingPosition = pivotPosition + (targetDirection * targetDistance);

            if (IsOrientTowardsUser)
            {
                draggingRotation = Quaternion.LookRotation(HostTransform.position - pivotPosition);
            }
            else
            {
                Vector3 objForward = mainCamera.transform.TransformDirection(objRefForward); // in world space
                Vector3 objUp = mainCamera.transform.TransformDirection(objRefUp);   // in world space
                draggingRotation = Quaternion.LookRotation(objForward, objUp);
            }

            // Apply Final Position
            Vector3 finalPosition = draggingPosition + mainCamera.transform.TransformDirection(objRefGrabPoint);

            switch (moveAxis)
            {
                case MoveAxis.X:
                    finalPosition.y = startPosition.y;
                    finalPosition.z = startPosition.z;
                    break;

                case MoveAxis.Y:
                    finalPosition.x = startPosition.x;
                    finalPosition.z = startPosition.z;
                    break;
            }

            HostTransform.position = finalPosition;

            HostTransform.rotation = !FreezeRotation 
                ? draggingRotation 
                : startRotation;

            if (IsKeepUpright)
            {
                Quaternion upRotation = Quaternion.FromToRotation(HostTransform.up, Vector3.up);
                HostTransform.rotation = !FreezeRotation
                    ? upRotation * HostTransform.rotation
                    : startRotation;
            }

            SV_Sharing.Instance.SendTransform(HostTransform.localPosition,
                                HostTransform.localRotation,
                                HostTransform.localScale,
                                SV_tag);

            SV_Sharing.Instance.SendValue("test",
                "tag");

        }

        /// <summary>
        /// Stops dragging the object.
        /// </summary>
        public void StopDragging()
        {
            if (!isDragging)
            {
                return;
            }

            // Remove self as a modal input handler
            InputManager.Instance.PopModalInputHandler();

            isDragging = false;
            currentInputSource = null;

            StoppedDragging.RaiseEvent();
        }

        #endregion

        #region Events

        public void OnFocusEnter()
        {
            if (!IsEnabled)
            {
                return;
            }

            if (isGazed)
            {
                return;
            }

            isGazed = true;
        }

        public void OnFocusExit()
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!isGazed)
            {
                return;
            }

            isGazed = false;
        }

        // This is Enter Point
        public void OnInputDown(InputEventData eventData)
        {
            // We're already handling drag input, so we can't start a new drag operation.
            if (isDragging)
            {
                return;
            }

            if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                // The input source must provide positional data for this script to be usable
                return;
            }

            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartDragging();
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (currentInputSource != null &&
                eventData.SourceId == currentInputSourceId)
            {
                StopDragging();
            }
        }

        public void OnSourceDetected(SourceStateEventData eventData) { }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (currentInputSource != null
                && eventData.SourceId == currentInputSourceId)
            {
                StopDragging();
            }
        }

        #endregion
    }
}
