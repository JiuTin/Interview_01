using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryException
{
    public static K GetTry<T, K>(this Dictionary<T,K> dic,T key) {
        K value;
        dic.TryGetValue(key, out value);
        return value;
    }
        
}
