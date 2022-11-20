using UnityEngine;

public class SpinesControl : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != null)
        {
            Destroy(collision.transform.parent.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
            animator.SetTrigger("Remove");
        }
    }
}
