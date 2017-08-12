// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace SpectatorView.InputModule
{
    /// <summary>
    /// SV_HandRotatable is a modification of HoloTools HandRotatable script, 
    /// but it also sharing transform between all devices using SV_Sharing class
    /// </summary>
    public class SV_HandRotatable : MonoBehaviour,
        INavigationHandler
    {
        #region Public Fields

        public bool IsEnabled = true;

        public string SV_SharingTag = "";

        public float rotationSensitivity = 10.0f;

        #endregion

        #region Private Fields

        private float rotationFactor;

        #endregion

        #region Actions

        public Action OnStart;
        public Action OnUpdate;
        public Action OnComplete;
        public Action OnCancel;

        #endregion

        #region Main Methods

        private void Update()
        {
#if UNITY_EDITOR
            IsEnabled = false;
#endif
        }

        #endregion

        #region Events

        public void OnNavigationStarted(NavigationEventData eventData)
        {
            if (IsEnabled && OnStart != null)
            {
                OnStart();
            }
        }

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                // rotate model
                rotationFactor = eventData.CumulativeDelta.x * rotationSensitivity;
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
                // share transform
                SV_ShareTransform();

                if (OnUpdate != null)
                {
                    OnUpdate();
                }
            }
        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                SV_ShareTransform();

                if (OnComplete != null)
                {
                    OnComplete();
                }
            }
        }

        public void OnNavigationCanceled(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                SV_ShareTransform();

                if (OnCancel != null)
                {
                    OnCancel();
                }
            }
        }

        #endregion

        #region Utility Methods

        public void SV_ShareTransform()
        {
            if (SV_Sharing.Instance.CanBeSent)
            {
                SV_Sharing.Instance.SendTransform(transform.localPosition,
                                              transform.localRotation,
                                              transform.localScale, SV_SharingTag);
            }
        }

#endregion
    }
}
