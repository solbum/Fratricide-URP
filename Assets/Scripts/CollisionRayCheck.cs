using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRayCheck : MonoBehaviour
{
    public CollisionInfo collisions;
    Entity entity;
    RaycastOrigins raycastOrigins;

    BoxCollider2D box2d;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float skinWidth = 0.02f;
    float horizontalRaySpacing;
    float verticalRaySpacing;


    private void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        UpdateCollisions();
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = box2d.bounds;

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = box2d.bounds;

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void UpdateCollisions()
    {
        collisions.Reset();

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin.x += verticalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, skinWidth, entity.platformLayer);
            Debug.DrawRay(rayOrigin, Vector2.down * skinWidth, Color.red);

            collisions.below = hit;

            if (hit)
                break;
        }

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin.x += verticalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth, entity.platformLayer);

            collisions.above = hit;

            if (hit)
                break;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomRight;
            rayOrigin.y += horizontalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, skinWidth, entity.wallLayer);

            collisions.right = hit;

            if (hit)
                break;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin.y += horizontalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, skinWidth, entity.wallLayer);

            collisions.left = hit;

            if (hit)
                break;
        }

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = left = right = false;
        }
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
