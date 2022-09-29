using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;

using BayatGames.SaveGameFree;

public class DragAndDropBuilding : MonoBehaviour
{

    [SerializeField]
    private cameraDrag cameraD;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private UIBuildingDataContainer _uiBuildingDataContainer;

    [SerializeField]
    private PlayerScript _player;

    [SerializeField]
    private Vector2Int gridSize;

    private MapData mapData;

    [SerializeField]
    private List<GameObject> unitsOnMap;

    [SerializeField]
    private List<MapUnitData> unitsInfo;

    private bool[,] grid;

    private MapUnitData _tmpMapUnitData;

    private GameObject _prefab;

    private Camera _camera;

    private bool _avalible;

    private int _X = 90;

    private int _Y = 90;

    private void Awake()
    {
        _camera = Camera.main;

        mapData = new MapData();

        mapData.grid = new bool[gridSize.x * gridSize.y];

        grid = new bool[gridSize.x, gridSize.y];

        unitsOnMap = new List<GameObject>();

        unitsInfo = new List<MapUnitData>();

        _tmpMapUnitData = new MapUnitData("", new Vector3(0,0,0), 0);
    }

    private void OnMouseDrag()
    {

        if (_prefab != null)
        {
            var _groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (_groundPlane.Raycast(_ray, out float position))
            {
                Vector3 _worldPosition = _ray.GetPoint(position);

                _X = Mathf.RoundToInt(_worldPosition.x);

                _Y = Mathf.RoundToInt(_worldPosition.z);

                _avalible = true;

                if (_player.GetTeam() == 0)
                {
                    if (_Y < 0 || _Y > 15 - _prefab.GetComponent<Unit>()._size.y) _avalible = false;
                }
                if (_player.GetTeam() == 1)
                {
                    if (_Y < 45 || _Y > gridSize.y - _prefab.GetComponent<Unit>()._size.y) _avalible = false;
                }

                if (_player.GetTeam() == -1)
                {
                    if (_Y < 0 || _Y > gridSize.y - _prefab.GetComponent<Unit>()._size.y) _avalible = false;
                }

                if (_X < 0 || _X > gridSize.x - _prefab.GetComponent<Unit>()._size.x) _avalible = false;


                if (_avalible && IsPlaceTaken(_X, _Y)) _avalible = false;

                _prefab.transform.position = new Vector3(_X, 0, _Y);

                _prefab.GetComponent<Unit>().SetAvalibleColor(_avalible);

            }
        }

    }

    private void OnMouseUp()
    {
        if (_avalible && _prefab != null)
        {
            PlaceBuild(_X, _Y, _player.GetTeam(), _player.GetPlayernumber(), _player.PlayerColor);
        }
    }

