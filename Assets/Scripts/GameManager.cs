using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Rigidbody2D whiteBall;
    public float power;
    public int points;

    public bool canShoot;

    public GameObject WinText, loseTxt;

    //Transform target;

    private Camera cam;

    
   // public WhiteBallLineRenderer Trajectory;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            shoot();
        }

        if (points == 10)
        {
            StartCoroutine(WinCoroutine());
            
        }


#if !UNITY_EDITOR       
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif      
        {

            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        };
    }
    public void shoot()
    {
        if(canShoot )
        whiteBall.AddForce(transform.right * power, ForceMode2D.Impulse);
    }
    IEnumerator WinCoroutine()
    {
        WinText.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator Lose()
    {
        loseTxt.SetActive(true);
        yield return new WaitForSeconds(3);
        Debug.LogWarning("Restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "goal")
            return;
        StartCoroutine(Lose());

    }
}

