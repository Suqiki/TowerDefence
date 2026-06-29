using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;


public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color purchasedColor;
    public Vector3 offset;
    private bool isHovering = false;
    
    [Header("Sound Effects")]
    public AudioClip nodeBuySoundFX;
    public float nodeBuySoundFXVolume;
    public AudioClip buildSoundFX;
    public float buildSoundFXVolume;
    public AudioClip[] upgradeSoundFX;
    
    public float upgradeSoundFXVolume;
    public AudioClip sellSoundFX;
    public float sellSoundFXVolume;

    [Header("Highlight Animation")]
    public Color highlightAnimColor; // Culoarea spre care va pulsa nodul
    public float animSpeed = 2f;     // Viteza cu care pulsează culoarea
    private Coroutine highlightCoroutine; // Referință ca să o putem opri
    
    [Header("For tutorial")]
    public bool tutorialMode = false;
    public bool tutorialCanBuild = false;
    
    [Header("Optional")]
    public GameObject turret;
    public bool isPurchased =  false;
    public bool turretBuild = false;
    
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgrade = false;
    [HideInInspector]
    public int currentUpgradeLevel = 0;


    
    private Renderer rend;
    private Color startColor;
    private NodeManager nodeManager;
    
    BuildManager buildManager;
    
    void Start()
    {
        nodeManager = NodeManager.instance;
        buildManager = BuildManager.instance;
        
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
        // Dacă node-ul e deja cumpărat
        if (isPurchased)
        {
            rend.material.color = purchasedColor;

            // Dacă are deja turret setat
            if (turretBuild && turret != null)
            {
                Vector3 position = transform.position + offset;
                turret = Instantiate(turret, position, Quaternion.identity);
            }
        }
    }
    private void OnMouseEnter()
    {
        //Debug.Log("Mouse over!");
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        rend.material.color = hoverColor;
        isHovering = true;
    }
    
    void OnMouseDown()
    {
        if (tutorialMode)
        {
            if (!tutorialCanBuild)
                return;
        }
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (!isPurchased)
        {
            PurchaseNode();
        }
        else if (turret != null)
        {
            buildManager.SelectNode(this);
        }
        else
        {
            BuildTurret(buildManager.GetTurretToBuild());
        }
    }

    private void BuildTurret(TurretBlueprint blueprint)
    {
        if (!buildManager.CanBuild)
            return;
        if (!buildManager.HasMoney)
        {
            WarningUI.instance.ShowWarning("Not enough gold!");
            return;
        }
        if (!isPurchased || turretBuild)
        {
            Debug.Log("Can't build turret");
            return;
        }
        if (PlayerStats.Gold < blueprint.cost)
            return;
        PlayerStats.Gold -= blueprint.cost;
        turretBuild = true;
        SpawnTurret(blueprint);
        StartCoroutine(
            PlayerProgressManager.instance.UpdateTurretStats(
                blueprint.prefab.name,
                1,
                0,
                0,
                0
            )
        );
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.TurretBought(
                blueprint.prefab.name
            );
        }
        Debug.Log("Turret build! Money left: " + PlayerStats.Gold);
    }
    
    
    private void SpawnTurret(TurretBlueprint blueprint)
    {
        GameObject _turret = Instantiate(
            blueprint.prefab,
            transform.position + offset,
            Quaternion.identity
        );
        turret = _turret;
        
        SoundEffectsManager.instance.PlaySoundEffect(buildSoundFX, transform, buildSoundFXVolume);

        turretBlueprint = blueprint;
        
        turet turretScript = turret.GetComponent<turet>();
        if (turretScript != null)
        {
            turretScript.SetNode(this);
        }

        GameObject effect = Instantiate(
            buildManager.buildEffect,
            transform.position,
            Quaternion.identity
        );

        Destroy(effect, 5f);
    }

    public void UpgradeTurret()
    {
        if (tutorialMode) return;
        // mai există upgrade?
        if (currentUpgradeLevel >= turretBlueprint.upgrades.Length)
        {
            Debug.Log("Max upgrade reached!");
            return;
        }

        TurretUpgrade upgradeData =  turretBlueprint.upgrades[currentUpgradeLevel];

        if (PlayerStats.Gold < upgradeData.cost)
        {
            //Debug.Log("Not enough gold!");
            WarningUI.instance.ShowWarning("Not enough gold!");
            return;
        }

        PlayerStats.Gold -= upgradeData.cost;
        // distrugem tureta veche
        Destroy(turret);
        SoundEffectsManager.instance.PlayrandomSoundEffect(upgradeSoundFX, transform, upgradeSoundFXVolume);
        // spawn upgrade
        GameObject _turret = Instantiate(upgradeData.prefab, transform.position + offset, Quaternion.identity);
        turret = _turret;
        // reconnect node
        turet turretScript = turret.GetComponent<turet>();
        
        if (turretScript != null)
        {
            turretScript.SetNode(this);
        }

        // efect
        GameObject effect = Instantiate(buildManager.buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        currentUpgradeLevel++;
        int up1 = 0;
        int up2 = 0;
        int up3 = 0;
        if (currentUpgradeLevel == 1)
            up1 = 1;
        else if (currentUpgradeLevel == 2)
            up2 = 1;
        else if (currentUpgradeLevel == 3)
            up3 = 1;

        StartCoroutine(
            PlayerProgressManager.instance.UpdateTurretStats(
                turretBlueprint.prefab.name,
                0,
                up1,
                up2,
                up3
            )
        );
        
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.TurretUpgraded(
                turret.name,
                currentUpgradeLevel
            );
        }

        Debug.Log("Turret upgraded! Current level: " + currentUpgradeLevel);
    }

    public void SellTurret()
    {
        if (tutorialMode) return;
        
        if (turret == null)
            return;

        int totalSpent = GetTotalSpent();

        int sellValue = Mathf.RoundToInt(totalSpent * 0.75f);

        GameObject effect = Instantiate(
            BuildManager.instance.sellEffect,
            transform.position,
            Quaternion.identity
        );
        
        SoundEffectsManager.instance.PlaySoundEffect(sellSoundFX, transform, sellSoundFXVolume);
        
        PlayerStats.Gold += sellValue;

        Debug.Log("Sold turret for: " + sellValue);

        Destroy(turret);
        Destroy(effect, 5f);
        turret = null;

        turretBuild = false;
        isUpgrade = false;
        currentUpgradeLevel = 0;
        turretBlueprint = null;
    }
    
    private int GetTotalSpent()
    {
        int total = 0;

        if (turretBlueprint != null)
        {
            total += turretBlueprint.cost;

            for (int i = 0; i < currentUpgradeLevel; i++)
            {
                if (i < turretBlueprint.upgrades.Length)
                {
                    total += turretBlueprint.upgrades[i].cost;
                }
            }
        }

        return total;
    }
    
    public int GetSellValue()
    {
        int total = GetTotalSpent();
        return Mathf.RoundToInt(total * 0.75f);
    }

    private void PurchaseNode()
    {
        int cost = nodeManager.GetCurrentCost();

        if (PlayerStats.Gold < cost)
        {
            //Debug.Log("Not enough money");
            WarningUI.instance.ShowWarning("Not enough gold!");
            return;
        }

        PlayerStats.Gold -= cost;

        isPurchased = true;
        
        StopHighlightAnimation();
        rend.material.color = purchasedColor;

        SoundEffectsManager.instance.PlaySoundEffect(nodeBuySoundFX, transform, nodeBuySoundFXVolume);
        
        nodeManager.RegisterPurchase();
    }

    
    void OnMouseExit()
    {
        if (!isPurchased)
        {
            rend.material.color = startColor;
        }
        else
        {
            rend.material.color = purchasedColor;
        }
        isHovering=false;
    }
    
    public void StartHighlightAnimation()
    {
        if (isPurchased) return; // Nu animăm dacă e deja cumpărat

        // Ne asigurăm că nu pornim două corutine în același timp pe același nod
        if (highlightCoroutine == null)
        {
            highlightCoroutine = StartCoroutine(AnimateHighlightColor());
        }
    }

    public void StopHighlightAnimation()
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            highlightCoroutine = null;
        }
    
        // Resetăm culoarea la cea normală (sau purchased dacă s-a cumpărat între timp)
        rend.material.color = isPurchased ? purchasedColor : startColor;
    }
    
    private System.Collections.IEnumerator AnimateHighlightColor()
    {
        float progress = 0f;
        int direction = 1; // 1 înseamnă că merge spre Highlight, -1 înseamnă că revine spre Bază

        while (!isPurchased)
        {
            // Dacă jucătorul ține mouse-ul pe nod, înghețăm animația
            if (isHovering)
            {
                yield return null;
                continue;
            }

            // Adăugăm sau scădem progresul în funcție de direcție
            progress += Time.deltaTime * animSpeed * direction;

            // Schimbăm culoarea treptat între cele două puncte (0f = Bază, 1f = Highlight)
            rend.material.color = Color.Lerp(startColor, highlightAnimColor, progress);

            // Dacă a ajuns la Highlight (1f), întoarcem direcția înapoi spre bază
            if (progress >= 1f)
            {
                progress = 1f;
                direction = -1;
            }
            // Dacă a revenit la Bază (0f), întoarcem direcția înainte spre highlight
            else if (progress <= 0f)
            {
                progress = 0f;
                direction = 1;
            }

            yield return null; 
        }
    }
}
