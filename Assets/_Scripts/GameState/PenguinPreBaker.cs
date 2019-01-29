using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The entire scene this thing is in literally just exists to pre-bake the 
 * randomization of the penguins, before the penguins are alotted into
 * game later
 * */
public class PenguinPreBaker : MonoBehaviour
{
    public List<GameObject> penguins = new List<GameObject>();
    public GameObject playerPenguin;
    
    public static PenguinPreBaker instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int pickPlayer = Random.Range(0, penguins.Count - 1);

        penguins = Shuffle(penguins);

        playerPenguin = penguins[pickPlayer];
        penguins.RemoveAt(pickPlayer);

        SceneManager.LoadScene("IntroLogo");
    }

    public List<GameObject> Shuffle(List<GameObject> penguins)
    {
        int i = penguins.Count;

        while (i > 1)
        {
            i--;
            int n = Random.Range(i, penguins.Count);
            GameObject temp = penguins[n];

            penguins[n] = penguins[i];
            penguins[i] = temp;
        }

        return penguins;
    }
}
