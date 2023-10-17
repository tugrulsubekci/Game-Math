### Freya's Assignments
---
<details>
   <summary>Bouncing lasers</summary>
   
```C#
public static List<Vector3> BounceLaser(float maxLaserDistance, Vector3 rayStartPoint, Vector3 rayStartDirection, bool useUnityLibrary = false, bool drawGizmos = true)
{
   var totalRayDistance = maxLaserDistance;
   var raycastSucceeded = true;
   var rayOrigin = rayStartPoint;
   var rayDirection = rayStartDirection;
   
   var hitPoints = new List<Vector3>(){rayOrigin};
   
   while(totalRayDistance > 0f && raycastSucceeded)
   {
       raycastSucceeded = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, totalRayDistance, Physics.AllLayers);
   
       var surfaceNormal = hitInfo.normal;
   
       hitPoints.Add(raycastSucceeded ? hitInfo.point : (rayOrigin + rayDirection * totalRayDistance));
   
       rayOrigin = hitInfo.point;
       rayDirection = useUnityLibrary ? Vector3.Reflect(rayDirection, surfaceNormal) : ReflectVector(rayDirection, surfaceNormal);
   
       totalRayDistance -= hitInfo.distance;
   }
   
   return hitPoints;
}
```
</details>

<details><summary>Transformation (local to world, world to local)</summary>
   
```C#
public static Vector3 LocalToWorld(Vector3 localPoint, Transform transform)
{
    var position = transform.position;

    // FIRST WAY ---------------------------------------------------------------
    // var right = (Vector3)transform.worldToLocalMatrix.GetColumn(0).normalized;
    // var up = (Vector3)transform.worldToLocalMatrix.GetColumn(1).normalized;
    // var forward = (Vector3)transform.worldToLocalMatrix.GetColumn(2).normalized;
    // var dotRight = Vector3.Dot(localPoint, right);
    // var dotUp = Vector3.Dot(localPoint, up);
    // var dotForward = Vector3.Dot(localPoint, forward);
    // position += new Vector3(dotRight, dotUp, dotForward);

    // SECOND WAY ---------------------------------------------------------------
    position += localPoint.x * transform.right;
    position += localPoint.y * transform.up;
    position += localPoint.z * transform.forward;

    return position;
}

public static Vector3 WorldToLocal(Vector3 worldPoint, Transform transform)
{
    var deltaVector = worldPoint - transform.position;
    return new Vector3(Vector3.Dot(deltaVector, transform.right), Vector3.Dot(deltaVector, transform.up), Vector3.Dot(deltaVector, transform.forward));
}
```

https://github.com/tugrulsubekci/unity-workspace/assets/104980354/b69fd833-5dba-4466-8369-c066e20b2e57
</details>

<details><summary>Turret placement according to the main camera</summary>
   
```C#
private Camera MainCamera => SceneView.lastActiveSceneView.camera;

public void PlaceTurret()
{
    bool raycastSucceeded = Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

    if(raycastSucceeded)
        transform.SetPositionAndRotation(hitInfo.point, Quaternion.LookRotation(Vector3.Cross(MainCamera.transform.right, hitInfo.normal), hitInfo.normal));
}
```

</details>
<details><summary>Turret targeting with constant rotation speed (frame-rate independent)</summary>

```C#
private void LookAtTarget(bool isTargetInside)
{
    var fromRotation = turretHead.transform.rotation;
    var toRotation = isTargetInside ? Quaternion.LookRotation(trigger.transform.position - turretHeadPosition, transform.up) : defaultOrientationOfHead;
    var deltaRotation = Quaternion.Angle(fromRotation, toRotation);

    turretHead.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, headRotationSpeed / deltaRotation * Time.deltaTime * 10.0f);
}
```
</details>

<details><summary>Cheese Wedge Sensor</summary>
   
