using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batterfly : MonoBehaviour
{
    private List<Vector3> points = new List<Vector3>();
    private float time, time2 = 0;
    public float Speed = 1;
    private float nextDist = 0;
    private bool detected;
    private bool flyingAway = false;

    public bool Detected
    {
        get
        {
            return detected;
        }
        set
        {
            points.Clear();
            detected = value;
        }
    }
    internal Vector3 pos;



    // Use this for initialization
    void Start() {
        GetComponentInChildren<Animator>().speed = UnityEngine.Random.Range(0.8f, 1.2f);
        LeanTween.delayedCall(Random.Range(0,1), ()=>{
            {
                if (this == null)
                    return;
                GetComponentInChildren<Animator>().SetTrigger("Active");
            }
        });
        points.Add(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        if (!flyingAway)
        {
            if (points.Count < 4 || Vector3.Distance(transform.position, points[points.Count - 1]) < 0.03f)
            {
                Vector3 newV;
                if (detected)
                {
                    newV = pos + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0, 0.3f), Random.Range(-0.3f, 0.3f)) + Camera.main.transform.forward * 0.8f;
                }
                else
                {
                    newV = Camera.main.transform.position + new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-1.5f, 1f), Random.Range(-2.5f, 2.5f));
                }

                points.Add(newV);

                time = 0;
                time2 = 0;
                nextDist = Vector3.Distance(newV, transform.position);
                //LeanTween.rotate(gameObject, Quaternion.LookRotation(points[points.Count - 1] - transform.position, Vector3.up).eulerAngles, 1f/Speed);

                //LeanTween.move(gameObject, points[points.Count - 1], Vector3.Distance(transform.position, points[points.Count - 1])/Speed);
            }

            if (points.Count > 4)
            {
                points.RemoveAt(0);
            }
        }

        time += Time.deltaTime*Speed/(nextDist*20);
        time2 += Time.deltaTime * Speed /4;

        transform.position = Vector3.Lerp(transform.position, points[points.Count - 1], time);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(points[points.Count - 1] - transform.position, Vector3.up), time2);
        transform.rotation = new Quaternion(0,transform.rotation.y,0,transform.rotation.w);
	}

    internal void FlyAway(Vector3 aim)
    {
        if (!flyingAway)
        {
            flyingAway = true;
            time = 0;
            time2 = 0;
            nextDist = Vector3.Distance(aim, transform.position);
            points.Add(aim);

            LeanTween.delayedCall(5, () =>
            {
                LeanTween.scale(gameObject, Vector3.zero, 2).setOnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
        }
    }
}
