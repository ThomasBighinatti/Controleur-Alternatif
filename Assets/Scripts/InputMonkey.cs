
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class InputMonkey : MonoBehaviour
{
    private AudioSource audioData;

    private void Awake()
    { 
        audioData = GetComponent<AudioSource>();
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) 
        {
            audioData.Play();

        }
    }
}