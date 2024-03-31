using Registrator;
using System;

namespace TemleLogic
{
    public interface ITempleExecutor
    {
        void SetData(int hashTemle);
        public void FireTemple(int hashTemle);
        public Action<Construction,Construction[]> OnFireTemple { get; set; }
        public Action<Construction[]> OnOffTemples { get; set; }
        Construction[] GetTemples();
        void SetTemples(Construction[] temples);
        void OffTemples(Construction[] temples);
        //
        Action<float> OnFaithDamage { get; set; }
        void FaithDamage(float damageValue);
        Action<float> OnPlusSpeedEntity { get; set; }
        void PlusSpeedEntity(float percentSpeed);
    }
}
