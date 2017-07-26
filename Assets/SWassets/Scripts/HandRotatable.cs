// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.Events;

namespace HoloTools.Unity.Input
{
    /// <summary>
    /// HandRotatable component - for object rotation
    /// </summary>
    public class HandRotatable : MonoBehaviour,
                                 INavigationHandler
    {
        #region Public Fields

        public bool IsEnabled = true;

        public float rotationSensitivity = 10.0f;

        public string SV_tag;

        #endregion

        #region Private Fields

        private float rotationFactor;

        #endregion

        #region Main Methods

        private void Start()
        {
            if (SV_tag == String.Empty)
            {
                SV_tag = gameObject.name;
            }
        }

        #endregion

        #region Events

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                rotationFactor = eventData.CumulativeDelta.x * rotationSensitivity;
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));

                // send transform for SV
                SV_Sharing.Instance.SendTransform(transform.localPosition,
                                transform.localRotation,
                                transform.localScale,
                                SV_tag);
            }
        }

        public void OnNavigationStarted(NavigationEventData eventData) { }

        public void OnNavigationCompleted(NavigationEventData eventData) { }

        public void OnNavigationCanceled(NavigationEventData eventData) { }

        #endregion
    }
}
