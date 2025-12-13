using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]

public class PlayerController : InputController
{

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");
    }

    public override float RetrieveUpDownInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Attack");
    }
}
