using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{
    public class init : SingletonMonoBehaviour<init>
    {
        public List< GameObject> UnDestory;
        private GameObject Globle;

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log("Start");
            Globle = this.gameObject;

            Globle.AddComponent<ConfigController>();
            Globle.AddComponent<EntityController>();
            Globle.AddComponent<ProgressController>();

            foreach(GameObject unDestoryObj in UnDestory)
            {
                if(unDestoryObj!=null)
                DontDestroyOnLoad(unDestoryObj);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}