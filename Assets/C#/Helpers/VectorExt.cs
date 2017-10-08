using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public static class VectorExt {
    private const double DegToRad = Mathf.Deg2Rad;
    private const double RadToDeg = Mathf.Rad2Deg;

    public static Vector3 Rotate(this Vector3 v, double degrees) {
        return v.RotateRadians(degrees * DegToRad);
    }

    // Assuming a horizontal rotation
    public static Vector3 RotateRadians(this Vector3 v, double radians) {
        var ca = Math.Cos(radians);
        var sa = Math.Sin(radians);
        return new Vector3((float)(ca * v.x - sa * v.y), 0, (float)(sa * v.x + ca * v.y));
    }
}
