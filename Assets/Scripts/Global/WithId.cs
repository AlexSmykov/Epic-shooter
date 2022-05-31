using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��� �������� ���� ��������, �� ��������� ���������� � �������
/// </summary>
[RequireComponent(typeof(ShopCostForObjects))]
[RequireComponent(typeof(Savable))]
public class WithId : MonoBehaviour
{
    private StateMachine _StateMachine;
    private Saver _Saver;
    private int _Id;
    [SerializeField] private StateMachine.WeaponLink _WhichWeapon;
    [HideInInspector] public StateMachine.WeaponLink WhichWeapon { get => _WhichWeapon; }
    public int Id
    {
        get => _Id;
        set
        {
            if (value >= 0)
            {
                _Id = value;
            }
        }
    }

    private void Awake()
    {
        _StateMachine = GameObject.Find("StateMachine").GetComponent<StateMachine>();
        _Saver = GameObject.Find("Saver").GetComponent<Saver>();
    }

    public StateMachine.ItemActivityState GetActivityState()
    {
        _StateMachine = GameObject.Find("StateMachine").GetComponent<StateMachine>();
        return _StateMachine.GetItemById(_Id).ActivityState;
    }

    public void OnGrab()
    {
        _Saver.DeleteObject(gameObject);
        _Saver.Save();
    }
}
