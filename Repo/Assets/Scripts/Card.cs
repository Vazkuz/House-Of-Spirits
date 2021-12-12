using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "8 Locos - Repo/Card", order = 0)]
public class Card : ScriptableObject 
{
    [Range(1,13)] public int cardNumber;
    public enum CardSuit {Orange /*Llama*/, Yellow /*Tigrillo*/, Green /*Jaguar*/, Purple /*Condor*/, NoColor};
    public CardSuit cardSuit;
    public Sprite artwork;

}

