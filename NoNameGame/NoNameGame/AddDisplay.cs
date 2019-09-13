using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoNameGame
{
    class AddDisplay
    {
        private String[] addSprites = new String[5];
        private Sprites[] addSpriteBlock = new Sprites[5];
        private int[] price = new int[5];

        public String[] getDisplay(int i, string color)
        {
            switch (i)
            {
                case 1:
                    addSprites[0] = "Graphics/Sprites/" + color + "/attackers/Swordsman/swordman_better";
                    addSprites[1] = "Graphics/Sprites/" + color + "/attackers/Axe_Class/axe_man";
                    addSprites[2] = "Graphics/Sprites/" + color + "/attackers/Swordsman/knight";
                    addSprites[3] = "Graphics/Sprites/" + color + "/attackers/Swordsman/swordman_better";
                    addSprites[4] = "Graphics/Sprites/" + color + "/attackers/Axe_Class/axe_man";

                    addSpriteBlock[0] = Sprites.SwordsMan;
                    addSpriteBlock[1] = Sprites.AxeMan;
                    addSpriteBlock[2] = Sprites.Knight;
                    addSpriteBlock[3] = Sprites.SwordsMan;
                    addSpriteBlock[4] = Sprites.AxeMan;

                    price[0] = 50;
                    price[1] = 50;
                    price[2] = 50;
                    price[3] = 50;
                    price[4] = 50;
                    break;
                default:
                    addSprites[0] = "Graphics/Textures/colortexture";
                    addSprites[1] = "Graphics/Textures/colortexture";
                    addSprites[2] = "Graphics/Textures/colortexture";
                    addSprites[3] = "Graphics/Textures/colortexture";
                    addSprites[4] = "Graphics/Textures/colortexture";

                    addSpriteBlock[0] = Sprites.Null;
                    addSpriteBlock[1] = Sprites.Null;
                    addSpriteBlock[2] = Sprites.Null;
                    addSpriteBlock[3] = Sprites.Null;
                    addSpriteBlock[4] = Sprites.Null;

                    price[0] = 0;
                    price[1] = 0;
                    price[2] = 0;
                    price[3] = 0;
                    price[4] = 0;
                    break;
            }
            return addSprites;
        }

        public Sprites getBlock(int i)
        {
            return addSpriteBlock[i - 1];
        }

        public int getPrice(int i)
        {
            return price[i - 1];
        }
    }
}
