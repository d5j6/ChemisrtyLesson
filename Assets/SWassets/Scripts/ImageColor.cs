using UnityEngine;
using UnityEngine.UI;

public class ImageColor : MonoBehaviour
{
    #region Public Fields

    public Color ActiveColor;

    public bool Active = false;

    public bool Multiselect = false;

    public ImageColor[] ResetOnActive;

    #endregion

    #region Private Fields

    private Image _image;
    private Color _normalColor;

    #endregion

    #region Private Properties

    private Image Image
    {
        get
        {
            if (!_image)
            {
                _image = GetComponent<Image>();
            }

            return _image;
        }
    }

    #endregion

    #region Main Methods

    private void Awake()
    {
        _normalColor = Image.color;

        if (Active)
        {
            ToggleColor();
        }
    }

    #endregion

    #region Utility Methods

    public void ToggleColor()
    {
        if (Image.color != ActiveColor)
        {
            ResetAllActive();
            
            Image.color = ActiveColor;

            Active = true;
        }
        else
        {
            ResetColor();
        }
    }

    public void ResetColor()
    {
        Image.color = _normalColor;

        Active = false;
    }

    public void ResetAllActive()
    {
        if (ResetOnActive.Length > 0)
        {
            foreach (var obj in ResetOnActive)
            {
                if (obj.Active)
                {
                    obj.ResetColor();
                }
            }
        }

        if (!Multiselect)
        {
            var objects = FindObjectsOfType<ImageColor>();

            foreach (var obj in objects)
            {
                if (obj.Active)
                {
                    obj.ResetColor();
                }
            }
        }
    }

    #endregion
}
