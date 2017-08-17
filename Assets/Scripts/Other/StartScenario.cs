using UnityEngine;
using System.Collections;
using System;

public class StartScenario : MonoBehaviour
{
    public GameObject periodicTableTemplatePrefab;
    private GameObject PeriodicTabletemplate;

    public GameObject periodicTable;

    //public GameObject projectorTemplatePrefab;
    //private GameObject projectorTemplate;

    public GameObject projector;

    public GameObject chaptersMenu;

    public static BtnTap[] chaptersBtn;


    void Start()
    {
        PeriodicTabletemplate = Instantiate(periodicTableTemplatePrefab);
        TemplateDrag templateTableScript = PeriodicTabletemplate.GetComponentInChildren<TemplateDrag>();  
        PlayerManager.Instance.TryToDragInteractive(templateTableScript);
        OwnGestureManager.Instance.OnTapEvent += PeriodicTableDropHandler;
    }


    public void DestroyPeriodicTableTemplate()
    {
        OwnGestureManager.Instance.OnTapEvent -= PeriodicTableDropHandler;
        Destroy(PeriodicTabletemplate);
        Vector3 chaptersSpawnPos = new Vector3(-1.0f, 0.0f, 0.1f);
        chaptersSpawnPos = periodicTable.transform.TransformPoint(chaptersSpawnPos);
        Instantiate(chaptersMenu, chaptersSpawnPos, periodicTable.transform.rotation);
        chaptersBtn = GameObject.FindObjectsOfType<BtnTap>();
    }

    private void PeriodicTableDropHandler(IInteractive interactive)
    {
        OwnGestureManager.Instance.OnTapEvent -= PeriodicTableDropHandler;

        periodicTable.transform.parent = null;
        periodicTable.transform.position = PeriodicTabletemplate.transform.position;
        periodicTable.transform.rotation = PeriodicTabletemplate.transform.rotation;

        Destroy(PeriodicTabletemplate);

        SV_Sharing.Instance.SendTransform(
            periodicTable.transform.position,
            periodicTable.transform.rotation,
            periodicTable.transform.localScale,
            "periodic_table");

        /*
        projectorTemplate = Instantiate(projectorTemplatePrefab);
        TemplateDrag spawnerTemplateScript = projectorTemplate.GetComponent<TemplateDrag>();
        PlayerManager.Instance.TryToDragInteractive(spawnerTemplateScript);

        OwnGestureManager.Instance.OnTapEvent += SpawnerTemplateDropHandler;
        */

        Vector3 chaptersSpawnPos = new Vector3(-1f, -0.0f, 0.1f);
        chaptersSpawnPos = periodicTable.transform.TransformPoint(chaptersSpawnPos);
        Instantiate(chaptersMenu, chaptersSpawnPos, periodicTable.transform.rotation);
        chaptersBtn = GameObject.FindObjectsOfType<BtnTap>();


        projector.transform.parent = null;
        projector.transform.position = periodicTable.transform.position + new Vector3(-1.2f, -0.4f, -1.6f);
        projector.transform.rotation = periodicTable.transform.rotation;
            

        SV_Sharing.Instance.SendTransform(
            projector.transform.position,
            projector.transform.rotation,
            projector.transform.localScale,
            "projector");

        DataManager.Instance.InitializeDictionaries();
    }

    /*
    private void SpawnerTemplateDropHandler(IInteractive interactive)
    {
        OwnGestureManager.Instance.OnTapEvent -= SpawnerTemplateDropHandler;
        Vector3 chaptersSpawnPos = new Vector3(-1f, 0, 0.1f);
        chaptersSpawnPos = periodicTable.transform.TransformPoint(chaptersSpawnPos);
        Instantiate(chaptersMenu, chaptersSpawnPos, periodicTable.transform.rotation);
        chaptersBtn = GameObject.FindObjectsOfType<BtnTap>();

        projector.transform.parent = null;
        projector.transform.position = projectorTemplate.transform.position;
        projector.transform.rotation = projectorTemplate.transform.rotation;

        Destroy(projectorTemplate);

        SV_Sharing.Instance.SendTransform(
            projector.transform.position,
            projector.transform.rotation,
            projector.transform.localScale,
            "projector");

        DataManager.Instance.InitializeDictionaries();
    }
    */
}
