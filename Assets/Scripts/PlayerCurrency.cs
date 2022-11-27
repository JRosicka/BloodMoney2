using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency {
    
    public CurrencyData Data;
    
    public float Amount;
    public float Delta;
    
    
    public PlayerCurrency(CurrencyData data) {
        Data = data;
    }

    public void Spend(int amount) {
        if (amount > Amount) {
            throw new Exception($"Tried to spend an amount of {Data.ID} currency that we don't have!");
        }

        Amount -= amount;
    }
}
