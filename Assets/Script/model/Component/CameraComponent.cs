using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace model
{
    public class CameraComponent :SingletonMonoBehaviour<CameraComponent>
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(ProgressController.Instance.IsGame)
                dragCamera();
        }

        public void GoToPorpulation()
        {
            Vector3 center=Vector3.zero ;
            int index=0;
            foreach (var entity in EntityController.Instance.entityPool)
            {
                populationType poputype = EntityController.Instance.GetPopulationType(entity.type);
                if (poputype == null)
                    continue;
                if (poputype == (populationType)GlobleDate.data.populationType)
                {
                    center += entity.transform.position;
                    index++;
                }
            }
            if (index == 0) return;
            center = center / index;

            // Ŀ��λ��
            Vector3 targetPosition = new Vector3(center.x, center.y, -10);
            // �ƶ���Ŀ��λ��
            transform.DOMove(targetPosition, 1f).SetEase(Ease.Linear);
            //transform.position = new Vector3(center.x,center .y,-10);


        }

        Vector3 dragOrigin;
        float dragSpeed=1;
        public void dragCamera()
        {

            // ��ȡ�����ֵĹ�����
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.orthographicSize += scrollAmount * 3;


            // ����������Ƿ���
            if (Input.GetMouseButtonDown(1))
            {
                // ��¼��갴��ʱ��λ��
                dragOrigin = Input.mousePosition;
                Debug.Log("Input.GetMouseButtonDown(2)");
                return;
            }

            // ����������Ƿ������ס
            if (!Input.GetMouseButton(1)) return;

            // ��������ƶ���ƫ����
            Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            // �ƶ����
            Camera.main.transform.Translate(move, Space.World);
            
        }
    }

}

