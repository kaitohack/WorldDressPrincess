﻿using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

[System.Serializable]
public class Princess
{
    public Renderer[] Dresses;

    public void Activate(bool bActivate)
    {
        foreach (Renderer dress in Dresses)
            dress.gameObject.SetActive(bActivate);
    }
}

public class GameModeMain : MonoBehaviour {

    public Transform ToonPivot;
    public Transform LightningPivot;

    public GameObject CameraPivot;
    public GameObject Camera;

    public Animator [] CharacterAnim;

    public Princess[] Princesses;

    private bool m_bLightning = true;
    private bool m_bZoomed = false;

    private int m_iDefaultAnim = 0;
    private int m_iPrincess = 0;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_bLightning = !m_bLightning;
            ScreenSpaceAmbientOcclusion aoFilter = CameraPivot.GetComponentInChildren<ScreenSpaceAmbientOcclusion>();
            if(aoFilter)
                aoFilter.enabled = this.m_bLightning;            
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            m_bZoomed = !m_bZoomed;
            if (m_bZoomed)
                Camera.transform.position = Camera.transform.position + Camera.transform.forward * 2f;
            else
                Camera.transform.position = Camera.transform.position - Camera.transform.forward * 2f;
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            int iAnim = (m_iDefaultAnim++) % 3;
            foreach (Animator anim in CharacterAnim)
                anim.SetInteger("Animation", iAnim);
        }

        Vector3 positionToReach = m_bLightning ? LightningPivot.position : ToonPivot.position;

        if (Vector3.Distance(CameraPivot.transform.position, positionToReach) < .01f) // Camera reach position
        {
            CameraPivot.transform.position = positionToReach;
        }
        else
        {
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position, positionToReach, Time.deltaTime * 2f);
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            for (int i = 0; i < Princesses.Length;i++)
            {
                Princesses[i].Activate(i == m_iPrincess);
            }

            m_iPrincess = (m_iPrincess + 1) % Princesses.Length;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}
}
