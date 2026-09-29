using UnityEngine;
using UnityEngine.SceneManagement;

public class Switch : MonoBehaviour
{
    public Material glowGreen;
    public Manager manager;
    bool pressed;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!pressed)
            {
                pressed = true;
                GetComponent<Light>().enabled = true;
                GetComponent<MeshRenderer>().materials = new Material[] { glowGreen };
                manager.switches++;
            }
        }
    }
}
