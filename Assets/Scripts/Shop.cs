using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint fireTurret;
    
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
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
}
