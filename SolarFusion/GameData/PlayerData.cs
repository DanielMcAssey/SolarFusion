using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameData
{
    public class PlayerData
    {
        //Generic
        public string playerName = "";

        //Animation
        public string playerAsset = "";
        public string defaultAnimation = "";
        public float playerScale = 1f;
        public int maxFrameCount = 1;
        public string[] playerAnimations;
        public string[] playerAnimationFrameCount;
        public Dictionary<string, int> playerAnimationsFPS = new Dictionary<string, int>();

        //Settings
        public float moveSpeed = 1f;
        public float jumpSpeed = 1f;
        public float jumpHeight = 10f;

        public bool hiddenCharacter = false;
    }
}
