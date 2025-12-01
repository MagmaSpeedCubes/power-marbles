using UnityEngine;

abstract public class InputElement : MonoBehaviour
{
    [SerializeField] protected string scriptOfVariable;
    [SerializeField] protected string valueToModify;
    protected float currentValue;

    abstract public void ModifyValue();
}
