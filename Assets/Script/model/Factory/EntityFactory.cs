using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;


namespace model
{
    public class EntityFactory:single<EntityFactory>
    {

        public Entity GetEntity(SummoneEntityType type ,int x, int y)
        {
            if(EntityController.Instance.occupy(x, y))
            {
                Debug.Log($"创建失败{x},{ y}");
                return null;
            }
            if(MapeController.Instance.isOutMap(x, y))
            {
                Debug.Log($"创建失败{x},{ y}");
                return null;
            }
            Entity entityRetun;
            switch (type) {
                case SummoneEntityType.Human:
                    entityRetun = EntityFactory.ins.CreatHumanEntity( x , y);
                    return entityRetun;
                case SummoneEntityType.SeaBunny:
                    entityRetun = EntityFactory.ins.CreatSeaBunnyEntity( x , y );
                    return entityRetun;
                case SummoneEntityType.Tree:
                    entityRetun = EntityFactory.ins.CreatTreeEntity(x,y);
                    return entityRetun;
                case SummoneEntityType.SeaBunny2:
                    entityRetun = EntityFactory.ins.CreatSeaBunny2Entity(x,y);
                    return entityRetun;
                case SummoneEntityType.Human2:
                    entityRetun = EntityFactory.ins.CreatHuman2Entity(x,y);
                    return entityRetun;

            }
            Debug.Log("GetEntity 错误类型");
            return null;
        }



        public HumanEntity CreatHumanEntity(int x, int y)
        {
            HumanEntity entity = new HumanEntity();
            entity.type = SummoneEntityType.Human;
            entity.attackPoint = 2;
            entity.healthPoint = 4;
            entity.attackRange =2;
            entity.parryPoint = 25;
            entity.summonCD = 2;
            entity.attackCd = 2;
            entity.life = 10;
            GameObject obj = ResourseController.ins.GetPerfab("Human");
            entity.Start(obj);
            entity.SetPosition(x,y);
            EntityController.Instance.AddEntity(entity);
            return entity;
        }

        public Human2Entity CreatHuman2Entity(int x, int y)
        {
            Human2Entity entity = new Human2Entity();
            entity.type = SummoneEntityType.Human2;
            entity.attackPoint = 2;
            entity.healthPoint = 2;
            entity.attackRange = 3;
            entity.parryPoint = 0;
            entity.summonCD = 3;
            entity.life = 10;
            entity.attackCd = 1;
        GameObject obj = ResourseController.ins.GetPerfab("Human2");
            entity.Start(obj);
            entity.SetPosition(x, y);
            EntityController.Instance.AddEntity(entity);
            return entity;
        }

        public SeaBunnyEntity CreatSeaBunnyEntity(int x, int y)
        {
            SeaBunnyEntity entity = new SeaBunnyEntity();
            entity.type = SummoneEntityType.SeaBunny;
            entity.attackPoint = 2;
            entity.healthPoint = 2;
            entity.attackRange = 1;
            entity.parryPoint = 50;
            entity.summonCD = 2;
            entity.attackCd = 1;
            entity.life = 10;
            GameObject obj = ResourseController.ins.GetPerfab("SeaBunny");
            entity.Start(obj);

            entity.SetPosition(x, y);
            EntityController.Instance.AddEntity(entity);
            return entity;
        }

        public SeaBunny2Entity CreatSeaBunny2Entity(int x, int y)
        {
            SeaBunny2Entity entity = new SeaBunny2Entity();
            entity.type = SummoneEntityType.SeaBunny2;
            entity.attackPoint = 2;
            entity.healthPoint = 1;
            entity.attackRange = 3;
            entity.parryPoint = 0;
            entity.summonCD = 2;
            entity.attackCd = 1;
            entity.life = 10;
            GameObject obj = ResourseController.ins.GetPerfab("SeaBunny2");
            entity.Start(obj);

            entity.SetPosition(x, y);
            EntityController.Instance.AddEntity(entity);
            return entity;
        }

        public TreeEntity CreatTreeEntity(int x, int y)
        {
            TreeEntity entity = new TreeEntity();
            entity.type = SummoneEntityType.Tree;
            entity.attackPoint = 0;
            entity.healthPoint = 30;
            entity.attackRange = 1;
            entity.parryPoint = 0;
            entity.summonCD = 2;
            entity.life = 10;
            GameObject obj = ResourseController.ins.GetPerfab("Tree");
            entity.Start(obj);

            entity.SetPosition(x, y);
            EntityController.Instance.AddEntity(entity);
            return entity;
        }

        //public WallEntity CreatWallEntity()
        //{
        //    WallEntity entity = new WallEntity();
        //    entity.type = SummoneEntityType.Wall;
        //    entity.attackPoint = 2;
        //    entity.healthPoint = 3;
        //    entity.attackRange = 1;
        //    entity.parryPoint = 25;
        //    entity.summonCD = 2;
        //    GameObject obj = ResourseController.ins.GetPerfab("Wall");
        //    entity.Start(obj);

        //    EntityController.Instance.AddEntity(entity);
        //    return entity;
        //}

    }
}