using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathCounter : MonoBehaviour {

    public int deathCount = 0;

    public TextMeshProUGUI deathText;
    public TextMeshProUGUI deathTextWin;

    public void IncreaseDeathCount()
    {
        deathCount++;
        deathText.text = deathCount.ToString();
        deathTextWin.text = deathCount.ToString();
    }
}

