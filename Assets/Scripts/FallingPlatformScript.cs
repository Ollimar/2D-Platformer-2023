using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{

    public float fallDelay = 2f;
    public float returnDelay = 3f;

    public bool falling = false;

    public Color normalColor;
    public Color fallColor;

    public Vector2 originalPos;

    private Rigidbody2D myRB;
    private SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && falling == false)
        {
            StartCoroutine("Fall");
        }
    }

    public IEnumerator Fall()
    {
        falling = true;
        mySprite.color = fallColor;
        yield return new WaitForSeconds(fallDelay);
        myRB.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(returnDelay);
        myRB.bodyType = RigidbodyType2D.Kinematic;
        myRB.velocity = Vector2.zero;
        mySprite.color = normalColor;
        transform.position = originalPos;
        falling = false;
    }
}
