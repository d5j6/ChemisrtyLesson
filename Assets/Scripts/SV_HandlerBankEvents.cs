using UnityEngine;
using HoloToolkit.Sharing;
using TMPro;

public class SV_HandlerBankEvents : SpectatorView.SV_Singleton<SV_HandlerBankEvents>
{
    #region Public Fields

    //public ImageColor[] AllButtons;

    public Transform Menu;

    #endregion

    #region Private Fields

    private Transform _target;
    private AtomUIStateController atomUIStateController;

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
            case "periodic_table":
                Target = PlayerManager.Instance.PeriodicTable.transform;
                if (Target != null)
                    ApplyTransform(msg);

                var startScenario = GameObject.FindObjectOfType<StartScenario>();
                if (startScenario != null)
                    startScenario.DestroyPeriodicTableTemplate();
                break;

            case "projector":
                Target = PlayerManager.Instance.Projector.transform;
                if (Target != null)
                    ApplyTransform(msg);
                DataManager.Instance.InitializeDictionaries();
                break;
        }


        Debug.Log("[SV_HandlerBank]: OnTransform");
    }

    public void OnBool(SV_Sharing.SharingData data)
    {
        atomUIStateController = PlayerManager.Instance.Projector.GetComponentInChildren<AtomUIStateController>();
        switch (data.tag)
        {
            case "change_view_to_3d":
                atomUIStateController.ChangeLocalViewTo3D(true);
                break;
            case "change_view_to_2d":
                atomUIStateController.ChangeLocalViewTo3D(false);
                break;
        }
        Debug.Log("[SV_HandlerBank]: OnBool");
    }

    public void OnString()
    {

    }

    public void OnInt(SV_Sharing.SharingData data)
    {
        switch (data.tag)
        {            
            case "highlight_element":
                var highlightElement = DataManager.Instance.GetInteractiveObject(data.intValue);
                highlightElement.GetComponent<TableElement>().HighlightElement();
                break;

            case "dehighlight_element":
                var dehighlightElement = DataManager.Instance.GetInteractiveObject(data.intValue);
                dehighlightElement.GetComponent<TableElement>().CanselHighlighting();
                break;

            case "select_element":
                var selectedElement = DataManager.Instance.GetInteractiveObject(data.intValue);
                if (!selectedElement.GetComponent<TableElement>().IsSelected)
                    selectedElement.GetComponent<TableElement>().Select();
                else
                    selectedElement.GetComponent<TableElement>().Deselect();
                break;

            case "highlight_menu_item":
                var chapter_highlight = DataManager.Instance.GetInteractiveObject(data.intValue);
                chapter_highlight.GetComponent<BtnTap>().HighlightMenuItem(true);
                break;

            case "dehighlight_menu_item":
                var chapter_dehighlight = DataManager.Instance.GetInteractiveObject(data.intValue);
                chapter_dehighlight.GetComponent<BtnTap>().DehighlightMenuItem(true);
                break;

            case "run_animation":
                var chapter = DataManager.Instance.GetInteractiveObject(data.intValue);
                chapter.GetComponent<BtnTap>().RunAnumation(true);
                break;

            case "SGB_change_to_demonstration":
                var SGB_ToDemo = DataManager.Instance.SkipGidButton;
                var SGB_ToDemonstration = SGB_ToDemo;
                SGB_ToDemonstration.ChangeStrategyToDemonstration();
                break;

            case "SGB_change_to_standart":
                var SGB_ToStd = DataManager.Instance.SkipGidButton;
                var SGB_ToStandart = SGB_ToStd;
                SGB_ToStandart.ChangeStrategyToStandart();
                break;

            case "SGB_highlight":
                var SGB_highlight = DataManager.Instance.SkipGidButton;
                var SGB_HighLight = SGB_highlight;
                SGB_HighLight.HighLightOnGazeEnter();
                break;

            case "SGB_dehighlight":
                var SGB_dehighlight = DataManager.Instance.SkipGidButton;
                var SGB_deHighLight = SGB_dehighlight;
                SGB_deHighLight.HighLightOnGazeLeave();
                break;
        }
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
