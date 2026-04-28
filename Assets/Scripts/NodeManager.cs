using UnityEngine;
using TMPro;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;
    public TextMeshProUGUI costText;


    [Header("Cost Settings")]
    public int baseCost = 10;
    public int nodesPerIncrease = 5;
    public float increasePercent = 0.2f;

    [Header("Runtime")]
    public int totalNodesPurchased = 0;

    void Awake()
    {
        instance = this;
        UpdateCostUI();
    }
    
    void Start()
    {
        // 1. Numărăm nodurile deja cumpărate la început
        CountInitialPurchasedNodes();
        
        // 2. Actualizăm UI-ul cu prețul corect calculat
        UpdateCostUI();
    }

    void CountInitialPurchasedNodes()
    {
        // Găsește toate obiectele care au scriptul Node pe ele
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        
        totalNodesPurchased = 0; // Resetăm pentru siguranță

        foreach (Node node in allNodes)
        {
            if (node.isPurchased)
            {
                totalNodesPurchased++;
            }
        }
        
        Debug.Log("Jocul a început cu " + totalNodesPurchased + " noduri cumpărate.");
    }

    public int GetCurrentCost()
    {
        int level = totalNodesPurchased / nodesPerIncrease;
        float multiplier = Mathf.Pow(1 + increasePercent, level);
        return Mathf.RoundToInt(baseCost * multiplier);
    }

    public void RegisterPurchase()
    {
        totalNodesPurchased++;
        UpdateCostUI();

    }
    
    void UpdateCostUI()
    {
        costText.text = "Land Price: " + GetCurrentCost();
    }
}
