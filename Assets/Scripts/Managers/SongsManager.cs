using UnityEngine;

public class SongsManager : MonoBehaviour
{
    public static SongsManager instance;

    public AudioSource audioSource;

    [SerializeField]
    private AudioClip[] musics = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = musics[Random.Range(0, musics.Length)];
            audioSource.Play();
        }
    }
}
