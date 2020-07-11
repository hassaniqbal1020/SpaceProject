using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    Rigidbody rb;

    public float ThrustForce;
    public float rForce;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
        }
        
    }

    void FixedUpdate()
    {
        if (state == State.Alive)
        {
            Rotation();
        }
    }

    void Rotation()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rForce * Time.deltaTime);

        }else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rForce * Time.deltaTime);

        }

    }

    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * ThrustForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("HitFriendly");
                break;

            case "FinishPad":
                state = State.Transcending;
                Invoke("NextLevel", 1.5f);
                print("Finish");
                break;

            default:
                state = State.Dying;
                Invoke("Respawn", 1.5f);
                print("PlayerDead");
                break;

        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
