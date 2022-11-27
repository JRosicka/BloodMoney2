using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour {
    public string AbilityID;
    
    [Header("References")]
    public Image SelectedImage_p1;
    public Image SelectedImage_p2;
    public RectTransform CooldownImage_p1;
    public RectTransform CooldownImage_p2;
    
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
        if (GameManager.Instance.PlayerManager.GetPlayerInfo(playerID).TryUseAbility(AbilityID)) {
            // TODO animate
            StartCooldownAnimation(playerID);
        } else {
            // TODO animate
        }
    }

    private void Update() {
        UpdateCooldownAnimation(ref _cooldownAnimation_p1);
        UpdateCooldownAnimation(ref _cooldownAnimation_p2);
    }

    private class CooldownAnimation {
        public PlayerManager.PlayerID ID;
        public RectTransform RT;
        public float StartTime;
        public float TargetEndTime;
        public float CurrentTime;
        public float OriginalYSizeDelta;
    }

    private CooldownAnimation _cooldownAnimation_p1;
    private CooldownAnimation _cooldownAnimation_p2;
    private void StartCooldownAnimation(PlayerManager.PlayerID playerID) {
        CooldownAnimation cdAnimation = new CooldownAnimation();
        cdAnimation.RT = playerID == PlayerManager.PlayerID.P1 ? CooldownImage_p1 : CooldownImage_p2;
        Vector2 sizeDelta = cdAnimation.RT.sizeDelta;
        cdAnimation.OriginalYSizeDelta = sizeDelta.y;
        sizeDelta = new Vector2(sizeDelta.x, 0);
        cdAnimation.RT.sizeDelta = sizeDelta;
        cdAnimation.RT.gameObject.SetActive(true);

        cdAnimation.StartTime = cdAnimation.CurrentTime = Time.time;
        cdAnimation.TargetEndTime = Time.time 
            + GameManager.Instance.PlayerManager.GetPlayerInfo(playerID).GetAbility(AbilityID).Data.CooldownTime;

        if (playerID == PlayerManager.PlayerID.P1) {
            _cooldownAnimation_p1 = cdAnimation;
        } else {
            _cooldownAnimation_p2 = cdAnimation;
        }
    }

    private void UpdateCooldownAnimation(ref CooldownAnimation cdAnimation) {
        if (cdAnimation == null) return;
        cdAnimation.CurrentTime += Time.deltaTime;

        if (cdAnimation.CurrentTime >= cdAnimation.TargetEndTime) {
            // End of cooldown
            cdAnimation.RT.sizeDelta = new Vector2(cdAnimation.RT.sizeDelta.x, cdAnimation.OriginalYSizeDelta);
            cdAnimation.RT.gameObject.SetActive(false);
            cdAnimation = null;
            return;
        }

        float progress = (cdAnimation.CurrentTime - cdAnimation.StartTime)
                         / (cdAnimation.TargetEndTime - cdAnimation.StartTime);
        cdAnimation.RT.sizeDelta = new Vector2(cdAnimation.RT.sizeDelta.x, Mathf.Lerp(0, 1, progress) * cdAnimation.OriginalYSizeDelta);
    }
}
