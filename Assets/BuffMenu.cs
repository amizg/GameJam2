using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMenu : MonoBehaviour
{

    public GameObject damageUI;
    public GameObject jumpUI;
    public GameObject speedUI;
    public GameObject healthUI;
    public GameObject healthBar;

    
    public void DamageBuff()
    {
        healthBar.SetActive(false);
        damageUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void JumpBuff()
    {
        healthBar.SetActive(false);
        jumpUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void SpeedBuff()
    {
        healthBar.SetActive(false);
        speedUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void HealthBuff()
    {
        healthBar.SetActive(false);
        healthUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        healthBar.SetActive(true);
        damageUI.SetActive(false);
        speedUI.SetActive(false);
        jumpUI.SetActive(false);
        healthUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
