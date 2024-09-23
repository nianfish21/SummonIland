using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace model
{
    public enum BlockType
    {
        None=0,
        Water=1,
        Grass=2,
        Rock=3,
    }

    [System.Serializable]
    public class block
    {
        public int x;
        public int y;
        public int height;
        public BlockType blockType;
        public Entity aggression; 
    }

    public class BlockEntitys
    {
        public  List<Entity> containList=new List<Entity>();

        public void Clear()
        {
            containList.Clear();
        }
    }


    public class MapeController : SingletonMonoBehaviour<MapeController>
    {
        public List<Sprite> sprites;
        public int mapx=32;
            public int mapy = 32;

        public Vector3 offset = new Vector3(0.5f, 0.5f,0);
        private block[,] blockData = new block[32, 32];
        private BlockEntitys[,] blockContainEntitys = new BlockEntitys[32, 32];
        private GameObject [,] blockGamobject = new GameObject[32, 32];
        public GameObject MapeParent;

        private void Awake()
        {
            blockData = new block[mapx, mapy];
            blockContainEntitys = new BlockEntitys[mapx, mapy];
            blockGamobject = new GameObject[mapx, mapy];
            for (int i = 0; i < mapx; i++)
            {
                for (int j = 0; j < mapx; j++)
                {
                    blockData[i, j] = new block() {x=i,y=j,height=0,blockType= BlockType.None };
                    blockContainEntitys[i, j] = new BlockEntitys();

                }
            }
            if (MapeParent == null)
            {
                MapeParent = new GameObject("Mape");
                MapeParent.transform.position = Vector3.zero;
                
                GameObject.DontDestroyOnLoad(MapeParent);
            }
        }


        public void ShaderMap()
        {

            for (int i = 0; i < mapx; i++)
            {
                for (int j = 0; j < mapx; j++)
                {
                    if(blockData[i, j].blockType!= BlockType.None)
                    {
                        if(blockGamobject[i, j] == null)
                        {
                            GameObject image = ResourseController.ins.GetPerfab("Block");
                            image.transform.SetParent(MapeParent.transform);
                            image.transform.localPosition = new Vector3(i,j,0)+ offset;
                            blockGamobject[i, j] = image;
                            
                        }
                        if(blockGamobject[i, j].GetComponent<SpriteRenderer>())
                        {
                            var spriteRender = blockGamobject[i, j].GetComponent<SpriteRenderer>();
                            spriteRender.sprite = GetTypeSprite(blockData[i, j].blockType);
                            
                            //Debug.Log("set Block Sprite: " + blockData[i, j].blockType);
                            blockGamobject[i, j].SetActive(true);
                        }
                    }else
                    {if(blockGamobject[i, j] != null)
                        {
                            blockGamobject[i, j].SetActive(false);
                        }
                    }

                }
            }
        }

        public void AddEntity(int x,int y,Entity a)
        {
            if (IndexIsOut(x, y))
                return;
            blockContainEntitys[x, y].containList.Add(a);
        }
        public Entity GetEntity(int x, int y)
        {
            if (IndexIsOut(x, y))
                return null;
            if(blockContainEntitys[x, y].containList.Count > 0)
                return blockContainEntitys[x, y].containList[0];
            
            return null;
        }

        public Sprite GetTypeSprite(BlockType type)
        {
            switch (type)
            {
                case BlockType.None:
                    return null;
                case BlockType.Water:
                    return sprites[0];
                case BlockType.Grass:
                    return sprites[1];
                case BlockType.Rock:
                    return sprites[2];
            }
            return null;
        }

        float _time = 0;
        public void Update()
        {
            _time += Time.deltaTime;
            if (_time > 1f)
            {
                UpdateblockEntitys();
            }
        }

        public block GetBlock(int x,int y)
        {

            if (IndexIsOut(x, y))
                return null;
            return blockData[x, y];
        }

        public bool IndexIsOut(int x, int y)
        {
            if (x >= mapx || x < 0) return true ;
            if (y >= mapy || y < 0) return true;
            return false;
        }

        public void SetBlock(int x, int y, BlockType type)
        {
            if (IndexIsOut(x, y))
                return;

            //Debug.Log(blockData.Length );
            blockData[x, y].blockType = type;

        }

        public void  ClearMape()
        {

            for (int i = 0; i < mapx; i++)
            {
                for (int j = 0; j < mapx; j++)
                {
                    if (blockData[i, j].blockType != BlockType.None)
                    {
                        if (blockGamobject[i, j] != null)
                        {
                            blockGamobject[i, j].SetActive(false);
                        }
                    }

                }
            }
        }

        public List<Entity> GetBlockEntitys(int x,int y)
        {

            return blockContainEntitys[x, y].containList;
        }


        /// <summary>
        /// 加载新地图
        /// </summary>
        /// <param name="MapeName"></param>
        public void LoadMape()
        {
            var blockConfig = GlobleDate.CurMapData;
            var blockdata = blockConfig.OutPutMapBlock();
            MapeController.Instance.setBlockData(blockdata);
            ShaderMap();

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="range">地块范围</param>
        /// <returns></returns>
        public List<Entity> AroundBlockContains(Entity entity,int Range)
        {
            List<Entity> aroundEntitys = new List<Entity>();
            Vector2Int pos = entity.GetPos();

            for (int i = -Range; i < Range+1; i++)
            {
                for (int j = -Range; j < Range+1; j++)
                {

                    if (!IndexIsOut(pos.x+i, pos.y+j))
                        aroundEntitys.AddRange(blockContainEntitys[pos.x + i, pos.y + j].containList);
                }
            }
            return aroundEntitys;
        }

        public Vector2Int GetNearBlock(Entity entity )
        {
            float disNear = 10000;
            int nearX=0;
            int nearY=0;
            for (int i = 0; i < mapx; i++)
            {
                for (int j = 0; j < mapx; j++)
                {
                    if (blockData[i, j].blockType != BlockType.None)
                    {
                        Vector3 BlockCenter = new Vector3(i + 0.5f, j + 0.5f,0);
                        float dis = Vector3.Distance(entity.transform.position, BlockCenter);
                        if (dis < disNear)
                        {
                            disNear = dis;
                            nearX = i;
                            nearY = j;
                        }
                    }
                }
            }
            Vector2Int pos =  new Vector2Int(nearX,nearY);
            if (nearY == 0 && nearX == 0)
                Debug.Log("输出了0,0坐标");
            return pos;

        }


        public bool isOutMap(int x ,int y)
        {
            if (Instance.IndexIsOut(x,y))
            {
                return true;
            }
            if (Instance.GetBlock(x, y).blockType == BlockType.None)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  用来减少cpu压力的实例装箱
        /// </summary>
        public void UpdateblockEntitys()
        {
            ClearBlockEntitys();
            foreach (var a in EntityController.Instance.entityPool)
            {
                Vector2Int pos = a.GetPos();
                if (IndexIsOut(pos.x, pos.y))
                    return;
                blockContainEntitys[pos.x, pos.y].containList.Add(a );
            }
        }

        public void ClearBlockEntitys()
        {
            foreach (var a in blockContainEntitys)
            {
                a.Clear();
            }
        }


        public block[,] GetBlockData()
        {
            return blockData;
        }

        public void  setBlockData(block[,] date)
        {
            blockData = date;
        }

    }
}