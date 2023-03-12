using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bentendo.typeSpire
{
    public class ShopUI : MonoBehaviour
    {
        public Button[] Buttons;

        public SpellDeck spells;

        public GameObject shopParent;

        public AudioSource purchaseSound;

        WordSpell[] choices;

        public void Awake()
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                int k = i;
                Buttons[i].onClick.AddListener(() => PurchaseOption(k));
            }
            choices = new WordSpell[Buttons.Length];
            StockRandom();
        }

        public void PurchaseOption(int n)
        {
            TypeTheSpire.Instance.Discard(new WordCard(choices[n]));
            purchaseSound.Play();
            shopParent.gameObject.SetActive(false);
        }

        public void StockRandom()
        {
            for (int i = 0; i < choices.Length; i++)
            {
                var spell = spells.spells[Random.Range(0, spells.spells.Length)];
                choices[i] = spell;
                Buttons[i].image.sprite = spell.icon;
            }
        }
        
        public IEnumerator WaitShopChoice()
        {
            StockRandom();
            TypeTheSpire.Instance.kInput.SetTarget(SelectOption);
            shopParent.gameObject.SetActive(true);
            while (shopParent.gameObject.activeSelf)
                yield return null;
            TypeTheSpire.Instance.kInput.SetTarget(TypeTheSpire.Instance.TryFind);
        }
        
        public void SelectOption(char c)
        {
            if (c == 'a' || c == '1')
                PurchaseOption(0);
            else if (c == 'b' || c == '2')
                PurchaseOption(1);
            else if (c == 'c' || c == '3')
                PurchaseOption(2);
        }
    }
}
