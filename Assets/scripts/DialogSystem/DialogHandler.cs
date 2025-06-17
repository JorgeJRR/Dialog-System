using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    [SerializeField]
    private RectTransform panelRT;
    [SerializeField]
    private RectTransform dialogRT;
    [SerializeField]
    public TMP_Text dialogText;

    public void SetDialogPosition(DialogInfo info)
    {
        if (info.isPlayer)
        {
            panelRT.offsetMin = new Vector2(90, panelRT.offsetMin.y);
            panelRT.offsetMax = new Vector2(0, panelRT.offsetMax.y);

            dialogRT.offsetMin = new Vector2(100, panelRT.offsetMin.y);
            dialogRT.offsetMax = new Vector2(-10, panelRT.offsetMax.y);
            panelRT.localScale = Vector3.one;
        }
        else
        {
            panelRT.offsetMin = new Vector2(0, panelRT.offsetMin.y);
            panelRT.offsetMax = new Vector2(-90, panelRT.offsetMax.y);

            dialogRT.offsetMin = new Vector2(10, panelRT.offsetMin.y);
            dialogRT.offsetMax = new Vector2(-100, panelRT.offsetMax.y);
            panelRT.localScale = new Vector3(-1, 1, 1);
        }

        dialogText.text = info.dialog;
    }
}
