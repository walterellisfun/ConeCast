using System;
using System.Collections.Generic;
using UnityEngine;

public static class Physics2DCones
{
    /// <summary>
    /// Similar to RayCast, but searches within a 2D cone (well, a pie-wedge I guess) instead of just along a straight line.
    /// This method works by using a <see cref="Physics2D.CircleCast"/> and then comparing the returned hit points to the edge of the cone.
    /// This method defines a cone by its length and the angle between its center and its edge.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="coneAngle">the angle between the center of the cone and the edge</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static RaycastHit2D ConeCast(Vector2 origin, Vector2 direction, float coneAngle, float maxDistance, 
                                        int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        float maxRadius = Mathf.Abs(Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance);
        return ConeCast(origin, maxRadius, direction, maxDistance, mask, minDepth, maxDepth);
    }

    /// <summary>
    /// Similar to RayCast, but searches within a 2D cone (well, a pie-wedge I guess) instead of just along a straight line.
    /// This method works by using a <see cref="Physics2D.CircleCast"/> and then comparing the returned hit points to the edge of the cone.
    /// This method defines a cone by its length and maximum radius.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="maxRadius">the largest radius of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static RaycastHit2D ConeCast(Vector2 origin, float maxRadius, Vector2 direction, float maxDistance, 
                                        int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        // if maxDistance is infinite then we can't figure out a max radius size. 
        // I don't think that physics2D.CircleCast would be happy with an infinite radius.
        if (float.IsInfinity(maxDistance)) throw new ArgumentException("Must be finite");
        float coneAngle = Mathf.Atan(maxRadius / maxDistance) * Mathf.Rad2Deg;

        // get everything inside our cone plus some and then pair the list down to the actual matches
        RaycastHit2D[] results = Physics2D.CircleCastAll(origin, maxRadius, direction, maxDistance, mask, minDepth, maxDepth);
        var numResults = ConsolidateConeMatches(results, results.Length, origin, direction, coneAngle);

        if (numResults > 0)
        {
            return results[0];
        }
        else
        {
            return default; // nothing was found
        }
    }

    /// <summary>
    /// Similar to ConeCast, but returns all colliders within the cone, not just the first. Hits are sorted from the closest to the origin to the farthest.
    /// This method works by using a <see cref="Physics2D.CircleCast"/> and then comparing the returned hit points to the edge of the cone.
    /// This method defines a cone by its length and the angle between its center and its edge.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="coneAngle">the angle between the center of the cone and the edge</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static RaycastHit2D[] ConeCastAll(Vector2 origin, Vector2 direction, float coneAngle, float maxDistance,
                                            int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        float maxRadius = Mathf.Abs(Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance);
        return ConeCastAll(origin, maxRadius, direction, maxDistance, mask, minDepth, maxDepth);
    }

    /// <summary>
    /// Similar to ConeCast, but returns all colliders within the cone, not just the first. Hits are sorted from the closest to the origin to the farthest.
    /// This method works by using a <see cref="Physics2D.CircleCast"/> and then comparing the returned hit points to the edge of the cone.
    /// This method defines a cone by its length and maximum radius.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="maxRadius">the largest radius of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static RaycastHit2D[] ConeCastAll(Vector2 origin, float maxRadius, Vector2 direction, float maxDistance,
                                             int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        // if maxDistance is infinite then we can't figure out a max radius size. 
        // I don't think that physics2D.CircleCast would be happy with an infinite radius.
        if (float.IsInfinity(maxDistance)) throw new ArgumentException("Must be finite");
        float coneAngle = Mathf.Atan(maxRadius / maxDistance) * Mathf.Rad2Deg;

        // get everything inside our cone plus some and then pair the list down to the actual matches
        RaycastHit2D[] results = Physics2D.CircleCastAll(origin, maxRadius, direction, maxDistance, mask, minDepth, maxDepth);
        var numResults = ConsolidateConeMatches(results, results.Length, origin, direction, coneAngle);

        // Resize the array to match our new results set
        Array.Resize(ref results, numResults);
        return results;
    }

    /// <summary>
    /// Similar to ConeCastAll, but uses a provided array (<paramref name="results"/>) instead of allocating memory for an array. 
    /// This allows you to allocate an array once and use it repeatedly in tight loops to prevent the need for garbage collection.
    /// This method defines a cone by its length and the angle between its center and its edge.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="coneAngle">the angle between the center of the cone and the edge</param>
    /// <param name="results">an array in which hits will be stored. Contents will be overwritten, but bounds will be respected. You must ensure it is big enough to get the hits you are looking for.</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static int ConeCastNonAlloc(Vector2 origin, Vector2 direction, float coneAngle, RaycastHit2D[] results, float maxDistance,
                                       int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        float maxRadius = Mathf.Abs(Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance);
        return ConeCastNonAlloc(origin, maxRadius, direction, results, maxDistance, mask, minDepth, maxDepth);
    }

    /// <summary>
    /// Similar to ConeCastAll, but uses a provided array (<paramref name="results"/>) instead of allocating memory for an array. 
    /// This allows you to allocate an array once and use it repeatedly in tight loops to prevent the need for garbage collection.
    /// This method defines a cone by its length and maximum radius.
    /// </summary>
    /// <param name="origin">origin of the cone</param>
    /// <param name="maxRadius">the largest radius of the cone</param>
    /// <param name="direction">direction of the center of the cone</param>
    /// <param name="results">an array in which hits will be stored. Contents will be overwritten, but bounds will be respected. You must ensure it is big enough to get the hits you are looking for.</param>
    /// <param name="maxDistance">the maximum distance to search. Must not be infinite.</param>
    /// <param name="mask">Only search on the layers given by the mask</param>
    /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
    /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
    /// <returns></returns>
    public static int ConeCastNonAlloc(Vector2 origin, float maxRadius, Vector2 direction, RaycastHit2D[] results, float maxDistance,
                                       int mask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
    {
        // if maxDistance is infinite then we can't figure out a max radius size. 
        // I don't think that physics2D.CircleCast would be happy with an infinite radius.
        if (float.IsInfinity(maxDistance)) throw new ArgumentException("Must be finite");
        float coneAngle = Mathf.Atan(maxRadius / maxDistance) * Mathf.Rad2Deg;

        // get everything inside our cone plus some and then pair the list down to the actual matches
        int numResults = Physics2D.CircleCastNonAlloc(origin, maxRadius, direction, results, maxDistance, mask, minDepth, maxDepth);
        numResults = ConsolidateConeMatches(results, numResults, origin, direction, coneAngle);

        return numResults;
    }




    /// <summary>
    /// This method does the hard work. It compares the angle to each hit point to the cone's angle and copies the ones that match to the beginning of the array.
    /// It preserves the nearest to farthest order or the items.
    /// Doing all this within the array allows this method to work on all three cast methodologies.
    /// </summary>
    /// <param name="results">Array containing all potential matches</param>
    /// <param name="numResults">The number of actual potential matches in <paramref name="results"/>, in case the array is a partially filled buffer.</param>
    /// <param name="origin">The tip of the cone</param>
    /// <param name="direction">the direction of the cone</param>
    /// <param name="coneAngle">the angle between the center of the cone and the edge of the cone.</param>
    /// <returns>The number of hits actually within the cone</returns>
    private static int ConsolidateConeMatches(RaycastHit2D[] results, int numResults, Vector2 origin, Vector2 direction, float coneAngle)
    {
        int j = 0;  // variable to keep track of the number of accepted results
        for (int i = 0; i < numResults; i++)
        {
            Vector2 hitPoint = results[i].point;
            Vector2 directionToHit = hitPoint - origin;
            float angleToHit = Vector2.Angle(direction, directionToHit);

            if (angleToHit < coneAngle)
            {
                results[j] = results[i];
                j++;
            }
        }
        return j;
    }
}