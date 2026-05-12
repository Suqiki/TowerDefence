using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color purchasedColor;
    public Vector3 offset;
    
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
    }
    
    void OnMouseDown()
    {
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
        // mai există upgrade?
        if (currentUpgradeLevel >= turretBlueprint.upgrades.Length)
        {
            Debug.Log("Max upgrade reached!");
            return;
        }

        TurretUpgrade upgradeData =
            turretBlueprint.upgrades[currentUpgradeLevel];

        if (PlayerStats.Gold < upgradeData.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        PlayerStats.Gold -= upgradeData.cost;

        // distrugem tureta veche
        Destroy(turret);

        // spawn upgrade
        GameObject _turret = Instantiate(
            upgradeData.prefab,
            transform.position + offset,
            Quaternion.identity
        );

        turret = _turret;

        // reconnect node
        turet turretScript = turret.GetComponent<turet>();

        if (turretScript != null)
        {
            turretScript.SetNode(this);
        }

        // efect
        GameObject effect = Instantiate(
            buildManager.buildEffect,
            transform.position,
            Quaternion.identity
        );

        Destroy(effect, 5f);

        currentUpgradeLevel++;

        Debug.Log("Turret upgraded! Current level: " + currentUpgradeLevel);
    }

    public void SellTurret()
    {
        if (turret == null)
            return;

        int totalSpent = GetTotalSpent();

        int sellValue = Mathf.RoundToInt(totalSpent * 0.75f);

        GameObject effect = Instantiate(
            BuildManager.instance.sellEffect,
            transform.position,
            Quaternion.identity
        );
        
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
        rend.material.color = purchasedColor;

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
    }
}
