using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static Vector3 SetYToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }
}
