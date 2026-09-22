using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartSound : MonoBehaviour
{
    private AudioSource audioData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        Debug.Log("commence");

    }
    }