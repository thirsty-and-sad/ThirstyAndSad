using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public Slider playerSanityBar;
    public Slider enemySanityBar;
    public TextMeshProUGUI battleLog;
    public GameObject hallucinationEffect;
    
    public GameObject buttonBackground;
    public Button attackButton1;
    public Button attackButton2;
    public Button fleeButton;

    private bool playerTurn = true;
    private bool inBattle = true;
    private bool isProcessing = false;

    private Character player;
    private Character enemy;

    void Start()
    {
       player = new Character("Remy", GameManager.Instance.playerSanity, new List<Attack> 
        {
            new Attack("Chant Impie", 15), 
            new Attack("Prière Inutile", 10) 
        });

        enemy = new Character("Créature Cthonienne", 80, new List<Attack> 
        { 
            new Attack("Hurlement Cosmique", 10) 
        });

        attackButton1.onClick.AddListener(() => StartCoroutine(PlayerAttack(0)));
        attackButton2.onClick.AddListener(() => StartCoroutine(PlayerAttack(1))); 
        fleeButton.onClick.AddListener(() => StartCoroutine(Flee()));

        UpdateUI();
        EnableButtons(false);
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop()
    {
        while (inBattle)
        {
            yield return new WaitUntil(() => !isProcessing);

            if (playerTurn)
            {
                battleLog.text = "À toi de jouer !";
                yield return new WaitForSeconds(1f);
                EnableButtons(true);
            }
            else
            {
                battleLog.text = "L'ennemi attaque...";
                yield return new WaitForSeconds(1f);
                yield return StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator PlayerAttack(int attackIndex)
    {
        if (!playerTurn || isProcessing) yield break;
        isProcessing = true;
        EnableButtons(false);

        if (player.Sanity < 20)
        {
            int confusionChance = Random.Range(0, 3);
            if (confusionChance == 0)
            {
                battleLog.text = $"{player.Name} est paralysé par la peur !";
                yield return new WaitForSeconds(1f);
                isProcessing = false;
                playerTurn = false;
                StartCoroutine(BattleLoop());
                yield break;
            }
            else if (confusionChance == 1)
            {
                battleLog.text = $"{player.Name} attaque... lui-même !";
                yield return new WaitForSeconds(1f);
                player.LoseSanity(10);
                UpdateUI();
                isProcessing = false;
                playerTurn = false;
                StartCoroutine(BattleLoop());
                yield break;
            }
        }

        Attack attack = player.Attacks[attackIndex];
        battleLog.text = $"{player.Name} utilise {attack.Name} !";
        yield return new WaitForSeconds(1f);

        enemy.LoseSanity(attack.Damage);
        battleLog.text = $"{enemy.Name} perd {attack.Damage} Sanité.";
        UpdateUI();
        yield return new WaitForSeconds(1f);

        if (enemy.IsInsane())
        {
            EndBattle(true);
            yield break;
        }

        isProcessing = false;
        playerTurn = false;
        StartCoroutine(BattleLoop());
    }

    IEnumerator EnemyTurn()
    {
        if (isProcessing) yield break;
        isProcessing = true;

        int accuracy = Random.Range(0, 100);
        if (accuracy < 20)
        {
            battleLog.text = $"{enemy.Name} rate son attaque !";
            yield return new WaitForSeconds(1f);
            isProcessing = false;
            playerTurn = true;
            StartCoroutine(BattleLoop());
            yield break;
        }

        Attack attack = enemy.Attacks[0];
        battleLog.text = $"{enemy.Name} attaque avec {attack.Name} !";
        yield return new WaitForSeconds(1f);

        player.LoseSanity(attack.Damage);
        battleLog.text = $"{player.Name} perd {attack.Damage} Sanité.";
        UpdateUI();
        yield return new WaitForSeconds(1f);

        if (player.IsInsane())
        {
            EndBattle(false);
            yield break;
        }

        isProcessing = false;
        playerTurn = true;
        StartCoroutine(BattleLoop());
    }

    IEnumerator Flee()
    {
        if (isProcessing) yield break;
        isProcessing = true;

        battleLog.text = "Tu as tenté de fuir...";
        yield return new WaitForSeconds(1f);

        int fleeChance = Random.Range(0, 2);
        if (fleeChance == 0)
        {
            battleLog.text = "Tu n'as pas réussi à fuir !";
            yield return new WaitForSeconds(1f);
            isProcessing = false;
            playerTurn = false;
            StartCoroutine(BattleLoop());
        }
        else
        {
            battleLog.text = "Tu as fui avec succès !";
            yield return new WaitForSeconds(1f);
            inBattle = false;
        }
    }

    void EndBattle(bool playerWon)
    {
        inBattle = false;
        battleLog.text = playerWon ? "Tu as vaincu la créature !" : "Tu as sombré dans la folie...";
        EnableButtons(false);
        GameManager.Instance.playerSanity = player.Sanity;
    }

    void UpdateUI()
    {
        playerSanityBar.value = (float)player.Sanity / player.MaxSanity;
        enemySanityBar.value = (float)enemy.Sanity / enemy.MaxSanity;

        Image img = hallucinationEffect.GetComponent<Image>();

        if (player.Sanity < 20)
        {
            float flicker = Mathf.Abs(Mathf.Sin(Time.time * 3f));
            img.color = new Color(img.color.r, img.color.g, img.color.b, flicker * 0.5f);
        }
        else
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
    }

    void EnableButtons(bool enable)
    {
        buttonBackground.SetActive(enable);
        attackButton1.interactable = enable;
        attackButton2.interactable = enable;
        fleeButton.interactable = enable;
    }
}
