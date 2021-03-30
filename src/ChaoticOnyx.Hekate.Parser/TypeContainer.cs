#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Контейнер для по-элементного считывания содержимого.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeContainer<T>
    {
        /// <summary>
        ///     Длина всей коллекции.
        /// </summary>
        public int Length
        {
            get;
        }

        /// <summary>
        ///     Коллекция контейнера.
        /// </summary>
        public List<T> List
        {
            get;
        }

        /// <summary>
        ///     Отступ от начала коллекции.
        /// </summary>
        public int Offset
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Последняя позиция от начала коллекции.
        /// </summary>
        public int Position
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Если текущий отступ от начала коллекции в конце и дальше возвращает true.
        /// </summary>
        public bool IsEnd => Offset >= Length;

        public TypeContainer(params T[] list)
        {
            List   = list.ToList();
            Length = List.Count;
        }

        public TypeContainer(List<T> list)
        {
            List   = list;
            Length = List.Count;
        }

        /// <summary>
        ///     Устанавливает отступ в начало коллекции.
        /// </summary>
        public virtual void Reset()
        {
            Offset = 0;
            Start();
        }

        /// <summary>
        ///     Устанавливает позицию на текущий отступ.
        /// </summary>
        public virtual void Start()
        {
            Position = Offset;
        }

        /// <summary>
        ///     Возвращает элемент на текущем отступе и увеличивает отступ на единицу.
        /// </summary>
        /// <returns></returns>
        public virtual T Read()
        {
            return List[Offset++];
        }

        /// <summary>
        ///     Возвращает элемент на определённом количестве шагов от текущего отступа и не передвигает его.
        /// </summary>
        /// <param name="offset">Количество шагов от текущего отступа.</param>
        /// <returns>Возвращает null если указанный отступ выходит за конец коллекции.</returns>
        public virtual T? Peek(int offset = 1)
        {
            var result = Offset + offset - 1;

            return result >= Length ? default : List[result];
        }

        /// <summary>
        ///     Передвигает отступ на указанное количество шагов.
        /// </summary>
        /// <param name="offset">Количество шагов</param>
        public virtual void Advance(int offset = 1)
        {
            Offset += offset;
        }
    }
}
