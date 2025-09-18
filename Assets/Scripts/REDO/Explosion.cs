using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionFVX;
    [SerializeField] private GameObject AfterExplosionDecal;

    private float Radius = 10f;
    private float Force = 1000f;

    private bool explosionDone;

    public bool TestActive;

    private void Update()
    {
        if ( TestActive)
        {
            Explode();
        }
        
    }

    public void Explode()
    {
        if (explosionDone) return;
        explosionDone = true;

        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, Radius);

        for (int i = 0; i < overlappedColliders.Length; i++) 
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(Force, transform.position, Radius);

                Explosion explosion = rigidbody.GetComponent<Explosion>();
                if (explosion)
                {
                    if (Vector3.Distance(transform.position, rigidbody.position) < Radius / 2f)
                    {
                        explosion.Invoke("Explode",0.5f);
                    }
                }
                
            }
            
        }

        foreach (var item in overlappedColliders)
        {
            if (item.CompareTag("Grounded"))
            {
                Vector3 pos = item.ClosestPointOnBounds(transform.position);
                Instantiate(AfterExplosionDecal, pos, Quaternion.Euler(-90, Random.Range(-90, 90), 0));
            }
        }
        
        Destroy(gameObject);
        //Instantiate(ExplosionFVX, transform.position, Quaternion.identity);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.indianRed;
        Gizmos.DrawWireSphere(transform.position, Radius/2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
