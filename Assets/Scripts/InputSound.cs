using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InputSound : MonoBehaviour
{
    private AudioSource audioData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private float varPitch = 0.2f;
    [SerializeField]private float setupPitch = 1f;
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        Debug.Log("commence");

    }

    void Update()
    {
        if (Input.anyKeyDown) 
        {
            audioData.pitch = Random.Range(setupPitch-varPitch,+varPitch);
            audioData.Play();

        }
    }
}