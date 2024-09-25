using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueObject;

    public AudioClip dialogueSFX = null;
    AudioSource audioPlayer;

    public static DialogueSystem GetMainDialogueSystem()
    {
        return mainDialogueSystem;
    }
    static DialogueSystem mainDialogueSystem = null;

    private void Awake()
    {
        if (mainDialogueSystem == null)
        {
            //DontDestroyOnLoad(gameObject);
            mainDialogueSystem = this;

            // subscribe to events here
        }
        else
        {
            Debug.Log("Second dialogue system destroys itself");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (mainDialogueSystem == this) mainDialogueSystem = null;
    }

    private void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Dialogue>() != null)
            {
                t.GetComponent<Dialogue>().SetUp(this);
            }
        }
        audioPlayer = GetComponent<AudioSource>();
    }

    public void HandleText(string textValue, float timer)
    {
        CancelInvoke(nameof(StopText));
        dialogueObject.text = textValue;
        PlaySound(dialogueSFX);
        Invoke(nameof(StopText), timer);
    }

    private void StopText()
    {
        dialogueObject.text = "";
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioPlayer != null && sound != null)
        {
            audioPlayer.PlayOneShot(sound);
        }
        else
        {
            Debug.Log("DialogueSystem: Cannot play sound. audioPlayer = " + (audioPlayer != null) + ", sound = " + (sound != null));
        }
    }

    // Method to call when interaction is completed
    public void InteractionCompleted(string interactionID)
    {
        // Find all dialogue objects that require this interaction to disable repetition
        Dialogue[] dialogues = FindObjectsOfType<Dialogue>();

        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.GetInteractionToDisable() == interactionID)
            {
                dialogue.CompleteInteraction();
            }
        }
    }
}
