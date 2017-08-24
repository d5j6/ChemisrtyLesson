using UnityEngine;
using System.Collections;

public class OwnCursorManager : Singleton<OwnCursorManager>
{
    public Transform cursor;

    [SerializeField]
    private float cursorIdleDistance = 4f;

    void LateUpdate()
    {
        if(OwnGazeManager.Instance.HitObjectType == OwnGazeManager.HitObjectTypes.None)
        {
            cursor.position = Camera.main.transform.TransformPoint(new Vector3(0f, 0f, cursorIdleDistance));
            cursor.LookAt(Camera.main.transform.position);

            return;
        }

        cursor.position = OwnGazeManager.Instance.HitPoint;
        cursor.rotation = Quaternion.LookRotation(OwnGazeManager.Instance.PointNormal);
    }
}
