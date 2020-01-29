using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera Camera;

    public static Vector2 mousePosition;
    public static float horizontal;

    public bool isGetRawHorizontal;


    private void Start()
    {

    }

    private void Update()
    {
        horizontal = isGetRawHorizontal ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");

        mousePosition = Input.mousePosition;
        mousePosition = Camera.ScreenToWorldPoint(mousePosition);
    }
}
