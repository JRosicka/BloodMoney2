using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour {
    public Image SelectedImage;

    public void Select() {
        SelectedImage.gameObject.SetActive(true);
    }

    public void Deselect() {
        SelectedImage.gameObject.SetActive(false);
    }
    
    public void PushButton() {
        // TODO
    }
}
