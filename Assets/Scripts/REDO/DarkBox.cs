
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class DarkBox : MonoBehaviour
{
    private float Radius = 7f;
    private float Force = 700f;

    private bool explosionDone;

    public GameObject smokeVFX;

    public void Use()
    {
        //Instantiate(smokeVFX, transform);
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
                        explosion.Invoke("Explode", 0.5f);

                        
                    }
                }
                if (rigidbody.TryGetComponent(out Breakable breakble)) breakble.Break(11, transform.position, Force/2f, Radius/2f);

            }

        }

        foreach (var item in overlappedColliders)
        {
            if (item.CompareTag("Grounded"))
            {
                Vector3 pos = item.ClosestPointOnBounds(transform.position);
                //Instantiate(AfterExplosionDecal, pos, Quaternion.Euler(-90, Random.Range(-90, 90), 0));
            }
        }

        Destroy(gameObject);
        //Instantiate(ExplosionFVX, transform.position, Quaternion.identity);


    }

    private void OnCollisionEnter(Collision collision)
    {
        MessageManager.instance.SendMsg(gameObject.name, $"{collision.gameObject.name}.layer = {collision.gameObject.layer}");
        switch (collision.gameObject.layer)
        {
            case 0: //default
                Explode();
                break;

            case 6: //grounded
                Explode();
                break;

            case 7: //interact
                Explode();
                break;

            default:
                break;
        }
    }
}
