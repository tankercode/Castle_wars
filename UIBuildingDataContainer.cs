using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBuildingDataContainer : MonoBehaviour
{
    [SerializeField]
    public GameObject _currentUnit;

    // BUILDING
    [SerializeField]
    private TextMeshProUGUI _unitName;

    [SerializeField]
    private Image _unitIcon;

    [SerializeField]
    private Slider _unitHpBar;

    [SerializeField]
    private TextMeshProUGUI _unitHpText;


    // Skill 
    [SerializeField]
    public GameObject _skillPanel;

    [SerializeField]
    private Image _skillIcon;

    [SerializeField]
    private Slider _skillStatus;

    [SerializeField]
    private TextMeshProUGUI _skillStatusText;

    [SerializeField]
    private TextMeshProUGUI _skillName;

    [SerializeField]
    private TextMeshProUGUI _skillDescription;

    private UnitData currentUnitData;

    private UnitState currentUnitState;

    private SkillUnitsCreator currentSkillUnitCreator;

    public void GetData(GameObject currentUnit)
    {
        _currentUnit = currentUnit;

        var unitScript = _currentUnit.GetComponent<Unit>();

        currentUnitData = unitScript._unitData;

        currentUnitState = unitScript._unitState;

        currentSkillUnitCreator = _currentUnit.GetComponent<SkillUnitsCreator>();

        if (currentUnitData != null)
        {
            _unitIcon.sprite = currentUnitData.Icon;

            _unitHpBar.maxValue = currentUnitData.MaxHp;

            _unitName.text = currentUnitData.Name;

            if (currentSkillUnitCreator != null)
            {

                ShowSkillPanel();

                _skillIcon.sprite = currentSkillUnitCreator._skillData.CreatebleObject.GetComponent<Unit>()._unitData.Icon;

                _skillName.text = currentSkillUnitCreator._skillData.Name;

                _skillDescription.text = currentSkillUnitCreator._skillData.CreatebleObject.GetComponent<Unit>()._unitData.Descriprion;

                _skillStatus.maxValue = currentSkillUnitCreator._skillData.CoolDown;

            }
            else
            {
                HideSkillPanel();
            }
        }
    }

    private void Update()
    {
        if (_currentUnit != null)
        {

            _unitHpBar.value = currentUnitState._currentHp;

            _unitHpText.text = currentUnitData.MaxHp + "/" + _unitHpBar.value.ToString("0.0"); ;

            if (currentSkillUnitCreator != null)
            {
                _skillStatus.value = currentSkillUnitCreator._skillState._currentTime;
                _skillStatusText.text = currentSkillUnitCreator._skillData.CoolDown + "/" + _skillStatus.value.ToString("0.0"); ;
            }

        }



    }

    public void HideSkillPanel() {
        _skillPanel.transform.DOLocalMoveY(-200, 0);
    }

    public void ShowSkillPanel()
    {
        _skillPanel.transform.DOLocalMoveY(0, 0);
    }
}
