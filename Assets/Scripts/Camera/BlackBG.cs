using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������� ������. ���� ������ ��� ���������� � ���������� ������, �������� ��� ����� ���� � ����� ������
/// </summary>
public class BlackBG : MonoBehaviour
{
    private Animator _Animator;

    private enum BlackBGState
    {
        Dark,
        Light,
    }

    private BlackBGState _state;

    public void Init()
    {
        _state = BlackBGState.Dark;
        _Animator = GetComponent<Animator>();
    }

    private void SetStateToDark()
    {
        _state = BlackBGState.Dark;
    }
    private void SetStateToLight()
    {
        _state = BlackBGState.Light;
    }

    public IEnumerator ToDark()
    {
        _Animator.Play("BlackBGToDark");
        yield return new WaitUntil(() => _state == BlackBGState.Dark);
    }

    public IEnumerator ToLight()
    {
        _Animator.Play("BlackBGToLight");
        yield return new WaitUntil(() => _state == BlackBGState.Light);
    }
}