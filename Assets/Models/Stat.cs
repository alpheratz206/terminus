using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Models
{
    public class Stat<T> where T : IComparable<T>
    {
        public T minValue;
        public T maxValue;

        private T value;

        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        private void SetValue(T incomingValue)
        {
            bool setMin = incomingValue.CompareTo(minValue) <= 0;
            bool setMax = incomingValue.CompareTo(maxValue) >= 0;

            if (setMin)
                value = minValue;

            if (setMax)
                value = maxValue;

            if (!setMin && !setMax)
                value = incomingValue;
        }

        #region Constructors

        public Stat(
            T minValue,
            T maxValue,
            T startingValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            value = startingValue;
        }

        public Stat(
            T minValue,
            T maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            value = maxValue;
        }

        #endregion
    }
}
