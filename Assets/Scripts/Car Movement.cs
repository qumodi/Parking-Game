using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Unity.VisualScripting;

public class CarMovement : MonoBehaviour
{
    private float _moveSpeed { get; set; } = 1f;
    private Car _car;

    private bool _collisionDetected;
    private bool _backRide;
    private bool _moving;

    void Awake()
    {
        _car = GetComponent<Car>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()//Vector2Int gridDestination
    {
        if (!_moving)
        {
            MoveOutParking();
            _moving = true;
        }
        // if(LevelGenerator.Instance.CheckCarCanExit(_car)){
        //    MoveOutParking();
        // }
        //StartCoroutine(MovingProcess(gridDestination));
    }

    private IEnumerator MovingProcess(Vector2 gridDestination)
    {
        Vector3 StartPosition = transform.position;
        Vector3 WorldDestination = new Vector3(gridDestination.x * LevelGenerator.SQUARE_SIZE, 0,
        gridDestination.y * LevelGenerator.SQUARE_SIZE);

        while (Math.Abs(Vector3.Distance(WorldDestination, transform.position)) > 0.1f)
        {
            if (_collisionDetected && !_backRide)
            {
                //HealthManager.Instance
                _backRide = true;
                _collisionDetected = false;
                StartCoroutine(MovingProcess(_car.GetRealGridPosition()));

                //_backRide = false;
                yield break;
            }
            transform.position = Vector3.Lerp(transform.position, WorldDestination, Time.deltaTime * _moveSpeed);

            // if(Math.Abs(Vector3.Distance(WorldDestination, transform.position)) > 0.3){
            //     transform.position = Vector3.Lerp(transform.position, WorldDestination, Time.deltaTime * _moveSpeed);
            // }else{
            //     transform.position = Vector3.Lerp(transform.position, WorldDestination, Time.deltaTime * _moveSpeed* 3);
            // }
            //Debug.Log(transform.position);
            yield return null;
        }
        if (_backRide == false)
        {
            transform.position = WorldDestination;

        }
        else
        {
            _moving = false;
            _backRide = false;
        }
    }

    private void MoveOutParking()
    {
        Vector2Int gridDestination;

        int extraSteps = 20;

        switch (_car.CarDirection)
        {
            case Direction.Up:
                gridDestination = new Vector2Int(_car.GridPosition.x, LevelGenerator.GRID_SIZE + extraSteps);
                break;
            case Direction.Down:
                gridDestination = new Vector2Int(_car.GridPosition.x, -extraSteps);
                break;
            case Direction.Right:
                gridDestination = new Vector2Int(LevelGenerator.GRID_SIZE + extraSteps, _car.GridPosition.y);
                break;
            case Direction.Left:
                gridDestination = new Vector2Int(-extraSteps, _car.GridPosition.y);
                break;
            default:
                gridDestination = new Vector2Int(-5, -5);
                break;
        }

        StartCoroutine(MovingProcess(gridDestination));
    }

    // private IEnumerator MoveTillCollision(){
    //     while(!CollisionDetected){

    //     }
    // }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DestructionBorder"))
        {
            LevelGenerator.Instance.ClearCar(_car);
            Destroy(this.gameObject);
            if (LevelGenerator.Instance.GetEmptyPoints().Count == LevelGenerator.GRID_SIZE * LevelGenerator.GRID_SIZE)
            {
                Debug.Log("Trying to call Win");
                LevelManager.Instance.Win();
            }
        }
        else if (_moving && !_backRide)
        {
            LevelManager.Instance.LoseHeart();
            _collisionDetected = true;
        }
    }

    private IEnumerator CallWin()
    {
        Debug.Log("Waiting for 0.5 seconds...");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Calling Win after wait");
        LevelManager.Instance.Win();
    }
}