```C#
 public override bool Check(Vector3 triggerPosition)
 {
     var dirToTargetWorld = triggerPosition - transformOrigin.position;
     var dirToTargetLocal = transformOrigin.InverseTransformVector(dirToTargetWorld);

     // height check
     if(dirToTargetLocal.y < 0 || dirToTargetLocal.y > height)
         return false;

     // cylindirical radius check
     var ignoreYAxis = new Vector3(dirToTargetLocal.x, 0, dirToTargetLocal.z);

     if(ignoreYAxis.magnitude > maxRadius || ignoreYAxis.magnitude < minRadius)
         return false;

     // angle check
     if(Vector3.Angle(transformOrigin.TransformVector(ignoreYAxis.normalized), transformOrigin.forward) > angle / 2.0f)
         return false;

     return true;
 }
```

https://github.com/tugrulsubekci/unity-workspace/assets/104980354/0bc0764e-f168-4174-94d8-237ee30a1b56
</details>

<details><summary>Spherical Sensor</summary>
   
```C#
public override bool Check(Vector3 triggerPosition)
{
    var distanceFromTurret = Vector3.Distance(triggerPosition, transformOrigin.position);
    // radius check
    if(distanceFromTurret > maxRadius || distanceFromTurret < minRadius)
        return false;

    return true;
}
```

https://github.com/tugrulsubekci/unity-workspace/assets/104980354/6fc8ba21-a22e-4a66-9f28-96987ceb86bb

</details>

<details><summary>Cone Sensor</summary>
   
```C#
public override bool Check(Vector3 triggerPosition)
{
    // radius check
    var distanceFromTurret = Vector3.Distance(triggerPosition, coneStartPosition);

    if(distanceFromTurret > maxRadius || distanceFromTurret < minRadius)
        return false;

    // angle check
    var dirToTargetWorld = triggerPosition - coneStartPosition;
    var dirToTargetLocal = transformOrigin.InverseTransformVector(dirToTargetWorld);

    if(Vector3.Angle(transformOrigin.InverseTransformVector(transformOrigin.forward), dirToTargetLocal) > angle / 2.0f)
        return false;
        
    return true;
}
```

https://github.com/tugrulsubekci/unity-workspace/assets/104980354/36cad9ca-2378-4288-ad70-06efb5821e22
</details>

<details><summary>Clock</summary>
   
```C#
private void OnDrawGizmos()
{
  Handles.Disc(transform.rotation, transform.position, transform.forward, 1.0f, false, 0f);

  for (int i = 0; i < TotalHours; i++)
      Handles.DrawLine
      (
          MathUtils.AngleToDirection(MathUtils.roundAngle / TotalHours * i) * 0.9f + transform.position,
          MathUtils.AngleToDirection(MathUtils.roundAngle / TotalHours * i) * 1.1f + transform.position,
          hourTickness
      );


  for (int i = 0; i < MathUtils.totalMinutes; i++)
      Handles.DrawLine
      (
          MathUtils.AngleToDirection(MathUtils.roundAngle / MathUtils.totalMinutes * i) * 0.95f + transform.position,
          MathUtils.AngleToDirection(MathUtils.roundAngle / MathUtils.totalMinutes * i) * 1.05f + transform.position,
          minuteTickness
      );

      
  var hourAngle = MathUtils.roundAngle / TotalHours * Hour;
  var minuteAngle = MathUtils.roundAngle / MathUtils.totalMinutes * Minute;
  var secondAngle = MathUtils.roundAngle / MathUtils.totalSeconds * Second;

  var hourDirection = MathUtils.AngleToDirection(hourAngle);
  var minuteDirection = MathUtils.AngleToDirection(minuteAngle);
  var secondDirection = MathUtils.AngleToDirection(secondAngle);

  var hourPosition = hourDirection * hourLength + transform.position;
  var minutePosition = minuteDirection * minuteLength + transform.position;
  var secondPosition = secondDirection * secondLength + transform.position;

  Handles.DrawLine(transform.position, hourPosition, hourTickness);
  Handles.DrawLine(transform.position, minutePosition, minuteTickness);

  Handles.color = Color.red;
  Handles.DrawLine(transform.position, secondPosition, secondTickness);
}
```

https://github.com/tugrulsubekci/unity-workspace/assets/104980354/8793071b-154e-4c27-96e5-0fe24dada582

</details>
