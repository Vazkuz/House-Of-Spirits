using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
     
    [SerializeField] public enum CardNumber {Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King};
    [SerializeField] public enum CardSuit {Hearts /*Corazón*/, Clovers /*Tréboles*/, Tiles /*Diamante*/, Pikes /*Espada*/}; //Suit = palo

    [SerializeField] public CardNumber cardNumber;
    [SerializeField] public CardSuit cardSuit;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
