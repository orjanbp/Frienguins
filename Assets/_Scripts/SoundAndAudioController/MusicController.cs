using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioMixer masterMixer;

    public PenguinRescueController rescueController;

    public float pengu1vol;
    public float pengu2vol;
    public float pengu3vol;
    public float pengu4vol;
    public float pengu5vol;
    public float pengu6vol;

    private bool[] penguTrackStatus =
    {
        false,      // Track 1 -
        false,      // Track 2 -
        false,      // Track 3 -
        false,      // Track 4 -
        false,      // Track 5 -
        false       // Track 6 -
    };

    public void Start()
    {
        UpdatePenguTrack();
    }

    public void UpdatePenguTrack()
    {
        List<GameObject> penguins = new List<GameObject>(rescueController.penguinsFound);

        foreach (GameObject pengu in penguins)
        {
            if (!pengu) continue;

            int penguNumber = pengu.GetComponentInChildren<PenguinNumber>().number - 1;

            Debug.Log(penguNumber);
            penguTrackStatus[penguNumber] = true;
        }

        if (penguTrackStatus[0] == true)
        {
            PlayPengu1();
        }

        if (penguTrackStatus[1] == true)
        {
            PlayPengu2();
        }

        if (penguTrackStatus[2] == true)
        {
            PlayPengu3();
        }

        if (penguTrackStatus[3] == true)
        {
            PlayPengu4();
        }

        if (penguTrackStatus[4] == true)
        {
            PlayPengu5();
        }

        if (penguTrackStatus[5] == true)
        {
            PlayPengu6();
        }

    }

    public void MutePengu1()
    {
        pengu1vol = -80;
        masterMixer.SetFloat("Pengu1vol", pengu1vol);
    }

    public void MutePengu2()
    {
        pengu2vol = -80;
        masterMixer.SetFloat("Pengu2vol", pengu2vol);
    }

    public void MutePengu3()
    {
        pengu3vol = -80;
        masterMixer.SetFloat("Pengu3vol", pengu3vol);
    }

    public void MutePengu4()
    {
        pengu4vol = -80;
        masterMixer.SetFloat("Pengu4vol", pengu4vol);
    }

    public void MutePengu5()
    {
        pengu5vol = -80;
        masterMixer.SetFloat("Pengu5vol", pengu5vol);
    }

    public void MutePengu6()
    {
        pengu6vol = -80;
        masterMixer.SetFloat("Pengu6vol", pengu6vol);
    }

    public void PlayPengu1()
    {
        pengu1vol = 0;
        masterMixer.SetFloat("Pengu1vol", pengu1vol);
    }

    public void PlayPengu2()
    {
        pengu2vol = 0;
        masterMixer.SetFloat("Pengu2vol", pengu2vol);
    }

    public void PlayPengu3()
    {
        pengu3vol = 0;
        masterMixer.SetFloat("Pengu3vol", pengu3vol);
    }

    public void PlayPengu4()
    {
        pengu4vol = 0;
        masterMixer.SetFloat("Pengu4vol", pengu4vol);
    }

    public void PlayPengu5()
    {
        pengu5vol = 0;
        masterMixer.SetFloat("Pengu5vol", pengu5vol);
    }

    public void PlayPengu6()
    {
        pengu6vol = 0;
        masterMixer.SetFloat("Pengu6vol", pengu6vol);
    }
}
