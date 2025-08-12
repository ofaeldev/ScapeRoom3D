using UnityEngine;

public class ClosedButtonUI : MonoBehaviour
{
    public GameObject panelUI;

    public void ClosedPanel()
    {
        if (panelUI == null) return;

        panelUI.SetActive(false);
    }
}
