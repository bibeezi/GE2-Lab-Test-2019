﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float tiberium = 0;

    public TextMeshPro text;

    public GameObject fighterPrefab;

    IEnumerator AccumulateTiberium() {
        while(true) {
            yield return new WaitForSeconds(1);

            if(tiberium != 10) {
                tiberium++;
            }

            if(tiberium == 10) {
                GameObject newPrefab = Instantiate(fighterPrefab, transform.position + new Vector3(1, 0, 1), Quaternion.identity, gameObject.transform);

                newPrefab.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", gameObject.GetComponent<Renderer>().material.color);

                tiberium -= 10;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AccumulateTiberium());

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + tiberium;        
    }
}
