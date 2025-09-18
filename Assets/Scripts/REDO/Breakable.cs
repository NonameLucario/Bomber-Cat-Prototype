using UnityEngine;
using UnityEngine.Rendering;

public class Breakable : MonoBehaviour
{
    public GameObject PartPrefab;
    public Vector3 spawnOfPart; //spawnOffset

    public int hp = 10;

    public void Break(int dmg, Vector3 expPos, float expForce, float radius)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        if (PartPrefab)
        {
            GameObject obj = Instantiate(PartPrefab, transform.position + spawnOfPart, transform.rotation);
            obj.GetComponent<Rigidbody>().AddExplosionForce(expForce, expPos, radius);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellowGreen;
        Gizmos.DrawSphere(transform.position + spawnOfPart, 0.2f);
        
    }
}
