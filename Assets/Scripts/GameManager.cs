using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Resource Values")] 
    [SerializeField] private int _gelatin;
    [SerializeField] private int _gold;

    [Header("UI References")] 
    [SerializeField] private Text _gelatinText;
    [SerializeField] private Text _goldText;
    // [SerializeField] private bool UI_Chanage_Immediately;

    // 애니메이션을 위한 현재 표시 값
    private float _displayedGelatin;
    private float _displayedGold;

    [Header("Animation Settings")] 
    [SerializeField] private float _smoothSpeed = 0.5f;
    [SerializeField] private bool _saveOnExit = true;

    // 상수
    private const string GELATIN_SAVE_KEY = "Gelatin";
    private const string GOLD_SAVE_KEY = "Gold";

    #region 속성(Properties)

    // 게임 내 자원에 대한 속성
    public int Gelatin
    {
        get { return _gelatin; }
        set
        {
            _gelatin = value;
            // 값이 변경될 때마다 저장 (선택적)
            // SaveResources();
        }
    }

    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            // 값이 변경될 때마다 저장 (선택적)
            // SaveResources();
        }
    }

    #endregion

    private void Awake()
    {
        LoadResources();
    }

    private void Start()
    {
        // 초기 표시 값 설정
        _displayedGelatin = _gelatin;
        _displayedGold = _gold;

        // 첫 프레임에 UI 값 설정
        UpdateUI(true);
    }

    private void LateUpdate()
    {
        // 부드러운 UI 업데이트
        UpdateUI(false);
    }

    /// <summary>
    /// UI 텍스트 업데이트
    /// </summary>
    /// <param name="immediate">즉시 업데이트 여부 (부드러운 효과 없음)</param>
    private void UpdateUI(bool immediate)
    {
        // 현재 표시 값을 대상 값까지 부드럽게 변경
        if (immediate)
        {
            _displayedGelatin = _gelatin;
            _displayedGold = _gold;
        }
        else
        {
            _displayedGelatin = Mathf.Lerp(_displayedGelatin, _gelatin, _smoothSpeed * Time.deltaTime * 10);
            _displayedGold = Mathf.Lerp(_displayedGold, _gold, _smoothSpeed * Time.deltaTime * 10);
        }

        // 값의 차이가 작을 때는 정확한 값으로 설정 (미세한 애니메이션 방지)
        if (Mathf.Abs(_displayedGelatin - _gelatin) < 0.1f)
            _displayedGelatin = _gelatin;
        if (Mathf.Abs(_displayedGold - _gold) < 0.1f)
            _displayedGold = _gold;

        // UI 텍스트 업데이트
        _gelatinText.text = string.Format("{0:n0}", (int)_displayedGelatin);
        _goldText.text = string.Format("{0:n0}", (int)_displayedGold);
    }

    /// <summary>
    /// 자원 추가 메서드
    /// </summary>
    public void AddResources(int gelatinAmount, int goldAmount)
    {
        _gelatin += gelatinAmount;
        _gold += goldAmount;
    }

    /// <summary>
    /// 자원 저장
    /// </summary>
    public void SaveResources()
    {
        PlayerPrefs.SetInt(GELATIN_SAVE_KEY, _gelatin);
        PlayerPrefs.SetInt(GOLD_SAVE_KEY, _gold);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 자원 불러오기
    /// </summary>
    private void LoadResources()
    {
        _gelatin = PlayerPrefs.GetInt(GELATIN_SAVE_KEY, 0);
        _gold = PlayerPrefs.GetInt(GOLD_SAVE_KEY, 0);
    }

    private void OnApplicationQuit()
    {
        if (_saveOnExit)
        {
            SaveResources();
        }
    }
}