using UnityEngine;

public class DrillMarbleHandler : BallHandler
{
    [SerializeField]private GameObject drillBitPrefab;
    override public void OnAbilityTick()
    {
        Debug.Log("Spawning new drill bit");
        GameObject bit = Instantiate(drillBitPrefab, transform.localPosition, Quaternion.identity);
        bit.name = "drillBit";
        bit.transform.parent = LocalClickManager.localInstance.ballParent.transform;
        bit.transform.localPosition = transform.localPosition;
        DrillBit db = bit.GetComponent<DrillBit>();
        db.Launch(new Vector2(0f, -1f), ballData.power,  numBounces);
        
    }

    override public float GetDamage()
    {
        return ballData.power;
    }
}
