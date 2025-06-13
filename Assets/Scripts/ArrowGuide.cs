using UnityEngine;

public class ArrowGuide : MonoBehaviour
{
    public string[] dechetTags = { "DechetJaune", "DechetVert", "DechetMarron" }; // Liste des tags
    public Transform target;
    public Transform player; // Transform du joueur (ou référence centrale)
    private Vector3 orientation = new Vector3(0, -90, 0);

    void Update()
    {
        FindClosestDechet();

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // reste à plat
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(orientation);
        }

    }

    void FindClosestDechet()
    {
        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (string tag in dechetTags)
        {
            GameObject[] dechets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject dechet in dechets)
            {
                float distance = Vector3.Distance(player.position, dechet.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = dechet;
                }
            }
        }

        if (closest != null)
        {
            target = closest.transform;
        }
        else
        {
            target = null; // aucun trouvé
        }
    }
}
