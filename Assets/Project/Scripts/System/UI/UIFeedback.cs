using TMPro;
using UnityEngine;
using System.Collections;

public class UIFeedback : MonoBehaviour
{
    public static UIFeedback Instance { get; private set; }

    [Header("Painel de Feedback Rápido")]
    public GameObject feedbackPanel;
    public TMP_Text feedbackText;
    public float feedbackDuration = 2f;

    [Header("Painel de Descrição (Examine)")]
    public GameObject examinePanel;
    public TMP_Text descriptionText;

    private Coroutine feedbackCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Já existe uma instância de UIFeedback na cena.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DefensiveMethod();
    }

    private void DefensiveMethod()
    {
        // Validação defensiva
        if (feedbackText == null) Debug.LogError("UIFeedback: feedbackText não atribuído no Inspector!");
        if (feedbackPanel == null) Debug.LogError("UIFeedback: feedbackPanel não atribuído no Inspector!");

        if (feedbackText != null) feedbackText.text = "";
        if (descriptionText != null) descriptionText.text = "";

        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (examinePanel != null) examinePanel.SetActive(false);
    }

    // Mensagem curta, tipo HUD ("Item coletado", etc.)
    public void ShowMessageFeedback(string message)
    {
        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        feedbackPanel.SetActive(true);
        feedbackText.text = message;

        yield return new WaitForSeconds(feedbackDuration);

        feedbackPanel.SetActive(false);
        feedbackText.text = "";
    }

    // Painel com descrição detalhada (Examinar item)
    public void ShowDescription(string description)
    {
        Debug.Log("ShowDescription chamado:\n" + description);
        examinePanel.SetActive(false);         // Fecha o painel, se necessário
        descriptionText.text = "";             // 🔥 Limpa completamente o texto
        descriptionText.ForceMeshUpdate();     // 🔄 Garante atualização da UI
        examinePanel.SetActive(true);          // Abre o painel
        descriptionText.text = description;    // 🔁 Define o novo texto
    }

    public void HideDescription()
    {
        examinePanel.SetActive(false);
    }
}
