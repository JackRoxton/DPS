using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool skillActive = false;
    bool startLocked = false;
    public bool locked = false;
    public int Cost;
    Image spr;

    bool mouseOver = false;
    
    public GameManager.Skills skill;

    public TMP_Text costText;

    public Skill[] nextSkills;

    private void Start()
    {
        spr = this.GetComponent<Image>();
        if(locked)
        {
            startLocked = true;
            spr.color = Color.gray;
        }
    }

    private void Update()
    {
        if(skillActive) return;
        if(locked) return;

        if (mouseOver && Input.GetMouseButton(0))
        {
            if (GameManager.Instance.Affordable(Cost))
            {
                skillActive = true;
                spr.color = Color.yellow;
                GameManager.Instance.BuySkill(skill, Cost);
                if (nextSkills != null)
                {
                    foreach (Skill skill in nextSkills)
                    {
                        if (skill.skillActive) return;
                        skill.locked = false;
                        skill.spr.color = Color.white;
                    }
                }
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (locked) return;
        if (!skillActive ) 
        { 
            spr.color = Color.blue; 
            costText.text = Cost.ToString();
        }
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (locked) return;
        if (!skillActive) 
        { 
            spr.color = Color.white;
            costText.text = "Cost";
        }
        mouseOver = false;
    }

    public void Resetvar()
    {
        if(skillActive)
        {
            skillActive = false;
            spr.color = Color.white;
        }

        if (startLocked)
        {
            locked = true;
            spr.color = Color.gray;
        }

        
    }
}
