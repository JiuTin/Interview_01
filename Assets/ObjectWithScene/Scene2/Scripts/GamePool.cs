using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReset
{
    public void OnReset();
}

public class GamePool :MonoBehaviour
{
    public static GamePool Instance;
    public void Awake()
    {
        Instance = this;
    }

    private Dictionary<string, List<GameObject>> cache=new Dictionary<string, List<GameObject>>();
    //创建对象
    public GameObject CreateObj(string key, GameObject obj, Transform parent)
    {
        GameObject item = FindUseableObj(key);
        if (item != null)
        {
            //key不为空，cache添加key和创建对象
            item.SetActive(true);
        }
        else
        {
            item = GameObject.Instantiate(obj,parent);
            //添加到池里
            AddObj(key, item);
        }
        return item;
    }
    /// <summary>
    /// 创建对象，如果对象池里有对象则激活对象，没有则添加并实例对象。
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="rotate"></param>
    /// <returns></returns>
    public GameObject CreateObj(string key, GameObject obj, Vector3 pos, Quaternion rotate)
    {
        GameObject item = FindUseableObj(key);
        if (item == null)
        {
            item = GameObject.Instantiate(obj, pos, rotate);
            AddObj(key, item);
        }
        //如果有实现IReset接口的对象则使用OnReset方法
        UseObject(item, pos, rotate);
        return item;
    }
    //对象池添加对象
    public void AddObj(string key,GameObject obj)
    {
        if (!cache.ContainsKey(key))
        {
            cache.Add(key, new List<GameObject>());
        }
        cache[key].Add(obj);
    }
    //查找对象池里是否有相应的对象
    public GameObject FindUseableObj(string key)
    {
        //池里对象key列表没有被使用的游戏对象
        if (cache.ContainsKey(key))
        {
            return cache[key].Find(p => !p.activeSelf);
        }
        return null;
    }
    //使用对象
    public void UseObject(GameObject obj,Vector3 pos,Quaternion rotate)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rotate;
        obj.SetActive(true);
        obj.GetComponent<IReset>()?.OnReset();
    }

    //回收对象
    public void RecoverObj(GameObject obj)
    {
        obj.SetActive(false);
    }
    //使用协程延迟回收资源
    public void RecoverObjDelay(GameObject obj, float time)
    {
        StartCoroutine(RecoverDelay(obj,time));
    }
    private IEnumerator RecoverDelay(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
    //释放资源
    public void Clear(string key)
    {
        if (cache.ContainsKey(key))
        {
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i]);
            }
        }
        cache.Remove(key);
    }
    public void ClearAll()
    {
        List<string> items = new List<string>(cache.Keys);
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Clear(items[i]);
            }
        }
    }
    public void OnApplicationQuit()
    {
        Instance = null;
    }
}
