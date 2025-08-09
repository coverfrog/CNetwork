using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data Group", menuName = "Avenue/Card/Group")]
public class AvenueCardDataSoGroup : ScriptableObject
{
    [SerializeField] private List<CountValue<AvenueCardDataSo>> mDataSoList =  new List<CountValue<AvenueCardDataSo>>();

    public List<AvenueCardNetworkData> GetNetworkDataList()
    {
        // -- copy
        List<AvenueCardDataSo> source = new List<AvenueCardDataSo>();
        foreach (CountValue<AvenueCardDataSo> countValue in mDataSoList)
        {
            for (int i = 0; i < countValue.count; i++)
            {
                source.Add(countValue.value);
            }
        }
        
        // -- shuffle
        int count = source.Count;
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < count; j++)
            {
                int r =  Random.Range(0, count);

                (source[i], source[r]) = (source[r], source[i]);
            }
        }
        
        // -- convert
        List<AvenueCardNetworkData> result = new();
        
        foreach (AvenueCardNetworkData networkData in source.Select(AvenueCardDataConverter.ToNetworkData))
        {
            result.Add(networkData);
        }

        // -- result
        return result;
    }
}