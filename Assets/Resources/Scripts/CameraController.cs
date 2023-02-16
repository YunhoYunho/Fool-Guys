using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private float SavedmoveSpeed;

    [SerializeField]
    private float dragSpeed;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float padding;


    [SerializeField]
    private float mouseSensitivity;

    public Transform follow;

    private GameManager gameManager;


    Vector2 m_Input;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SavedmoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (!gameManager.onGameStart) return;
        Move();
        Zoom();
        Rotate();
    }


    private void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(moveSpeed * Vector3.left * DT(), Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Vector3.right * DT(), Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(moveSpeed * Vector3.back * DT(), Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(moveSpeed * Vector3.forward * DT(), Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = SavedmoveSpeed * 3;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = SavedmoveSpeed;
        }

    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(zoomSpeed * scroll * Vector3.back * DT(), Space.Self);
    }

    private float DT()
    {
        return Time.deltaTime;
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            m_Input.x = Input.GetAxis("Mouse X");
            m_Input.y = Input.GetAxis("Mouse Y");

            if (m_Input.magnitude != 0)
            {
                Quaternion q = follow.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x + m_Input.y * 1f, q.eulerAngles.y + m_Input.x * -1f, q.eulerAngles.z);
                follow.rotation = q;

            }
        }
    }
}