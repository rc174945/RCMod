using System.Collections;
using UnityEngine;

namespace Utility
{
    class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator _target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                result = _target.Current;
                yield return result;
            }
        }
    }
}
