﻿// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

namespace SpectatorView.InputModule
{
    /// <summary>
    /// SV_HandRotatable is a modification of HoloTools HandScalable script, 
    /// but it also sharing transform between all devices using SV_Sharing class
    /// </summary>
    public class SV_HandScalable : MonoBehaviour,
                                IManipulationHandler
    {
        #region Public Fields

        public bool IsEnabled = true;

        public string SV_SharingTag = "";

        public float min = 0.5f;
        public float max = 2f;

        [Tooltip("Auto calculate min and max bounds")]
        public bool AutoCalc = true;
        [Tooltip("How much times scale model")]
        public float multiply = 2;

        #endregion

        #region Actions

        public Action OnStart;
        public Action OnUpdate;
        public Action OnComplete;
        public Action OnCancel;

        public Action OnSizeChanged;

        #endregion

        #region Main Methods

        private void Start()
        {
            if (AutoCalc)
            {
                min = transform.localScale.x / multiply;
                max = transform.localScale.x * multiply;
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            IsEnabled = false;
#endif
        }

        #endregion

        #region Events

        public void OnManipulationStarted(ManipulationEventData eventData)
        {
            if (OnStart != null)
            {
                OnStart();
            }
        }

        public void OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (IsEnabled)
            {
                if (eventData.CumulativeDelta.x > 0)
                {
                    transform.localScale += Vector3.one * (max - min) * Time.deltaTime;
                }
                else
                {
                    transform.localScale -= Vector3.one * (max - min) * Time.deltaTime;
                }

                transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x, min, max);

                SV_ShareTransform();

                if (OnUpdate != null)
                {
                    OnUpdate();
                }

                if (OnSizeChanged != null)
                {
                    OnSizeChanged();
                }
            }
        }

        public void OnManipulationCompleted(ManipulationEventData eventData)
        {
            SV_ShareTransform();

            if (OnComplete != null)
            {
                OnComplete();
            }
        }

        public void OnManipulationCanceled(ManipulationEventData eventData)
        {
            SV_ShareTransform();

            if (OnCancel != null)
            {
                OnCancel();
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
                                    transform.localScale,
                                    SV_SharingTag);
            }
        }

#endregion
    }
}
