using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float tiberium = 7;
    // public TrailRenderer trail;
    // public GameObject[] bases;
    public GameObject myBase;
    // public GameObject targetBase;
    public GameObject bulletPrefab;
    // public Arrive arrive;
    // public Boid boid;
    // public Base baseScript;

    // void ReturnToBase() {
    //     arrive.targetPosition = myBase.transform.position;

    //     boid.enabled = true;
    // }

    // public class AttackTargetBase : State {
    //     public override void Enter()
    //     {
    //         base.Enter();
    //     }

    //     public override void Think()
    //     {
    //         base.Think();
    //     }

    //     public override void Exit()
    //     {
    //         base.Exit();
    //     }
    // }

    // void AttackTargetBase() {
    //     while(ReferenceEquals(myBase, targetBase) || targetBase == null) {
    //         int index = Random.Range(0, bases.Length);

    //         targetBase = bases[index];
    //     }

    //     Vector3 toTarget = targetBase.transform.position - myBase.transform.position;
    //     float distance = toTarget.magnitude;
    //     Vector3 direction = toTarget / distance;

    //     arrive.targetPosition = targetBase.transform.position - (direction * 10);
    // }

    // IEnumerator ShootBase() {
    //     while(tiberium > 0) {
    //         yield return new WaitForSeconds(0.2f);

    //         GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

    //         newBullet.GetComponent<Renderer>().material.SetColor("_Color", gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color);

    //         if(tiberium == 0) {
    //             ReturnToBase();

    //             yield break;
    //         }
    //         else {
    //             tiberium--;
    //         }
    //     }
    // }

    // IEnumerator CheckForBase() {
    //     while(true) {
    //         yield return new WaitForSeconds(1);

    //         if(Vector3.Distance(arrive.targetPosition, transform.position) < 2 && tiberium > 0) {

    //             StartCoroutine(ShootBase());
    //         }
    //     }
    // }

    public class RefuelAtBase : State {
        public override void Think()
        {
            if(owner.GetComponent<Fighter>().myBase.GetComponent<Base>().tiberium >= 7) {
                owner.GetComponent<Fighter>().myBase.GetComponent<Base>().tiberium -= 7;
                owner.GetComponent<Fighter>().tiberium += 7;
                owner.GetComponent<StateMachine>().ChangeState(new CheckForTargetBase());
            }
        }
    }

    public class ReturnToBase : State {
        public override void Enter()
        {
            owner.GetComponent<Arrive>().targetPosition = owner.GetComponent<Fighter>().myBase.transform.position;
            owner.GetComponent<Arrive>().enabled = true;
        }

        public override void Think()
        {
            if(Vector3.Distance(owner.GetComponent<Arrive>().targetPosition, owner.GetComponent<Fighter>().transform.position) < 2) {
                owner.GetComponent<StateMachine>().ChangeState(new RefuelAtBase());
            }
        }

        public override void Exit()
        {
            owner.GetComponent<Arrive>().enabled = false;
        }
    }

    public class ShootTargetBase : State {
        public override void Think()
        {
            if(owner.GetComponent<Fighter>().tiberium > 0) {
                GameObject newBullet = Instantiate(owner.GetComponent<Fighter>().bulletPrefab, owner.GetComponent<Fighter>().transform.position, owner.GetComponent<Fighter>().transform.rotation);

                newBullet.GetComponent<Renderer>().material.SetColor("_Color", owner.GetComponent<Fighter>().gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color);

                owner.GetComponent<Fighter>().tiberium--;
            }
            else {
                owner.GetComponent<StateMachine>().ChangeState(new ReturnToBase());
            }
        }
    }

    public class CheckForTargetBase : State {

        public override void Enter()
        {
            owner.GetComponent<Arrive>().enabled = true;

            GameObject myBase = owner.GetComponent<Fighter>().myBase;
            GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
            GameObject targetBase = myBase;

            while(ReferenceEquals(myBase, targetBase) || targetBase == null) {
                int index = Random.Range(0, bases.Length);

                targetBase = bases[index];
            }

            Vector3 toTarget = targetBase.transform.position - owner.GetComponent<Fighter>().transform.position;
            float distance = toTarget.magnitude;
            Vector3 direction = toTarget / distance;

            owner.GetComponent<Arrive>().targetPosition = targetBase.transform.position - (direction * 10);
        }
        public override void Think()
        {
            if(Vector3.Distance(owner.GetComponent<Arrive>().targetPosition, owner.GetComponent<Fighter>().transform.position) < 2 && 
                owner.GetComponent<Fighter>().tiberium > 0) 
            {
                owner.GetComponent<StateMachine>().ChangeState(new ShootTargetBase());
            }
        }

        public override void Exit()
        {
            owner.GetComponent<Arrive>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // arrive = GetComponent<Arrive>();
        // boid = GetComponent<Boid>();
        // baseScript = gameObject.transform.parent.gameObject.GetComponent<Base>();

        // bases = GameObject.FindGameObjectsWithTag("Base");

        // StartCoroutine(CheckForBase());

        // myBase = gameObject.transform.parent.gameObject;

        // AttackTargetBase();

        GetComponent<StateMachine>().ChangeState(new CheckForTargetBase());
    }

    // Update is called once per frame
    void Update()
    {
        // if(Vector3.Distance(arrive.targetPosition, gameObject.transform.position) < 2) {
            // boid.enabled = false;
        // }
    }

}
