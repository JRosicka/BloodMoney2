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
    }
    
    private void Update() {
        UpdateHealthDisplay();
        foreach (string currencyID in CurrencyDisplays.Keys) {
            UpdateCurrencyDisplay(currencyID);
        }
    }

    private void UpdateHealthDisplay() {
        HealthValueDisplay.SetValueNumerator(Mathf.CeilToInt(_playerInfo.HealthCurrent));
        HealthValueDisplay.SetValueNumerator(Mathf.CeilToInt(_playerInfo.HealthMax));
        HealthBar.SetPct(_playerInfo.HealthCurrent / _playerInfo.HealthMax);
    }

    private void UpdateCurrencyDisplay(string currencyID) {
        PlayerCurrency currency = _playerInfo.Currencies[currencyID];
        CurrencyDisplays[currencyID].SetValueNumerator(Mathf.FloorToInt(currency.Amount));
        CurrencyDisplays[currencyID].SetValueDenominator(Mathf.FloorToInt(currency.Data.Max));
        CurrencyDisplays[currencyID].SetDelta(Mathf.FloorToInt(currency.Delta));
    }
    

}
