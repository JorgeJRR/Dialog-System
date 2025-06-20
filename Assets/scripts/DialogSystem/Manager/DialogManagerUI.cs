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
    [SerializeField]
    private GameObject closeDialogBtn;

    [Header("Dialog Settings")]
    [SerializeField]
    private float dialogSpeed;
    [SerializeField]
    public float SpeakerPortraitAlpha;
    [SerializeField]
    public float ListenerPortraitAlpha;

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

    private Coroutine currentDialogRoutine;

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
        GameObject obj = Instantiate(dialogPrefab, prefabParent.transform);
        DialogHandler handler = obj.GetComponent<DialogHandler>();
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

        currentDialogRoutine = StartCoroutine(ShowDialogs(NpcData.dialogs));
    }

    private IEnumerator ShowDialogs(List<DialogInfo> dialogs)
    {
        foreach (DialogInfo dialog in dialogs)
        {
            AddDialogToTranscript(dialog);
            SetDialogueVisuals(dialog.isPlayer);

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

    private void SetDialogueVisuals(bool isPlayer)
    {
        if (isPlayer)
        {
            playerPortrait.color = new Color(255,255,255,1);
            npcPortrait.color = new Color(255, 255, 255, 0.5f);
        }
        else
        {
            playerPortrait.color = new Color(255, 255, 255, 0.5f);
            npcPortrait.color = new Color(255, 255, 255, 1);
        }
    }

    public void PrepareTranscriptElements()
    {
        if (currentDialogRoutine != null)
        {
            StopCoroutine(currentDialogRoutine);
            currentDialogRoutine = null;
        }
        playerPortrait.color = new Color(255, 255, 255, 1);
        npcPortrait.color = new Color(255, 255, 255, 1);

        openTranscriptBtn.gameObject.SetActive(true);
        closeDialogBtn.SetActive(true);
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

    public void FinishDialogs()
    {
        dialogUI.SetActive(false);
        ClearTranscript();
        transcriptPanel.SetActive(false);
        openTranscriptBtn.SetActive(false);
        closeDialogBtn.SetActive(false);
    }

}

