using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class penyihir : MonoBehaviour
{
    [SerializeField,Range( 0, 1)] float moveDuration;
    [SerializeField,Range( 0, 1)] float jumpHeight = 0.5f;
    [SerializeField] int leftMoveLimit;
    [SerializeField] int rightMoveLimit;
    [SerializeField] int backMoveLimit;


    public UnityEvent<Vector3> OnJumpEnd;
    void Update()
    {
        if (DOTween.IsTweening(transform))
        
            return;
        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction += Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
           direction += Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }

        if(direction == Vector3.zero)
            return;
        
            Move(direction);
        
    } 
    public void Move(Vector3 direction)
    {
        // transform.DOMoveX(transform.position.x + direction.x, moveDuration);
        // transform.DOMoveZ(transform.position.z + direction.z, moveDuration);
        var targetPosition = transform.position + direction;

        if (targetPosition.x < leftMoveLimit || targetPosition.x > rightMoveLimit || targetPosition.z < backMoveLimit)
            targetPosition = transform.position;

        transform.DOJump(transform.position + direction,
            jumpHeight,
            1,
            moveDuration).onComplete = BroadCastPositionOnJumpEnd;
        // var seq =DOTween.Sequence();
        // seq.Append(transform.DOMoveY(jumpHeight, moveDuration* 0.5f));
        // seq.Append(transform.DOMoveY(0, moveDuration* 0.5f));
        transform.forward = direction;
    }
   
    public void UpdateMoveLimit(int horizontalSize, int backLimit)
    {
        leftMoveLimit = - horizontalSize / 2;
        rightMoveLimit = horizontalSize / 2;
        backMoveLimit = -backLimit;
    }
    private void BroadCastPositionOnJumpEnd()
    {
        OnJumpEnd.Invoke(transform.position);
    }

}
