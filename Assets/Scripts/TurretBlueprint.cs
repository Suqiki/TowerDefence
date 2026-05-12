using UnityEngine;

[System.Serializable]
public class TurretUpgrade
{
    public GameObject prefab;
    public int cost;
}

[System.Serializable]
public class TurretBlueprint
{
    // level 0
    public GameObject prefab;
    public int cost;

    // upgrades
    public TurretUpgrade[] upgrades;
}