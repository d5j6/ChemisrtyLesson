using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StartCoroutine (Test2());
        Debug.Log(GameObject.FindObjectOfType<HoloToolkit.Unity.KeywordManager>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Test2()
	{
		yield return new WaitForSeconds (1f);
		transform.position = Vector3.zero;
		yield return new WaitForSeconds (1f);
		transform.position = Vector3.one;
	}
}
