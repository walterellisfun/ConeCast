# ConeCast
A ConeCast class for Unity3d that can easily be made an extension method for the Physics class.

It uses SphereCastAll, which is like a RayCast tube, but then it uses Vector3.Angle to filter out hitpoints according to a cone.

Using it is very similar to using SphereCastAll.

Variables:
  Vector3 origin,
  float maxRadius,
  Vector3 direction,
  float maxDistance,
  float coneAngle
