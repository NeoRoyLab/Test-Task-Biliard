using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetector : MonoBehaviour
{
    public Text txt;
    public GameManager GM;
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "red ball")
        {
            GM.points++;
            txt.text = "Points " + GM.points.ToString();
            Debug.Log(GM.points);
            Destroy(collision.gameObject);
        }
    }

}
