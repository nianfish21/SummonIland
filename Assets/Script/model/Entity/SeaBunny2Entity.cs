using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using System;

using DG.Tweening; //命名空间

namespace model {

    public class SeaBunny2Entity : Entity
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
                GameObject Buttle= ResourseController.ins.GetPerfab("ZiDanBunny");
                Buttle.transform.position = transform.position;
                Shake(0.3f, 0.4f);
                Buttle.transform.DOMove(new Vector3(enemy.GetPos().x + 0.5f, enemy.GetPos().y + 0.5f), 0.3f).SetEase(Ease.Linear).OnComplete(() => {
                    
                    Attack(enemy);
                    GameObject.Destroy(Buttle);
                }); ;
            }
            //即使没有攻击对象也会浪费攻击次数，直到所有攻击次数使用为止
        }
        
        /// <summary>
        /// 在水地攻击速度加0.5秒
        /// </summary>
        public void cycleFight()
        {

            float  BuffCD = 0;
            if (MapeController.Instance.GetBlock(GetPos().x, GetPos().y).blockType == BlockType.Water)
                BuffCD = 0.5f;
            Timer.Register(attackCd-BuffCD, () => {

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
            hit.Injured(this);
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
