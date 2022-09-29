using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class pickRace : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Timer;

    [SerializeField]
    private Unit unitWithListRace;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject fieldForCards;

    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    private GameObject card;

    [SerializeField]
    private UnityEvent autostart;

    private List<UnitData> RaceList;

    private List<GameObject> cards;

    private UnitData pickedUnit;

    private void Awake()
    {
        cards = new List<GameObject>();

        RaceList = new List<UnitData>();
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

        foreach (var up in RaceList)
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

            cards[x].GetComponent<UnitCard>().Dd.AddListener(GetRace);
        }

        GetRace(cards[0].GetComponent<UnitCard>().UnitData);
    }

    public void GetRaceList()
    {
        RaceList = unitWithListRace.upgradeList;

        ClearCards();

        CreateCards();

        StartCoroutine(timer());
    }

    public void GetRace(UnitData unitData)
    {
        pickedUnit = unitData;

        _name.text = pickedUnit.Name;
    }

    public UnitData SetRace() {
        return pickedUnit;
    }

    public void AutoPick() {
        pickedUnit = RaceList[Random.Range(0, RaceList.Count)];
    }

    public IEnumerator timer() {

        var x = 30;
        while (x > 0)
        {
            yield return new WaitForSeconds(1);
            x--;
            Timer.text = "AutoPick after: " + x + " s";
        }

        x = 30;
        Timer.text = "AutoPick after: " + x + " s";

        autostart.Invoke();
    }
}
