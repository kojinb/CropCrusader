using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public int gold;
    public int landCost;
    public TextMeshProUGUI goldText;

    // Called before start
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        gold = 100;
        landCost = 10;

        SetGoldText();
    }

    void SetGoldText()
    {
        goldText.text = "Gold: " + gold.ToString();
    }

    public void PurchaseLand()
    {
        gold = gold - landCost;
        SetGoldText();

        landCost = landCost * 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
