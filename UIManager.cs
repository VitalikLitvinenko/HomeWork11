using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _damageButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Image _hpImage;
    [SerializeField] private Toggle _easyToggle;
    [SerializeField] private Toggle _mediumToggle;
    [SerializeField] private Toggle _hardToggle;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _victoryText;

    private string _playerName = "Игрок";
    private int _maxHP = 10;
    private int _currentHP = 10;

    private void Start()
    {
        _hpSlider.navigation = new Navigation { mode = Navigation.Mode.None };
        _menuPanel.SetActive(true);
        _damageButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _victoryText.gameObject.SetActive(false);

        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.wholeNumbers = true;
        _hpSlider.value = _currentHP;

        _continueButton.onClick.AddListener(OnContinueClicked);
        _damageButton.onClick.AddListener(OnDamageClicked);
        _hpSlider.onValueChanged.AddListener(OnSliderChanged);
        _restartButton.onClick.AddListener(OnRestartClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);

        UpdateHP();
    }

    public void TakeDamage()
    {
        _currentHP--;

        Debug.Log($"Игрок {_playerName} получил урон, осталось {_currentHP}");

        UpdateHP();

        if (_currentHP <= 0)
            GameOver();
    }

    public void Victory()
    {
        _victoryText.gameObject.SetActive(true);
        _damageButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    private void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _damageButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OnContinueClicked()
    {
        if (!string.IsNullOrEmpty(_playerNameInput.text))
            _playerName = _playerNameInput.text;

        if (_easyToggle.isOn)
            _maxHP = 10;
        else if (_mediumToggle.isOn)
            _maxHP = 7;
        else if (_hardToggle.isOn)
            _maxHP = 5;

        _currentHP = _maxHP;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.value = _currentHP;

        _menuPanel.SetActive(false);
        _damageButton.gameObject.SetActive(true);

        UpdateHP();
    }

    private void OnDamageClicked()
    {
        TakeDamage();
    }

    private void OnSliderChanged(float value)
    {
        _currentHP = (int)value;
        UpdateHP();
    }

    private void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnQuitClicked()
    {
        Time.timeScale = 1f;
        Debug.Log("Выход из игры");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void UpdateHP()
    {
        _hpText.text = "HP: " + _currentHP.ToString();
        _hpSlider.value = _currentHP;
        _hpImage.fillAmount = (float)_currentHP / _maxHP;
    }
}