    private void OnDrawGizmos()
    {  
        if(grid != null)
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (grid[x,y] == true)
                    {
                        Gizmos.color = new Color(1, 0.6f, 0.4f, 0.6f);

                    }
                    else
                    {
                        Gizmos.color = new Color(0.7f, 0.9f, 0.5f, 0.6f);
                    }

                    Gizmos.DrawCube(transform.position + new Vector3(x, 0, y) - new Vector3(gridSize.x / 2 - 0.5f, 0, gridSize.y / 2 - 0.5f), new Vector3(1, 0.2f, 1));
                }
            }
    }

    public void ClearMap() {

        grid = new bool[gridSize.x, gridSize.y];

        Unit[] units = Object.FindObjectsOfType<Unit>();

        for (int x = 0; x < units.Length; x++) {
            Destroy(units[x].gameObject);
        }

        unitsOnMap.Clear();

        unitsInfo.Clear();
    }

    public bool IsPlaceTaken(int _placeX, int _placeY)
    {
        for (int x = 0; x < _prefab.GetComponent<Unit>()._size.x; x++)
        {
            for (int y = 0; y < _prefab.GetComponent<Unit>()._size.y; y++)
            {
                if (grid[_placeX + x, _placeY + y] == true) return true;
            }
        }

        return false;
    }

    public void PlaceBuild(int _placeX, int _placeY, int _team, int _playerNumber, Color _color, bool checkPlayer = true) 
    {
        for (int x = 0; x < _prefab.GetComponent<Unit>()._size.x; x++)
        {
            for (int y = 0; y < _prefab.GetComponent<Unit>()._size.y; y++)
            {
                grid[_placeX + x, _placeY + y] = true;
            }
        }

        _prefab.GetComponent<Unit>().Init(_team, _playerNumber, _color);

        unitsOnMap.Add(_prefab);

        _tmpMapUnitData._unitPosition = new Vector3(_placeX, 0, _placeY);
        _tmpMapUnitData._unitTeam = _team;

        unitsInfo.Add(new MapUnitData(_tmpMapUnitData._prefabPath, _tmpMapUnitData._unitPosition, _tmpMapUnitData._unitTeam));

        _prefab.transform.position = new Vector3(_placeX, 0, _placeY);

        var ud = _prefab.GetComponent<Unit>()._unitData;
        

        if(checkPlayer)
        if (ud.WoodCost > 0 || ud.CrystalCost > 0)
        {
                _player.AddPlayerGold(-_prefab.GetComponent<Unit>()._unitData.GoldCost);
                _player.AddPlayerWood(-_prefab.GetComponent<Unit>()._unitData.WoodCost);
                _player.AddPlayerCrystal(-_prefab.GetComponent<Unit>()._unitData.CrystalCost);

                _player.Income += _prefab.GetComponent<Unit>()._unitData.GoldCost / 100;
        }
        else {
                _player.AddPlayerGold(-_prefab.GetComponent<Unit>()._unitData.GoldCost);
                _player.AddPlayerWood(_prefab.GetComponent<Unit>()._unitData.GoldCost/2);

                _player.Income += _prefab.GetComponent<Unit>()._unitData.GoldCost / 100;
            }

        cameraD.dragEnabled = true;

        _prefab = null;
        
    }

    public void StartPlacing(UnitData unitData, bool checkplayer = true) 
    {

        if (_prefab != null) {
            Destroy(_prefab.gameObject);
        }

        if (checkplayer)
        {
            if (_player.GetGold() >= unitData.GoldCost && _player.GetWood() >= unitData.WoodCost && _player.GetCrystal() >= unitData.CrystalCost)
            {

                _prefab = Instantiate(unitData.Prefab);
            }
        }
        else {

            _prefab = Instantiate(unitData.Prefab);
        }
        
        
        
    }

    public void StartPlacing(UnitData unitData) {

        if (_prefab != null)
        {
            Destroy(_prefab.gameObject);
        }

        if (_player.GetGold() >= unitData.GoldCost && _player.GetWood() >= unitData.WoodCost && _player.GetCrystal() >= unitData.CrystalCost)
        {
            _prefab = Instantiate(unitData.Prefab);

            cameraD.dragEnabled = false;
        }
    }

    public void SellUnit() {

        var s = _uiBuildingDataContainer._currentUnit;

        if (s != null && s.GetComponent<Unit>()._unitData.Name != "castle")
        {

            _player.AddPlayerGold(_uiBuildingDataContainer._currentUnit.GetComponent<Unit>()._unitData.GoldCost / 2);

            _player.Income -= _uiBuildingDataContainer._currentUnit.GetComponent<Unit>()._unitData.GoldCost / 100;

            for (int x = 0; x < s.GetComponent<Unit>()._size.x; x++)
            {
                for (int y = 0; y < s.GetComponent<Unit>()._size.y; y++)
                {
                    grid[(int)s.transform.position.x + x, (int)s.transform.position.z + y] = false;
                }
            }

            Destroy(_uiBuildingDataContainer._currentUnit);

        }

    }

    public void switchAutoCreate() {
        _uiBuildingDataContainer._currentUnit.GetComponent<SkillUnitsCreator>().switchAutoCreate();
        
    }

}
