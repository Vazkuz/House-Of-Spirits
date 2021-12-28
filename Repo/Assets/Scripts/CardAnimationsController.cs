using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimationsController : MonoBehaviour
{
    public static CardAnimationsController CAC;
    public Image emptyCard;
    public Sprite[] emptyCardSprites;
    public Card.CardSuit cardSuit;

    [SerializeField] int noAnimIndex = -1;
    public int genericAnimIndex = 0;
    public int wawaAnimIndex = 2;
    public int wiracochaAnimIndex = 8;
    public int llamaAnimIndex = 11;
    public int sapaInkaAnimIndex = 12;
    public int supayAnimIndex = 13;
    Animator animator;
    
    void Awake()
    {
        CAC = this; //SapaInka_Card
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("index", noAnimIndex);
        emptyCard.enabled = false;
    }

    public void SetAnimation(int animationIndex)
    {
        animator.SetInteger("index", animationIndex);
        StartCoroutine(BackToNoAmination());
    }
    public void SetCardColor()
    {
        emptyCard.enabled = true;
        if(cardSuit == Card.CardSuit.Green)
        {
            print("Green back");
            emptyCard.sprite = emptyCardSprites[0];
        }
        else if(cardSuit == Card.CardSuit.Orange)
        {
            print("Orange back");
            emptyCard.sprite = emptyCardSprites[1];
        }
        else if(cardSuit == Card.CardSuit.Purple)
        {
            print("Purple back");
            emptyCard.sprite = emptyCardSprites[2];
        }
        else if(cardSuit == Card.CardSuit.Yellow)
        {
            print("Yellow back");
            emptyCard.sprite = emptyCardSprites[3];
        }
        else if(cardSuit == Card.CardSuit.NoColor)
        {
            print("X back");
            emptyCard.sprite = emptyCardSprites[4];            
        }
    }

    public void ClearCardColor()
    {
        emptyCard.enabled = false;
    }

    IEnumerator BackToNoAmination()
    {
        yield return null;
        animator.SetInteger("index", noAnimIndex);
    }
}
