using System;
using UnityEngine;

using Random = UnityEngine.Random;

public enum Direction{ Up,Down,Left,Right};

public static class DirectionExtensions{
    public static Direction GetRandomDirection(){
        int num = Random.Range(0,4);
        return (Direction) num;
    }

    public static Direction Next(Direction dir){
        switch(dir){
            case Direction.Up :
                return Direction.Right;
            case Direction.Right :
                return Direction.Down;
            case Direction.Down :
                return Direction.Left;
            case Direction.Left :
                return Direction.Up;
            default:
             return Direction.Up;
        }
    }
}