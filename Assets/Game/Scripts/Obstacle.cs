using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleID
    {
        Barrier,
        Space,
        Finish,
        None
    }

    public ObstacleID currentObstacleID = ObstacleID.None;


}
