using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public bool isInside = false;
    public bool meetAPenguin = false;
    public bool isMusicPlaying = false;
    
    // int trackNumber = penguin.GetComponent<PenguinTrackNumber>().number;

    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<MusicController>().MutePengu1();
        GetComponent<MusicController>().MutePengu2();
        GetComponent<MusicController>().MutePengu3();
        GetComponent<MusicController>().MutePengu4();
        GetComponent<MusicController>().MutePengu5();
        GetComponent<MusicController>().MutePengu6();

    }

    // Update is called once per frame
    void Update()
    {
        if (isInside == true)
        {
            if (isMusicPlaying == false)
            {
                GetComponent<MusicController>().UpdatePenguTrack();
                //skaff pingvin nummere som er der og skru opp volum på de    
                GetComponent<AudioController>().Play("Innevind");
                GetComponent<AudioController>().Play("Flame");

                GetComponent<AudioController>().Stop("Utevind");
                GetComponent<AudioController>().Stop("Outdoor Intro");
                isMusicPlaying = true;
            }
            else
            {
                return;
            }
            
            
        }
        else
        {
            if (isMusicPlaying == false)
            {
                GetComponent<MusicController>().MutePengu1();
                GetComponent<MusicController>().MutePengu2();
                GetComponent<MusicController>().MutePengu3();
                GetComponent<MusicController>().MutePengu4();
                GetComponent<MusicController>().MutePengu5();
                GetComponent<MusicController>().MutePengu6();
                GetComponent<AudioController>().Play("Utevind");
                //GetComponent<AudioController>().Play("Outdoor Intro"); Må se på om får til ¨fungere, men det høres greit ut uten denne delen også
                GetComponent<AudioController>().Play("Outdoor Theme");

                GetComponent<AudioController>().Stop("Innevind");
                GetComponent<AudioController>().Stop("Flame");
                isMusicPlaying = true;
            }
            else
            {
                return;
            }
                
            
            
        }

        //Finn nummeret på den pingvinen du møter og sett opp volummet på den
    }

    public void InnOutTransition()
    {
        isMusicPlaying = false;
        if (isInside == true)
        {
            isInside = false;
        }
        else
        {
            isInside = true;
        }
    }
}


