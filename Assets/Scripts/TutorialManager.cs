using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public Button sellButton;
    
    
    [Header("UI")]
    public GameObject tutoriaUI;
    public TextMeshProUGUI tutorialText;
    public GameObject tutorialContinueText;
    public GameObject shopUI;
    
    [Header("Tutorial Visuals")]
    public GameObject spawnHighlight;
    public GameObject baseHighlight;
    
    [Header("Tutorial Nodes")]
    private Node[] nodes;

    public Node nodeToBuy;
    
    [Header("Tutorial Settings")]
    public bool tutorialEnabled = true;
    public WaveSpawner waveSpawner;

    private int tutorialStep = 0;

    [Header("Sounds")]
    public AudioClip hiSound;
    public float hiSoundVolume = 1;
    public AudioClip[] randomSound;
    public float randomSoundVolume = 1;
    
    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    
    private bool waitingForInput = false;
    private string currentText = "";
    private bool transitionTriggered = false;
    private bool turretBuildTriggered = false;
    private bool waveClearedTriggered = false;
    private bool turretUpgradeTriggered = false;
    
    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Siguranță pentru Singleton
    }
    
    private void Start()
    {
        if (!tutorialEnabled)
        {
            tutoriaUI.SetActive(false);
            return;
        }
        nodes = Object.FindObjectsByType<Node>(FindObjectsSortMode.None);
        
        Debug.Log("Am găsit " + nodes.Length + " noduri în scenă!");
        
        sellButton.interactable = false;
        shopUI.gameObject.SetActive(false);
        tutoriaUI.gameObject.SetActive(false);
        waveSpawner.isTutorialLVL = true;

        StartCoroutine(WaitToStart());
        
    }
    
    private void Update()
    {
        if (!tutorialEnabled) return;
        
        if (PauseMenu.isPaused)
            return;

        if (tutorialStep == 5 && nodeToBuy != null && nodeToBuy.isPurchased && !transitionTriggered)
        {
            transitionTriggered = true;
            NextStep();
        }

        if (tutorialStep == 11 && nodeToBuy != null && nodeToBuy.turretBuild && !turretBuildTriggered)
        {
            turretBuildTriggered = true;
            NextStep();
        }

        if (tutorialStep == 12 && EnemyCounter.enemiesAlive <= 0 && !waveClearedTriggered)
        {
            if (PlayerStats.rounds > 0) 
            {
                waveClearedTriggered = true;
                StartCoroutine(WaitAfterWave());
            }
        }
        
        if (tutorialStep == 18 && nodeToBuy != null && nodeToBuy.currentUpgradeLevel > 0 && !turretUpgradeTriggered)
        {
            turretUpgradeTriggered = true;
            NextStep();
        }
        
        if (waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            HideContinueText();
            NextStep();
        }
        
       
    }
    
    void ShowContinueText()
    {
        tutorialContinueText.SetActive(true);
    }

    void HideContinueText()
    {
        tutorialContinueText.SetActive(false);
    }
    
    void StartTutorial()
    {
        tutoriaUI.gameObject.SetActive(true);
        tutorialStep = 0;
        ShowStep();
    }

    // O funcție publică pe care o poți apela de oriunde pentru a avansa
    public void NextStep()
    {
        if (!tutorialEnabled) return;

        tutorialStep++;
        ShowStep();
    }
    
    void ShowStep()
    {
        tutoriaUI.SetActive(true);

        switch (tutorialStep)
        {
            //introducere
            case 0:
                waveSpawner.tutorialPaused = true;
                PlayerStats.Gold = 0;
                Time.timeScale = 0f;
                foreach (Node node in nodes)
                {
                    node.tutorialMode = true;
                    node.tutorialCanBuild = false;
                }
                StartTyping("Welcome young artificer!");
                break;

            case 1:
                StartTyping("I was hoping to meet you in better times. Dark forces are coming!");
                break;

            case 2:
                StartTyping("The King has called upon YOU to defend the realm.");
                break;
            
            case 3:
                spawnHighlight.SetActive(true);
                

                StartTyping(
                    "Enemies will spawn from this cave...");
                break;
            
            case 4:
                spawnHighlight.SetActive(false);
                baseHighlight.SetActive(true);
                StartTyping("They will follow the stone path towards the kingdom...\n\n" +
                            "Stop them before they reach it."
                );
                break;
            case 5:
                baseHighlight.SetActive(false);
                Time.timeScale = 1f;
                PlayerStats.Gold = 10;
                
                // REPARAT: Dezactivăm tutorialMode doar pentru acest nod ca să poată fi cumpărat
                if (nodeToBuy != null)
                {
                    nodeToBuy.tutorialMode = false; 
                    nodeToBuy.StartHighlightAnimation();
                }

                StartTyping("To stop them, we need defences. Here, have some gold. <b>Click on the glowing plot</b> to purchase your first land.");
                break;
            case 6:
                // Pasul ăsta va fi activat AUTOMAT din Node.cs când se cumpără nodul!
                if (nodeToBuy != null)
                {
                    nodeToBuy.tutorialMode = true; // Îl blocăm la loc ca jucătorul să nu mai dea click pe el aiurea până nu îi cerem asta
                }
                // Aici reactivăm Space-ul automat (prin TypeText), deci îi spunem jucătorului să apese Space
                StartTyping("Excellent! You have purchased your first piece of land. Be aware, The King doesn't like when commoners buy too much of his land");
                break;

            case 7:
                StartTyping("The more land you buy, the more expensive it will get. In the top left of your screen, you can see your land price");
                break;
            
            case 8:
                StartTyping("Now, let's place some turrets");
                break;
            
            case 9:
                shopUI.SetActive(true);
                StartTyping("In the left part of your screen, is the shop. You have 3 types of turrets");
                break;
            case 10:
                StartTyping("The green one shoots arrows. The red one shoots fire balls that explode on impact and the blue one has a laser that slows your enemies");
                break;
            case 11:
                PlayerStats.Gold = 125;
                nodeToBuy.tutorialMode = true;
                nodeToBuy.tutorialCanBuild = true;
                StartTyping("A first wave of enemies is coming. Here have 125 gold. Get the first turret to defend the road!");
                break;
            case 12:
                tutoriaUI.SetActive(false);
                waveSpawner.tutorialPaused =  false;
                nodeToBuy.tutorialCanBuild = false;
                break;
            case 13:
                waveSpawner.tutorialPaused = true;
                Time.timeScale = 0f; 
                StartTyping("Incredible job! You held the line perfectly. The kingdom might stand a chance with you after all.\nLook he also dropped 50 gold");
                break;
            case 14:
                StartTyping("<b>Young artificer!</b> You see that? A new hoard is coming and this one is bigger! And we don't have enough gold for a new turret");
                break;
            case 15:
                StartTyping("Don't panic, we can work this out. You can upgrade or sell your turrets. All turrets get better the more upgrades they have.");
                break;
            case 16:
                StartTyping("The green one has higher <b>DMG and shoots faster</b>. At max upgrade, it can also set enemies on fire.");
                break;
            case 17:
                StartTyping("The red one deals explosive burn damage, increasing its fire rate, while the blue one inflicts massive damage and an even stronger slow effect.");
                break;
            case 18:
                if (nodeToBuy != null)
                {
                    nodeToBuy.tutorialMode = false;
                }
                StartTyping("Upgrade your turret then you are free to go for this night. After this you will be on your own");
                break;
            case 19:
                // --- PAS NOU ---
                // Blocăm la loc nodul ca să nu mai poată face modificări în timpul valului final
                if (nodeToBuy != null)
                {
                    nodeToBuy.tutorialMode = true;
                }

                tutoriaUI.SetActive(false); 
                Time.timeScale = 1f;        
                waveSpawner.tutorialPaused=false; 
                break;

            case 20:
                // --- PAS NOU ---
                EndTutorial();
                break;
        }
    }
    
    void EndTutorial()
    {
        waveSpawner.tutorialPaused = false;
        tutoriaUI.SetActive(false);
        tutorialEnabled = false;
        Time.timeScale = 1f; // Ne asigurăm că jocul continuă normal
    }
    
    IEnumerator TypeText(string text)
    {
        tutorialText.text = "";
        HideContinueText();
        waitingForInput = false;

        foreach (char letter in text)
        {
            tutorialText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        if (tutorialStep == 5 || tutorialStep == 11 || tutorialStep == 18)
        {
            // Dacă suntem la pasul 5, blocăm intenționat apăsarea pe Space 
            // și ascundem textul de „Continue” pentru că vrem click pe nod!
            waitingForInput = false;
            HideContinueText();
        }
        else
        {
            // Pentru toți ceilalți pași normali, lăsăm Space-ul activ
            waitingForInput = true;
            ShowContinueText();
        }
    }
    
    IEnumerator WaitAfterWave()
    {
        yield return new WaitForSeconds(2f);

        NextStep();
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);
        StartTutorial();
    }
    
    void StartTyping(string text)
    {
        // oprește typing anterior dacă există
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (tutorialStep == 0)
        {
            SoundEffectsManager.instance.PlaySoundEffect(hiSound,transform,hiSoundVolume);
        }
        else
        {
            SoundEffectsManager.instance.PlayrandomSoundEffect(randomSound,transform,randomSoundVolume);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    
    public bool IsTutorialPaused()
    {
        return tutorialStep == 0 ||
               tutorialStep == 1 ||
               tutorialStep == 2 ||
               tutorialStep == 3 ||
               tutorialStep == 4 ||
               tutorialStep == 13 ||
               tutorialStep == 14 ||
               tutorialStep == 15 ||
               tutorialStep == 16 ||
               tutorialStep == 17;
    }
}