using System;
using System.Collections;
using UnityEngine;

namespace Core.Services.Updater
{
    public interface IProjectUpdater
    {
        event Action UpdateCalled;
        event Action FixedUpdateCalled;
        event Action LateUpdateCalled;
        bool IsPaused { get; }
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        void StopAllCoroutines();
    }
}