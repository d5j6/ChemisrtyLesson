using UnityEngine;
using HoloToolkit.Sharing;

public class SV_HandlerBankEvents : SpectatorView.SV_Singleton<SV_HandlerBankEvents>
{
    #region Public Fields

    //public ImageColor[] AllButtons;

    public Transform Menu;

    #endregion

    #region Private Fields

    private Transform _target;

    #endregion

    #region Public Properties

    public Transform Target
    {
        get
        {
            return _target;
        }
        private set
        {
            _target = value;
        }
    }

    #endregion

    #region Event Methods

    public void OnTransform(NetworkInMessage msg)
    {
        msg.ReadInt64(); // get user_id but not use

        string tag = SV_Sharing.Instance.ReadString(msg, true).stringValue; // get message_tag

        Target = null;

        switch (tag)
        {
            case "Gas_Station":
                break;
        }

        if (Target != null)
        {
            ApplyTransform(msg);
        }

        Debug.Log("[SV_HandlerBank]: OnTransform");
    }

    public void OnBool(SV_Sharing.SharingData data)
    {
        switch (data.tag)
        {
            // 1
            case "show_gas_station":
                break;
        }

        Debug.Log("[SV_HandlerBank]: OnBool");
    }

    public void OnString()
    {

    }

    #endregion

    #region Utility Methods

    private void ApplyTransform(NetworkInMessage msg)
    {
        Target.localPosition = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
        Target.localRotation = SV_Sharing.Instance.ReadQuaternion(msg, true).quaternionValue;
        Target.localScale = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
    }

    #endregion
}
