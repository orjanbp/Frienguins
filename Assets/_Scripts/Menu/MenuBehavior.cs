using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    public Transform pengPoint;
    public Button menuButton;

    public PenguinPreBaker prebaked;

    // Start is called before the first frame update
    void Start()
    {
        // Get sceneless prebake of penguins, so we can get our main character
        prebaked = GameObject.FindGameObjectWithTag("_PenguinPrebake").GetComponent<PenguinPreBaker>();

        GameObject player = Instantiate(prebaked.playerPenguin, Vector3.zero, new Quaternion());
        player.transform.parent = pengPoint;
        player.transform.localPosition = Vector3.zero;

        menuButton.onClick.AddListener(() => LoadOutdoors());
    }

    void LoadOutdoors()
    {
        SceneManager.LoadScene("Outside", LoadSceneMode.Single);
    }
}
