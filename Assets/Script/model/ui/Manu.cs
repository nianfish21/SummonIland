using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace model {

    public class Manu :SingletonMonoBehaviour<Manu>
    {
        public GameObject Show;
        public Button GameStart;
        public Button Gameleave;
        public Button GameNext;
        public Button GameNextPlan;
        public Button BackManu;

        public GameObject set;
        public GameObject startManu;
        // Start is called before the first frame update
        void Start()
        {
            GameStart.onClick.AddListener(start);
            Gameleave.onClick.AddListener(Exit);
            GameNextPlan.onClick.AddListener(NextPlan);
            BackManu.onClick.AddListener(Back);
            
            set.gameObject.SetActive(false  );
            GameNext.gameObject.SetActive(false  );
            GameNextPlan.gameObject.SetActive(false  );
            startManu.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
        }

        //-------------打开ui------------

        public void OpenStartManu()
        {
            startManu.SetActive(true);
        }

        /// <summary>
        /// 开启下一个关卡
        /// </summary>
        public void OpenNextPlan()
        {
            GameNextPlan.gameObject.SetActive(true);
            GameNext.gameObject.SetActive(false);
        }
        

        public void OpenSet()
        {
            set.SetActive(true);
        }

        //--------功能-------------
        private void start()
        {
            ProgressController.Instance.GameStart();
            startManu.gameObject.SetActive(false);
        }

        private void Exit()
        {
            ProgressController.Instance.Exit();
        }
        

        private void NextPlan()
        {
            ProgressController.Instance.NextPlan();
            GameNextPlan.gameObject.SetActive(false);
        }

        private void Back()
        {
            ProgressController.Instance.BackManu();
            set.SetActive(false);
        }


    }
}

