using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();

    public abstract bool RetrieveJumpInput();

    public abstract bool RetrieveJumpHoldInput();

    public abstract float RetrieveUpDownInput();

    public abstract bool RetrieveAttackInput();
}
