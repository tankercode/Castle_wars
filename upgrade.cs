using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class upgrade : MonoBehaviour
{
    /*
        - выбераем юнита из списка
        - нажимаем кнопку апгрейд

        что нужно для этого
        - список доступных для апгрейда зданий
        - выбранный из списка юнит
        - достаточное для апа количество средств
        
        
     */
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject fieldForCards;

    [SerializeField]
    private DragAndDropBuilding dragAndDrop;

    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    private UIBuildingDataContainer _uIBuildingDataContainer;

    [SerializeField]
    private GameObject card;

    private List<UnitData> upgradeList;

    private List<GameObject> cards;

    private GameObject upgradebleUnit;

    private UnitData pickedUnit;

    private void Awake()
    {
        cards = new List<GameObject>();

        upgradeList = new List<UnitData>();
    }

    private void ClearCards() {

        if (cards.Count > 0) {
            
            foreach (var card in cards) {
                Destroy(card.gameObject);
            }

            cards.Clear();
        }
    }

    private void CreateCards() {
        
        foreach (var up in upgradeList)
        {

            var cardScript = card.GetComponent<UnitCard>();

            cardScript.UnitData = up;

            cards.Add(Instantiate(card));
        }

        fieldForCards.GetComponent<RectTransform>().sizeDelta = new Vector2(cards.Count * 130, 200);

        for(int x = 0; x < cards.Count; x++) {

            cards[x].transform.SetParent(fieldForCards.transform);

            cards[x].transform.localPosition = new Vector3(x * 130 + offset.x, offset.y, 0);

            cards[x].GetComponent<UnitCard>().Dd.AddListener(GetUpgrade);
        }
    }

    public void GetUpgradeList() {

        upgradebleUnit = _uIBuildingDataContainer._currentUnit;

        upgradeList = _uIBuildingDataContainer._currentUnit.GetComponent<Unit>().upgradeList;

        ClearCards();

        CreateCards();
    }

    public void GetUpgrade(UnitData unitData) {
        pickedUnit = unitData;

        _name.text = pickedUnit.Name;
    }

    public void UpgradeUnit()
    {

        if (upgradebleUnit != null && pickedUnit != null)
        {

            Vector2Int pos = new Vector2Int((int)upgradebleUnit.transform.position.x, (int)upgradebleUnit.transform.position.z);

            var data = upgradebleUnit.GetComponent<Unit>();

            int team = data._unitState._team;

            int number = data._unitState._playerNumber;

            Color color = data._unitState._color;

            dragAndDrop.SellUnit();

            dragAndDrop.StartPlacing(pickedUnit);

            dragAndDrop.PlaceBuild(pos.x, pos.y, team, number, color, true);
        }

    }

}
