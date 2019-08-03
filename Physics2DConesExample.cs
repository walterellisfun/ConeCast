using UnityEngine;

public class ConeCast2DExample : MonoBehaviour {

    public float radius;
    public float depth;
    public float angle;

    private Physics physics;

	void FixedUpdate ()
    {
        RaycastHit2D[] coneHits = Physics2DCones.ConeCastAll(transform.position, transform.forward, angle, depth);

        for (int i = 0; i < coneHits.Length; i++)
        {
            //do something with collider information
            coneHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
        }
	}
}


public class ConeCast2DExample2 : MonoBehaviour
{
    public float radius;
    public float depth;
    public float angle;

    private RayCastHit2D[] hits = new RayCastHit2D[100];

    void FixedUpdate()
    {

        int numResults = Physics2DCones.ConeCastNonAlloc(transform.position, radius, transform.forward, hits, depth);

        for (int i = 0; i < numResults; i++)
        {
            //do something with collider information
            hits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
        }
    }
}
