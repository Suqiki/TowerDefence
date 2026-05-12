using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUi : MonoBehaviour
{
    public GameObject UI;
    
    private Node target;

    public Button upgradeButton; 
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellCostText;

    public void SetTarget(Node target)
    {
        this.target = target;

        transform.position = target.transform.position + target.offset;
        
        int level = target.currentUpgradeLevel;

        if (level < target.turretBlueprint.upgrades.Length)
        {
            int cost = target.turretBlueprint.upgrades[level].cost;

            upgradeCostText.text = $"<b>Upgrade</b>\n{cost} G";
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCostText.text = "<b>MAX LEVEL</b>";
            upgradeButton.interactable = false;
        }
        
        int sellValue = target.GetSellValue();

        sellCostText.text = $"<b>Sell</b>\n{sellValue} G";
        
        UI.SetActive(true);
    }

    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }
    
    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
