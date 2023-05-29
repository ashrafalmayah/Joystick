using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]private float speed = 5f;
    private void Update() {
        Vector2 movementVector = GameInput.Instance.GetMovementVector();
        Vector3 moveDirection = new Vector3(movementVector.x * Time.deltaTime * speed, 0f , movementVector.y * Time.deltaTime * speed);
        transform.position += moveDirection;
    }
}
