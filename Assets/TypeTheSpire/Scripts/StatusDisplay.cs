using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    public GameObject statusIconPrefab;
    public Sprite[] Icons;
    public wStatusEffect reference;
    Dictionary<wStatusEffect, Status> displayedStatuses = new Dictionary<wStatusEffect, Status>();
    bEntity entity;
    public void Setup(bEntity e)
    {
        entity = e;
        e.StatusUpdate.AddListener(UpdateStatuses);
    }

    public void UpdateStatuses()
    {
        List<wStatusEffect> toRemove = new();
        foreach (var key in displayedStatuses.Keys)
            if (!entity.has(key))
                toRemove.Add(key);
        foreach (var k in toRemove)
        {
            GameObject.Destroy(displayedStatuses[k].go);
            displayedStatuses.Remove(k);
        }
        foreach (var key in entity.statusEffects.Keys)
            if (!displayedStatuses.ContainsKey(key))
            {
                var go = Instantiate(statusIconPrefab, transform);
                displayedStatuses.Add(key, new Status(Icons[(int)key], entity.statusEffects[key].amount, go));
            }
            else
                displayedStatuses[key].UpdateStatus(entity.statusEffects[key].amount);
    }

    public class Status
    {
        public GameObject go;
        TMP_Text text;

        public Status(Sprite icon, int amt, GameObject inst)
        {
            inst.GetComponentInChildren<Image>().sprite = icon;
            text = inst.GetComponentInChildren<TMP_Text>();
            text.text = amt.ToString();
            go = inst;
        }

        public void UpdateStatus(int amt)
        {
            text.text = amt.ToString();
        }
    }
}
