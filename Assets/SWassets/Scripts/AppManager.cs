using UnityEngine;
using HoloToolkit.Unity;
using RenderHeads.Media.AVProVideo;

public class AppManager : Singleton<AppManager>
{
    #region Public Fields

    public bool HideModelsOnChange = false;

    public GameObject[] AllModels;

    public MediaPlayer videoPlayer;

    #endregion

    #region Main Methods

    private void Start()
    {
        HideAll(true); // Hide All Models On start
    }

    #endregion

    #region Event Methods
    // 1
    public void GasStation(bool fromSharing = false)
    {
        HideAll();
        ShowModel("Gas_Station");

        if (!fromSharing)
        {
            SV_Sharing.Instance.SendBool(true, "show_gas_station");
        }
    }

    // 2
    public void Video(bool fromSharing = false)
    {
        HideAll();
        ShowModel("Video");

        if (videoPlayer)
        {
            videoPlayer.Play();
        }

        if (!fromSharing)
        {
            SV_Sharing.Instance.SendBool(true, "show_video");
        }
    }

    // 3
    public void Cell(bool fromSharing = false)
    {
        HideAll();
        ShowModel("Cell");

        if (!fromSharing)
        {
            SV_Sharing.Instance.SendBool(true, "show_cell");
        }
    }

    // 4
    public void Mine(bool fromSharing = false)
    {
        HideAll();
        ShowModel("Mine");

        if (!fromSharing)
        {
            SV_Sharing.Instance.SendBool(true, "show_mine");
        }
    }

    // 5
    public void Butterflies(bool fromSharing = false)
    {
        HideAll();
        ShowModel("Batterflies");

        if (!fromSharing)
        {
            SV_Sharing.Instance.SendBool(true, "show_butterflies");
        }
        else
        {
            // if from sharing, enable batterflies
            GetModelByName("Batterflies").GetComponent<Batterflies>().OnInputEnabled();
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Get Model By Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetModelByName(string name)
    {
        foreach (var model in AllModels)
        {
            if (model.name == name)
            {
                return model;
            }
        }

        return null;
    }

    /// <summary>
    /// Show Model In Scene (GameObject)
    /// </summary>
    /// <param name="model"></param>
    public void ShowModel(GameObject model)
    {
        if (model)
        {
            model.SetActive(true);
        }
    }

    /// <summary>
    /// Show Model In Scene (String)
    /// </summary>
    /// <param name="model"></param>
    public void ShowModel(string modelName)
    {
        var model = GetModelByName(modelName);

        if (model)
        {
            model.SetActive(true);
        }
    }

    /// <summary>
    /// Toggle Model In Scene (GameObject)
    /// </summary>
    /// <param name="model"></param>
    public void ToggleModel(GameObject model)
    {
        if (model)
        {
            model.SetActive(!model.activeSelf);
        }
    }

    /// <summary>
    /// Toggle Model In Scene (String)
    /// </summary>
    /// <param name="model"></param>
    public void ToggleModel(string modelName)
    {
        var model = GetModelByName(modelName);

        if (model)
        {
            model.SetActive(!model.activeSelf);
        }
    }

    /// <summary>
    /// Hide All Models In Scene
    /// </summary>
    public void HideAll(bool onStart = false)
    {
        if (HideModelsOnChange || onStart)
        {
            foreach (var model in AllModels)
            {
                model.SetActive(false);
            }
        }
    }

    #endregion

}
