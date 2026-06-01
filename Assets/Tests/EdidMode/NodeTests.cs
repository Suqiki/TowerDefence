using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools; // Obligatoriu pentru LogAssert

public class NodeTests
{
    private GameObject nodeObject;
    private Node node;
    private GameObject dummyManagerObj;
    private GameObject dummyPrefab;

    [SetUp]
    public void Setup()
    {
        nodeObject = new GameObject("TestNode");
        node = nodeObject.AddComponent<Node>();

        MeshRenderer renderer = nodeObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));

        node.hoverColor = Color.cyan;
        node.purchasedColor = Color.green;

        // Injectăm componenta renderer în câmpul privat din Node.cs
        System.Reflection.FieldInfo rendField = typeof(Node).GetField("rend", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rendField != null)
        {
            rendField.SetValue(node, renderer);
        }

        dummyManagerObj = new GameObject("DummyManagers");
        NodeManager.instance = dummyManagerObj.AddComponent<NodeManager>();
        BuildManager.instance = dummyManagerObj.AddComponent<BuildManager>();
        SoundEffectsManager.instance = dummyManagerObj.AddComponent<SoundEffectsManager>();
        WarningUI.instance = dummyManagerObj.AddComponent<WarningUI>();
        PlayerProgressManager.instance = dummyManagerObj.AddComponent<PlayerProgressManager>();

        dummyPrefab = new GameObject("DummyPrefab");
        PlayerStats.Gold = 400;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(nodeObject);
        if (dummyManagerObj != null)
        {
            Object.DestroyImmediate(dummyManagerObj);
        }
        if (dummyPrefab != null)
        {
            Object.DestroyImmediate(dummyPrefab);
        }
    }

    [Test]
    public void Node_InitialState_IsNotPurchased_And_HasNoTurret()
    {
        Assert.IsFalse(node.isPurchased);
        Assert.IsNull(node.turret);
        Assert.IsFalse(node.turretBuild);
    }

    [Test]
    public void Node_GetSellValue_CalculatesThreeQuartersOfTotalSpent()
    {
        TurretBlueprint blueprint = new TurretBlueprint();
        blueprint.cost = 100;
        blueprint.upgrades = new TurretUpgrade[0]; 

        node.turretBlueprint = blueprint;
        node.currentUpgradeLevel = 0;

        int sellValue = node.GetSellValue();

        Assert.AreEqual(75, sellValue);
    }

    [Test]
    public void Node_StartHighlightAnimation_DoesNotRun_IfNodeIsAlreadyPurchased()
    {
        // Spunem testului să se aștepte la eroarea automată de material leak din Unity EditMode
        LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Instantiating material due to calling renderer.material"));

        // ARRANGE
        node.isPurchased = true;

        // ACT
        node.StartHighlightAnimation();

        // ASSERT
        var renderer = node.GetComponent<Renderer>();
        Assert.AreEqual(Color.white, renderer.material.color); 
    }

    [Test]
    public void Node_StopHighlightAnimation_ResetsColorToPurchasedColor_IfPurchased()
    {
        // Spunem testului să se aștepte la eroarea automată de material leak din Unity EditMode
        LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Instantiating material due to calling renderer.material"));

        // ARRANGE
        node.isPurchased = true;

        // ACT
        node.StopHighlightAnimation();

        // ASSERT
        var renderer = node.GetComponent<Renderer>();
        Assert.AreEqual(node.purchasedColor, renderer.material.color);
    }
}