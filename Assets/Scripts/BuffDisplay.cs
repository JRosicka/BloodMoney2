using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour {

    public Image ArtImage;
    public Image BackgroundImage;
    public DisplayBar DurationBar;
    public TextMeshProUGUI DurationText;

    public void Initialize(BuffData buff) {
        ArtImage.sprite = ArtImage.sprite;
        BackgroundImage.sprite = buff.BackgroundSprite;
    }

}
