using SpectatorView;
using UnityEngine;

public class SV_RemoteControl : SV_Singleton<SV_RemoteControl>
{
    #region Public Fields

    [Tooltip("Postions offset")]
    public Vector3 SV_Camera_Position;

    [Tooltip("Rotation offset")]
    public Vector3 SV_Camera_Rotation;

    #endregion
}
