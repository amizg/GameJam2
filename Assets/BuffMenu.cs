using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMenu : MonoBehaviour
{

    public GameObject damageUI;
    public GameObject jumpUI;
    public GameObject speedUI;
    public GameObject healthUI;

    
    public void DamageBuff()
    {
        damageUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void JumpBuff()
    {
        jumpUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void SpeedBuff()
    {
        speedUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void HealthBuff()
    {
        healthUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        damageUI.SetActive(false);
        speedUI.SetActive(false);
        jumpUI.SetActive(false);
        healthUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
