using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace model
{




    [System.Serializable]
    public class ConfigMap 
    {
        public blockConfig[] mapBlock;
        public EntityConfig[] EntityData;


        
        public void InPutEntityData(List<Entity> data)
        {
                EntityData = new EntityConfig[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                EntityData[i] = new EntityConfig()
                {
                    x = (int)data[i].transform.position.x,
                    y = (int)data[i].transform.position.y,
                    EntityType = (int)data[i].type
                };

            }
        }

        public void OutPutEntity()
        {

        }

        public void InPutMapBlock(block[,] block)
        {
            mapBlock = new blockConfig[MapeController.Instance.mapx* MapeController.Instance.mapy];
            int index = 0;
            int rows = MapeController.Instance.mapx;
            int cols = MapeController.Instance.mapy;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    mapBlock[index] = new blockConfig();
                    mapBlock[index].x = block[i, j].x;
                    mapBlock[index].y = block[i, j].y;
                    mapBlock[index].height = block[i, j].height;
                    mapBlock[index].blockType = (int)block[i, j].blockType;

                    index++;
                }
            }

        }
        public block[,] OutPutMapBlock()
        {
            block[,] block = new block[MapeController.Instance.mapx, MapeController.Instance.mapy];
            int rows = MapeController.Instance.mapx;
            int cols = MapeController.Instance.mapy;
            int index = 0;
            // 遍历二维数组，将一维数组中的元素填充到二维数组中
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // 确保一维数组的索引不超出范围
                    if (index < mapBlock.Length)
                    {
                        block b = new block();

                        b = new block();
                        b.x = mapBlock[index].x;
                        b.y = mapBlock[index].y;
                        b.height = mapBlock[index].height;
                        b.blockType = (BlockType) mapBlock[index].blockType;

                        block[i, j] = b;
                        index++;
                    }
                    else
                    {
                        // 如果一维数组的长度不足以填满二维数组，可以选择进行一些处理
                        // 这里简单地将超出范围的位置设置为 0
                        block[i, j] = new block(); 
                    }
                }
            }
            return block;
        }
    }



    [System.Serializable]
    public class blockConfig
    {
        public int x;
        public int y;
        public int height;
        public int blockType;
    }


    [System.Serializable]
    public class EntityConfig
    {
        public int x;
        public int y;
        public int EntityType;
    }
}