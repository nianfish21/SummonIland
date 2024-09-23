using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{
    public enum populationType
    {
        Human,
        Tree,
        SeaBunny,
        None,
    }

    public enum ProgressState
    {
        UnStart,
        PlayerTime,
        Stop,//暂停
    }
    public class ProgressController : SingletonMonoBehaviour<ProgressController>
    {
        public const string mapeName = "Mape";
        public  ProgressState progressState;
        private bool isgame=false ;
        public  bool IsGame
        {
            get
            {
                return isgame;
            }
        }
        float MapeUpdateTime = 0;
        
        void Start()
        {
            progressState = ProgressState.UnStart;
        }
        
        public void NextPlan()
        {
            GlobleDate.data.planId++;
            LoadMapAndEntity(GlobleDate.data.planId);
            Hint.Instance.Print("NextLand");
            Timer.Register(2, () => {
                ChangeProgressState(ProgressState.PlayerTime);
            });
        }

        public void GameStart()
        {
            loadGameData();
            LoadMapAndEntity(GlobleDate.data.planId);
            CameraComponent.Instance.GoToPorpulation();

            Hint.Instance.Print("GameStart");
            isgame = true;

            Timer.Register(2, () => {
                ChangeProgressState(ProgressState.PlayerTime);
            });
        }

        public void GameOver()
        {

        }

        //退出游戏流程
        public void BackManu()
        {
            SaveGameData();
            ChangeProgressState( ProgressState.UnStart);
            isgame = false;
        }

        public void Exit()
        {
            Application.Quit();
        }


        void loadGameData()
        {
            GlobleDate.data = ConfigController.Instance.ReadConfigGameData();
            if (GlobleDate.data == null)
                GlobleDate.data = new ConfigGameData();
        }

        void SaveGameData()
        {
            ConfigController.Instance.SaveConfigGameData(GlobleDate.data);

        }




        public void ChackWin()
        {

            int Enemy=0;
            int Friend = 0;
            
            foreach (var entity in EntityController.Instance.entityPool)
            {
                populationType poputype = EntityController.Instance.GetPopulationType(entity.type);
                //树是中立的
                if (poputype == populationType.Tree) continue;
                if ( poputype != (populationType)GlobleDate.data.populationType)
                {
                    Enemy++;
                }else
                {
                    Friend++;
                }
            }
            if (Enemy == 0)
            {
                Hint.Instance.LeftTip("Win");
                Manu.Instance.OpenNextPlan();
                ChangeProgressState( ProgressState.Stop);
                Debug.Log("Win");
            }if (Friend == 0)
            {
                Hint.Instance.LeftTip("You Die");
                Manu.Instance.OpenSet();
                ChangeProgressState(ProgressState.Stop);
                Debug.Log("You Die");
            }


        }
        

        public void GetGold(int num)
        {
            GlobleDate.data.glod += num;
        }

        /// <summary>
        /// 暂时的状态更改方法
        /// </summary>
        /// <param name="state"></param>
        public void ChangeProgressState(ProgressState state)
        {
            if (state == progressState) return;
            switch (state) {
                case ProgressState.PlayerTime:
                    Hint.Instance.LeftTip("PlayerTime...");
                    break;
                case ProgressState.Stop:
                    Hint.Instance.LeftTip("Stop...");
                    break;
            }
            progressState = state;
        }

        void StateProgress()
        {
            if(progressState == ProgressState.Stop)
            {

            }
            if(progressState == ProgressState.PlayerTime)
            {
                ChackWin();
                foreach (var entity in EntityController.Instance.copeAll())
                {
                    entity.Update();
                }
                upgrade();


            }


            MapeController.Instance.UpdateblockEntitys();
            //MapeUpdateTime += Time.deltaTime;
            //if (MapeUpdateTime > 0.2f)
            //{
            //    MapeUpdateTime = 0;
            //}

        }

        public void upgrade()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
                if (!EntityController.Instance.occupy((int)position_target.x, (int)position_target.y))
                {
                    if (GlobleDate.data.glod > 21)
                    {
                        GlobleDate.data.glod -= 21;
                        var entity = EntityFactory.ins.GetEntity(SummoneEntityType.Human, (int)position_target.x, (int)position_target.y);

                    }else
                    {
                        Hint.Instance.Print("Need gold 21");
                    }
                } else 
                {
                    var entity = MapeController.Instance.GetEntity((int)position_target.x, (int)position_target.y);
                    if (entity != null)
                    {
                        if(entity.type== SummoneEntityType.Human)
                        {
                            if (GlobleDate.data.glod > 24)
                            {
                                GlobleDate.data.glod -= 24;
                                entity.Die();
                                var entity2 = EntityFactory.ins.GetEntity(SummoneEntityType.Human2, (int)position_target.x, (int)position_target.y);

                            }else
                            {

                                Hint.Instance.Print("Need gold 24");
                            }
                        }
                    }
                }
            }
        }



        // Update is called once per frame
        void Update()
        {
            if(GlobleDate.data!=null)
            if (GlobleDate.data.planId >= 5)
            {
                Hint.Instance.Print("Clearance!!! ThankYOU PLAY!!! ");
            }
            StateProgress();
        }

        public string GetMapName(int PlanId)
        {
            if (PlanId < 0)
            {
                Debug.Log("地图id错误");
                return "";
            }
            return mapeName + PlanId;

        }

        //加载地图数据
        public void  LoadMapAndEntity(int PlanId)
        {
            string LoadMapeName=GetMapName( GlobleDate.data.planId);
            GlobleDate.CurMapData = ConfigController.Instance.ReadConfigMape(LoadMapeName);
            MapeController.Instance.LoadMape();
            EntityController.Instance.LoadEntity();
        }

        public void OutProgress()
        {
            SaveGameData();
        }
    }
}
