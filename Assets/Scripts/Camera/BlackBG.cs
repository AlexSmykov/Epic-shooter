using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт черного экрана. Есть методы для затемнения и осветления экрана, работает при смене сцен и смене комнат
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