using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBuffs : MonoBehaviour
{
    public List<Buff> availableBuffs;
    public List<Debuff> availableDebuffs;
    public GameObject buffPanel;  // Reference to the BuffPanel UI element
    public GameObject debuffPanel;  // Reference to the DebuffPanel UI element
    public GameObject buffTextTemplate;  // Reference to the BuffTextTemplate UI element
    public GameObject debuffTextTemplate;  // Reference to the DebuffTextTemplate UI element

    private List<Buff> activeBuffs = new List<Buff>();
    private List<Debuff> activeDebuffs = new List<Debuff>();
    private List<GameObject> buffTextObjects = new List<GameObject>();
    private List<GameObject> debuffTextObjects = new List<GameObject>();

    private float verticalSpacing = 30f;  // Adjust this value to set the spacing between text elements

    private void Start()
    {
        Debug.Log("PlayerBuffs Start method called.");
        if (buffTextTemplate != null) buffTextTemplate.SetActive(false);
        if (debuffTextTemplate != null) debuffTextTemplate.SetActive(false);
    }

    public void ApplyRandomBuff()
    {
        if (availableBuffs.Count > 0)
        {
            int randomIndex = Random.Range(0, availableBuffs.Count);
            Buff selectedBuff = availableBuffs[randomIndex];
            selectedBuff.ApplyBuff(this);
            activeBuffs.Add(selectedBuff);
            Debug.Log("Buff applied: " + selectedBuff.buffName);
            AddBuffDisplay(selectedBuff);
        }
        else
        {
            Debug.LogWarning("No available buffs to apply.");
        }
    }

    public void ApplyRandomDebuff()
    {
        if (availableDebuffs.Count > 0)
        {
            int randomIndex = Random.Range(0, availableDebuffs.Count);
            Debuff selectedDebuff = availableDebuffs[randomIndex];
            selectedDebuff.ApplyDebuff(this);
            activeDebuffs.Add(selectedDebuff);
            Debug.Log("Debuff applied: " + selectedDebuff.debuffName);
            AddDebuffDisplay(selectedDebuff);
        }
        else
        {
            Debug.LogWarning("No available debuffs to apply.");
        }
    }

    public void RemoveRandomBuff()
    {
        if (activeBuffs.Count > 0)
        {
            int randomIndex = Random.Range(0, activeBuffs.Count);
            activeBuffs.RemoveAt(randomIndex);
            Destroy(buffTextObjects[randomIndex]);
            buffTextObjects.RemoveAt(randomIndex);
            UpdateBuffPositions();  // Update positions after removing
            Debug.Log("Buff removed at index: " + randomIndex);
        }
        else
        {
            Debug.LogWarning("No active buffs to remove.");
        }
    }

    public void RemoveRandomDebuff()
    {
        if (activeDebuffs.Count > 0)
        {
            int randomIndex = Random.Range(0, activeDebuffs.Count);
            activeDebuffs.RemoveAt(randomIndex);
            Destroy(debuffTextObjects[randomIndex]);
            debuffTextObjects.RemoveAt(randomIndex);
            UpdateDebuffPositions();  // Update positions after removing
            Debug.Log("Debuff removed at index: " + randomIndex);
        }
        else
        {
            Debug.LogWarning("No active debuffs to remove.");
        }
    }

    private void AddBuffDisplay(Buff buff)
    {
        Debug.Log("Adding Buff Display");
        if (buffPanel == null || buffTextTemplate == null)
        {
            Debug.LogError("Buff panel or text template is not assigned.");
            return;
        }

        GameObject buffText = Instantiate(buffTextTemplate, buffPanel.transform);
        buffText.SetActive(true);
        TextMeshProUGUI textComponent = buffText.GetComponent<TextMeshProUGUI>();
        textComponent.text = buff.buffName;

        // Ensure text is visible
        textComponent.color = Color.white; // Set the color to white for visibility
        textComponent.alpha = 1.0f; // Set the alpha to fully opaque
        textComponent.enabled = true; // Enable the text component

        // Check font and material
        if (textComponent.font == null)
        {
            Debug.LogError("TextMeshProUGUI font is not assigned.");
        }

        if (textComponent.fontMaterial == null)
        {
            Debug.LogError("TextMeshProUGUI material is not assigned.");
        }

        RectTransform rectTransform = buffText.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;

        // Position the new text element
        rectTransform.anchoredPosition = new Vector2(0, -buffTextObjects.Count * verticalSpacing); // Adjust the Y offset

        buffTextObjects.Add(buffText);

        Debug.Log($"Buff '{buff.buffName}' displayed with color {textComponent.color} and alpha {textComponent.alpha}");
    }

    private void AddDebuffDisplay(Debuff debuff)
    {
        Debug.Log("Adding Debuff Display");
        if (debuffPanel == null || debuffTextTemplate == null)
        {
            Debug.LogError("Debuff panel or text template is not assigned.");
            return;
        }

        GameObject debuffText = Instantiate(debuffTextTemplate, debuffPanel.transform);
        debuffText.SetActive(true);
        TextMeshProUGUI textComponent = debuffText.GetComponent<TextMeshProUGUI>();
        textComponent.text = debuff.debuffName;

        // Ensure text is visible
        textComponent.color = Color.white; // Set the color to white for visibility
        textComponent.alpha = 1.0f; // Set the alpha to fully opaque
        textComponent.enabled = true; // Enable the text component

        // Check font and material
        if (textComponent.font == null)
        {
            Debug.LogError("TextMeshProUGUI font is not assigned.");
        }

        if (textComponent.fontMaterial == null)
        {
            Debug.LogError("TextMeshProUGUI material is not assigned.");
        }

        RectTransform rectTransform = debuffText.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;

        // Position the new text element
        rectTransform.anchoredPosition = new Vector2(0, -debuffTextObjects.Count * verticalSpacing); // Adjust the Y offset

        debuffTextObjects.Add(debuffText);

        Debug.Log($"Debuff '{debuff.debuffName}' displayed with color {textComponent.color} and alpha {textComponent.alpha}");
    }

    private void UpdateBuffPositions()
    {
        for (int i = 0; i < buffTextObjects.Count; i++)
        {
            RectTransform rectTransform = buffTextObjects[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * verticalSpacing);
        }
    }

    private void UpdateDebuffPositions()
    {
        for (int i = 0; i < debuffTextObjects.Count; i++)
        {
            RectTransform rectTransform = debuffTextObjects[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * verticalSpacing);
        }
    }
}
