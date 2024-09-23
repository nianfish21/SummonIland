using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using System;

using DG.Tweening; //命名空间

namespace model {

    public class SeaBunnyEntity : Entity
    {
        public Vector3 moveTarget;
        private bool StartMove;
        private float JumpTiming = 0;
        private float JumpCycal = 0.5f;

        private float jumpHeight = 0.2f;

        private float liftTime = 0;


        // Start is called before the first frame update
        public override void Start(GameObject _obj)
        {
            base.Start(_obj);
            JumpTiming = JumpCycal;
            StartMove = false;
                cycleSummone();
                cycleFight();

            //if (obj == null) obj = transform.Find("MainPng").gameObject;
        }


        public override void FightUpDate()
        {
            Entity enemy = EntityController.Instance.GetNear(this, this.attackRange, true);
            if (enemy != null)
            {
                Attack(enemy);
            }
            //即使没有攻击对象也会浪费攻击次数，直到所有攻击次数使用为止
        }

        //重复召唤
        public void cycleFight()
        {
            Timer.Register(attackCd, () => {

                if (!this.isDie)
                {
                    if (ProgressController.Instance.progressState == ProgressState.PlayerTime)
                        FightUpDate();
                    cycleFight();
                }

            });
        }


        //重复毒
        public void cyclePoi(Entity hit)
        {
            Timer.Register(1, () => {

                if (!this.isDie)
                {
                    if (ProgressController.Instance.progressState == ProgressState.PlayerTime)
                        hit.Injured(1);
                    cyclePoi( hit);
                }

            });
        }

        public override void Attack(Entity hit)
        {
            base.Attack(hit);
            Timer.Register(1,()=> { cyclePoi(hit); },null,true);
        }

        //重复召唤
        public void cycleSummone()
        {
            Timer.Register(summonCD, () => {

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

            //JumpAndMove();
        }

        public void StopWalkTarget()
        {
            StartMove = false;
        }
        public void JumpAndMove()
        {
            if (JumpTiming < JumpCycal)
            {
                JumpTiming += Time.deltaTime;
                Vector3 pngPosition = Image.transform.position;
                Image.transform.localPosition = new Vector3(0, AlgorithmTool.AbsSinHeightFunc(JumpTiming, JumpCycal, jumpHeight), 0);
            }

        }

        public void Jump()
        {
            if (JumpTiming < JumpCycal)
            {

            }
            else
            {
                JumpTiming = 0;
            }
        }


        public override void Die()
        {
            base.Die();
        }

        public override void Injured(Entity attacker)
        {
            base.Injured(attacker);

            Jump();
        }

    }



}
