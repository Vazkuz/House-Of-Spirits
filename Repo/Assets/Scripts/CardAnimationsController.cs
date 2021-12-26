using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationsController : MonoBehaviour
{
    public static CardAnimationsController CAC;
    [SerializeField] int noAnimIndex = -1;
    [SerializeField] int genericAnimIndex = 0;
    Animator animator;
    
    void Awake()
    {
        CAC = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("index", noAnimIndex);
    }

    IEnumerator BackToNoAmination()
    {
        yield return null;
        animator.SetInteger("index", noAnimIndex);
    }

    public void SetGenericAnimation()
    {
        animator.SetInteger("index", genericAnimIndex);
        StartCoroutine(BackToNoAmination());
    }
}
