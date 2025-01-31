using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkstationTypes
{
    Timon, Vela, Cannon, Repair
}


public class Workstation : MonoBehaviour
{
    public WorkstationTypes workstationType;
    public GameObject outline;
    public GameObject sprite;

    public static float timonRotationSpeed = 360f/3f;
    public static float velaRotationSpeed = 90f/3f;
    public static float velaOpeningSpeed = 1f;

    SpriteRenderer[] outlineRenderers;
    LineRenderer lineRenderer;
    PlayerController currentPlayer = null;

    public static float timonRotation = 0; // -360 to 360
    public static float velaRotation = 0; // -90 to 90
    public static float velaOpen = 0; // 0 to 1 (unused for now)

    // Start is called before the first frame update
    void Start()
    {
        outlineRenderers =  outline.GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null)
        {
            switch (workstationType)
            {
                case WorkstationTypes.Timon:
                    float timonChange = Input.GetAxis(currentPlayer.horizontalAxis);
                    timonRotation = Mathf.Clamp(timonRotation - timonChange * timonRotationSpeed * Time.deltaTime, -360, 360);
                    sprite.transform.rotation = Quaternion.Euler(0, 0, timonRotation);
                    break;
                case WorkstationTypes.Vela:
                    float velaRotChange = Input.GetAxis(currentPlayer.horizontalAxis);
                    velaRotation = Mathf.Clamp(velaRotation - velaRotChange * velaRotationSpeed * Time.deltaTime, -90, 90);
                    sprite.transform.rotation = Quaternion.Euler(0, 0, velaRotation);
                    float velaOpenChange = Input.GetAxis(currentPlayer.verticalAxis);
                    velaOpen = Mathf.Clamp(velaOpen + velaOpenChange * velaOpeningSpeed * Time.deltaTime, 0, 1);
                    break;
                case WorkstationTypes.Cannon:
                    break;
                case WorkstationTypes.Repair:
                    break;
            }
        }
    }

    public void Work(PlayerController player)
    {
        currentPlayer = player;
        SetOutlineColor(Color.green);
    }

    public void Leave(PlayerController player)
    {
        currentPlayer = null;
        SetOutlineColor(Color.black);
    }

    public void SetOutlineActive(bool state)
    {
        outline.SetActive(state);
    }

    public void SetOutlineColor(Color color)
    {
        foreach (var renderer in outlineRenderers)
        {
            renderer.color = color;
        }
    }
}
