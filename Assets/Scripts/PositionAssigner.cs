﻿using UnityEngine;

public class PositionAssigner
{
    Transform playerTransform;
    float xHalf, yHalf, minDistance;
    Vector3 newPosition;

    public PositionAssigner(Transform playerTransform, float xHalf, float yHalf, float minDistance)
    {
        this.playerTransform = playerTransform;
        this.xHalf = xHalf;
        this.yHalf = yHalf;
        this.minDistance = minDistance;
    }

    public Vector3 GetPosition()
    {        
        while (true)
        {
            newPosition = new Vector3(Random.Range(-xHalf, xHalf), Random.Range(-yHalf, yHalf));
            if(Vector3.Distance(newPosition,playerTransform.position)>=minDistance)
                return newPosition;
        }
    }
}