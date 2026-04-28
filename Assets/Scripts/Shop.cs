using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI standardCostText;
    public TextMeshProUGUI fireCostText;
    public TextMeshProUGUI iceCostText;


    
    public TurretBlueprint standardTurret;
    public TurretBlueprint fireTurret;
    public TurretBlueprint iceTurret;

    
    
    
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
        
        standardCostText.text = standardTurret.cost.ToString();
        fireCostText.text = fireTurret.cost.ToString();
        iceCostText.text = iceTurret.cost.ToString();
    }
    
    public void SelectStandardTuret()
    {
        Debug.Log("Selected Standard Turet");
        buildManager.SellectTurretToBuild(standardTurret);
    }
    
    public void SelectFireTuret()
    {
        Debug.Log("Selected Fire Turet");
        buildManager.SellectTurretToBuild(fireTurret);
    }
    
    public void SelectIceTuret()
    {
        Debug.Log("Selected Ice Turet");
        buildManager.SellectTurretToBuild(iceTurret);
    }
}
