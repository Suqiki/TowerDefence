using System;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseUI();
    }

    public void CloseUI()
    {
        this.gameObject.SetActive(false);
    }
    
}
