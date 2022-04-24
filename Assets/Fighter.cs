using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int tiberium = 7;
    public GameObject[] bases;
    public GameObject myBase;
    public GameObject targetBase;
    public Arrive arrive;
    public Boid boid;

    // Start is called before the first frame update
    void Start()
    {
        arrive = GetComponent<Arrive>();
        boid = GetComponent<Boid>();
        bases = GameObject.FindGameObjectsWithTag("Base");

        myBase = gameObject.transform.parent.gameObject;

        while(ReferenceEquals(myBase, targetBase) || targetBase == null) {
            int index = Random.Range(0, bases.Length);

            targetBase = bases[index];
        }

        arrive.targetGameObject = targetBase;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(targetBase.transform.position, gameObject.transform.position) < 10) {
            boid.enabled = false;
        }
    }
}
