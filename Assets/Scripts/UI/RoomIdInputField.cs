using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lobby
{
    [RequireComponent(typeof(TMP_InputField))]
    public class RoomIdInputField : MonoBehaviour
    {
        const string roomIdPrefKey = "RoomKey";

        // Start is called before the first frame update
        void Start()
        {
            string defaultRoomKey = string.Empty;
            TMP_InputField tmpInput = GetComponent<TMP_InputField>();
            if (tmpInput != null)
            {
                if (PlayerPrefs.HasKey(roomIdPrefKey))
                {
                    defaultRoomKey = PlayerPrefs.GetString(roomIdPrefKey);
                    tmpInput.text = defaultRoomKey;
                }
            }
            Launcher.roomID = defaultRoomKey;
        }

        public void SetRoomID(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.Log("No Room ID");
                return;
            }
            Launcher.roomID = value;
            PlayerPrefs.SetString(roomIdPrefKey, value);
        }

    }
}
