﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamController : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public CinemachineTargetGroup targetGroup;
    Cinemachine.CinemachineTargetGroup.Target target;
    Cinemachine.CinemachineTargetGroup.Target target2;
    private bool inTransition = false;
    public enum CamState
    {
        menu,
        Transition,
        InGame,
        EndGame
    }
    /*[HideInInspector]*/public CamState camState;

    public void Awake()
    {
        camState = CamState.menu;
        //targetGroup.m_Targets = new CinemachineTargetGroup.Target[2];
        vCam2.gameObject.SetActive(true);
        vCam1.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (camState == CamState.Transition )
        {
            if (!inTransition)
            {
                inTransition = true;
                DoTransition();
            }
        }
        else if (camState == CamState.InGame )
        {
            vCam1.GetCinemachineComponent<CinemachineGroupComposer>().m_MinimumFOV = 60;
        }
        else if(camState == CamState.EndGame)
        {
            vCam1.GetCinemachineComponent<CinemachineGroupComposer>().m_MinimumFOV = 80;
        }
    }

    private void DoTransition()
    {
        StartCoroutine(transition(5.0f, vCam2.GetCinemachineComponent<CinemachineTrackedDolly>()));
    }

    private IEnumerator transition(float TransitionTime, CinemachineTrackedDolly track)
    {
        float ElapsedTime = 0.0f;
        float accumulateur = 0;
        while (ElapsedTime < TransitionTime)
        {
            accumulateur += (1 / TransitionTime) * Time.deltaTime;
            ElapsedTime += Time.deltaTime;
            track.m_PathPosition = Mathf.Lerp(0, 8, accumulateur);
            yield return null;
        }
        SetVcam2();
        inTransition = false;
        camState = CamState.InGame;
        yield return new WaitForEndOfFrame();
    }

    private void SetVcam2()
    {
        target.target = PlayerManager.Instance.player1.transform;
        target.radius = 30;
        targetGroup.m_Targets.SetValue(target, 0);
        target2.target = PlayerManager.Instance.player2.transform;
        target2.radius = 30;
        targetGroup.m_Targets.SetValue(target2, 1);
        vCam2.gameObject.SetActive(false);
        vCam1.gameObject.SetActive(true);
    }
}
