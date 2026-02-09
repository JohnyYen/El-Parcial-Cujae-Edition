using UnityEngine;

[CreateAssetMenu(fileName = "NewBossAttack", menuName = "Boss Attack")]
public class BossAttackSO : ScriptableObject, IBossAttack
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;

    public AttackType AttackType => attackType;
    public float Damage => damage;
    public float Cooldown => cooldown;
}