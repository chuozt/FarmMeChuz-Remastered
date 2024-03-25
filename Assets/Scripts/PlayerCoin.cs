using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoin : Singleton<PlayerCoin>
{
    private Text playerCoinText;

    void Awake() => playerCoinText = GetComponent<Text>();

    public Text PlayerCoinText{ get{ return playerCoinText; } set{ playerCoinText = value; }}
}
