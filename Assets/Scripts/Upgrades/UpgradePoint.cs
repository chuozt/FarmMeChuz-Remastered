using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePoint : Singleton<UpgradePoint>
{
    [SerializeField] private Text upgradePointText;

    public Text UpgradePointText { get { return upgradePointText; } set { upgradePointText = value; } }
}
