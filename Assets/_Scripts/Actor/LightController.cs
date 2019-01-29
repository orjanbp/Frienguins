using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public Light targetLight;

    public float lightDuration;
    public float lightRange = 15;
    public float lightIntensity = 1;

  

    public bool staticLight = true;
    private bool runningCorutines = false;

    //Starter og stopper IEnumerator ved hjelp av staticLight bool-en
    private void FixedUpdate()
    {
     

        if (staticLight == false)
        {
            
            //Dette er for å spare memory
            if(runningCorutines == false)
            {
                StartCoroutine(DecreseLightRange(targetLight, lightRange, 2, lightDuration));
                StartCoroutine(DecreseLightIntensity(targetLight, lightIntensity, 0, lightDuration));
                runningCorutines = true;
                return;
            }

            return;
        }
        else
        {
            StopAllCoroutines();
            runningCorutines = false;
            //StopCoroutine(DecreseLightRange(targetLight, lightRange, 2, lightDuration));
            //StopCoroutine(DecreseLightIntensity(targetLight, lightIntensity, 0, lightDuration));  
            return;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "_LampRefiller")
        {
            FillUpTheLamp();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "_LampRefiller")
        {
            staticLight = false;
        }
    }

    //For å kunne gjøre lyset mindre over tid
    IEnumerator DecreseLightRange(Light lightToFade, float a, float b, float duration)
    {
        float counter = 0f;

        while (staticLight == true)
        {
            yield break;
        }

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lightToFade.range = Mathf.Lerp(a, b, counter / duration);
            
            yield return null;
        }

    }

    //For å kunne gjøre lyset mindre intenst over tid
    IEnumerator DecreseLightIntensity(Light fadeLight, float c, float d, float duration)
    {
        float counter = 0f;

        while (staticLight == true)
        {
            yield break;
        }

        while (counter < duration)
        {
            counter += Time.deltaTime;

            fadeLight.intensity = Mathf.Lerp(c, d, counter / duration);
            
            yield return null;
        }
    }



    //For å kunne starte og stoppe dempingen av lyset
    public void GoAndReturnToLight()
    {
       if (staticLight == false)
        {
            staticLight = true;
        }
        else
        {
            staticLight = false;
        }
    }

    public void MeetAnotherPenguin()
    {
        targetLight.range += 2;
        
    }

    public void FillUpTheLamp()
    {
        targetLight.range = 15;
        targetLight.intensity = 1;
        staticLight = true;
    }
}
