using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
namespace model {

    public class Hint : SingletonMonoBehaviour<Hint>
    {

        public Text text;
        public Text Tip;
        public GameObject  damageText;
        public Text Glod;
        public Text plan;
        public float time;
        // Start is called before the first frame update
        void Start()
        {
            if (text == null)
                return;
            text.gameObject.SetActive(false );
            Tip.gameObject.SetActive(true );
            Tip.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            if (Glod == null)
            {
                return;
            }

            if (GlobleDate.data != null)
            {
                Glod.text =  "Glod:"+GlobleDate.data.glod.ToString();
                plan.text = "Island:" + (GlobleDate.data.planId + 1).ToString();
            }else
            {
                Glod.text = "" ;
                plan.text = "" ;
            }
            if (time != 0)
            {
                time -= Time.deltaTime;
                if (time < 0)
                    time = 0;
            }else
            {

                text.gameObject.SetActive(false);
            }
            text.transform.localScale =Vector3.Lerp(text.transform.localScale,Vector3.one * (time>1?1:time), 0.2f) ;


            //if (Input.GetMouseButtonDown(0))
            //{
            //    Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
            //    DamagePrint(position_target,"2");
            //    //Debug.Log("Ë¢ÐÂÇøÓò" + (int)position_target.x + "," + (int)position_target.y);
            //}
        }

        public void Print(string message)
        {
            text.text = message;
            text.gameObject.SetActive(true);
            time = 2;
        }


        public void LeftTip(string message)
        {
            Tip.text = message;
            Tip.gameObject.SetActive(true);
        }

        public void CloseLeftTip()
        {
            Tip.text="";

        }
        public void DamagePrint(Vector3 pos,string damage)
        {
            if (damageText == null) return;
            GameObject damageObj = Instantiate(damageText );
            damageObj.GetComponentInChildren<Text>().text =damage;
            damageObj.SetActive(true);
            damageObj.transform.position = pos;
            damageObj.transform .DOMove(pos+=0.5f*Vector3.up, 1f).SetEase(Ease.Linear).OnComplete(()=> {
                Destroy(damageObj);
            }); ;
        }

    }

}
