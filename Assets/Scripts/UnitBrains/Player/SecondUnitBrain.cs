﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model.Runtime.Projectiles;
using Unity.VisualScripting;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            float temp = GetTemperature();
            if (temp >= overheatTemperature)  return;
           
            for (int i = 0;temp >=i; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
               
            }
             IncreaseTemperature();
        }

        

        protected override List<Vector2Int> SelectTargets()
        {
          
            List<Vector2Int> result = GetReachableTargets();
            List<Vector2Int> targets = new List<Vector2Int>();
            float minDistance = float.MaxValue;
            Vector2Int nearestTarget = Vector2Int.zero;

            foreach (var target in result)
            {
                float distance = DistanceToOwnBase(target);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestTarget = target;                  
                }
            }

            if (minDistance != float.MaxValue)
            { 
                targets.Add(nearestTarget);
            }
            return targets;
            
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}