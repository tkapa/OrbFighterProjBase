﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SuperComponent : MonoBehaviour {

    public float maximumSuper = 100.0f;
    public float currentSuper = 0.0f;
    
    void Start()
    {
        EventManager.instance.OnSuperMove.AddListener((GameObject superMoveUser) =>
        {

        });
    }

    void YellowSuperMove()
    {

    }

    void RedSuperMove()
    {

    }

    void BlueSuperMove()
    {

    }

    void GreenSuperMove()
    {

    }

    void PurpleSuperMove()
    {

    }

    void BlackSuperMove()
    {

    }
}
