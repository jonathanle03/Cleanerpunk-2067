using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    public Transform statusPanel;
    public GameObject statusEntryPrefab;
    private Dictionary<StatusEffect, GameObject> statusEntries = new();

    public void UpdateStatusUI(List<StatusEffect> activeEffects)
    {
        // Clear missing effects
        foreach (StatusEffect effect in new List<StatusEffect>(statusEntries.Keys))
        {
            if (!activeEffects.Contains(effect))
            {
                Destroy(statusEntries[effect]);
                statusEntries.Remove(effect);
            }
        }

        // Create/update effects
        foreach (StatusEffect effect in activeEffects)
        {
            if (!statusEntries.ContainsKey(effect))
            {
                GameObject entry = Instantiate(statusEntryPrefab, statusPanel);
                entry.transform.Find("Icon").GetComponent<Image>().sprite = effect.icon;
                statusEntries.Add(effect, entry);
            }

            TextMeshProUGUI text = statusEntries[effect].transform.Find("Text").GetComponent<TextMeshProUGUI>();
            if (effect.duration > 0)
            {
                text.text = effect.duration.ToString();
            }
            else
            {
                text.text = "";
            }
        }
    }
}
