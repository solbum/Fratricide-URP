using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int hp, damage;
    public float moveSpeed, jumpPower;
    public LayerMask platformLayer, wallLayer;
}
