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
        else
        {
            BuildTurret();
        }
    }

    private void BuildTurret()
    {
        if(!buildManager.CanBuild)
            return;

        if (!buildManager.HasMoney)
        {
            WarningUI.instance.ShowWarning("Not enough gold!");
            return;
        }
        
        if (turretBuild == false && isPurchased == true)
        {
            turretBuild = true;
            //Debug.Log("Building Turret");
            buildManager.BuildTurretOn(this);
        }
        else
        {
            Debug.Log("Can't Building Turret");
            return;
        }
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
