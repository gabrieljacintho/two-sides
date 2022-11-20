using UnityEngine;

public class PlayerSidesControl : MonoBehaviour
{
    [SerializeField]
    private bool whitePlayer = false;
    private PlayerControl playerControl;

    [SerializeField]
    private AudioClip coinSound = null, impactSound = null;

    private void Start()
    {
        if (whitePlayer)
        {
            playerControl = GetComponent<PlayerControl>();
        }
        else
        {
            playerControl = transform.parent.GetComponent<PlayerControl>();
        }
    }

    private void Update()
    {
        if (!whitePlayer)
        {
            if (transform.localPosition != new Vector3(0, -2.5f, 0)) transform.localPosition = new Vector3(0, -2.5f, 0);
            if (transform.localRotation != Quaternion.Euler(0, 0, 0)) transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("WhiteBall") && whitePlayer) || (collision.gameObject.CompareTag("BlackBall") && !whitePlayer))
        {
            ScoreManager.instance.AddScore();
            SongsManager.instance.audioSource.PlayOneShot(coinSound, 0.5f);
            Destroy(collision.gameObject);
        }
        else if (!collision.gameObject.CompareTag("Player"))
        {
            SongsManager.instance.audioSource.PlayOneShot(impactSound);
            playerControl.ytargetPosition--;
        }
    }
}
