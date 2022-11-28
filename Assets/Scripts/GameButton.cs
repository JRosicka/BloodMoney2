using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour {
    public AbilityData AbilityData;

    [Header("References")] 
    public Image CurrencyIcon;
    public Image AbilityIcon;
    public GameObject SelectedImage_p1;
    public GameObject SelectedImage_p2;
    public RectTransform CooldownImage_p1;
    public RectTransform CooldownImage_p2;
    public TextMeshProUGUI CooldownText_p1;
    public TextMeshProUGUI CooldownText_p2;
    public TextMeshProUGUI Cost_p1;
    public TextMeshProUGUI Cost_p2;
    public TextMeshProUGUI AbilityText;

    private PlayerInfo PlayerInfo(PlayerManager.PlayerID id) => GameManager.Instance.PlayerManager.GetPlayerInfo(id);
    
    public void ToggleSelected(bool selected, PlayerManager.PlayerID playerID) {
        
        if (selected) UIObjectFX.DoEffect("Button Selected", gameObject);
        
        switch(playerID){
            case PlayerManager.PlayerID.P1:
                SelectedImage_p1.SetActive(selected);
                break;
            case PlayerManager.PlayerID.P2:
                SelectedImage_p2.SetActive(selected);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerID), playerID, null);
        }
    }
    
    public void TryPushButton(PlayerManager.PlayerID playerID) {
        if (PlayerInfo(playerID).TryUseAbility(AbilityData)) {
            UpdateCostText(playerID);
            
            // TODO animate
            StartCooldownAnimation(playerID);
        } else {
            // TODO animate
            UIObjectFX.DoEffect("Purchase Failure", gameObject);
        }
    }

    private void Start() {
        CooldownText_p1.alpha = 0;
        CooldownText_p2.alpha = 0;

        if (GameManager.Instance.PlayerManager.PlayersCreated) {
            SetInitialInfo();
        } else {
            // We must not have created the players yet, but are just about to. Wait for that to happen. 
            GameManager.Instance.PlayerManager.OnPlayersCreated += SetInitialInfo;
        }
    }

    private void Update() {
        UpdateCooldownAnimation(ref _cooldownAnimation_p1);
        UpdateCooldownAnimation(ref _cooldownAnimation_p2);
    }

    private void SetInitialInfo() {
        // Set initial costs
        UpdateCostText(PlayerManager.PlayerID.P1);
        UpdateCostText(PlayerManager.PlayerID.P2);
        
        if (AbilityData == null) return;

        // Set currency icon
        string currencyID = AbilityData.Currency.ID;
        CurrencyData currencyData = GameManager.Instance.PlayerManager.GameData.Currencies.First(c => c.ID == currencyID);
        CurrencyIcon.sprite = currencyData.Sprite;
        AbilityIcon.sprite = AbilityData.AbilityIcon;
        
        // Ability text
        AbilityText.text = AbilityData.ID;
    }
    
    private void UpdateCostText(PlayerManager.PlayerID playerID) {
        if (AbilityData == null) return;
        
        TextMeshProUGUI text = playerID == PlayerManager.PlayerID.P1 ? Cost_p1 : Cost_p2;
        text.text = PlayerInfo(playerID).GetAbility(AbilityData)
            .CostToUse().ToString("N0");
    }

    private class CooldownAnimation {
        public PlayerManager.PlayerID ID;
        public RectTransform RT;
        public float StartTime;
        public float TargetEndTime;
        public float CurrentTime;
        public float OriginalYSizeDelta;
        public TextMeshProUGUI Text;
    }

    private CooldownAnimation _cooldownAnimation_p1;
    private CooldownAnimation _cooldownAnimation_p2;
    private void StartCooldownAnimation(PlayerManager.PlayerID playerID) {
        CooldownAnimation cdAnimation = new CooldownAnimation();
        cdAnimation.RT = playerID == PlayerManager.PlayerID.P1 ? CooldownImage_p1 : CooldownImage_p2;
        cdAnimation.OriginalYSizeDelta = cdAnimation.RT.sizeDelta.y;
        AdjustImageAlpha(cdAnimation.RT.GetComponent<Image>(), 0.63f);

        cdAnimation.StartTime = cdAnimation.CurrentTime = Time.time;
        cdAnimation.TargetEndTime = Time.time 
            + AbilityData.CooldownTime;

        if (playerID == PlayerManager.PlayerID.P1) {
            _cooldownAnimation_p1 = cdAnimation;
        } else {
            _cooldownAnimation_p2 = cdAnimation;
        }
        
        // Show cooldown text
        cdAnimation.Text = playerID == PlayerManager.PlayerID.P1 ? CooldownText_p1 : CooldownText_p2;
        cdAnimation.Text.alpha = 1;
        cdAnimation.Text.text = $"{cdAnimation.TargetEndTime - cdAnimation.StartTime}s";
    }

    private void UpdateCooldownAnimation(ref CooldownAnimation cdAnimation) {
        if (cdAnimation == null) return;
        cdAnimation.CurrentTime += Time.deltaTime;
        cdAnimation.Text.text = $"{(int)(cdAnimation.TargetEndTime - cdAnimation.CurrentTime)}<size=0.7em>s</size>";

        if (cdAnimation.CurrentTime >= cdAnimation.TargetEndTime) {
            // End of cooldown
            cdAnimation.RT.sizeDelta = new Vector2(cdAnimation.RT.sizeDelta.x, cdAnimation.OriginalYSizeDelta);
            AdjustImageAlpha(cdAnimation.RT.GetComponent<Image>(), 0);
            cdAnimation.Text.alpha = 0;
            cdAnimation = null;
            return;
        }

        float progress = (cdAnimation.CurrentTime - cdAnimation.StartTime)
                         / (cdAnimation.TargetEndTime - cdAnimation.StartTime);
        cdAnimation.RT.sizeDelta = new Vector2(cdAnimation.RT.sizeDelta.x, Mathf.Lerp(1, 0, progress) * cdAnimation.OriginalYSizeDelta);
    }

    private void AdjustImageAlpha(Image image, float newAlpha) {
        Color temp = image.color;
        temp.a = newAlpha;
        image.color = temp;
    }
}