using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public Transform Pointer;
    public Transform StartRayPos;

    public GameManager GM;

    public Rigidbody2D rb;

    float radius = 0.25f;

    public LayerMask mask;

    public LineRenderer greenLine, redLine;

    

    private void Awake()
    {
        
        greenLine.startWidth = 0.05f;
        greenLine.endWidth = 0.05f;

        redLine.startWidth = 0.05f;
        redLine.endWidth = 0.05f;
    }
    private void Update()
    {
        if (rb.velocity == Vector2.zero)
        {
            Pointer.gameObject.SetActive(true);
            GM.canShoot = true;
            greenLine.positionCount = 3;
            RaycastTrajectory();
        }
        else
        {
            Pointer.gameObject.SetActive(false);
            GM.canShoot = false;
            ClearTrajectory();
        }
    }
    
    void RaycastTrajectory()
    {
        

        Vector2 position = new Vector2(StartRayPos.position.x, StartRayPos.position.y); //Точка старта рейкаста
        Vector2 direction = transform.right; //направление рейкаста

        //первый рейкаст

            RaycastHit2D hit = Physics2D.CircleCast(position, radius, direction * 1000);
            Debug.DrawLine(position, direction * 1000, Color.green);
            Vector3 greenLineStart = new Vector3 (transform.position.x, transform.position.y, -0.1f);
            Vector3 greenLineEnd = position + direction * 1000;
            Vector3 greenLineSecondEnd;
            if (hit)
            {
                Pointer.position = hit.point + (hit.normal * radius);
                position = hit.point + (hit.normal * radius);
                greenLineEnd = position;
            greenLine.SetPosition(0, greenLineStart);
            greenLine.SetPosition(1, greenLineEnd);


            if (hit.collider.tag == "barier" || hit.collider.tag == "goal")
            {
                Vector2 secondDirection = Vector2.Reflect(direction * 1000, hit.normal); //вектор синей линии
                // Второй рейкаст
                RaycastHit2D hit2 = Physics2D.CircleCast(position, radius, secondDirection); //рейкаст синей линии
                greenLineSecondEnd = new Vector3((hit2.point.x + position.x)/2,
                                                 (hit2.point.y + position.y)/2, -0.1f);
                greenLine.SetPosition(2, greenLineSecondEnd);
                Debug.DrawLine(position, secondDirection, Color.blue);

                redLine.positionCount = 0;
            }

            

            if (hit.collider.tag == "red ball")
                {
                //траектория красного шара
                redLine.positionCount = 2;

                    Vector2 vec2 = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                    Vector2 RedTrajectory = vec2 + -hit.normal*100;
                    RedTrajectory = Vector2.Reflect(hit.normal, RedTrajectory);
                
                    Debug.DrawLine(hit.point, RedTrajectory, Color.red);
                RaycastHit2D redHit = Physics2D.CircleCast(hit.point, radius, RedTrajectory,1000, mask);
                Vector3 redLineEnd = new Vector3((redHit.point.x + position.x) / 2,
                                                 (redHit.point.y + position.y) / 2, -0.1f);
                redLine.SetPosition(0, greenLineEnd);
                redLine.SetPosition(1, redLineEnd);

// отскок белого шара от красного шара

                    Vector2 BlackTrajectory = Vector2.Perpendicular(RedTrajectory);


                    float angle = Vector2.Angle(direction, -BlackTrajectory);
                    Debug.Log(angle);
                if (angle > 90)
                    BlackTrajectory = -BlackTrajectory;
                else if (angle == 90)
                    BlackTrajectory = Vector2.zero;

                Debug.DrawLine(hit.centroid, -BlackTrajectory, Color.black);
                RaycastHit2D hit2 = Physics2D.CircleCast(position, radius, -BlackTrajectory);
                greenLineSecondEnd = new Vector3((hit2.point.x + position.x) / 2,
                                                 (hit2.point.y + position.y) / 2, -0.1f);
                greenLine.SetPosition(2, greenLineSecondEnd);


            }
            }
    }

    void ClearTrajectory()
    {
        redLine.positionCount = 0;
        greenLine.positionCount = 0;
    }
}