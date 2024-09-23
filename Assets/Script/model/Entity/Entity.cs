using UnityEngine;
using DG.Tweening; //命名空间
using System.Collections;
using System.Collections.Generic;
namespace model
{
    public enum SummoneEntityType
    {
        Human=0,
        Tree=1,
        SeaBunny=2,

        Human2=3,
        Tree2=4,
        SeaBunny2=5,
        //Wall,
    }
    public enum SummoneEntityState
    {
        Idle,
        Attack,
    }

    public abstract class Entity
    {
        public long id;
        public SummoneEntityType type;
        public SummoneEntityState state;
        public int healthPoint=1;
        public int defensePoint=0;
        public int attackPoint=2;
        public int parryPoint=0;
        public int attackRange=1;
        public int attackCd=2;
        public int summonCD=3;
        public int life=1;
        private float liftTime = 0;
        public bool isDie;

        public GameObject gamobject;
        public Transform transform;
        public GameObject Image;

        public void SetPosition(int x,int y)
        {

            transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
            MapeController.Instance.AddEntity(x, y, this);
        }

        public virtual void Start(GameObject _obj)
        {
            id = Tool.IdGenerater.GenerateId();
            gamobject = _obj;
            transform = gamobject.transform;
            Image = gamobject.transform.GetChild(0).gameObject ;
            Image.transform.localScale = Vector3.zero;

            //放大动画
            Image.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }
        
        // Update is called once per frame
        public virtual void Update()
        {

            liftTime += Time.deltaTime;
            if (liftTime > life + GetAroundFriend())
            {
                Die();
            }

            //随着时间变黑
            Image.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.black, liftTime / life);
        }

        public abstract void FightUpDate();

        public virtual void Summone()
        {

            var pos = GetCanUseGrid();
            if (pos != Vector2Int.left)
            {
                liftTime += 2;
                var summon = EntityFactory.ins.GetEntity(type, pos.x, pos.y);

                GameObject magic = ResourseController.ins.GetPerfab("Magic");
                magic.transform.position = transform.position;
                magic.transform.DOMove(summon.transform.position, 0.2f).SetEase(Ease.OutBounce).OnComplete(
                    () =>
                    {
                        GameObject.Destroy(magic);
                        GameObject icon = ResourseController.ins.GetPerfab("SummonIcon");
                        icon.transform.position = summon.transform.position;
                        icon.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce).OnComplete(
                        () =>
                        {
                            icon.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBounce).OnComplete(
                            () =>
                            {

                                GameObject.Destroy(icon);

                            });

                        });
                    });

                MapeController.Instance.GetBlock(GetPos().x, GetPos().y).aggression = null;
            }
        }

        public virtual  void Attack(Entity hit)
        {
            Shake(0.3f,0.4f);
            hit.Injured(this);

            Image.transform.DOMove(new Vector3(hit.GetPos().x + 0.5f, hit.GetPos().y + 0.5f), 0.2f).SetEase(Ease.Linear).OnComplete(() => {
                Image.transform.DOMove(new Vector3(GetPos().x+0.5f,GetPos().y+0.5f), 0.2f).SetEase(Ease.Linear);
            }); ;
        }
        
        public virtual  void Injured(Entity attacker)
        {
            Shake(0.1f);
            int damage= GlobleDate.GetDamage(attacker, this);
            healthPoint -= damage;
            if (damage==0)
                Hint.Instance.DamagePrint(transform.position, "Miss");
            else 
                Hint.Instance.DamagePrint(transform.position,"-"+ damage.ToString());
            if (healthPoint <= 0)
            {
                Die();

                attacker.Kill(this);
            }
        }

        public virtual void Injured(int damage)
        {
            if (transform == null)
                return;

            Shake(0.1f);
            healthPoint -= damage;
            Hint.Instance.DamagePrint(transform.position, damage.ToString());
            if (healthPoint <= 0)
            {
                Die();
            }
        }


        public Vector2Int GetCanUseGrid()
        {
            int Range = 1;
            List<Vector2Int> canuse = new List<Vector2Int>();
            for (int i = -Range; i < Range + 1; i++)
            {
                for (int j = -Range; j < Range + 1; j++)
                {
                    //被侵略的地块无法召唤
                    var a = MapeController.Instance.GetBlock(GetPos().x + i, GetPos().y + j);
                    if (a != null)
                    if (a.aggression != null)
                    {
                        if(EntityController.Instance.IsEnemy(this.type  , a.aggression.type ))
                        {
                            continue;
                        }
                    }

                    if (!EntityController.Instance.occupy(GetPos().x + i, GetPos().y + j) && 
                        !MapeController.Instance.isOutMap(GetPos().x + i, GetPos().y + j))
                    {
                        canuse.Add( new Vector2Int(GetPos().x + i, GetPos().y + j));
                    }
                }
            }
            if (canuse.Count > 0)
            {
                return canuse[UnityEngine.Random.Range(0, canuse.Count)];
            }


            return Vector2Int.left;
        }

        /// <summary>
        /// 周围的伙伴数量
        /// </summary>
        /// <returns></returns>
        public int GetAroundFriend()
        {
            int firendNum=0;
            int Range = 1;
            for (int i = -Range; i < Range + 1; i++)
            {
                for (int j = -Range; j < Range + 1; j++)
                {
                    var entity = MapeController.Instance.GetEntity(GetPos().x + i, GetPos().y + j);
                    if (entity != null)
                    {
                        if (entity == this) continue;
                        if(EntityController.Instance.IsEnemy(entity.type, type))
                        {
                            firendNum++;
                        }
                    }
                }
            }
            return firendNum;
        }

        public virtual void Shake(float height=0.2f,float time=0.2f)
        {
            if (isDie) return;
            Image.transform.DOMove(Image.transform .position + height * Vector3.up, time/2).SetEase(Ease.Linear).OnComplete(() => {
                Image.transform.DOMove(Image.transform.position - height * Vector3.up, time/2).SetEase(Ease.Linear);
            }); ;
        }

        public virtual void Die()
        {
            if (isDie) return;
            isDie = true;
            Hint.Instance.DamagePrint(transform .position ,"Die");

            EntityController.Instance.RemoveEntity(this);

            Image.transform.DOScale(Vector3.zero , 1f).SetEase(Ease.OutBounce).OnComplete(() => {


                Timer.Register(1,()=> {

                    gamobject.SetActive(false);
                    GameObject.Destroy(gamobject);
                });
            });

        }

        public void Kill(Entity dieEntity)
        {
            bool isEnemy= EntityController.Instance. IsEnemy(dieEntity.type, type);
            if (isEnemy)
            {

                if (EntityController.Instance.GetPopulationType(type) == populationType.Human)
                {
                    GameObject gold = ResourseController.ins.GetPerfab("Gold");
                    gold.transform.position = dieEntity.transform.position;
                    gold.transform.DOMove(Camera.main.transform.position + new Vector3(-5, 4), 1f).SetEase(Ease.Linear).OnComplete(() => {
                        GameObject.Destroy(gold);
                    });
                    ProgressController.Instance.GetGold(GlobleDate.rewordGold);

                }
                
            }
        }

        public Vector2Int GetPos()
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            return new Vector2Int(x,y);
        }
    }
}