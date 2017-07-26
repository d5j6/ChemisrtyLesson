using UnityEngine;
using HoloToolkit.Sharing;
using TMPro;

public class SV_HandlerBankEvents : SpectatorView.SV_Singleton<SV_HandlerBankEvents>
{
    #region Public Fields

    public ImageColor[] AllButtons;

    public PeriodicTable Table;

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
            case "Periodiodic table":
                Target = AppManager.Instance.GetModelByName("Periodiodic table").transform;
                break;

            case "Projector":
                Target = AppManager.Instance.GetModelByName("Projector").transform;
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

                AppManager.Instance.GasStation(true);
                ToggleButtonColor("Button1");

                break;

            // 2
            case "show_video":

                AppManager.Instance.Video(true);
                ToggleButtonColor("Button2");

                break;

            // 3
            case "show_cell":

                AppManager.Instance.Cell(true);
                ToggleButtonColor("Button3");

                break;

            // 4
            case "show_mine":

                AppManager.Instance.Mine(true);
                ToggleButtonColor("Button4");

                break;

            // 5
            case "show_butterflies":

                AppManager.Instance.Butterflies(true);
                ToggleButtonColor("Button5");

                break;

            case "terminate_app":

                Application.Quit();

                break;
        }

        Debug.Log("[SV_HandlerBank]: OnBool");
    }

    public void OnString()
    {

    }

    #endregion

    #region Utility Methods

    private void ToggleButtonColor(string name)
    {
        var button = GetButtonByName(name);

        if (button)
        {
            button.ToggleColor();
        }
    }

    //HSE
    private void HighlightingSelectedElement(string name)
    {
        TextMeshPro _elementText = Table.SelectedElement.gameObject.GetComponent<TextMeshPro>();
        _elementText.color = Color.gray;
    }

    private ImageColor GetButtonByName(string name)
    {
        foreach (var button in AllButtons)
        {
            if (button.gameObject.name == name)
            {
                return button;
            }
        }

        return null;
    }

    private void ApplyTransform(NetworkInMessage msg)
    {
        Target.localPosition = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
        Target.localRotation = SV_Sharing.Instance.ReadQuaternion(msg, true).quaternionValue;
        Target.localScale = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
    }

    #endregion
}
