using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MaterialList : ICollection<MaterialInfo>
{
    public List<MaterialInfo> materialInfos = new List<MaterialInfo>();

    private Dictionary<GvfxType, MaterialInfo> map = new Dictionary<GvfxType, MaterialInfo>();
    
    public IEnumerator<MaterialInfo> GetEnumerator()
    {
       return materialInfos.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Set(List<MaterialInfo> source)
    {
        materialInfos.Clear();
        map.Clear();
        
        source.ForEach(info =>
        {
            Insert(0, info);
        });
        Sort();
    }

    public void Add(MaterialInfo item)
    {
       materialInfos.Insert(0,item);
       Sort();
    }

    public void Insert(int index, MaterialInfo item)
    {
        materialInfos.Insert(index, item);
        map[item.ID] = item;
    }

    public void SetPriority(GvfxType id, int priority)
    {
        if (map.TryGetValue(id, out MaterialInfo info))
        {
            info.Priority = priority;
            Sort();
        }
    }

    public void Clear()
    {
       materialInfos.Clear();
       map.Clear();
    }

    public bool Contains(MaterialInfo item)
    {
       return materialInfos.Contains(item);
    }

    public void CopyTo(MaterialInfo[] array, int arrayIndex)
    {
        materialInfos.CopyTo(array, arrayIndex);
    }

    public bool Remove(MaterialInfo item)
    {
        if (materialInfos.Remove(item))
        {
            map.Remove(item.ID);
            return true;
        }
        return false;
    }

    public void Sort()
    {
        materialInfos.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }

    public int Count => materialInfos.Count;
    public bool IsReadOnly => false;
}