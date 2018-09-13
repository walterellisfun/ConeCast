![ConeCast](https://github.com/walterellisfun/ConeCast/blob/master/ConeCast.gif)
# ConeCastAll extension method
A Unity3d ConeCastAll extension method for the Physics class.

Use this to find colliders within a cone-shaped volume.

It uses SphereCastAll, which is like a RayCast tube, but then it uses Vector3.Angle to filter out hitpoints according to a cone.

Using it is very similar to using SphereCastAll.

Variables:
  Vector3 origin,
  float maxRadius,
  Vector3 direction,
  float maxDistance,
  float coneAngle
