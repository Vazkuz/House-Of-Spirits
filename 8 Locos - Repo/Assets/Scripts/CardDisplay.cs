using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] GameObject myDeckButton;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject myCardsFolder;
    [SerializeField] float cardSpriteSize = 1.5f;
    [SerializeField] Vector3 cardLocalPosition;
    [SerializeField] float distanceBetweenCardsX = 200f;
    [SerializeField] float distanceBetweenCardsY = 300f;
    [SerializeField] int maxCardsPerRow = 8;
    [SerializeField] int cardsDrawnInitially = 8;
    List<Card> cardsAvailable;
    List<Card> myCards = new List<Card>();
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        cardsAvailable = new List<Card>(Resources.LoadAll<Card>("Cards")); //OJO: "Cards" es el path de donde se cargan todos los ScriptableObjects tipo Card (Resources)
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DealCards());
        }
    }

    IEnumerator DealCards()
    {
        yield return new WaitForSeconds(4);
        for(int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
        {
            Debug.Log("Starting the draws: Player number " + playerIndex + ", player: " + PhotonNetwork.PlayerList[playerIndex]);
            PV.RPC("SendOrderOfDraw", RpcTarget.All, playerIndex);
        }
    }

    [PunRPC]
    void SendOrderOfDraw(int playerIndex)
    {
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if(playerCustom.GetComponent<PhotonView>() != null)
            {
                //Cuando encontremos al owner, lo usamos para instanciar todo
                if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner && playerCustom.GetComponent<PhotonView>().IsMine)
                {
                    DrawCards(cardsDrawnInitially, playerCustom, playerIndex);
                    myDeckButton.SetActive(true);
                }
            }
        }
    }

    void DrawCards(int cardsToDrawn, PhotonPlayer photonPlayer, int playerIndex)
    {
        for (int drawIndex = 0; drawIndex < cardsToDrawn; drawIndex++)
        {
            int cardDrawnIndex = Random.Range(0, cardsAvailable.Count);
            myCards.Add(cardsAvailable[cardDrawnIndex]);
            PV.RPC("RPC_RemoveFromDeck",RpcTarget.All,cardDrawnIndex);
            photonPlayer.AddCardToHand();
            photonPlayer.UpdateNumberOfCardsInDisplay(playerIndex, photonPlayer.GetNumberOfCards().ToString());
            SetupDrawnCard(photonPlayer);
        }
    }

    [PunRPC]
    void RPC_RemoveFromDeck(int cardDrawnIndex)
    {
        cardsAvailable.Remove(cardsAvailable[cardDrawnIndex]);
    }

    void SetupDrawnCard(PhotonPlayer photonPlayer)
    {
        // Name and parenting of the card drawn. We parent it in a GO/folder to organize them easily. 
        // GameObject cardDrawn = new GameObject(myCards[drawIndex].cardNumber.ToString() + " " + myCards[drawIndex].cardSuit.ToString());
        GameObject cardDrawn = Instantiate(cardPrefab, new Vector3(0,0,0), Quaternion.identity);
        cardDrawn.transform.SetParent(myCardsFolder.transform, false);
        cardDrawn.name = myCards[myCards.Count - 1].cardNumber.ToString() + " " + myCards[myCards.Count - 1].cardSuit.ToString();

        cardDrawn.transform.localScale = new Vector3(cardSpriteSize, cardSpriteSize, 0);
        int multiplyBy = (photonPlayer.GetNumberOfCards()-1)/maxCardsPerRow;
        cardDrawn.transform.localPosition = cardLocalPosition + 
                            new Vector3(distanceBetweenCardsX * (photonPlayer.GetNumberOfCards() - 1 - multiplyBy * maxCardsPerRow), - distanceBetweenCardsY * multiplyBy, 0);

        // We configure it's number and suit. We'll use that info later for the game mechanics.
        cardDrawn.GetComponent<CardController>().SetCardSuit(myCards[myCards.Count - 1].cardSuit);
        cardDrawn.GetComponent<CardController>().SetCardNumber(myCards[myCards.Count - 1].cardNumber);

        // Setup the visuals
        cardDrawn.GetComponent<Image>().overrideSprite = myCards[myCards.Count - 1].artwork;

    }
}
