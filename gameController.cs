using System.Collections;
using UnityEngine;

public class gameController : MonoBehaviour
{

    [Header("Параметры игрока")]

    [SerializeField]
    private PlayerScript player;

    [Space]

    [SerializeField]
    private int _playerTeam;

    [SerializeField]
    private int _playerNumber;

    [SerializeField]
    private Color _playerColor;

    [SerializeField]
    private int _playerIncome;

    [SerializeField]
    private int _playerIncomeCoolDown;

    [SerializeField]
    private int _playerGold;

    [SerializeField]
    private int _playerWood;

    [SerializeField]
    private int _playerCrystal;

    [SerializeField]
    private int _playerRace;

    [Space]

    [SerializeField]
    private DragAndDropBuilding builder;

    [SerializeField]
    private UnitData castle;

    [Header("Игровое поле")]

    [Space]

    [SerializeField]
    private GameObject game;

    [Header("UI")]

    [Space]

    [SerializeField]
    private UIManager uIManager;

    private void SetStartPlayerValues(int gold, int wood, int crystals, int team, int number, int race, int income, int incomeCD, Color color)
    {

        player.SetGold(0);

        player.SetWood(0);

        player.SetCrystal(0);

        player.AddPlayerGold(gold);

        player.AddPlayerWood(wood);

        player.AddPlayerCrystal(crystals);

        player.SetTeam(team);

        player.PlayerColor = color;

        player.SetPlayerNumber(number);

        player.PlayerRace = race;

        player.Income = income;

        player.IncomeCoolDown = incomeCD;
    }

    private void CreateTwoCastles()
    {

        builder.StartPlacing(castle, false);

        builder.PlaceBuild(6, 4, 0, 0, Color.red, false);


        builder.StartPlacing(castle, false);

        builder.PlaceBuild(6, 52, 1, 0, Color.blue, false);
    }

    public void SetPlayerRace(int race)
    {
        _playerRace = race;
    }

    public void StartNewGame()
    {

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {

        game.SetActive(true);

        yield return new WaitForSeconds(2f);

        CreateTwoCastles();

        SetStartPlayerValues(_playerGold, _playerWood, _playerCrystal, _playerTeam, _playerNumber, _playerRace, _playerIncome, _playerIncomeCoolDown, _playerColor);
    }

    public void StopGame()
    {
        StartCoroutine(SGame());
    }

    private IEnumerator SGame() {

        builder.ClearMap();

        yield return new WaitForSeconds(1);
        
        game.SetActive(false);
    }

    public void GameOver(int team) {
        if (_playerTeam == team)
        {
            uIManager.ShowPanelWithTag("lose");
        }
        else {
            uIManager.ShowPanelWithTag("win");
        }
    }

}
