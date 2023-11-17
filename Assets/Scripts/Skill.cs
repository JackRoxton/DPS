using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool skillActive = false;
    public int Cost;
    Image spr;

    bool mouseOver = false;
    
    public GameManager.Skills skill;

    private void Start()
    {
        spr = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (skillActive) return;

        if (mouseOver && Input.GetMouseButton(0))
        {
            if (GameManager.Instance.Affordable(Cost))
            {
                skillActive = true;
                spr.color = Color.yellow;
                GameManager.Instance.BuySkill(skill, Cost);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!skillActive ) { spr.color = Color.blue; }
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!skillActive) { spr.color = Color.white; }
        mouseOver = false;
    }

    private void OnMouseUp()
    {
        if(GameManager.Instance.Affordable(Cost)) 
        { 
            skillActive = true;
            spr.color = Color.yellow;
            GameManager.Instance.BuySkill(skill,Cost);
        }
    }
}
