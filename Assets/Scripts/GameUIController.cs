﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BulletFlick {
    public class GameUIController : MonoBehaviour {
        [SerializeField] private GameObject scoreboard;
        [SerializeField] private GameObject menu;
        [SerializeField] private Slider xSensitivitySlider;
        [SerializeField] private Slider ySensitivitySlider;

        [SerializeField] private CursorLockMode cursorLockMode = CursorLockMode.Locked;

        private GameManager gameManager;

        private bool showMenu;
        private Text scoreboardText;

        // Use this for initialization
        void Awake () {
            scoreboardText = scoreboard.GetComponentInChildren<Text>();
            scoreboard.SetActive(false);

            menu.SetActive(false);
            showMenu = false;

            
        }

        private void Start () {
            gameManager = GameManager.Instance();

            xSensitivitySlider.value = gameManager.XSensitivity;
            ySensitivitySlider.value = gameManager.YSensitivity;
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                UpdateScoreboard();
                scoreboard.SetActive(true);
            } else if (Input.GetKeyUp(KeyCode.Tab)) {
                scoreboard.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                showMenu = !showMenu;
                menu.SetActive(showMenu);
                if (showMenu) {
                    gameManager.DisableLocalPlayer();
                    cursorLockMode = CursorLockMode.None;
                    Cursor.visible = true;
                } else {
                    gameManager.EnableLocalPlayer();
                    cursorLockMode = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            Cursor.lockState = cursorLockMode;
        }

        public void UpdateScoreboard () {
            StringBuilder text = new StringBuilder();
            text.Append(PhotonNetwork.playerName + " ");
            text.Append(PhotonNetwork.player.CustomProperties["kills"] + "/");
            text.Append(PhotonNetwork.player.CustomProperties["deaths"] + "\n");
            foreach (PhotonPlayer player in PhotonNetwork.playerList) {
                if (player.ID != PhotonNetwork.player.ID) {
                    text.Append(player.NickName + " ");
                    text.Append(player.CustomProperties["kills"] + "/");
                    text.Append(player.CustomProperties["deaths"] + "\n");
                }
            }
            scoreboardText.text = text.ToString();
        }

        public void UpdateXSensitivity(float sensitivity) {
            PlayerPrefs.SetFloat("X Sensitivity", sensitivity);
            gameManager.XSensitivity = sensitivity;
        }

        public void UpdateYSensitivity(float sensitivity) {
            PlayerPrefs.SetFloat("Y Sensitivity", sensitivity);
            gameManager.YSensitivity = sensitivity;
        }
    }
}