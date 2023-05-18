using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILivesController : MonoBehaviour
{
    [SerializeField] private List<Image> _healthContainers= new List<Image>();

    private HealthManager _healthManager;
    private readonly Color _lostLifeColor = new Color(0.15f, 0.15f, 0.15f, 1f);

    private void Awake()
    {
        _healthManager = FindObjectOfType<HealthManager>();
    }
    void Start()
    {
        HealthManager.OnHealthChange += LivesChange;
        LivesChange(_healthManager.Health);
    }

    private void LivesChange(int health)
    {
        for (int i = 0; i < _healthContainers.Count; i++)
        {
            if (i >= _healthManager.MaxHealth)
            {
                _healthContainers[i].color = _lostLifeColor;
                _healthContainers[i].enabled = false;
                continue;
            }
            
            _healthContainers[i].enabled = true;
            
            var change = false;
            if (i >= health)
            {
                change = _healthContainers[i].color != _lostLifeColor;
                _healthContainers[i].color = _lostLifeColor;
            }
            else
            {
                change = _healthContainers[i].color != Color.white;
                _healthContainers[i].color = Color.white;   
            }
            
            if (change)
            {
                DOTween.Kill(_healthContainers[i]);
                _healthContainers[i].transform.localScale = Vector3.one;
                _healthContainers[i].transform.DOScale(1.2f, 0.15f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
            }
        }
        
    }

    private void OnDestroy()
    {
        HealthManager.OnHealthChange -= LivesChange;
    }
}
