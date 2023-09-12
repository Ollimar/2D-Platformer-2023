using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{

    public Transform target;
    public float scrollSpeed;
    public float scrollSpeedY;
    public float startX;
    public float startY;
    public float spriteWidth;

    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.transform;
        startX = transform.position.x;
        startY = transform.position.y;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = target.position.x * scrollSpeed;
        float yPos = target.position.y * scrollSpeedY;
        transform.position = new Vector3(startX + xPos, startY + yPos, transform.position.z);

        float cameraDistance = target.position.x * (1-scrollSpeed);
        if(cameraDistance > startX + spriteWidth)
        {
            startX += spriteWidth;
        }
        else if(cameraDistance < startX - spriteWidth)
        {
            startX -= spriteWidth;
        }
    }
}
