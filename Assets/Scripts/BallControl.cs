using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D thisRigidbody2D;

    private void Start()
    {
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        thisRigidbody2D.gravityScale = GameManager.instance.gravityScale;
    }

    private void Update()
    {
        thisRigidbody2D.gravityScale = GameManager.instance.gravityScale;
    }
}
