using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    
    public GameObject StandardTuretPrefab;
    public GameObject FireTuretPrefab;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("BuildManager: more then one building managers in scene");
        }
        instance = this;
    }
    
    
    private TurretBlueprint turretToBuild;
    
    public bool CanBuild {get {return turretToBuild != null;}}

    public void SellectTurretToBuild(TurretBlueprint turretBlueprint)
    {
        turretToBuild = turretBlueprint;    
    }

    
    public void BuildTurretOn(Node node)
    {
        if (PlayerStats.Gold < turretToBuild.cost)
        {
            Debug.Log("BuildTurretOn: Not enough gold");
            return;
        }
        PlayerStats.Gold -= turretToBuild.cost;
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.transform.position + node.offset, Quaternion.identity );
        node.turret = turret;
        
        Debug.Log("Turret build! Money left: " + PlayerStats.Gold);
    }
}
