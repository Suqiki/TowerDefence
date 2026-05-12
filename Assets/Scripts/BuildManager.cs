using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    
    public GameObject StandardTuretPrefab;
    public GameObject FireTuretPrefab;
    public GameObject IceTuretPrefab;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("BuildManager: more then one building managers in scene");
        }
        instance = this;
    }

    public GameObject buildEffect;
    public GameObject sellEffect;
    private TurretBlueprint turretToBuild;
    private Node SelectedNode;
    
    public bool CanBuild {get {return turretToBuild != null;}}
    public bool HasMoney {get {return PlayerStats.Gold >= turretToBuild.cost;}}

    public NodeUi nodeUI;


    public void SellectTurretToBuild(TurretBlueprint turretBlueprint)
    {
        turretToBuild = turretBlueprint;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SelectNode(Node node)
    {
        if (SelectedNode == node)
        {
            DeselectNode();
            return;
        }
        
        SelectedNode = node;
        turretToBuild = null;
        
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        SelectedNode = null;
        nodeUI.Hide();
    }
    
    public void BuildTurretOn(Node node)
    {
        if (PlayerStats.Gold < turretToBuild.cost)
        {
            //Debug.Log("BuildTurretOn: Not enough gold");
            return;
        }
        PlayerStats.Gold -= turretToBuild.cost;
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.transform.position + node.offset, Quaternion.identity );
        node.turret = turret;
        
        turet turretScript = turret.GetComponent<turet>();
        turretScript.SetNode(node);
        
        GameObject effect = (GameObject)Instantiate(buildEffect, node.transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        
        Debug.Log("Turret build! Money left: " + PlayerStats.Gold);
    }
}
