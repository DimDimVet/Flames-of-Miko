namespace Registrator
{
    public interface IListDataExecutor
    {
        void ClearData();
        public Construction[] GetData();
        public Construction[] GetEnemys();
        public Construction GetPlayer();
        public Construction GetCamera();
        public void SetData(Construction registrator);
        public Construction GetObjectHash(int hash);
    }
}