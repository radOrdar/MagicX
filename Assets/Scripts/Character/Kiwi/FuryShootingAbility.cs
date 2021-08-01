using UnityEngine;

public class FuryShootingAbility : BaseDurationAbility {
    [SerializeField] private float multiplierDmgToEnemy;
    [SerializeField] private float multiplierDmgSelf;

    public float MultiplierDmgToEnemy => multiplierDmgToEnemy;

    public float MultiplierDmgSelf => multiplierDmgSelf;
}