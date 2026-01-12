using UnityEngine;

public class PickaxeMarbleHandler : BallHandler
{
    [SerializeField]private GameObject toolhead;

    public float rpm;
    private float dpm;
    public float currentAngleDegrees;
    public float offsetDistance;

    override protected void Initialize()
    {
        base.Initialize();
        DrillBit toolheadScript = toolhead.GetComponent<DrillBit>();
        dpm = toolheadScript.tickRate;
    }

    override protected void OnUpdate()
    {
        base.OnUpdate();
        UpdateRotation();

    }

    protected void UpdateRotation()
    {
        currentAngleDegrees = (currentAngleDegrees + Time.deltaTime * rpm / 60 * 360 )%360;
        Quaternion rot=new Quaternion();
        rot.eulerAngles = new Vector3(0, 0, currentAngleDegrees);
        toolhead.transform.rotation = rot;

        float currentAngleRadians = currentAngleDegrees / 180 * Mathf.PI;
        toolhead.transform.localPosition = new Vector3(Mathf.Cos(currentAngleRadians), Mathf.Sin(currentAngleRadians), 0) * offsetDistance;
    

    }

    public override void OnAbilityTick()
    {

        dpm = dpm + dpm * numBounces/(numBounces + 10);
        DrillBit toolheadScript = toolhead.GetComponent<DrillBit>();
        toolheadScript.tickRate = dpm;

        Debug.Log("rpm updated to " + dpm);
    }

    override public float GetDamage()
    {
        return ballData.power;
    }
}
