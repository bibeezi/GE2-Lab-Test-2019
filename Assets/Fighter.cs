using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float tiberium = 7;
    public TrailRenderer trail;
    public GameObject[] bases;
    public GameObject myBase;
    public GameObject targetBase;
    public Arrive arrive;
    public Boid boid;
    public GameObject bulletPrefab;
    public bool shootCoroutine = false;

    IEnumerator ReturnToBase() {
        while(true) {

        }
    }

    IEnumerator ShootBase() {
        while(tiberium > 0) {
            yield return new WaitForSeconds(0.2f);

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            newBullet.transform.SetParent(gameObject.transform);
            newBullet.GetComponent<Renderer>().material.SetColor("_Color", gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color);

            tiberium--;

            if(tiberium == 0) {
                shootCoroutine = false;

                // StartCoroutine(ReturnToBase());
                StopCoroutine(ShootBase());
            }
        }
    }

    IEnumerator CheckForBase() {
        while(true) {
            yield return new WaitForSeconds(1);

            if(Vector3.Distance(arrive.targetPosition, transform.position) < 2 && tiberium > 0 && !shootCoroutine) {
                StartCoroutine(ShootBase());

                shootCoroutine = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
        arrive = GetComponent<Arrive>();
        boid = GetComponent<Boid>();
        bases = GameObject.FindGameObjectsWithTag("Base");

        StartCoroutine(CheckForBase());

        trail.material.SetColor("_Color", gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color);

        myBase = gameObject.transform.parent.gameObject;

        while(ReferenceEquals(myBase, targetBase) || targetBase == null) {
            int index = Random.Range(0, bases.Length);

            targetBase = bases[index];
        }

        Vector3 toTarget = targetBase.transform.position - myBase.transform.position;
        float distance = toTarget.magnitude;
        Vector3 direction = toTarget / distance;

        arrive.targetPosition = targetBase.transform.position - (direction * 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(arrive.targetPosition, gameObject.transform.position) < 2) {
            boid.enabled = false;
        }
    }

}
