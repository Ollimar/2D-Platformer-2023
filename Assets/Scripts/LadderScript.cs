using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DetachFromLadders()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.7f);
        GetComponent<Collider2D>().enabled = true;
    }
}
