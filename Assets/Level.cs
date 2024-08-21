using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int GoalTime { get; }
    public Vector3 ToyStartPosition { get; }
    public List<Vector3> AttractorPositions { get; }

    public Level(int goalTime, Vector3 toyStartPosition, List<Vector3> attractorPositions)
    {
        GoalTime = goalTime;
        ToyStartPosition = toyStartPosition;
        AttractorPositions = new(attractorPositions);
    }
}
