using UnityEngine;
using System.Collections;

public class StartScenario : MonoBehaviour
{
    public GameObject periodicTableTemplatePrefab;

    private GameObject PeriodicTabletemplate;

    public GameObject periodicTable;

    public GameObject projectorTemplatePrefab;

    private GameObject projectorTemplate;

    public GameObject projector;

    public GameObject chaptersMenu;
    
    public static BtnTap[] chaptersBtn;
    void Start()
    {
        PeriodicTabletemplate = Instantiate(periodicTableTemplatePrefab);
        TemplateDrag templateTableScript = PeriodicTabletemplate.GetComponentInChildren<TemplateDrag>();
    
        PlayerManager.Instance.TryToDragInteractive(templateTableScript);
        OwnGestureManager.Instance.onTapEvent += PeriodicTableDropHandler;
    }

    private void PeriodicTableDropHandler(IInteractive interactive)
    {
        OwnGestureManager.Instance.onTapEvent -= PeriodicTableDropHandler;

        periodicTable.transform.parent = null;
        periodicTable.transform.position = PeriodicTabletemplate.transform.position;
        periodicTable.transform.rotation = PeriodicTabletemplate.transform.rotation;

        Destroy(PeriodicTabletemplate);

        projectorTemplate = Instantiate(projectorTemplatePrefab);
        TemplateDrag spawnerTemplateScript = projectorTemplate.GetComponent<TemplateDrag>();
        PlayerManager.Instance.TryToDragInteractive(spawnerTemplateScript);

        OwnGestureManager.Instance.onTapEvent += SpawnerTemplateDropHandler;
    }

    private void SpawnerTemplateDropHandler(IInteractive interactive)
    {
        OwnGestureManager.Instance.onTapEvent -= SpawnerTemplateDropHandler;
        Vector3 chaptersSpawnPos = new Vector3(-1f, 0, 0.1f);
        chaptersSpawnPos = periodicTable.transform.TransformPoint(chaptersSpawnPos);
        Instantiate(chaptersMenu, chaptersSpawnPos, periodicTable.transform.rotation);
        chaptersBtn = GameObject.FindObjectsOfType<BtnTap>();

        projector.transform.parent = null;
        projector.transform.position = projectorTemplate.transform.position;
        projector.transform.rotation = projectorTemplate.transform.rotation;

        Destroy(projectorTemplate);
    }
}
