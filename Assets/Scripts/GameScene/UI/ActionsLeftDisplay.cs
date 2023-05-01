using TMPro;
using UnityEngine;

public class ActionsLeftDisplay : MonoBehaviour
{
    public TextMeshPro actionsLeftText;

    public void SetActionsLeft(int actionsLeft)
    {
        actionsLeftText.text = actionsLeft.ToString();
    }
}
