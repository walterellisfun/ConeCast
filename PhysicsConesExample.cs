using UnityEngine;

public class ConeCastExample : MonoBehaviour {

    public float radius;
    public float depth;
    public float angle;

	void FixedUpdate () {
        
        RaycastHit[] coneHits = PhysicsCones.ConeCastAll(transform.position, transform.forward, angle, depth);

        for (int i = 0; i < coneHits.Length; i++)
        {
            //do something with collider information
            coneHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
        }
	}
}


public class ConeCastExample2 : MonoBehaviour
{
    public float radius;
    public float depth;
    public float angle;

    private RayCastHit[] hits = new RayCastHit[100];

    void FixedUpdate()
    {
        int numResults = PhysicsCones.ConeCastNonAlloc(transform.position, radius, transform.forward, hits, depth);

        for (int i = 0; i < hits.Length; i++)
        {
            //do something with collider information
            hits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
        }
    }
}