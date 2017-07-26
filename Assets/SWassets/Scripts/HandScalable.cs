// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

namespace HoloTools.Unity.Input
{
    public class HandScalable : MonoBehaviour,
                                IManipulationHandler
    {
        #region Public Fields

        public bool IsEnabled = true;

        public float min = 0.5f;
        public float max = 2f;
		
		[Tooltip("Auto calculate min and max bounds")]
        public bool AutoCalc = true;
        [Tooltip("How much times scale model")]
        public float multiply = 2;

        public string SV_tag;

        #endregion

        #region Main Methods

        private void Start()
		{
			if (AutoCalc)
            {
                min = transform.localScale.x / multiply;
                max = transform.localScale.x * multiply;
            }

            if (SV_tag == String.Empty)
            {
                SV_tag = gameObject.name;
            }
        }

        #endregion

        #region Events

        public void OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (IsEnabled)
            {
                if (eventData.CumulativeDelta.x > 0)
                {
                    transform.localScale += (Vector3.one * eventData.CumulativeDelta.x) * (max - min) * Time.deltaTime;
                }
                else
                {
                    transform.localScale -= (Vector3.one * -eventData.CumulativeDelta.x) * (max - min) * Time.deltaTime;
                }

                transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x, min, max);

                // send transform for SV
                SV_Sharing.Instance.SendTransform(transform.localPosition,
                                transform.localRotation,
                                transform.localScale,
                                SV_tag);
            }
        }

        public void OnManipulationStarted(ManipulationEventData eventData) { }

        public void OnManipulationCompleted(ManipulationEventData eventData) { }

        public void OnManipulationCanceled(ManipulationEventData eventData) { }

        #endregion
    }
}
