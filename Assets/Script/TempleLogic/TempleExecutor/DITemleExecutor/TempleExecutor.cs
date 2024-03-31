using Registrator;
using System;
using System.Diagnostics;
using Zenject;

namespace TemleLogic
{

    public class TempleExecutor : ITempleExecutor
    {
        private Construction temple;
        private MasivConstruction<Construction> masiv = new MasivConstruction<Construction>();
        private Construction[] templeExecutorOn;
        public Action<Construction,Construction[]> OnFireTemple { get { return onFireTemple; } set { onFireTemple = value; } }
        private Action<Construction, Construction[]> onFireTemple;
        public Action<Construction[]> OnOffTemples { get { return onOffTemples; } set { onOffTemples = value; } }
        private Action<Construction[]> onOffTemples;
        public Action<float> OnFaithDamage { get { return onFaithDamage; } set { onFaithDamage = value; } }
        private Action<float> onFaithDamage;
        public Action<float> OnPlusSpeedEntity { get { return onPlusSpeedEntity; } set { onPlusSpeedEntity = value; } }
        private Action<float> onPlusSpeedEntity;

        private IListDataExecutor dataList;
        [Inject]
        public void Init(IListDataExecutor _dataList)
        {
            dataList = _dataList;
        }
        public void SetData(int hashTemle)
        {
            temple = dataList.GetObjectHash(hashTemle);
            if (temple.Hash != 0) { templeExecutorOn = masiv.Creat(temple, templeExecutorOn); }
        }
        public void FireTemple(int hashTemle)
        {
            for (int i = 0; i < templeExecutorOn.Length; i++)
            {
                if (templeExecutorOn[i].Hash == hashTemle) { RezultFireTemple(templeExecutorOn[i]); }
            }
        }
        private void RezultFireTemple(Construction temple)
        {
            onFireTemple?.Invoke(temple, templeExecutorOn);
        }
        public Construction[] GetTemples() 
        {
            return templeExecutorOn;
        }
        public void SetTemples(Construction[] temples)
        {
            templeExecutorOn= temples;
        }
        public void OffTemples(Construction[] temples)
        {
            templeExecutorOn = temples;
            onOffTemples?.Invoke(templeExecutorOn);
        }
        //для Андрея
        public void FaithDamage(float damageValue)
        {
            onFaithDamage?.Invoke(damageValue);
        }
        public void PlusSpeedEntity(float percentSpeed)
        {
            onPlusSpeedEntity?.Invoke(percentSpeed);
        }
    }
}

