using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour {
    public Image SelectedImage_p1;
    public Image SelectedImage_p2;

    public void ToggleSelected(bool selected, SelectionController.PlayerID playerID) {
        switch(playerID){
            case SelectionController.PlayerID.P1:
                SelectedImage_p1.gameObject.SetActive(selected);
                break;
            case SelectionController.PlayerID.P2:
                SelectedImage_p2.gameObject.SetActive(selected);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerID), playerID, null);
        }
    }
    
    public void TryPushButton(SelectionController.PlayerID playerID) {
        // TODO
    }
}
