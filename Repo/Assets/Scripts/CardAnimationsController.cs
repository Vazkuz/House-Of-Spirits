using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationsController : MonoBehaviour
{
    public static CardAnimationsController CAC;
    [SerializeField] int noAnimIndex = -1;
    public int genericAnimIndex = 0;
    public int sapaInkaAnimIndex = 12;
    public int wawaAnimIndex = 2;
    public int wiracochaAnimIndex = 8;
    Animator animator;
    
    void Awake()
    {
        CAC = this; //SapaInka_Card
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("index", noAnimIndex);
    }

    public void SetAnimation(int animationIndex)
    {
        animator.SetInteger("index", animationIndex);
        StartCoroutine(BackToNoAmination());
    }

    IEnumerator BackToNoAmination()
    {
        yield return null;
        animator.SetInteger("index", noAnimIndex);
    }
}
