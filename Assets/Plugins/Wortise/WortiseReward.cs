using System;
using UnityEngine;

public class WortiseReward
{
    public int    amount;
    public string label;
    public bool   success;


    public WortiseReward(bool success, string label, int amount)
    {
        this.amount  = amount;
        this.label   = label;
        this.success = success;
    }
}
