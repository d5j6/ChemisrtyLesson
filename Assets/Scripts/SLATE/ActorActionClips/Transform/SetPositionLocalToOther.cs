using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Transform")]
public class SetPositionLocalToOther : ActorActionClip
{
    public Transform other;
    public Vector3 localPosition;

    public override string info
    {
        get
        {
            if(other != null)
            {
                return string.Format("Set Position Local To {0}", other.name);
            }
            return base.info;
        }
    }

    protected override void OnEnter()
    {
        actor.transform.position = other.transform.TransformPoint(localPosition);
    }
}
