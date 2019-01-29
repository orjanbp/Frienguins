using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour
{
    public AudioSource talking;

    public AudioClip[] wizardTalk;
    public AudioClip[] motherTalk;
    public AudioClip[] kidTalk;
    public AudioClip[] teenTalk;
    public AudioClip[] chefTalk;
    public AudioClip[] pyseTalk;

    public void WizardTalk()
    {
        talking.clip = wizardTalk[Random.Range(0, wizardTalk.Length)];
        talking.PlayOneShot (talking.clip);
    }

    public void MotherTalk()
    {
        talking.clip = motherTalk[Random.Range(0, motherTalk.Length)];
        talking.PlayOneShot(talking.clip);
    }

    public void KidTalk()
    {
        talking.clip = kidTalk[Random.Range(0, kidTalk.Length)];
        talking.PlayOneShot(talking.clip);
    }

    public void TeenTalk()
    {
        talking.clip = teenTalk[Random.Range(0, teenTalk.Length)];
        talking.PlayOneShot(talking.clip);
    }

    public void ChefTalk()
    {
        talking.clip = chefTalk[Random.Range(0, chefTalk.Length)];
        talking.PlayOneShot(talking.clip);
    }

    public void PyseTalk()
    {
        talking.clip = pyseTalk[Random.Range(0, pyseTalk.Length)];
        talking.PlayOneShot(talking.clip);
    }
}
