using System;
using System.Collections.Generic;

public static class ArrayHelper
{
    public static T Find<T>(this T[] array, Func<T, bool> condition)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
            {
                return array[i];
            }
        }
        return default(T);
    }
    public static T[] FindAll<T>(this T[] array, Func<T,bool> condition)
    {
        List<T> temps = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
            {
                temps.Add(array[i]);
            }
        }
        return temps.ToArray();
    }
    //public static T GetMin<T, Q>(this T[] array, Func<T, float> condition)
    //{
    //    T min = array[0];
    //    for (int i = 1; i < array.Length; i++)
    //    {
    //        if (condition(min).CompareTo(condition(array[i])) > 0)
    //        {
    //            min = array[i];
    //        }
    //    }
    //    return min;
    //}
    //public static Q[] Select<Q, T>(this T[] array, Func<T, Q> condition)
    //{
    //    Q[] result = new Q[array.Length];
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        result[i] = condition(array[i]);
    //    }
    //    return result;
    //}
}
