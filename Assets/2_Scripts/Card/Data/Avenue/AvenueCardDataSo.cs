using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Card Data", menuName = "Avenue/Card/Data")]
public class AvenueCardDataSo : ScriptableObject
{
    #region Text

    [Header("[ Text ]")]
    [SerializeField] private string mCodeName;
    [SerializeField] private string mDisplayName;
    [SerializeField][TextArea] private string mDescription;
        
    public string CodeName => mCodeName;
    public string DisplayName => mDisplayName;
    public string Description => mDescription;
        
    #endregion

    #region Texture

    [Header("[ Texture ]")]
    [SerializeField] private AssetReferenceTexture2D mFrontTexture;
    
    public AssetReferenceTexture2D FrontTexture => mFrontTexture;

    #endregion
}