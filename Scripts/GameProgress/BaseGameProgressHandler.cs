using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;

namespace GameProgress
{
    abstract class BaseGameProgressHandler : MonoBehaviour
    {
        public virtual void RegisterTitanKill(GameObject character, TITAN victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterHumanKill(GameObject character, HERO victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
        {
        }
        public virtual void RegisterSpeed(GameObject character, float speed)
        {
        }
        public virtual void RegisterInteraction(GameObject character, GameObject interact, InteractionType interactionType)
        {
        }
    }
}
