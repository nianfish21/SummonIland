using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using System;

namespace model {

    public class TreeEntity : Entity
    {

        public Vector3 moveTarget;
        private bool StartMove;
        private float JumpTiming = 0;
        private float JumpCycal = 0.5f;

        private float jumpHeight = 0.2f;



        // Start is called before the first frame update
        public override void Start(GameObject _obj)
        {
            base.Start(_obj);
            JumpTiming = JumpCycal;
            StartMove = false;
            cycleSummone();


            //if (obj == null) obj = transform.Find("MainPng").gameObject;
        }


        public override void FightUpDate()
        {
        }
        
        /// <summary>
        /// 如果在草地下召唤速度加快1秒
        /// </summary>
        public void cycleSummone()
        {
            int BuffCD = 0;
            if (MapeController.Instance.GetBlock(GetPos().x, GetPos().y).blockType == BlockType.Grass)
                BuffCD = 1;
            Timer.Register(summonCD- BuffCD, () => {

                if (!this.isDie)
                {
                    if (ProgressController.Instance.progressState == ProgressState.PlayerTime)
                        Summone();
                    cycleSummone();

                }

            });
        }


        public override void Update()
        {
            base.Update();
            
        }
        

        public override void Die()
        {
            base.Die();
        }
        

    }



}
