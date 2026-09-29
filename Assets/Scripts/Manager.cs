using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Monster monster;
    public TextMeshProUGUI monitorText;
    public int switches;
    // Update is called once per frame
    void Update()
    {
        if (switches >= 4)
        {
            SceneManager.LoadScene("Win");
        }
    }

    public void Hunting()
    {
        monster.hunting = true;
        monitorText.text = "Hunting";
        monitorText.color = Color.red;
    }

    public void StopHunting()
    {
        monster.hunting = false;
        monitorText.text = "Wandering";
        monitorText.color = Color.white;
    }
}
