using UnityEngine;

public class InputButton : InputElement{
    override public void ModifyValue()
    {
        Utility.SetVariableValue(scriptOfVariable, valueToModify, currentValue);
    }
            
}
