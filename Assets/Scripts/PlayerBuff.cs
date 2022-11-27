using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff {

    public BuffData Data;
    public float TimeStarted;

    public PlayerBuff(BuffData data) {
        Data = data;
        TimeStarted = Time.time;
    }

    public void Refresh() {
        TimeStarted = Time.time;
    }

    public float TimeLeft() {
        return Data.Duration - (Time.time - TimeStarted);
    }

}
