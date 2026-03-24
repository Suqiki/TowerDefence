using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Gold;
    public int startGold = 400;

    void Start()
    {
        Gold = startGold;
    }
}
