using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 sourcePosition;
    private Vector2 anchorPosition;
    private Vector2 secondAnchorPosition;
    private Vector2 targetPosition;

    public float ytargetPosition = -1.0f;

    private readonly int speed = 8;

    private bool onTop = true;
    public bool onPosition = false;

    private bool onLeft = false;
    private bool onRight = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sourcePosition = transform.position;
        targetPosition = Vector2.zero;
        StartCoroutine(ChangeRotation());
    }

    private void Update()
    {
        if (onPosition)
        {
            Movement();
            if (transform.parent != null) transform.parent.DetachChildren();
        }
    }

    private void Movement()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sourcePosition = transform.position;
                anchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
            }
        }

#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount > 0)
            {
                if (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y <= 3)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        sourcePosition = transform.position;
                        anchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
                    }
                }
            }

#endif

        transform.position = Vector2.Lerp(transform.position, new Vector2(Mathf.Clamp(sourcePosition.x + targetPosition.x, -2.25f, 2.25f), ytargetPosition), speed * Time.deltaTime);

        if (UIManager.instance.mouseAnimator != null)
        {
            if (UIManager.instance.mouseAnimator.gameObject.activeSelf)
            {
                if (transform.position.x >= 0.5)
                {
                    onRight = true;
                }
                else if (transform.position.x <= -0.5)
                {
                    onLeft = true;
                }

                if (onLeft && onRight)
                {
                    UIManager.instance.mouseAnimator.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnPosition()
    {
        onPosition = true;
    }

    private IEnumerator ChangeRotation()
    {
        yield return new WaitForSeconds(5);

        if (onTop)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    animator.SetTrigger("TopToTop");
                    break;
                case 1:
                    animator.SetTrigger("TopToBottom");
                    onTop = false;
                    break;
            }
        }
        else
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    animator.SetTrigger("BottomToTop");
                    onTop = true;
                    break;
                case 1:
                    animator.SetTrigger("BottomToBottom");
                    break;
            }
        }


        StartCoroutine(ChangeRotation());
    }
}
