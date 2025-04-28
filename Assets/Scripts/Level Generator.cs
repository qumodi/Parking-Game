using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;


public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;
    [SerializeField] List<Car> CarPrefabs = new List<Car>();

    public const int GRID_SIZE = 6;
    public const int SQUARE_SIZE = 3;
    [SerializeField] private int CarsCount = 3;
    public Car[,] Grid = new Car[GRID_SIZE, GRID_SIZE];
    private Car[,] _savedGrid = new Car[GRID_SIZE, GRID_SIZE];
    private List<Car> _savedCars = new List<Car>();

    [SerializeField] private GameObject LevelClearer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Class Level Generator have Duplicate");
        }
    }

    void Start()
    {
        GenerateLevel();
        // for (int x = 0; x < GRID_SIZE; x++)
        // {
        //     for (int y = 0; y < GRID_SIZE; x++)
        //     {
        //         _savedGrid[x, y] = Grid[x, y];
        //     }
        // }

    }

    public void GenerateLevel()
    {
        for (int i = 0; i < GetCarsCountByLevel(GlobalData.Instance.LevelNumber); i++)
        {
            // if (i == 1)
            // {
            //     Debug.Log("d");
            // }
            List<Vector2Int> emptyPoints = GetEmptyPoints();
            if (emptyPoints.Count == 0)
            {
                Debug.LogError("Grid Have Zero Empty Points");
            }
            else
            {
                int prefabNumber = Random.Range(0, CarPrefabs.Count);

                int maxTries = 10;
                int currTry = 0;
                while (currTry < maxTries)
                {
                    Direction CarDirection = DirectionExtensions.GetRandomDirection();
                    Vector2Int RandomPoint = emptyPoints[Random.Range(0, emptyPoints.Count)];

                    if (CheckFreePlace(RandomPoint, CarDirection, CarPrefabs[prefabNumber].Length)
                    && CheckCarCanExit(RandomPoint, CarDirection, CarPrefabs[prefabNumber].Length))
                    {
                        AddCarToGrid(CarPrefabs[prefabNumber], RandomPoint, CarDirection);
                        break;
                    }
                    currTry++;
                }
                if (currTry == maxTries)
                {
                    Debug.Log("Failed Atemped to create Car");
                }

            }
        }
    }

    public void AddCarToGrid(Car carPrefab, Vector2Int position, Direction direction)
    {
        Car car = Instantiate(carPrefab);
        car.GridPosition = position;
        car.CarDirection = direction;

        car.ChangeRotation(direction);
        car.ChangePosition(car.GetRealGridPosition(), SQUARE_SIZE);

        _savedCars.Add(car);
        car.gameObject.SetActive(false);

        Car realCar = Instantiate(car);
        realCar.gameObject.SetActive(true);
        realCar.GridPosition = position;
        realCar.CarDirection = direction;

        realCar.ChangeRotation(direction);
        realCar.ChangePosition(car.GetRealGridPosition(), SQUARE_SIZE);
        WriteCarToGrid(realCar);

    }

    private void WriteCarToGrid(Car car)
    {
        Vector2Int StartPosition = car.GridPosition;
        switch (car.CarDirection)
        {
            case Direction.Up:
                for (int y = car.GridPosition.y; y < StartPosition.y + car.Length; y++)
                {
                    Grid[car.GridPosition.x, y] = car;
                }
                break;
            case Direction.Down:
                for (int y = car.GridPosition.y; y > StartPosition.y - car.Length; y--)
                {
                    Grid[car.GridPosition.x, y] = car;
                }
                break;
            case Direction.Right:
                for (int x = car.GridPosition.x; x < StartPosition.x + car.Length; x++)
                {
                    Grid[x, car.GridPosition.y] = car;
                }
                break;
            case Direction.Left:
                for (int x = car.GridPosition.x; x > StartPosition.x - car.Length; x--)
                {
                    Grid[x, car.GridPosition.y] = car;
                }
                break;
        }
    }

    private bool CheckFreePlace(Vector2Int position, Direction direction, int length)
    {
        if (length == 0)
        {
            return true;
        }
        if (position.x < 0 || position.y < 0 || position.x >= GRID_SIZE || position.y >= GRID_SIZE)
        {
            Debug.Log($"Index Error: x:{position.x}|y:{position.y}");
        }

        Vector2Int StartPosition = position;
        switch (direction)
        {
            case Direction.Up:
                if (position.y + length > GRID_SIZE)
                {
                    Debug.Log($"Cant Generate Car position: x:{position.x}|y:{position.y} \n Direction:{direction}");
                    return false;
                }
                for (int y = position.y; y < StartPosition.y + length; y++)
                {
                    if (Grid[position.x, y] != null)
                    {
                        return false;
                    }
                }
                return true;

            case Direction.Down:
                if (position.y - (length - 1) < 0)
                {
                    Debug.Log($"Cant Generate Car position: x:{position.x}|y:{position.y} \n Direction:{direction}");
                    return false;
                }
                for (int y = position.y; y > StartPosition.y - length; y--)
                {
                    if (Grid[position.x, y] != null)
                    {
                        return false;
                    }
                }
                return true;

            case Direction.Right:
                if (position.x + length > GRID_SIZE)
                {
                    Debug.Log($"Cant Generate Car position: x:{position.x}|y:{position.y} \n Direction:{direction}");
                    return false;
                }
                for (int x = position.x; x < StartPosition.x + length; x++)
                {
                    if (Grid[x, position.y] != null)
                    {
                        return false;
                    }
                }
                return true;

            case Direction.Left:
                if (position.x - (length - 1) < 0)
                {
                    Debug.Log($"Cant Generate Car position: x:{position.x}|y:{position.y} \n Direction:{direction}");
                    return false;
                }
                for (int x = position.x; x > StartPosition.x - length; x--)
                {
                    if (Grid[x, position.y] != null)
                    {
                        return false;
                    }
                }
                return true;

            default:
                UnityEngine.Debug.LogAssertion("Unexpected Error: Unknown Direction");
                return false;
        }
    }

    public bool CheckCarCanExit(Car car)
    {
        int checkLength;
        Vector2Int CheckStartPosition;
        switch (car.CarDirection)
        {
            case Direction.Up:
                checkLength = GRID_SIZE - car.GridPosition.y - car.Length;
                CheckStartPosition = new Vector2Int(car.GridPosition.x, car.GridPosition.y + car.Length);
                return CheckFreePlace(CheckStartPosition, car.CarDirection, checkLength);

            case Direction.Down:
                checkLength = car.GridPosition.y - (car.Length - 1);
                CheckStartPosition = new Vector2Int(car.GridPosition.x, car.GridPosition.y - car.Length);
                return CheckFreePlace(CheckStartPosition, car.CarDirection, checkLength);

            case Direction.Right:
                checkLength = GRID_SIZE - car.GridPosition.x - car.Length;
                CheckStartPosition = new Vector2Int(car.GridPosition.x + car.Length, car.GridPosition.y);
                return CheckFreePlace(CheckStartPosition, car.CarDirection, checkLength);

            case Direction.Left:
                checkLength = car.GridPosition.x - (car.Length - 1);
                CheckStartPosition = new Vector2Int(car.GridPosition.x - car.Length, car.GridPosition.y);
                return CheckFreePlace(CheckStartPosition, car.CarDirection, checkLength);

            default:
                Debug.LogAssertion("Unexpexted Error in function CheckCarCanExit. Unknown Direction");
                return false;
        }
    }

    public bool CheckCarCanExit(Vector2Int gridPosition, Direction carDirection, int carLength)
    {
        int checkLength;
        Vector2Int checkStartPosition;

        switch (carDirection)
        {
            case Direction.Up:
                checkLength = GRID_SIZE - gridPosition.y - carLength;
                checkStartPosition = new Vector2Int(gridPosition.x, gridPosition.y + carLength);
                return CheckFreePlace(checkStartPosition, carDirection, checkLength);

            case Direction.Down:
                checkLength = gridPosition.y - (carLength - 1);
                checkStartPosition = new Vector2Int(gridPosition.x, gridPosition.y - carLength);
                return CheckFreePlace(checkStartPosition, carDirection, checkLength);

            case Direction.Right:
                checkLength = GRID_SIZE - gridPosition.x - carLength;
                checkStartPosition = new Vector2Int(gridPosition.x + carLength, gridPosition.y);
                return CheckFreePlace(checkStartPosition, carDirection, checkLength);

            case Direction.Left:
                checkLength = gridPosition.x - (carLength - 1);
                checkStartPosition = new Vector2Int(gridPosition.x - carLength, gridPosition.y);
                return CheckFreePlace(checkStartPosition, carDirection, checkLength);

            default:
                Debug.LogAssertion("Unexpected error in function CheckCarCanExit. Unknown Direction.");
                return false;
        }
    }


    public List<Vector2Int> GetEmptyPoints()
    {
        int pointsCount = 0;

        List<Vector2Int> emptyPoints = new List<Vector2Int>();
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                if (Grid[x, y] == null)
                {
                    pointsCount++;
                    emptyPoints.Add(new Vector2Int(x, y));
                }
            }
        }

        return emptyPoints;
    }

    public void ClearCar(Car car)
    {
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                if (Grid[x, y] == car)
                {
                    Grid[x, y] = null;
                }
            }
        }
    }

    public void Restart()
    {

        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                if (Grid[x, y] != null)
                {
                    //ClearCar(Grid[x,y]);
                    Destroy(Grid[x,y]?.gameObject);
                
                    //Grid[x,y] = null;

                }
            }
        }

        for(int i = 0; i < _savedCars.Count; i++)
        {

            Car car = Instantiate(_savedCars[i]);
            car.GridPosition = _savedCars[i].GridPosition;
            car.CarDirection = _savedCars[i].CarDirection;

            car.ChangeRotation(_savedCars[i].CarDirection);
            car.ChangePosition(car.GetRealGridPosition(), SQUARE_SIZE);
            WriteCarToGrid(car);
            car.gameObject.SetActive(true);
        }

        
        Time.timeScale = 1;
    }

    public void GenerateNewLevel(){
        _savedCars = new List<Car>();
        Grid = new Car[GRID_SIZE, GRID_SIZE];
        StartCoroutine(ClearLevel());

    }

    IEnumerator ClearLevel(){
        LevelClearer.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        LevelClearer.SetActive(false);

        GlobalData.Instance.LevelNumber++;
        LevelManager.Instance.UpdateLevelNumber();
        GenerateLevel();

    }

    int GetCarsCountByLevel(int level){
        if(level < 5){
            return 5;
        }else if(level < 10){
            return 10;
        }else{
            return 16;
        }
    }
}