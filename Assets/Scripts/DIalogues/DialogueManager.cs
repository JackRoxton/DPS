using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private Queue<string> sentences;
    private Queue<string> names;

    public Sprite[] characters;
    public Image image;

    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        nameText = UIManager.Instance.nameText;
        dialogueText = UIManager.Instance.dialogueText;
        image = UIManager.Instance.dialogueImage;
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
        
        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        if (name.ToString() == "Player")
            image.sprite = characters[0];
        else
            image.sprite = characters[1];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
    }

    public void EndDialogue() 
    {
        UIManager.Instance.EndDialogue();
    }
}
