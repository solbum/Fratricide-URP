using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDirection : MonoBehaviour
{
    LineRenderer lineRenderer;
    PlayerStatus playerStatus;
    Rigidbody2D rigid;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();

        lineRenderer.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;

            Vector2 direction = GameManager.mousePosition - rigid.position;
            direction.Normalize();

            lineRenderer.SetPosition(0, rigid.position);
            lineRenderer.SetPosition(1, rigid.position + direction * playerStatus.chainMaxLength);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
