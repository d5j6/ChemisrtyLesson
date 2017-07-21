using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, AtomInformation> _atomInformations = new Dictionary<string, AtomInformation>();

    private bool _isInitialized;

    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        string stringData = Resources.Load<TextAsset>("JSON/PeriodicTableFormatted").text;
        JArray jData = JArray.Parse(stringData);

        for(int i = 0; i < jData.Count; i++)
        {
            for(int j = 0; j < JArray.Parse(jData[i]["elements"].ToString()).Count; j++)
            {
                AtomInformation atomInfo = jData[i]["elements"][j].ToObject<AtomInformation>();

                _atomInformations.Add(atomInfo.name, atomInfo);
            }
        }
    }

    public AtomInformation GetAtominfo(string name)
    {
        AtomInformation atomInfo;
        if(_atomInformations.TryGetValue(name, out atomInfo))
        {
            return atomInfo;
        }
        else
        {
            Debug.Log(string.Format("DataManager has no have information about {0} atom", name));
            return null;
        }
    }
}

public class AtomInformation
{
    public string name;
    public string shortname;
    public int position;
    public int index;
    public float molar;
    public int type;
    public string formula;
    public int[] electrons;
}
