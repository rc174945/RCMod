using System.Collections;
using UnityEngine;

namespace Utility
{
    class RateLimit
    {
        private int _currentUsage;
        private int _maxUsage;
        private float _resetDelay;
        private float _lastResetTime;

        public RateLimit(int maxUsage, float resetDelay)
        {
            _currentUsage = 0;
            _lastResetTime = Time.time;
            _maxUsage = maxUsage;
            _resetDelay = resetDelay;
        }

        public bool Check(int usage = 1)
        {
            TryReset();
            if (_currentUsage + usage <= _maxUsage)
            {
                _currentUsage += usage;
                return true;
            }
            return false;
        }

        private void TryReset()
        {
            if (Time.time >= _lastResetTime + _resetDelay)
            {
                _currentUsage = 0;
                _lastResetTime = Time.time;
            }
        }
    }
}
