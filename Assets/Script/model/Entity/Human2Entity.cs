using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using System;

using DG.Tweening; //命名空间

namespace model {

    public class Human2Entity : Entity
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
            
        }


        public override void FightUpDate()
        {
            Entity enemy = EntityController.Instance.GetFar(this, this.attackRange, true);
            if (enemy != null)
            {
                GameObject Buttle= ResourseController.ins.GetPerfab("ZiDanHuman");
                Buttle.transform.position = transform.position;
                Shake(0.3f, 0.4f);
                Buttle.transform.DOMove(new Vector3(enemy.GetPos().x + 0.5f, enemy.GetPos().y + 0.5f), 0.3f).SetEase(Ease.Linear).OnComplete(() => {
                    
                    Attack(enemy);
                    GameObject.Destroy(Buttle);
                }); ;
            }
            //即使没有攻击对象也会浪费攻击次数，直到所有攻击次数使用为止
        }
        
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

        

        public override void Attack(Entity hit)
        {
            hit.Injured(this);
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
