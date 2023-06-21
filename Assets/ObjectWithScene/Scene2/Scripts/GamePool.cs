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
    //��������
    public GameObject CreateObj(string key, GameObject obj, Transform parent)
    {
        GameObject item = FindUseableObj(key);
        if (item != null)
        {
            //key��Ϊ�գ�cache���key�ʹ�������
            item.SetActive(true);
        }
        else
        {
            item = GameObject.Instantiate(obj,parent);
            //��ӵ�����
            AddObj(key, item);
        }
        return item;
    }
    /// <summary>
    /// �������������������ж����򼤻����û������Ӳ�ʵ������
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
        //�����ʵ��IReset�ӿڵĶ�����ʹ��OnReset����
        UseObject(item, pos, rotate);
        return item;
    }
    //�������Ӷ���
    public void AddObj(string key,GameObject obj)
    {
        if (!cache.ContainsKey(key))
        {
            cache.Add(key, new List<GameObject>());
        }
        cache[key].Add(obj);
    }
    //���Ҷ�������Ƿ�����Ӧ�Ķ���
    public GameObject FindUseableObj(string key)
    {
        //�������key�б�û�б�ʹ�õ���Ϸ����
        if (cache.ContainsKey(key))
        {
            return cache[key].Find(p => !p.activeSelf);
        }
        return null;
    }
    //ʹ�ö���
    public void UseObject(GameObject obj,Vector3 pos,Quaternion rotate)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rotate;
        obj.SetActive(true);
        obj.GetComponent<IReset>()?.OnReset();
    }

    //���ն���
    public void RecoverObj(GameObject obj)
    {
        obj.SetActive(false);
    }
    //ʹ��Э���ӳٻ�����Դ
    public void RecoverObjDelay(GameObject obj, float time)
    {
        StartCoroutine(RecoverDelay(obj,time));
    }
    private IEnumerator RecoverDelay(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
    //�ͷ���Դ
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
