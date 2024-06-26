using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceMember : MonoBehaviour
{
    public abstract bool completed();

    public abstract void LEFT_MOUSE(float mouseX, float mouseY);
    public abstract void RIGHT_MOUSE(float mouseX, float mouseY);
    public abstract void Z();
    public abstract void X();
    public abstract void C();
    public abstract void UP();
    public abstract void LEFT();
    public abstract void DOWN();
    public abstract void RIGHT();
    public abstract void ENTER();
    public abstract void ESCAPE();
}
