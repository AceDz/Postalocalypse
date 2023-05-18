using Sirenix.OdinInspector;
using System;
using TeamTheDream.Delivery;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skills", order = 1)]
public class SkillScriptableObject : ScriptableObject , IUnlockable
{
    [SerializeField] private bool _skillUnlocked;

    public bool SkillUnlocked => _skillUnlocked;
    public event Action OnSkillStateChange;
    
    [Button("Change State")]
    public void ChangeState()
    {
        _skillUnlocked = !_skillUnlocked;
        OnSkillStateChange?.Invoke();
    }

    public void ChangeState(bool unlocked)
    {
        _skillUnlocked = unlocked;
        OnSkillStateChange?.Invoke();
    }

    public void SavePlayerPref()
    {
        PlayerPrefs.SetInt(this.name,Convert.ToInt32(_skillUnlocked));
    }

    public void LoadPlayerPref()
    {
        _skillUnlocked = PlayerPrefs.GetInt(this.name) == 1;
    }

    public void Unlock()
    {
        ChangeState(true);
    }
}