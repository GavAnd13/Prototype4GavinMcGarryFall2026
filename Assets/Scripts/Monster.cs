using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public GameObject player;
    public float chaseSpeed = 1.5f;
    public float wanderSpeed = .5f;
    public float wanderRadius = 5f;
    public bool hunting = false;

    Vector3 randomPoint;
    bool atRandomPoint = true;

    // Update is called once per frame
    void Update()
    {
        if (hunting)
        {
            Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, transform.position.y, player.transform.position.z - transform.position.z);
            transform.LookAt(direction);
            direction.Normalize();
            float scaledChaseSpeed = chaseSpeed * Time.deltaTime;
            transform.position += direction * chaseSpeed;
        }
        else
        {
            if (atRandomPoint)
            {
                Debug.Log(randomPoint);
                Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
                randomPoint = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
            }
            else
            {
                Debug.Log(randomPoint);
                transform.LookAt(randomPoint);
                Vector3 unitRandomPoint = randomPoint;
                unitRandomPoint.Normalize();
                float scaledWanderSpeed = wanderSpeed * Time.deltaTime;
                transform.position += unitRandomPoint * scaledWanderSpeed;
                if (Vector3.Distance(transform.position, randomPoint) < .1f)
                {
                    atRandomPoint = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
