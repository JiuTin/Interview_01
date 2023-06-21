using System.Collections.Generic;

public class DataApater <T>
{
    List<T> allData = new List<T>();
    LinkedList<T> currentData = new LinkedList<T>();
    /// <summary>
    /// 默认返回第一个数据，如果有其它值，则返回当前显示列表的第一个数据的前一个数据
    /// </summary>
    /// <returns></returns>
    public T GetHeaderData()
    {
        if (allData.Count == 0)
        {
            return default(T);
        }
        if (currentData.Count == 0)
        {
            T header = allData[0];
            currentData.AddFirst(header);
            return header;
        }
        T t = currentData.First.Value;
        int index = allData.IndexOf(t);
        if (index != 0)
        {
            T header= allData[index - 1];
            currentData.AddFirst(header);
            return header;
        }
        return default(T);    
    }
    public bool RemoveHeader()
    {
        if (currentData.Count == 0 || currentData.Count == 1)
        {
            return false;
        }
        currentData.RemoveFirst();
        return true;
    }
    public T GetLastData()
    {
        if (allData.Count == 0)
        {
            return default(T);
        }
        if (currentData.Count == 0)
        {
            T last = allData[0];
            currentData.AddLast(last);
            return last;
        }
        T t = currentData.Last.Value;
        int index = allData.IndexOf(t);
        if (index != allData.Count - 1)
        {
            T last = allData[index + 1];
            currentData.AddLast(last);
            return last;
        }
        return default(T);
    }

    public bool RemoveLastData()
    {
        if (currentData.Count == 0 || currentData.Count == 1)
        {
            return false;
        }
        currentData.RemoveLast();
        return true;
    }
    public void InitData(T[] list)
    {
        allData.Clear();
        currentData.Clear();
        allData.AddRange(list);
    }
    public void InitData(List<T> list)
    {

        InitData(list.ToArray());
    }
    public void AddData(T[] t)
    {
        allData.AddRange(t);
    }
    public void AddData(List<T> list)
    {
        AddData(list.ToArray());
    }
}
