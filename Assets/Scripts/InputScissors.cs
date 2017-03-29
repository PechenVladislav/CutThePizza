﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScissors : MonoBehaviour
{

    public LayerMask pizzaMask;

    public static Action<Vector2, Vector2> successfulInput;

    private LineRenderer lineRend;
    private Vector2[] positions = new Vector2[2];
    private SpriteRenderer spriteRend;
    private Material material;

    // Use this for initialization
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        material = lineRend.material;
    }

    private void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 9f);
        if (Input.GetMouseButtonDown(0))
        {
            lineRend.SetPosition(0, mouseWorldPos);
            lineRend.SetPosition(1, mouseWorldPos);
        }
        else if (Input.GetMouseButton(0))
        {
            lineRend.SetPosition(1, mouseWorldPos);
            material.SetTextureScale("_MainTex", new Vector2(Vector2.Distance(mouseWorldPos, lineRend.GetPosition(0)) * 2f, 0f));
            material.SetTextureOffset("_MainTex", new Vector2(-2f * Time.time, 0f));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CastLine(lineRend.GetPosition(0), lineRend.GetPosition(1));
            lineRend.SetPosition(0, mouseWorldPos);
            lineRend.SetPosition(1, mouseWorldPos);
        }
    }

    private void CastLine(Vector2 from, Vector2 to)
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];
        hits[0] = Physics2D.Linecast(from, to, pizzaMask);
        hits[1] = Physics2D.Linecast(to, from, pizzaMask);

        if (hits[0] && hits[1] && hits[0].collider.gameObject == hits[1].collider.gameObject)
        {
            spriteRend = hits[0].collider.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRend)
            {
                successfulInput.Invoke(hits[0].point, hits[1].point);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (positions != null && positions.Length == 2)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(positions[0], 0.2f);

    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(positions[1], 0.2f);
    //    }

    //    if (spriteRend != null)
    //    {
    //        Sprite spr = spriteRend.sprite;
    //        Vector2[] vertices = spr.vertices;

    //        for (int i = 0; i < vertices.Length - 1; i++)
    //        {
    //            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
    //        }
    //        Gizmos.color = Color.red;

    //        Gizmos.DrawLine(vertices[vertices.Length - 1], vertices[0]);
    //    }
    //}
}
