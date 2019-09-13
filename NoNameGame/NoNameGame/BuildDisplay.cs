using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoNameGame
{
    class BuildDisplay
    {
        private String[] buildSprites = new String[5];
        private Assets[] buildSpriteBlock = new Assets[5];
        private int[] price = new int[5];

        public String[] getDisplay(int i)
        {
            switch (i)
            {
                case 1:
                    buildSprites[0] = "Graphics/Assets/castle";
                    buildSprites[1] = "Graphics/Assets/castle";
                    buildSprites[2] = "Graphics/Assets/castle";
                    buildSprites[3] = "Graphics/Assets/castle";
                    buildSprites[4] = "Graphics/Assets/castle";

                    buildSpriteBlock[0] = Assets.Castle;
                    buildSpriteBlock[1] = Assets.Castle;
                    buildSpriteBlock[2] = Assets.Castle;
                    buildSpriteBlock[3] = Assets.Castle;
                    buildSpriteBlock[4] = Assets.Castle;

                    price[0] = 50;
                    price[1] = 50;
                    price[2] = 50;
                    price[3] = 50;
                    price[4] = 50;
                    break;
                default:
                    buildSprites[0] = "Graphics/Textures/Assets/colortexture";
                    buildSprites[1] = "Graphics/Textures/Assets/colortexture";
                    buildSprites[2] = "Graphics/Textures/Assets/colortexture";
                    buildSprites[3] = "Graphics/Textures/Assets/colortexture";
                    buildSprites[4] = "Graphics/Textures/Assets/colortexture";

                    buildSpriteBlock[0] = Assets.Null;
                    buildSpriteBlock[1] = Assets.Null;
                    buildSpriteBlock[2] = Assets.Null;
                    buildSpriteBlock[3] = Assets.Null;
                    buildSpriteBlock[4] = Assets.Null;

                    price[0] = 0;
                    price[1] = 0;
                    price[2] = 0;
                    price[3] = 0;
                    price[4] = 0;
                    break;
            }
            return buildSprites;
        }

        public Assets getBlock(int i)
        {
            return buildSpriteBlock[i - 1];
        }

        public int getPrice(int i)
        {
            return price[i - 1];
        }
    }
}
