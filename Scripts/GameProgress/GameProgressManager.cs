using UnityEngine;
using System.Collections;
using Utility;
using System;
using System.Collections.Generic;

namespace GameProgress
{
    class GameProgressManager : MonoBehaviour
    {
        static GameProgressManager _instance;
        public static GameProgressContainer GameProgress;
        private static GameStatHandler _gameStatHandler;
        private static AchievmentHandler _achievmentHandler;
        private static QuestHandler _questHandler;
        private static List<BaseGameProgressHandler> _handlers = new List<BaseGameProgressHandler>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            GameProgress = new GameProgressContainer();
            _instance.StartCoroutine(_instance.IncrementPlayTime());
        }

        public static void FinishLoadAssets()
        {
            _gameStatHandler = new GameStatHandler(GameProgress.GameStat);
            _achievmentHandler = new AchievmentHandler(GameProgress.Achievment);
            _questHandler = new QuestHandler(GameProgress.Quest);
            _handlers.Add(_gameStatHandler);
            _handlers.Add(_achievmentHandler);
            _handlers.Add(_questHandler);
        }

        private void OnApplicationQuit()
        {
            Save();
        }
        
        public static void OnMainMenu()
        {
            Save();
            _achievmentHandler.ReloadAchievments();
            _questHandler.ReloadQuests();
        }

        private static void Save()
        {
            GameProgress.Save();
        }

        public static int GetExpToNext()
        {
            return _gameStatHandler.GetExpToNext();
        }

        public static void AddExp(int exp)
        {
            _gameStatHandler.AddExp(exp);
        }

        public static void RegisterTitanKill(GameObject character, TITAN victim, KillWeapon weapon)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterTitanKill(character, victim, weapon);
        }

        public static void RegisterHumanKill(GameObject character, HERO victim, KillWeapon weapon)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterHumanKill(character, victim, weapon);
        }

        public static void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
        {
            if (FengGameManagerMKII.level.StartsWith("Custom"))
                return;
            TITAN titan = victim.GetComponent<TITAN>();
            if (titan != null && titan.myLevel > 3f)
                return;
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterDamage(character, victim, weapon, damage);
        }

        public static void RegisterSpeed(GameObject character, float speed)
        {
            if (FengGameManagerMKII.level.StartsWith("Custom"))
                return;
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterSpeed(character, speed);
        }

        public static void RegisterInteraction(GameObject character, GameObject interact, InteractionType interactionType)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterInteraction(character, interact, interactionType);
        }

        private IEnumerator IncrementPlayTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                GameProgress.GameStat.PlayTime.Value += 10f;
            }
        }
    }

    public enum KillWeapon
    {
        Blade,
        Gun,
        ThunderSpear,
        Cannon,
        Shifter,
        Titan
    }

    public enum InteractionType
    {
        ShareGas,
        CarryHuman
    }
}
