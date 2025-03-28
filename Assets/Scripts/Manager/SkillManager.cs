using System;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public DashSkill Dash { get; private set; }
    public CloneSkill Clone { get; private set; }
    public SwordSkill Sword { get; private set; }
    public BlackholeSkill Blackhole { get; private set; }
    public CrystalSkill Crystal { get; private set; }
    public ParrySkill parry { get; private set; }
    public DodgeSkill dodge { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
        Sword = GetComponent<SwordSkill>();
        Blackhole = GetComponent<BlackholeSkill>();
        Crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
