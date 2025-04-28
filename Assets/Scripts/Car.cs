using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

using Vector2 = UnityEngine.Vector2;

public class Car : MonoBehaviour
{
    [SerializeField] private CarSize _carsize;
    public int Length { get { return GetLengthByCarSize(_carsize); } }
    public Vector2Int GridPosition { get; set; } = new Vector2Int(0, 0);
    public Direction CarDirection { get; set; }

    public UnityEvent OnClick = new UnityEvent();

    Car() { }
    public Car(CarSize carSize, Vector2Int position, Direction carDirection)
    {
        //this.Length = GetLengthByCarSize(carSize);
        this._carsize = carSize;
        this.GridPosition = position;
        this.CarDirection = carDirection;
    }
    public Car(Car car)
    {
        //this.Length = GetLengthByCarSize(carSize);
        this._carsize = car._carsize;
        this.GridPosition = car.GridPosition;
        this.CarDirection = car.CarDirection;
    }
    void Awake()
    {
        OnClick.AddListener(Move);
        //OnClick.AddListener(() => LevelGenerator.Instance.ClearCar(this));
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move()
    {
        Debug.Log("Car is moving");
        GetComponent<CarMovement>().Move();//new Vector2Int(0,1)
    }

    public void ChangeRotation(Direction direction)
    {
        const int up = 0;
        const int right = 90;
        const int down = 180;
        const int left = 270;

        switch (direction)
        {
            case Direction.Up:
                transform.eulerAngles = new Vector3(transform.rotation.x, up, transform.rotation.z);
                break;
            case Direction.Right:
                transform.eulerAngles = new Vector3(transform.rotation.x, right, transform.rotation.z);
                break;
            case Direction.Down:
                transform.eulerAngles = new Vector3(transform.rotation.x, down, transform.rotation.z);
                break;
            case Direction.Left:
                transform.eulerAngles = new Vector3(transform.rotation.x, left, transform.rotation.z);
                break;
        }
    }

    public void Rotate()
    {
        CarDirection = DirectionExtensions.Next(CarDirection);
        ChangeRotation(CarDirection);
    }

    public void ChangePosition(Vector2 gridPosition, int squareSize)
    {

        Vector3 newPosition = new Vector3(gridPosition.x * squareSize, transform.position.y, gridPosition.y * squareSize);
        transform.position = newPosition;
    }

    private int GetLengthByCarSize(CarSize carsize)
    {
        const int middle = 2;
        const int large = 4;

        switch (carsize)
        {
            case CarSize.Middle:
                return middle;

            case CarSize.Large:
                return large;

            default:
                Debug.LogAssertion("Unexpected Error: Unknown CarSize");
                return 2;
        }
    }

    public Vector2 GetRealGridPosition()
    {
        switch (CarDirection)
        {
            case Direction.Up:
                return GridPosition + new Vector2(0, 0.5f + Length / 4); ;
            case Direction.Down:
                return GridPosition - new Vector2(0, 0.5f + Length / 4); ;
            case Direction.Right:
                return GridPosition + new Vector2(0.5f + Length / 4, 0); ;
            case Direction.Left:
                return GridPosition - new Vector2(0.5f + Length / 4, 0); ;
            default:
                return new Vector2(-5f,-5f);
        }
    }
}
