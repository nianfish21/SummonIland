using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;


namespace model
{
    public class EntityController : SingletonMonoBehaviour<EntityController>
    {
        public List<Entity> entityPool = new List<Entity>();

        public void AddEntity(Entity entity)
        {
            if (entityPool.Contains(entity))
            {
                Debug.Log($"对象{entity.id}已经存在");
            }
            else
            {
                entityPool.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (entityPool.Contains (entity))
            {
                entityPool.Remove(entity);
            }
        }
        public List<Entity> copeAll()
        {
            List<Entity> entityAll = new List<Entity>();
            foreach (var a in entityPool)
            {
                entityAll.Add(a);
            }
            return entityAll;
        }

        public void Clear()
        {
            foreach (var entity in copeAll())
            {
                
                    RemoveEntity(entity);
                    entity.Die();
                
            }
        }
        

        public List<Entity> GetAroundEntitys(Entity entity,int range=1)
        {
            return MapeController.Instance.AroundBlockContains(entity, range);
        }

        public bool occupy(int x,int y)
        {
            foreach(var a in entityPool)
            {
                if(a.GetPos().x==x && a.GetPos().y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadEntity()
        {
            Clear();

            var MapConfig =GlobleDate.CurMapData;
            foreach (var data in MapConfig.EntityData)
            {
                var humanEntityFirst = EntityFactory.ins.GetEntity((SummoneEntityType)data.EntityType,data.x,data.y);
                Debug.Log("刷新角色");
            }
        }


        public Entity GetFar(Entity center,int range,bool findEnemy)
        {
            Entity farEntity=null;
            float farDis=0;
            populationType centerType = GetPopulationType(center.type);
            foreach (var a in GetAroundEntitys(center ,range))
            {
                if (findEnemy)
                {
                    if (centerType == GetPopulationType(a.type))
                        continue;
                }else
                {
                    if (centerType != GetPopulationType(a.type))
                        continue;
                }
                float dis = Vector3.Distance(center.transform .position ,a .transform .position );
                if (dis>= farDis)
                {
                    farEntity = a;
                    farDis = dis;
                }
            }
            return farEntity;

        }


        public Entity GetNear(Entity center, int range, bool findEnemy)
        {
            Entity nearEntity = null;
            float nearDis = range;
            populationType centerType = GetPopulationType(center.type);
            foreach (var a in GetAroundEntitys(center, range))
            {
                if (findEnemy)
                {
                    if (centerType == GetPopulationType(a.type))
                        continue;
                }
                else
                {
                    if (centerType != GetPopulationType(a.type))
                        continue;
                }
                float dis = Vector3.Distance(center.transform.position, a.transform.position);
                if (dis <= nearDis)
                {
                    nearEntity = a;
                    nearDis = dis;
                }
            }
            return nearEntity;

        }

        public populationType GetPopulationType(SummoneEntityType type)
        {
            switch (type) {
                case SummoneEntityType.Human:
                    return populationType.Human;
                case SummoneEntityType.SeaBunny:
                    return populationType.SeaBunny;
                case SummoneEntityType.Tree:
                    return populationType.Tree;
                case SummoneEntityType.Human2:
                    return populationType.Human;
                case SummoneEntityType.SeaBunny2:
                    return populationType.SeaBunny;
                case SummoneEntityType.Tree2:
                    return populationType.Tree;

                default:
                    return populationType.None;

            }
        }

        public bool IsEnemy(SummoneEntityType a,SummoneEntityType b )
        {
            if(GetPopulationType(a)== populationType.None|| GetPopulationType(b) == populationType.None)
            {
                return false; 
            }
            if (GetPopulationType(a) == GetPopulationType(b))
            {
                return false;
            }
            return true;
        }
    }
}