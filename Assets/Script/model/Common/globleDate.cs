using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{
    public class GlobleDate
    {
        public const int rewordGold = 2;
        public static ConfigGameData data;
        public static ConfigMap CurMapData;
        public static HumanEntity mainRole;
        public static int GetDamage(Entity atker, Entity beAtker)
        {
            int damage = 0;
            int randomRate = Random.Range(0,100);
            if(randomRate <= beAtker.parryPoint)
            {
                return 0;
            }else
            {
                damage = atker.attackPoint - beAtker.defensePoint;
                if (damage <= 0)
                {
                    damage = 0;
                }
                return damage;
            }
        }

    }
}