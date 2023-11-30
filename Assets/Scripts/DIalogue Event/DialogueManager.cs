using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image speakerLeft, speakerRight;

    private Queue<string> sentences;
    private Queue<string> names;

    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        nameText = UIManager.Instance.nameText;
        dialogueText = UIManager.Instance.dialogueText;
        speakerLeft = UIManager.Instance.dialogueImageLeft;
        speakerRight = UIManager.Instance.dialogueImageRight;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        names.Clear();

        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator TypeSentence (string sentence, string name)
    {
        nameText.text = name;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }
        if (TutorialEventManager.Instance != null)
            if (TutorialEventManager.Instance.eventLock == true)
                return;

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();

        if(name == "Event")
        {
            TutorialEventManager.Instance.Event(sentence);
            return;
        }

        if (name.ToString() == "Player")
        {
            speakerLeft.gameObject.SetActive(true);
            speakerRight.gameObject.SetActive(false);
        }
        else
        {
            speakerLeft.gameObject.SetActive(false);
            speakerRight.gameObject.SetActive(true);
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
    }

    public void EndDialogue() 
    {
        UIManager.Instance.EndDialogue();
    }
}
