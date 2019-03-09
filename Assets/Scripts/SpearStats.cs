using UnityEngine;

[CreateAssetMenu(fileName = "New SpearStat", menuName = "Spear/SpearStats")]
public class SpearStats : ScriptableObject
{
    public float minForce;
    public float maxForce;
    public float chargeTime;
}