using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour {
    public AbstractAbility Ability;
    
    public Image SelectedImage_p1;
    public Image SelectedImage_p2;

    public void ToggleSelected(bool selected, PlayerManager.PlayerID playerID) {
        switch(playerID){
            case PlayerManager.PlayerID.P1:
                SelectedImage_p1.gameObject.SetActive(selected);
                break;
            case PlayerManager.PlayerID.P2:
                SelectedImage_p2.gameObject.SetActive(selected);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerID), playerID, null);
        }
    }
    
    public void TryPushButton(PlayerManager.PlayerID playerID) {
        if (GameManager.Instance.PlayerManager.GetPlayerInfo(playerID).CanUseAbility(Ability)) {
            // TODO
        } else {
            // TODO
        }
    }
}