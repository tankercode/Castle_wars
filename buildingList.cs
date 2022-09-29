using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingList : MonoBehaviour
{
    [SerializeField]
    private pickRace pr;

    [SerializeField]
    private DragAndDropBuilding dragAndDrop;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject fieldForCards;

    [SerializeField]
    private GameObject card;

    private List<UnitData> TmpUnitsList;

    private List<GameObject> cards;

    private void Awake()
    {
        cards = new List<GameObject>();

        TmpUnitsList = new List<UnitData>();
    }

    private void ClearCards()
    {

        if (cards.Count > 0)
        {

            foreach (var card in cards)
            {
                Destroy(card.gameObject);
            }

            cards.Clear();
        }
    }

    private void CreateCards()
    {

        foreach (var up in TmpUnitsList)
        {

            var cardScript = card.GetComponent<UnitCard>();

            cardScript.UnitData = up;

            cards.Add(Instantiate(card));
        }

        fieldForCards.GetComponent<RectTransform>().sizeDelta = new Vector2(cards.Count * 130, 200);

        for (int x = 0; x < cards.Count; x++)
        {

            cards[x].transform.SetParent(fieldForCards.transform);

            cards[x].transform.localPosition = new Vector3(x * 130 + offset.x, offset.y, 0);

            cards[x].GetComponent<UnitCard>().Dd.AddListener(dragAndDrop.StartPlacing);
        }
    }

    public void GetUnitsList()
    {
        TmpUnitsList = pr.SetRace().Prefab.GetComponent<Unit>().upgradeList;

        ClearCards();

        CreateCards();
    }

}
