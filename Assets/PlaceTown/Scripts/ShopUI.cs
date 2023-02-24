using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Button[] Buttons;

    int[] choices;

    Tileset set;
    PlaceTown town;

    public void Awake()
    {
        choices = new int[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            int bid = i;
            Buttons[i].onClick.AddListener(() => PurchaseOption(bid));
        }
        RandomOptions();
    }

    public void Init(PlaceTown town_, Tileset set_)
    {
        town = town_;
        set = set_;
    }

    public void PurchaseOption(int n)
    {
        town.PurchaseCardFromShop(choices[n]);
    }

    public void RandomOptions()
    {
        var options = new List<int>() { 2, 3, 4, 5 };
        for (int i = 0; i < choices.Length; i++)
        {
            int n = Random.Range(0, options.Count);
            choices[i] = options[n];
            options.Remove(n);
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponentInChildren<Image>().sprite = set[choices[i]].sprite;
        }
    }
}
