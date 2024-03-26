using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class DialogueManager : Singleton<DialogueManager>
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image speakerLeft, speakerRight;

    private Queue<string> sentences;
    private Queue<string> names;

    //public DialogueClick dialogueClick;

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
            if (letter == '1')
                dialogueText.text += "<color=#ea3434>";
            else if (letter == '2')
                dialogueText.text += "<color=#25ad18>";
            else if (letter == '3')
                dialogueText.text += "<color=#234bfc>";
            else if (letter == '0')
                dialogueText.text += "</color>";
            else
                dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentence()
    {
        //dialogueClick.LockTimer();
        if (DialogueEventManager.Instance != null)
            if (DialogueEventManager.Instance.eventLock == true)
                return;
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();

        if(name == "Event")
        {
            DialogueEventManager.Instance.Event(sentence);
            return;
        }

        if (name.ToString() == "Mage")
        {
            speakerLeft.gameObject.SetActive(false);
            speakerRight.gameObject.SetActive(true);
        }
        else
        {
            speakerLeft.gameObject.SetActive(true);
            speakerRight.gameObject.SetActive(false);
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
    }

    public void EndDialogue() 
    {
        UIManager.Instance.EndDialogue();
    }

    /*public void LockDialogue()
    {
        dialogueClick.LockTimer();
    }*/
}
