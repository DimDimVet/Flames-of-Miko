using UnityEngine;

namespace Registrator
{
    public class ListDataExecutor : IListDataExecutor
    {
        private MasivConstruction<Construction> masiv = new MasivConstruction<Construction>();
        private Construction[] allList, enemyList;
        private Construction camera, player;
        public void SetData(Construction registrator)
        {
            allList = masiv.Creat(registrator, allList);
        }
        public void ClearData()
        {
            if (allList != null) { masiv.Clean(allList); }
        }
        public Construction[] GetData()
        {
            return allList;
        }
        public Construction GetObjectHash(int hash)
        {
            if (allList != null)
            {
                for (int i = 0; i < allList.Length; i++)
                {
                    if (allList[i].Hash == hash)
                    {
                        return allList[i];
                    }
                    else
                    {
                        Construction tempRezult = GetChildrenHash(allList[i], hash);
                        if (tempRezult.Hash != 0) { return tempRezult; }
                    }
                }
            }
            return new Construction();
        }
        private Construction GetChildrenHash(Construction itemAllList, int setHash)
        {
            for (int j = 0; j < itemAllList.ChildrenHash.Length; j++)
            {
                if (itemAllList.ChildrenHash[j] == setHash) { return itemAllList; }
            }
            return new Construction();
        }
        public Construction GetPlayer()
        {
            if (allList != null)
            {
                for (int i = 0; i < allList.Length; i++)
                {
                    if (allList[i].TypeObject is TypeObject.Player)
                    {
                        player = allList[i];
                    }
                }
                return player;
            }
            return new Construction();
        }
        public Construction[] GetEnemys()
        {
            if (allList != null)
            {
                for (int i = 0; i < allList.Length; i++)
                {
                    if (allList[i].TypeObject is TypeObject.Enemy)
                    {
                        enemyList = masiv.Creat(allList[i], enemyList);
                    }
                }
                return enemyList;
            }
            else { return null; }
        }
        public Construction GetCamera()
        {
            if (allList != null)
            {
                for (int i = 0; i < allList.Length; i++)
                {
                    if (allList[i].CameraComponent != null)
                    {
                        camera = allList[i];
                    }
                }
            }
            return camera;
        }
    }
}

