using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour {

    private PlayerInfo _playerInfo;
    private GameData _gameData;

    [Header("Health")]
    public NumberValueDisplay HealthValueDisplay;
    public DisplayBar HealthBar;

    [Header("Currencies")]
    public RectTransform CurrencyGroupRT;
    public NumberValueDisplay CurrencyDisplayTemplate;
    private Dictionary<string, NumberValueDisplay> CurrencyDisplays;

    [Header("Buffs")]
    public RectTransform BuffGroupRT;
    public BuffDisplay BuffDisplayTemplate;
    private Dictionary<string, BuffDisplay> BuffDisplays;

    public void Initialize(PlayerInfo playerInfo, GameData gameData) {
        _playerInfo = playerInfo;
        CurrencyDisplays = new Dictionary<string, NumberValueDisplay>();
        
        // Initialize Health Display
        UpdateHealthDisplay();
        
        // Create Currency Displays
        foreach (CurrencyData currency in gameData.Currencies) {
            NumberValueDisplay newCurrencyDisplay = Instantiate(CurrencyDisplayTemplate, CurrencyGroupRT);
            CurrencyDisplays.Add(currency.ID, newCurrencyDisplay);
            newCurrencyDisplay.SetLabel(currency.DisplayName);
            UpdateCurrencyDisplay(currency.ID);
        }
        
        // Deactivate Template Currency
        CurrencyDisplayTemplate.gameObject.SetActive(false);
        
        // Deactivate Template Buff
        BuffDisplayTemplate.gameObject.SetActive(false);
        
        // Watch PlayerInfo Delegates
        _playerInfo.OnBuffCreated += OnBuffCreated;
    }
    
    // UPDATE LOOP
    private void Update() {
        
        // Update Health Display
        UpdateHealthDisplay();
        
        // Update Currency Display
        foreach (string currencyID in CurrencyDisplays.Keys) {
            UpdateCurrencyDisplay(currencyID);
        }
        
        // Update Buff Display
        foreach (string buffID in CurrencyDisplays.Keys) {
            UpdateBuffDisplay(buffID);
        }
        
    }

    private void UpdateHealthDisplay() {
        HealthValueDisplay.SetValueNumerator(Mathf.CeilToInt(_playerInfo.HealthCurrent));
        HealthValueDisplay.SetValueDenominator(Mathf.CeilToInt(_playerInfo.HealthMax));
        HealthBar.SetPct(_playerInfo.HealthCurrent / _playerInfo.HealthMax);
    }

    private void UpdateCurrencyDisplay(string currencyID) {
        PlayerCurrency currency = _playerInfo.Currencies[currencyID];
        CurrencyDisplays[currencyID].SetValueNumerator(Mathf.FloorToInt(currency.Amount));
        CurrencyDisplays[currencyID].SetValueDenominator(Mathf.FloorToInt(currency.Data.Max));
        CurrencyDisplays[currencyID].SetDelta(Mathf.FloorToInt(currency.Delta));
    }

    private void UpdateBuffDisplay(string buffID) {
        BuffDisplay buffDisplay = BuffDisplays[buffID];
        PlayerBuff activeBuff = _playerInfo.GrabExistingBuff(buffID);
        buffDisplay.DurationText.text = Mathf.CeilToInt(activeBuff.TimeLeft()).ToString();
        buffDisplay.DurationBar.SetPct(activeBuff.TimeLeft() / activeBuff.Data.Duration);
    }

    private void OnBuffCreated(PlayerBuff playerBuff) {
        BuffDisplay newBuffDisplay = Instantiate(BuffDisplayTemplate, BuffGroupRT);
        newBuffDisplay.gameObject.SetActive(true);
        newBuffDisplay.Initialize(playerBuff.Data);
        BuffDisplays.Add(playerBuff.Data.ID, newBuffDisplay);
        UpdateBuffDisplay(playerBuff.Data.ID);
    }

    private void OnBuffDestroyed(PlayerBuff playerBuff) {
        if (!BuffDisplays.ContainsKey(playerBuff.Data.ID)) {
            Debug.Log("No displayer found for " + playerBuff.Data.ID);
            return;;
        }
        Destroy(BuffDisplays[playerBuff.Data.ID]);
        BuffDisplays.Remove(playerBuff.Data.ID);
    }
    

}
