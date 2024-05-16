using UnityEngine;

public class NO_TREE_COLLISION : MonoBehaviour
{
    public Material[] materialsToDisableColliders;

    void Start()
    {
        DisableColliders();
    }

    void DisableColliders()
    {
        foreach (Material material in materialsToDisableColliders)
        {
            Renderer[] renderers = FindObjectsOfType<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                if (renderer.sharedMaterial == material)
                {
                    Collider collider = renderer.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }
    }
}