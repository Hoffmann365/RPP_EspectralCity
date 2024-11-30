using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveKeyWay : MonoBehaviour
{
    public GameObject keyWay;
    public GameObject keyWayPlatforms;
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        key.SetActive(false);
        keyWayPlatforms.SetActive(false);
        keyWay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            key.SetActive(true);
            keyWayPlatforms.SetActive(true);
            keyWay.SetActive(true);
        }
    }
}
