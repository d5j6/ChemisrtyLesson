using System.Collections.Generic;
using UnityEngine;
using System;

public class Batterflies : MonoBehaviour
{
    #region Public Fields

    public GameObject baterflyPrefab;

    [Range(0, 100)]
    public int count;

    public float Speed = 0.1f;

    #endregion

    #region Private Fields

    private List<Batterfly> _baterflies = new List<Batterfly>();

    private bool _enable = false;

    private bool _handDetected = false;

    #endregion

    #region Public Properties

    public bool Enable
    {
        get
        {
            return _enable;
        }
        set
        {
            if (_enable!=value)
            {

                _enable = value;

                if (_enable)
                {
                    CreateBatterflies();
                }
                else
                {
                    DestroyBatterflies();
                }
            }
        }
    }

    #endregion

    #region Main Methods

    private void Start()
    {
        transform.position = Camera.main.transform.position;

        // just for test
        Enable = true;
    }

    private void Update()
    {
        //Enable Test
        if (Input.GetKeyUp(KeyCode.B))
        {
            Enable = !Enable;
        }

        OnInputUpdate();
    }

    #endregion

    #region Utility Methods

    private void CreateBatterflies()
    {
        count = 60;

        Speed = 0.1f;

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(baterflyPrefab);
            g.transform.localScale = Vector3.zero;

            LeanTween.scale(g, Vector3.one * UnityEngine.Random.Range(0.3f, 0.75f), 3);

            g.transform.SetParent(transform);
            g.transform.position = GetPointOnSphere(Camera.main.transform.position, 5);
            g.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = (Texture)Resources.Load("BaterfliesTextures/butterfly" + UnityEngine.Random.Range(1, 5), typeof(Texture));
            _baterflies.Add(g.GetComponent<Batterfly>());
            g.GetComponent<Batterfly>().Speed = Speed;
        }
    }

    public void DestroyBatterflies()
    {
        if (_baterflies != null)
        {
            foreach (Batterfly b in _baterflies)
            {
                if (b == null)
                    continue;
                b.FlyAway(GetPointOnSphere(Camera.main.transform.position, 5));
            }

            _baterflies.Clear();
        }
    }

    public void OnInputEnabled()
    {
        Debug.Log("Hand Detected");

        _handDetected = true;

        Vector3 pos = Camera.main.transform.position;

        if (_baterflies != null)
        {
            foreach (Batterfly b in _baterflies)
            {
                if (b == null)
                    continue;

                if (Vector3.Distance(b.transform.position, pos) < 1.5f && UnityEngine.Random.Range(0, 1f) < 0.5f)
                {
                    b.Detected = true;
                }
            }
        }
    }

    public void OnInputDisabled()
    {
        Debug.Log("Hand Lost");

        _handDetected = false;

        foreach (Batterfly b in _baterflies)
        {
            b.Detected = false;
        };
    }

    private void OnInputUpdate()
    {
        Vector3 pos = Camera.main.transform.position;

        if (_handDetected 
            && _baterflies != null)
        {
            foreach (Batterfly b in _baterflies)
            {
                if (b == null)
                    continue;
                b.pos = pos;
            }
        }
    }

    private Vector3 GetPointOnSphere(Vector3 position, float r)
    {
        var u = UnityEngine.Random.value;
        var v = UnityEngine.Random.value;
        var theta = 2 * Math.PI * u;
        var phi = Mathf.Acos(2 * v - 1);
        float x = (float)(position.x + (r * Mathf.Sin(phi) * Math.Cos(theta)));
        float y = (float)(position.y + (r * Mathf.Sin(phi) * Math.Sin(theta)));
        float z = (float)(position.z + (r * Mathf.Cos(phi)));

        return new Vector3(x, y, z);
    }

    #endregion
}
