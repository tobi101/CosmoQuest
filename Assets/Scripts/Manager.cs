using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject[] positions;
    public GameObject[] figures;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int rand = Random.Range(0, i + 1);

            GameObject tempObject = positions[rand];
            positions[rand] = positions[i];
            positions[i] = tempObject;
        }

        for (int i = 0; i < figures.Length; i++)
        {
            figures[i].transform.position = positions[i].transform.position;
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
