using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GameData;

namespace SolarFusion.Core.Screen
{
    public class ScreenCharSelect : BaseGUIScreen
    {
        private EntityManager _obj_entitymanager = null;

        int mPlayerSelectIndex;
        Vector2 mPlayerSelectPos;
        MenuItemCharacterSelect _obj_selector;
        Player[] _obj_availablePlayers;

        public ScreenCharSelect()
            : base("Character Select", Color.White, false, null, false, 1f)
        {
            this.mPlayerSelectPos = Vector2.Zero;
        }

        public override void loadContent()
        {
            this.mPlayerSelectIndex = 0;
            this.mPlayerSelectPos = new Vector2(this.ScreenManager.GameViewport.Width / 2, this.ScreenManager.GameViewport.Height / 2);
            this._obj_entitymanager = new EntityManager(this.GlobalContentManager);
            this._obj_availablePlayers = this.LoadPlayersFile(this.GlobalContentManager, this._obj_entitymanager.NextID());
            this._obj_selector = new MenuItemCharacterSelect(this.ScreenManager.ContentManager, this.ScreenManager.GameViewport);
            this._obj_selector.OnIncrement += EventTriggerNextCharacter;
            this._obj_selector.OnDecrement += EventTriggerPreviousCharacter;
            this.LoadPlayer(this.mPlayerSelectIndex);
            base.loadContent();
        }

        public override void update()
        {
            this._obj_selector.update(this);

            if (this.GlobalInput.IsPressed("NAV_RIGHT", this.ControllingPlayer))
                this._obj_selector.OnIncrementEntry(this.ControllingPlayer);
            else if (this.GlobalInput.IsPressed("NAV_LEFT", this.ControllingPlayer))
                this._obj_selector.OnDecrementEntry(this.ControllingPlayer);

            if (this.GlobalInput.IsPressed("NAV_SELECT", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
            {
                ScreenLoading.Load(this.ScreenManager, "LOADING", true, this.ControllingPlayer, new ScreenGame(this._obj_availablePlayers[this.mPlayerSelectIndex], this._obj_entitymanager)); //Load Game
                this._obj_availablePlayers[this.mPlayerSelectIndex].PlayerAnimation.Scale = this._obj_availablePlayers[this.mPlayerSelectIndex].PlayerAnimation.OriginalScale;
            }

            if (this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
                this.exitScreen(); //Exit the screen.

            if (this.mPlayerSelectIndex < this._obj_availablePlayers.Length && this._obj_availablePlayers[this.mPlayerSelectIndex] != null)
                this._obj_availablePlayers[this.mPlayerSelectIndex].Update(this.ScreenManager.Timer); //Update the player

            base.update();
        }

        public override void render()
        {
            this._obj_selector.render(this);

            if (this.mPlayerSelectIndex < this._obj_availablePlayers.Length && this._obj_availablePlayers[this.mPlayerSelectIndex] != null)
            {
                this.ScreenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                this._obj_availablePlayers[this.mPlayerSelectIndex].Draw(this.ScreenManager.SpriteBatch);
                this.ScreenManager.SpriteBatch.End();
            }

            base.render();
        }

        public void LoadPlayer(int _index)
        {
            if (this.mPlayerSelectIndex < this._obj_availablePlayers.Length && this._obj_availablePlayers[this.mPlayerSelectIndex] != null && this._obj_selector != null)
            {
                this._obj_availablePlayers[this.mPlayerSelectIndex].Position = this.mPlayerSelectPos;
                this._obj_selector.Text = this._obj_availablePlayers[this.mPlayerSelectIndex].CharacterName;
            }
        }

        /// <summary>
        /// Event Handler to show next character.
        /// </summary>
        void EventTriggerNextCharacter(object sender, EventPlayer e)
        {
            if ((this.mPlayerSelectIndex + 1) < this._obj_availablePlayers.Length)
                this.mPlayerSelectIndex += 1;
            else
                this.mPlayerSelectIndex = 0;

            this.LoadPlayer(this.mPlayerSelectIndex);
        }

        /// <summary>
        /// Event Handler to show previous character.
        /// </summary>
        void EventTriggerPreviousCharacter(object sender, EventPlayer e)
        {
            if ((this.mPlayerSelectIndex - 1) >= 0)
            {
                this.mPlayerSelectIndex -= 1;
            }
            else
            {
                if (this._obj_availablePlayers.Length != 0)
                    this.mPlayerSelectIndex = this._obj_availablePlayers.Length - 1;
                else
                    this.mPlayerSelectIndex = 0;
            }

            this.LoadPlayer(this.mPlayerSelectIndex);
        }

        #region "Load Players"
        private Player[] LoadPlayersFile(ContentManager _content, uint _playerID)
        {
            string[] playerRef = _content.Load<string[]>("Core/Data/players"); //Load available players.
            PlayerData[] players = new PlayerData[playerRef.Length]; //Load the data from the library.

            try
            {
                for (int i = 0; i < playerRef.Length; i++)
                    players[i] = _content.Load<PlayerData>("Core/Data/Players/" + playerRef[i]); //Load individual player data.

            }
            catch (Exception ex)
            {
                throw new Exception("Error: Cannot Load Player Data, Exception occured. ", ex);
            }

            return this.LoadCharacterData(players, _content, _playerID); //Then return the parsed data.
        }

        private Player[] LoadCharacterData(PlayerData[] players, ContentManager _content, uint _playerID) //Function to parse the data.
        {
            Player[] playerObjects = new Player[players.Length]; //Creates an array of player objects.
            AnimatedSprite[] characterSprites; //Creates array of animated sprites.
            characterSprites = new AnimatedSprite[players.Length];

            for (int i = 0; i < players.Length; i++) //Loops through loaded players, ready to parse.
            {
                Texture2D tmpTexture = _content.Load<Texture2D>("Sprites/Characters/" + players[i].playerRef + "/spritesheet"); //Loads the texture from the data.
                characterSprites[i] = new AnimatedSprite(tmpTexture, players[i].maxFrameCount, players[i].playerAnimations.Length); //And several animations.

                for (int j = 0; j < players[i].playerAnimations.Length; j++) //Loops through each available animation in the data and adds them.
                    characterSprites[i].AddAnimation(players[i].playerAnimations[j], (j + 1), int.Parse(players[i].playerAnimationFrameCount[j]), players[i].playerAnimationsFPS[players[i].playerAnimations[j]]);

                //Other configuration things for sprite animation.
                characterSprites[i].Loop = true;
                characterSprites[i].Position = this.mPlayerSelectPos;
                characterSprites[i].Origin = new Vector2((tmpTexture.Width / (float)players[i].maxFrameCount) / 2f, (tmpTexture.Height / (float)players[i].playerAnimations.Length) / 2f);
                characterSprites[i].Scale = players[i].playerScale * 3f;
                characterSprites[i].OriginalScale = players[i].playerScale;
                characterSprites[i].CurrentAnimation = players[i].defaultAnimation;
                playerObjects[i] = new Player(_playerID, characterSprites[i], characterSprites[i].Position, players[i].moveSpeed, players[i].jumpSpeed, this._obj_entitymanager);
                playerObjects[i].CharacterName = players[i].playerName;
            }

            return playerObjects;
        }
        #endregion
    }
}
