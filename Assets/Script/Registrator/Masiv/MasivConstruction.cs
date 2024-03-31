using System;

namespace Registrator
{
    public class MasivConstruction<T> where T : IConstruction
    {
        public bool CompareHash(T[] massivObject, T constructor)
        {
            for (int i = 0; i < massivObject.Length; i++)
            {

                if (constructor.Hash == massivObject[i].Hash)
                {
                    return true;
                }
            }
            return false;
        }
        public T[] Creat(T intObject, T[] massivObject)
        {
            bool isStop = false;
            if (massivObject != null)
            {
                for (int i = 0; i < massivObject.Length; i++)
                {
                    if (!isStop)
                    {
                        if (massivObject[i].Hash == 0)
                        {
                            massivObject[i] = intObject;
                            isStop = true;
                        }
                    }
                }
                if (!isStop)
                {
                    int newLength = massivObject.Length + 1;
                    Array.Resize(ref massivObject, newLength);
                    massivObject[newLength - 1] = intObject;
                    return massivObject;
                }
            }
            else
            {
                massivObject = new T[] { intObject };
                return massivObject;
            }
            return massivObject;
        }
        public void Clean(T[] massivObject)
        {
            if (massivObject != null)
            {
                Array.Clear(massivObject, 0, massivObject.Length);
                return;
            }
        }
    }
}

