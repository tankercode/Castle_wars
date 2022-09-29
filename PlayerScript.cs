using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private TMP_Dropdown _dropDown;

    private int _team = 0;

    private int _playerNumber = 0;

    private Color _playerColor;

    private int income = 0;

    private int incomeCoolDown = 0;

    private int playerGold = 0;

    private int playerWood = 0;

    private int playerCrystal = 0;

    private int playerRace = 0;

    public Color PlayerColor { get => _playerColor; set => _playerColor = value; }
    public int PlayerRace { get => playerRace; set => playerRace = value; }
    public int Income { get => income; set => income = value; }
    public int IncomeCoolDown { get => incomeCoolDown; set => incomeCoolDown = value; }

    private void Awake()
    {
        uiManager.playerGold.text = playerGold.ToString();

        uiManager.playerWood.text = playerWood.ToString();

        uiManager.playerCrystal.text = playerCrystal.ToString();

        StartCoroutine(goldIncome());
    }

    public void AddPlayerGold(int value) {
        playerGold += value;
        uiManager.playerGold.text = playerGold.ToString();
    }

    public void AddPlayerWood(int value)
    {
        playerWood += value;
        uiManager.playerWood.text = playerWood.ToString();
    }
    public void AddPlayerCrystal(int value)
    {
        playerCrystal += value;
        uiManager.playerCrystal.text = playerCrystal.ToString();
    }

    public int GetGold() {
        return playerGold;
    }

    public int GetWood()
    {
        return playerWood;
    }

    public int GetCrystal()
    {
        return playerCrystal;
    }

    public void SetGold(int gold)
    {
        playerGold = gold;
    }

    public void SetWood(int wood)
    {
        playerWood = wood;
    }

    public void SetCrystal(int crystals)
    {
        playerCrystal = crystals;
    }

    public IEnumerator goldIncome() {
        while (true) {
            yield return new WaitForSeconds(IncomeCoolDown);
            playerGold += Income;
            uiManager.playerGold.text = playerGold.ToString();
        }
    }

    public void SetTeam(int team) {
        _team = team;
    }

    public int GetTeam() {
        return _team;
    }

    public void SetPlayerNumber(int playerNumber) {
        _playerNumber = playerNumber;
    }

    public int GetPlayernumber() {
        return _playerNumber;
    }

    public void SetDropDownTeam() {
        _team = _dropDown.value;
    }
}
