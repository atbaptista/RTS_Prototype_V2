using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private GameObject _pMenu;
    [SerializeField] private GameObject _hMenu;
    public Selection SelectionCode;
    private bool _paused = false;
    private bool _hActive = false;

    private void Start() {
        PlayGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!_paused) {
                PauseGame();
            } else {
                PlayGame();
            }
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        _pMenu.SetActive(true);
        _paused= true;
        SelectionCode.isPaused = true;

        _hActive = false;
        _hMenu.SetActive(false);
    }

    public void PlayGame() {
        Time.timeScale = 1f;    
        _pMenu.SetActive(false);
        _paused= false;
        SelectionCode.isPaused = false;

        _hActive = false;
        _hMenu.SetActive(false);
    }

    public void Quit() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HelpButton() {
        if (_hActive) {
            _hMenu.SetActive(false);
            _hActive = false;
        }
        else {
            _hMenu.SetActive(true);
            _hActive = true;
        }
        
    }
}
