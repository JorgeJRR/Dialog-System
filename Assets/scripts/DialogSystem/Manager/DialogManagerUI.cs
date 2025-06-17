using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using TMPro;

public class DialogManagerUI : MonoBehaviour
{
    [Header("Dialogs Elements")]
    [SerializeField]
    private GameObject dialogUI;
    [SerializeField]
    private GameObject dialogPanel;
    [SerializeField]
    private TMP_Text dialogTxt;
    [SerializeField]
    private Image playerPortrait;
    [SerializeField]
    private TMP_Text playerNameTxt;
    [SerializeField]
    private Image npcPortrait;
    [SerializeField]
    private TMP_Text npcNameTxt;
    [SerializeField]
    private GameObject openTranscriptBtn;

    [Header("Dialog Settings")]
    [SerializeField]
    private float dialogSpeed;

    [Header("Transcript Elements")]
    [SerializeField]
    private GameObject transcriptPanel;
    [SerializeField]
    private GameObject closeTranscriptBtn;
    [SerializeField]
    private GameObject prefabParent;
    [SerializeField]
    private GameObject dialogPrefab;

    private ObjectPool<DialogHandler> dialogPool;
    private List<DialogHandler> activeDialogs;

    public void Awake()
    {
        activeDialogs = new List<DialogHandler>();
        dialogPool = new ObjectPool<DialogHandler>(CreateDialog, OnGetFromPool, OnReleaseToPool, OnDestroyDialog);

        int initialPoolSize = 10;
        for (int i = 0; i < initialPoolSize; i++)
        {
            DialogHandler handler = CreateDialog();
            dialogPool.Release(handler);
        }
    }

    private DialogHandler CreateDialog()
    {
        // Instantiate el prefab y obtiene el componente DialogHandler
        GameObject obj = Instantiate(dialogPrefab, prefabParent.transform);
        DialogHandler handler = obj.GetComponent<DialogHandler>();
        // Asegúrate de que el objeto esté inicialmente inactivo.
        obj.SetActive(false);
        return handler;
    }

    private void OnGetFromPool(DialogHandler handler)
    {
        handler.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(DialogHandler handler)
    {
        handler.gameObject.SetActive(false);
        handler.transform.SetParent(prefabParent.transform);
        handler.dialogText.text = "";
    }

    private void OnDestroyDialog(DialogHandler handler)
    {
        Destroy(handler.gameObject);
    }

    public void PrepareDialogElements(NpcInfo NpcData)
    {
        dialogUI.SetActive(true);
        //transcriptPanel.SetActive(false);
        //openTranscriptBtn.SetActive(false);
        npcNameTxt.text = NpcData.NpcName;
        if (NpcData.portrait != null)
        {
            npcPortrait.sprite = NpcData.portrait;
        }

        dialogTxt.text = "";

        StartCoroutine(ShowDialogs(NpcData.dialogs));
    }

    private IEnumerator ShowDialogs(List<DialogInfo> dialogs)
    {
        foreach (DialogInfo dialog in dialogs)
        {
            AddDialogToTranscript(dialog);

            foreach (char letter in dialog.dialog)
            {
                dialogTxt.text += letter.ToString();
                yield return new WaitForSeconds(dialogSpeed);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            dialogTxt.text = "";
        }

        PrepareTranscriptElements();
    }

    public void PrepareTranscriptElements()
    {
        openTranscriptBtn.gameObject.SetActive(true);
    }

    public void OpenTranscription()
    {
        transcriptPanel.SetActive(true);
        openTranscriptBtn.SetActive(false);
    }

    public void CloseTranscription()
    {
        transcriptPanel.SetActive(false);
        openTranscriptBtn.SetActive(true);
    }

    public void AddDialogToTranscript(DialogInfo info)
    {
        DialogHandler dialogHandler = dialogPool.Get();
        dialogHandler.SetDialogPosition(info);
        activeDialogs.Add(dialogHandler);
    }

    public void ClearTranscript()
    {
        foreach (DialogHandler handler in activeDialogs)
        {
            dialogPool.Release(handler);
        }

        activeDialogs.Clear();
    }

    private void FinishDialogs()
    {
        dialogUI.SetActive(false);
    }

}

