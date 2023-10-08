# Unity Workspace
---
### Freya's Assignments
* Bouncing lasers
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
* Transformation (local to world, world to local)
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
* Turret placement according to the main camera
```C#
private Camera MainCamera => SceneView.lastActiveSceneView.camera;

public void PlaceTurret()
{
    bool raycastSucceeded = Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

    if(raycastSucceeded)
        transform.SetPositionAndRotation(hitInfo.point, Quaternion.LookRotation(Vector3.Cross(MainCamera.transform.right, hitInfo.normal), hitInfo.normal));
}
```
![](unity-workspace-01.mp4)
* Turret targeting with constant rotation speed (frame-rate independent)
```C#
private void LookAtTarget(bool isTargetInside)
{
    var fromRotation = turretHead.transform.rotation;
    var toRotation = isTargetInside ? Quaternion.LookRotation(trigger.transform.position - turretHeadPosition, transform.up) : defaultOrientationOfHead;
    var deltaRotation = Quaternion.Angle(fromRotation, toRotation);

    turretHead.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, headRotationSpeed / deltaRotation * Time.deltaTime * 10.0f);
}
```
* __Turret Sensors__
* Cheese Wedge
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
* Spherical
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
* Cone
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
