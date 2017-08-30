using UnityEngine;
using HoloToolkit.Unity;

// WE DON'T NEED THIS

/// <summary>
/// Script to handle the user selecting the avatar to represent themself.
/// </summary>
public class AvatarSelector : MonoBehaviour
{
    /// <summary>
    /// This is the index set by the PlayerAvatarStore for the avatar.
    /// </summary>
    public int AvatarIndex { get; set; }

    /// <summary>
    /// Called when the user is gazing at this avatar and air taps it.
    /// This sends the user's selection to the rest of the devices in the experience.
    /// </summary>
    void OnSelect()
    {
        //PlayerAvatarStore.Instance.DismissAvatarPicker();

        //LocalPlayerManager.Instance.SetUserAvatar(AvatarIndex);
    }
}