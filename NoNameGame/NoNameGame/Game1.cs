using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NoNameGame
{
    // Enumeration tells where you are in the game (i.e. main menu, in the game, etc.)
    enum States { Main_Menu, Start_Game_Menu, In_Game, Pause_Menu, Instructions, Options, Win };

    // Enumeration tells where you are while playing the game (null is for when you are not in the game)
    enum InGameStates { Null, Look, Move, Attack, Add, Build, Trade, Inventory }

    // Enumeration for minerals
    enum Minerals { Dirt, Grass, Coal, Iron, Diamond };

    // Enumeration for sprites
    enum Sprites { Null, AxeMan, SwordsMan, Knight };

    // Enumeration for assets
    enum Assets { Null, Tree, Mountain, Castle };

    // Enumeration for difficulties
    enum Difficulty { Novice, Adept, Deft };

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Graphics for menus
        Texture2D mainMenu;
        Texture2D toolBox;
        Texture2D pauseMenu;
        Texture2D startGameMenu;
        Texture2D winMenu;
        Texture2D leftArrow;
        Texture2D rightArrow;
        Texture2D inventoryMenu;
        Texture2D grid;
        Texture2D volumeBar;
        Texture2D volumeSlider;
        Texture2D sellButton;

        // Graphics for sprites
        Texture2D swordsMan;
        Texture2D axeMan;
        Texture2D knight;


        // Graphics for textures
        Texture2D grass;
        Texture2D dirt;
        Texture2D coal;
        Texture2D iron;
        Texture2D diamond;

        // Graphics for assets
        Texture2D tree;
        Texture2D mountain;
        Texture2D castle;

        Texture2D allyDot;
        Texture2D enemyDot;
        Texture2D npcDot;

        Texture2D colorTexture;

        Texture2D goodselector;
        Texture2D badselector;
        Texture2D delete;

        Texture2D goodSpace;
        Texture2D badSpace;

        // Graphics for fonts
        SpriteFont font;

        // Graphics for cursors
        Texture2D cursor;

        // Graphics for rupees
        Texture2D rupee;

        // Music for game
        Song mainTheme;
        float volume = 1.0f;
        MediaState playState = new MediaState();

        // Sound Effects for game
        SoundEffect menuSelect;
        SoundEffect type;
        SoundEffect click;
        SoundEffect attack;
        SoundEffect sell;

        // Initialize the display for the add function
        AddDisplay spriteDisplay = new AddDisplay();
        String[] addSprite = new String[5];
        int addDisplayCount = 1;
        int addBlockSelected = 0;

        // Initialize the display for the build function
        BuildDisplay assetDisplay = new BuildDisplay();
        String[] buildSprite = new String[5];
        int buildDisplayCount = 1;
        int buildBlockSelected = 0;

        // Initialize money system
        int rupees = 0;

        // Initialize the inventory
        
        // Inventory for minerals (Wood, Coal, Iron, Diamond)
        int[] mineralInventory = { 0, 0, 0, 0 };

        // Inventory for units (SwordMan, Axeman, Halberd)
        int[] unitInventory = { 0, 0, 0 };

        // Initialize the states
        States gameState = States.Main_Menu;

        InGameStates inGameState = InGameStates.Null;

        bool moveSelected = false;
        int selectedX = 0;
        int selectedY = 0;

        KeyboardState previousKeyBoard;
        KeyboardState keyBoard;

        MouseState previousMouseState;
        MouseState mouse;

        string typedText = "";

        Color colorSelected = Color.Blue;
        string colorChosen = "Blue";

        const int turnActions = 5;
        int actionsLeft = turnActions;
        int compActionsLeft = turnActions;
        int compChoice;

        bool isYourTurn = true;

        const int timeDelay = 3;
        int timeLeft = timeDelay;

        int difficultyPosition = 410;
        Difficulty difficulty = Difficulty.Adept;

        int drawStartX = 0;
        int drawStartY = 0;

        int cursorX = 12;
        int cursorY = 6;

        const int windowWidth = 800;
        const int windowHeight = 490;

        const int initTileWidth = 25;
        const int initTileHeight = 12;

        // Initialize asset, sprite and tile engines
        TileMap tileEngine = new TileMap(initTileWidth, initTileHeight);
        AssetMap assetEngine = new AssetMap(initTileWidth, initTileHeight);
        SpriteMap spriteEngine = new SpriteMap(initTileWidth, initTileHeight);

        int k = 2;
        Ranges range = new Ranges();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 490;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "No Name Game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteEngine.loadMap(tileEngine, assetEngine);
            spriteEngine.newMap();

            unitInventory[0] = spriteEngine.countSprite(Sprites.SwordsMan);
            unitInventory[1] = spriteEngine.countSprite(Sprites.AxeMan);
            unitInventory[2] = spriteEngine.countSprite(Sprites.Knight);

            // Graphics for menus
            mainMenu = Content.Load<Texture2D>("Graphics/Menus/mainmenu");
            toolBox = Content.Load<Texture2D>("Graphics/Menus/toolbox");
            pauseMenu = Content.Load<Texture2D>("Graphics/Menus/pausemenu");
            startGameMenu = Content.Load<Texture2D>("Graphics/Menus/startgamemenu");
            winMenu = Content.Load<Texture2D>("Graphics/Menus/winmenu");
            leftArrow = Content.Load<Texture2D>("Graphics/Menus/leftarrow");
            rightArrow = Content.Load<Texture2D>("Graphics/Menus/rightarrow");
            inventoryMenu = Content.Load<Texture2D>("Graphics/Menus/inventorymenu");
            grid = Content.Load<Texture2D>("Graphics/Menus/Grid/grid");
            volumeBar = Content.Load<Texture2D>("Graphics/Menus/volumebar");
            volumeSlider = Content.Load<Texture2D>("Graphics/Menus/volumeslider");
            sellButton = Content.Load<Texture2D>("Graphics/Menus/sell");

            // Graphics for textures
            grass = Content.Load<Texture2D>("Graphics/Textures/grass");
            dirt = Content.Load<Texture2D>("Graphics/Textures/dirt");
            coal = Content.Load<Texture2D>("Graphics/Textures/coal");
            iron = Content.Load<Texture2D>("Graphics/Textures/iron");
            diamond = Content.Load<Texture2D>("Graphics/Textures/diamond");

            // Graphics for assets
            tree = Content.Load<Texture2D>("Graphics/Assets/tree");
            mountain = Content.Load<Texture2D>("Graphics/Assets/mountain");
            castle = Content.Load<Texture2D>("Graphics/Assets/castle");

            allyDot = Content.Load<Texture2D>("Graphics/Menus/Grid/allydot");
            enemyDot = Content.Load<Texture2D>("Graphics/Menus/Grid/enemydot");
            npcDot = Content.Load<Texture2D>("Graphics/Menus/Grid/npcdot");

            colorTexture = Content.Load<Texture2D>("Graphics/Textures/Asset/colortexture");

            goodselector = Content.Load<Texture2D>("Graphics/Textures/Asset/goodselector");
            badselector = Content.Load<Texture2D>("Graphics/Textures/Asset/badselector");
            delete = Content.Load<Texture2D>("Graphics/Textures/Asset/delete");

            goodSpace = Content.Load<Texture2D>("Graphics/Textures/Asset/goodspace");
            badSpace = Content.Load<Texture2D>("Graphics/Textures/Asset/badspace");

            // Graphics for fonts
            font = Content.Load<SpriteFont>("Graphics/Fonts/Buttons");

            // Graphics for cursors
            cursor = Content.Load<Texture2D>("Graphics/Cursors/cursor");

            // Graphics for rupees
            rupee = Content.Load<Texture2D>("Graphics/Textures/Asset/rupee");

            // Music for game
            mainTheme = Content.Load<Song>("Audio/maintheme");

            // Sound effects for game
            menuSelect = Content.Load<SoundEffect>("Audio/menuselect");
            type = Content.Load<SoundEffect>("Audio/type");
            click = Content.Load<SoundEffect>("Audio/click");
            attack = Content.Load<SoundEffect>("Audio/attack");
            sell = Content.Load<SoundEffect>("Audio/sell");

            MediaPlayer.Play(mainTheme);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            if(rupees >= 1000 && difficulty == Difficulty.Novice)
            {
                gameState = States.Win;
            }

            if (rupees >= 1500 && difficulty == Difficulty.Adept)
            {
                gameState = States.Win;
            }

            if (rupees >= 2000 && difficulty == Difficulty.Deft)
            {
                gameState = States.Win;
            }

            mouse = Mouse.GetState();

            spriteEngine.loadMap(tileEngine, assetEngine);

            /**************************************
             * This script occurs when you are on the main menu.
             * ***********************************/
            if (gameState == States.Main_Menu)
            {
                if (playState == MediaState.Stopped)
                {
                    MediaPlayer.Play(mainTheme);
                }

                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);

                    // When you click on play
                    if (new Rectangle(2, 360, 144, 477).Contains(mousePos))
                    {
                        menuSelect.Play();
                        do
                        {
                            MediaPlayer.Volume = volume;
                            volume = volume - 0.0001f;
                        }
                        while (volume > 0.0f);
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 1.0f;

                        switch (difficulty)
                        {
                            case Difficulty.Novice:
                                rupees = 300;
                                break;
                            case Difficulty.Adept:
                                rupees = 200;
                                break;
                            case Difficulty.Deft:
                                rupees = 100;
                                break;
                        }

                        gameState = States.Start_Game_Menu;
                    }

                    // When you click on tutorial
                    if (new Rectangle(215, 360, 144, 477).Contains(mousePos))
                    {
                        gameState = States.Instructions;
                    }

                    // When you click on difficulty
                    if (new Rectangle(428, 360, 144, 477).Contains(mousePos))
                    {
                        gameState = States.Options;
                    }

                    // When you click on exit
                    if (new Rectangle(643, 360, 144, 477).Contains(mousePos))
                    {
                        this.Exit();
                    }
                }
                // Reset the keyboard and mouse states
                playState = MediaPlayer.State;
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }

            /**************************************
             * This script occurs when you are on the tutorials.
             * ***********************************/
            if (gameState == States.Instructions)
            {
                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);

                    if (new Rectangle(373, 390, 50, 50).Contains(mousePos))
                    {
                        gameState = States.Main_Menu;
                    }
                }
                // Reset the keyboard and mouse states
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }

            /**************************************
             * This script occurs when you are choosing your difficulty.
             * ***********************************/
            if (gameState == States.Options)
            {
                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);

                    // If user clicked on Novice
                    if (new Rectangle(250, 200, 70, 32).Contains(mousePos))
                    {
                        difficultyPosition = 265;
                        difficulty = Difficulty.Novice;
                        gameState = States.Main_Menu;
                    }

                    // If user clicked on Adept
                    if (new Rectangle(400, 200, 60, 32).Contains(mousePos))
                    {
                        difficultyPosition = 410;
                        difficulty = Difficulty.Adept;
                        gameState = States.Main_Menu;
                    }

                    // If user clicked on Deft
                    if (new Rectangle(550, 200, 50, 32).Contains(mousePos))
                    {
                        difficultyPosition = 555;
                        difficulty = Difficulty.Deft;
                        gameState = States.Main_Menu;
                    }
                }
                // Reset the keyboard and mouse states
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }

            /**************************************
             * This script occurs when you are on the win menu.
             * ***********************************/
            if (gameState == States.Win)
            {
                /*string path = @"..\..\..\Saves.mdf";
                string fullPath;

                fullPath = Path.GetFullPath(path);

                SqlConnection sc = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename='" + fullPath + "';Integrated Security=True;Connect Timeout=30;User Instance=True");
                SqlCommand cmd;

                sc.Open();
                // Data for id, name, difficulty, units, territory, mapX, and mapY
                cmd = new SqlCommand("Insert into Data (name, difficulty, units, territory, mapX, mapY) values('" + typedText + "','" + difficulty.ToString() + "'," + unitInventory.Sum() + "," + 3 + "," + tileEngine.worldWidth() + "," + tileEngine.worldHeight() + ")", sc);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT * FROM Data", sc);
                SqlDataReader read = cmd.ExecuteReader();
                StreamWriter output = new StreamWriter("ohnoes.log");
                while (read.Read())
                {
                    output.Write(read[0].ToString() + ", " + read[1].ToString());
                }
                output.Close();

                sc.Close();*/

                if (!Directory.Exists("Saves"))
                    Directory.CreateDirectory("Saves");
                if (!Directory.Exists("Saves\\" + typedText))
                    Directory.CreateDirectory("Saves\\" + typedText);
                if (!Directory.Exists("Saves\\" + typedText + "\\stats"))
                    Directory.CreateDirectory("Saves\\" + typedText + "\\stats");

                StreamWriter highOutput = new StreamWriter("Saves\\" + typedText + "\\highscore.sav");
                highOutput.WriteLine(difficulty.ToString());
                highOutput.WriteLine(rupees);
                highOutput.WriteLine(spriteEngine.countSprite(Sprites.SwordsMan) + spriteEngine.countSprite(Sprites.AxeMan) + spriteEngine.countSprite(Sprites.Knight));
                highOutput.WriteLine(spriteEngine.worldWidth() * spriteEngine.worldHeight());
                highOutput.Close();

                StreamWriter tileOutput = new StreamWriter("Saves\\" + typedText + "\\tilemap.map");

                for (int y = 0; y < tileEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < tileEngine.worldWidth(); x++)
                    {
                        tileOutput.Write(tileEngine.tileAt(x, y));
                        if (x < tileEngine.worldWidth() - 1)
                            tileOutput.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        tileOutput.WriteLine();
                }

                tileOutput.Close();

                StreamWriter assetOutput = new StreamWriter("Saves\\" + typedText + "\\assetmap.map");

                for (int y = 0; y < assetEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < assetEngine.worldWidth(); x++)
                    {
                        assetOutput.Write(assetEngine.assetAt(x, y));
                        if (x < assetEngine.worldWidth() - 1)
                            assetOutput.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        assetOutput.WriteLine();
                }

                assetOutput.Close();

                StreamWriter spriteOutput = new StreamWriter("Saves\\" + typedText + "\\spritemap.map");

                for (int y = 0; y < spriteEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < spriteEngine.worldWidth(); x++)
                    {
                        spriteOutput.Write(spriteEngine.spriteAt(x, y));
                        if (x < spriteEngine.worldWidth() - 1)
                            spriteOutput.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        spriteOutput.WriteLine();
                }

                spriteOutput.Close();

                StreamWriter statL1Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer1.map");

                for (int y = 0; y < spriteEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < spriteEngine.worldWidth(); x++)
                    {
                        statL1Output.Write(spriteEngine.getStat(x, y, 0));
                        if (x < spriteEngine.worldWidth() - 1)
                            statL1Output.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        statL1Output.WriteLine();
                }

                statL1Output.Close();

                StreamWriter statL2Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer2.map");

                for (int y = 0; y < spriteEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < spriteEngine.worldWidth(); x++)
                    {
                        statL2Output.Write(spriteEngine.getStat(x, y, 1));
                        if (x < spriteEngine.worldWidth() - 1)
                            statL2Output.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        statL2Output.WriteLine();
                }

                statL2Output.Close();

                StreamWriter statL3Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer3.map");

                for (int y = 0; y < spriteEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < spriteEngine.worldWidth(); x++)
                    {
                        statL3Output.Write(spriteEngine.getStat(x, y, 2));
                        if (x < spriteEngine.worldWidth() - 1)
                            statL3Output.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        statL3Output.WriteLine();
                }

                statL3Output.Close();

                StreamWriter statL4Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer4.map");

                for (int y = 0; y < spriteEngine.worldHeight(); y++)
                {
                    for (int x = 0; x < spriteEngine.worldWidth(); x++)
                    {
                        statL4Output.Write(spriteEngine.getStat(x, y, 3));
                        if (x < spriteEngine.worldWidth() - 1)
                            statL4Output.Write(",");
                    }
                    if (y < tileEngine.worldHeight() - 1)
                        statL4Output.WriteLine();
                }

                statL4Output.Close();

                this.Exit();
            }

            /**************************************
             * This script occurs when you are on the pause menu inside the game.
             * ***********************************/
            if (gameState == States.Pause_Menu)
            {
                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);

                    // When you click on resume
                    if (new Rectangle(239, 58, 353, 62).Contains(mousePos))
                    {
                        gameState = States.In_Game;
                        inGameState = InGameStates.Look;
                    }

                    // When you click on main menu
                    if (new Rectangle(239, 143, 353, 63).Contains(mousePos))
                    {
                        gameState = States.Main_Menu;
                        inGameState = InGameStates.Null;
                    }

                    // When you click on save
                    if (new Rectangle(239, 229, 353, 63).Contains(mousePos))
                    {
                        /*string path = @"..\..\..\Saves.mdf";
                        string fullPath;

                        fullPath = Path.GetFullPath(path);

                        SqlConnection sc = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename='" + fullPath + "';Integrated Security=True;Connect Timeout=30;User Instance=True");
                        SqlCommand cmd;

                        sc.Open();
                        // Data for id, name, difficulty, units, territory, mapX, and mapY
                        cmd = new SqlCommand("Insert into Data (name, difficulty, units, territory, mapX, mapY) values('" + typedText + "','" + difficulty.ToString() + "'," + unitInventory.Sum() + "," + 3 + "," + tileEngine.worldWidth() + "," + tileEngine.worldHeight() + ")", sc);
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("SELECT * FROM Data", sc);
                        SqlDataReader read = cmd.ExecuteReader();
                        StreamWriter output = new StreamWriter("ohnoes.log");
                        while (read.Read())
                        {
                            output.Write(read[0].ToString() + ", " + read[1].ToString());
                        }
                        output.Close();

                        sc.Close();*/

                        if (!Directory.Exists("Saves"))
                            Directory.CreateDirectory("Saves");
                        if (!Directory.Exists("Saves\\" + typedText))
                            Directory.CreateDirectory("Saves\\" + typedText);
                        if (!Directory.Exists("Saves\\" + typedText + "\\stats"))
                            Directory.CreateDirectory("Saves\\" + typedText + "\\stats");

                        StreamWriter tileOutput = new StreamWriter("Saves\\" + typedText + "\\tilemap.map");

                        for (int y = 0; y < tileEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < tileEngine.worldWidth(); x++)
                            {
                                tileOutput.Write(tileEngine.tileAt(x, y));
                                if (x < tileEngine.worldWidth() - 1)
                                    tileOutput.Write(",");
                            }
                            if(y < tileEngine.worldHeight() - 1)
                                tileOutput.WriteLine();
                        }

                        tileOutput.Close();

                        StreamWriter assetOutput = new StreamWriter("Saves\\" + typedText + "\\assetmap.map");

                        for (int y = 0; y < assetEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < assetEngine.worldWidth(); x++)
                            {
                                assetOutput.Write(assetEngine.assetAt(x, y));
                                if (x < assetEngine.worldWidth() - 1)
                                    assetOutput.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                assetOutput.WriteLine();
                        }

                        assetOutput.Close();

                        StreamWriter spriteOutput = new StreamWriter("Saves\\" + typedText + "\\spritemap.map");

                        for (int y = 0; y < spriteEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < spriteEngine.worldWidth(); x++)
                            {
                                spriteOutput.Write(spriteEngine.spriteAt(x, y));
                                if (x < spriteEngine.worldWidth() - 1)
                                    spriteOutput.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                spriteOutput.WriteLine();
                        }

                        spriteOutput.Close();

                        StreamWriter statL1Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer1.map");

                        for (int y = 0; y < spriteEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < spriteEngine.worldWidth(); x++)
                            {
                                statL1Output.Write(spriteEngine.getStat(x, y, 0));
                                if (x < spriteEngine.worldWidth() - 1)
                                    statL1Output.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                statL1Output.WriteLine();
                        }

                        statL1Output.Close();

                        StreamWriter statL2Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer2.map");

                        for (int y = 0; y < spriteEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < spriteEngine.worldWidth(); x++)
                            {
                                statL2Output.Write(spriteEngine.getStat(x, y, 1));
                                if (x < spriteEngine.worldWidth() - 1)
                                    statL2Output.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                statL2Output.WriteLine();
                        }

                        statL2Output.Close();

                        StreamWriter statL3Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer3.map");

                        for (int y = 0; y < spriteEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < spriteEngine.worldWidth(); x++)
                            {
                                statL3Output.Write(spriteEngine.getStat(x, y, 2));
                                if (x < spriteEngine.worldWidth() - 1)
                                    statL3Output.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                statL3Output.WriteLine();
                        }

                        statL3Output.Close();

                        StreamWriter statL4Output = new StreamWriter("Saves\\" + typedText + "\\stats\\layer4.map");

                        for (int y = 0; y < spriteEngine.worldHeight(); y++)
                        {
                            for (int x = 0; x < spriteEngine.worldWidth(); x++)
                            {
                                statL4Output.Write(spriteEngine.getStat(x, y, 3));
                                if (x < spriteEngine.worldWidth() - 1)
                                    statL4Output.Write(",");
                            }
                            if (y < tileEngine.worldHeight() - 1)
                                statL4Output.WriteLine();
                        }

                        statL4Output.Close();
                    }

                    // When you click on exit
                    if (new Rectangle(239, 315, 353, 63).Contains(mousePos))
                    {
                        this.Exit();
                    }
                }

                keyBoard = Keyboard.GetState();

                // If you press escape, go back to the game
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    gameState = States.In_Game;
                    inGameState = InGameStates.Look;
                }

                previousMouseState = mouse;
                previousKeyBoard = keyBoard;

                // Reset the keyboard and mouse states
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }

            /**************************************
             * This script occurs when you are on the start game menu.
             * ***********************************/
            if (gameState == States.Start_Game_Menu)
            {
                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {

                    Point mousePos = new Point(mouse.X, mouse.Y);

                    // When you click on the left arrow
                    if (new Rectangle(200, 96, 32, 32).Contains(mousePos))
                    {
                        click.Play();
                        if (colorSelected == Color.Blue)
                        {
                            colorSelected = Color.Red;
                            colorChosen = "Red";
                        }
                        else if (colorSelected == Color.Green)
                        {
                            colorSelected = Color.Blue;
                            colorChosen = "Blue";
                        }
                        else if (colorSelected == Color.LightBlue)
                        {
                            colorSelected = Color.Green;
                            colorChosen = "Green";
                        }
                        else if (colorSelected == Color.Orange)
                        {
                            colorSelected = Color.LightBlue;
                            colorChosen = "Light Blue";
                        }
                        else if (colorSelected == Color.Pink)
                        {
                            colorSelected = Color.Orange;
                            colorChosen = "Orange";
                        }
                        else if (colorSelected == Color.Purple)
                        {
                            colorSelected = Color.Pink;
                            colorChosen = "Pink";
                        }
                        else if (colorSelected == Color.Red)
                        {
                            colorSelected = Color.Purple;
                            colorChosen = "Purple";
                        }
                    }

                    // When you click on the right arrow
                    if (new Rectangle(windowWidth - 232, 96, 32, 32).Contains(mousePos))
                    {
                        click.Play();
                        if (colorSelected == Color.Blue)
                        {
                            colorSelected = Color.Green;
                            colorChosen = "Green";
                        }
                        else if (colorSelected == Color.Green)
                        {
                            colorSelected = Color.LightBlue;
                            colorChosen = "Light Blue";
                        }
                        else if (colorSelected == Color.LightBlue)
                        {
                            colorSelected = Color.Orange;
                            colorChosen = "Orange";
                        }
                        else if (colorSelected == Color.Orange)
                        {
                            colorSelected = Color.Pink;
                            colorChosen = "Pink";
                        }
                        else if (colorSelected == Color.Pink)
                        {
                            colorSelected = Color.Purple;
                            colorChosen = "Purple";
                        }
                        else if (colorSelected == Color.Purple)
                        {
                            colorSelected = Color.Red;
                            colorChosen = "Red";
                        }
                        else if (colorSelected == Color.Red)
                        {
                            colorSelected = Color.Blue;
                            colorChosen = "Blue";
                        }
                    }

                    //When you click on open
                    if (new Rectangle(223, 267, 355, 65).Contains(mousePos) && Directory.Exists("Saves\\" + typedText))
                    {
                        menuSelect.Play();

                        /*string path = @"..\..\..\Saves.mdf";
                        string fullPath;

                        fullPath = Path.GetFullPath(path);

                        SqlConnection sc = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename='" + fullPath + "';Integrated Security=True;Connect Timeout=30;User Instance=True");
                        SqlCommand cmd;

                        sc.Open();

                        // Data for id, name, difficulty, units, territory, mapX, and mapY
                        cmd = new SqlCommand("SELECT * FROM Data", sc);
                        SqlDataReader read = cmd.ExecuteReader();
                        read.Read();

                        typedText = read[1].ToString();
                        switch (read[2].ToString())
                        {
                            case "Novice":
                                difficulty = Difficulty.Novice;
                                difficultyPosition = 265;
                                break;
                            case "Adept":
                                difficulty = Difficulty.Adept;
                                difficultyPosition = 410;
                                break;
                            case "Deft":
                                difficulty = Difficulty.Deft;
                                difficultyPosition = 555;
                                break;
                        }*/

                        /*******************************************************
                         * Open data for minerals
                         * ****************************************************/
                        try
                        {
                            using (StreamReader input = new StreamReader("Saves\\" + typedText + "\\tilemap.map"))
                            {
                                List<List<Minerals>> parsedData = new List<List<Minerals>>();
                                string line;
                                while ((line = input.ReadLine()) != null)
                                {
                                    List<Minerals> newRow = new List<Minerals>();
                                    string[] row = line.Split(',');
                                    foreach (string i in row)
                                    {
                                        switch (i)
                                        {
                                            case "Grass":
                                                newRow.Add(Minerals.Grass);
                                                break;
                                            case "Coal":
                                                newRow.Add(Minerals.Coal);
                                                break;
                                            case "Iron":
                                                newRow.Add(Minerals.Iron);
                                                break;
                                            case "Diamond":
                                                newRow.Add(Minerals.Diamond);
                                                break;
                                            default:
                                                newRow.Add(Minerals.Dirt);
                                                break;
                                        }
                                    }
                                    parsedData.Add(newRow);
                                }
                                input.Close();

                                tileEngine.openMap(parsedData, parsedData[0].Count, parsedData.Count);
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter output = new StreamWriter("error.log");
                            output.WriteLine(ex);
                        }

                        /*******************************************************
                         * Open data for assets
                         * ****************************************************/
                        try
                        {
                            using (StreamReader input = new StreamReader("Saves\\" + typedText + "\\assetmap.map"))
                            {
                                List<List<Assets>> parsedData = new List<List<Assets>>();
                                string line;
                                while ((line = input.ReadLine()) != null)
                                {
                                    List<Assets> newRow = new List<Assets>();
                                    string[] row = line.Split(',');
                                    foreach (string i in row)
                                    {
                                        switch (i)
                                        {
                                            case "Tree":
                                                newRow.Add(Assets.Tree);
                                                break;
                                            case "Mountain":
                                                newRow.Add(Assets.Mountain);
                                                break;
                                            case "Castle":
                                                newRow.Add(Assets.Castle);
                                                break;
                                            default:
                                                newRow.Add(Assets.Null);
                                                break;
                                        }
                                    }
                                    parsedData.Add(newRow);
                                }
                                input.Close();

                                assetEngine.openMap(parsedData, parsedData[0].Count, parsedData.Count);
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter output = new StreamWriter("error.log");
                            output.WriteLine(ex);
                        }

                        /*******************************************************
                         * Open data for sprites
                         * ****************************************************/
                        try
                        {
                            using (StreamReader input = new StreamReader("Saves\\" + typedText + "\\spritemap.map"))
                            {
                                List<List<Sprites>> parsedData = new List<List<Sprites>>();
                                string line;
                                while ((line = input.ReadLine()) != null)
                                {
                                    List<Sprites> newRow = new List<Sprites>();
                                    string[] row = line.Split(',');
                                    foreach (string i in row)
                                    {
                                        switch (i)
                                        {
                                            case "SwordsMan":
                                                newRow.Add(Sprites.SwordsMan);
                                                break;
                                            case "AxeMan":
                                                newRow.Add(Sprites.AxeMan);
                                                break;
                                            case "Knight":
                                                newRow.Add(Sprites.Knight);
                                                break;
                                            default:
                                                newRow.Add(Sprites.Null);
                                                break;
                                        }
                                    }
                                    parsedData.Add(newRow);
                                }
                                input.Close();

                                spriteEngine.openMap(parsedData, parsedData[0].Count, parsedData.Count);
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter output = new StreamWriter("error.log");
                            output.WriteLine(ex);
                            output.Close();
                        }

                        /*******************************************************
                         * Open data for stats
                         * ****************************************************/
                        try
                        {
                            using(StreamReader input = new StreamReader("Saves\\" + typedText + "\\stats\\layer1.map"))
                            {
                                StreamReader input2 = new StreamReader("Saves\\" + typedText + "\\stats\\layer2.map");
                                StreamReader input3 = new StreamReader("Saves\\" + typedText + "\\stats\\layer3.map");
                                StreamReader input4 = new StreamReader("Saves\\" + typedText + "\\stats\\layer4.map");

                                string temp;
                                string[] row, row2, row3, row4;

                                List<List<List<int>>> parsedData = new List<List<List<int>>>();

                                while ((temp = input.ReadLine()) != null)
                                {
                                    List<List<int>> newRow = new List<List<int>>();
                                    row = temp.Split(',');
                                    row2 = input2.ReadLine().Split(',');
                                    row3 = input3.ReadLine().Split(',');
                                    row4 = input4.ReadLine().Split(',');

                                    for (int i = 0; i < row.ToList().Count; i++)
                                    {
                                        List<int> newNewRow = new List<int>();
                                        newNewRow.Add(Convert.ToInt32(row[i]));
                                        newNewRow.Add(Convert.ToInt32(row2[i]));
                                        newNewRow.Add(Convert.ToInt32(row3[i]));
                                        newNewRow.Add(Convert.ToInt32(row4[i]));
                                        newRow.Add(newNewRow);
                                    }
                                    parsedData.Add(newRow);
                                }
                                input.Close();
                                input2.Close();
                                input3.Close();
                                input4.Close();

                                spriteEngine.openStats(parsedData);
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter output = new StreamWriter("error.log");
                            output.WriteLine(ex);
                            output.Close();
                        }

                        // Graphics for sprites
                        swordsMan = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Swordsman/swordman_better");
                        axeMan = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Axe_Class/axe_man");
                        knight = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Swordsman/knight");

                        gameState = States.In_Game;
                        inGameState = InGameStates.Look;
                    }

                    // When you click on begin
                    if (new Rectangle(222, 353, 355, 65).Contains(mousePos) && typedText != "")
                    {
                        menuSelect.Play();

                        // Graphics for sprites
                        swordsMan = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Swordsman/swordman_better");
                        axeMan = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Axe_Class/axe_man");
                        knight = Content.Load<Texture2D>("Graphics/Sprites/" + colorChosen + "/attackers/Swordsman/knight");

                        gameState = States.In_Game;
                        inGameState = InGameStates.Look;
                    }
                }

                keyBoard = Keyboard.GetState();

                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Q))
                {
                    typedText += "Q";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    typedText += "W";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.E))
                {
                    typedText += "E";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.R))
                {
                    typedText += "R";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.T))
                {
                    typedText += "T";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    typedText += "Y";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.U))
                {
                    typedText += "U";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.I))
                {
                    typedText += "I";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.O) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.O))
                {
                    typedText += "O";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.P))
                {
                    typedText += "P";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    typedText += "A";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    typedText += "S";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    typedText += "D";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F))
                {
                    typedText += "F";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.G))
                {
                    typedText += "G";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.H))
                {
                    typedText += "H";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.J))
                {
                    typedText += "J";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.K))
                {
                    typedText += "K";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.L))
                {
                    typedText += "L";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    typedText += "Z";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    typedText += "X";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.C))
                {
                    typedText += "C";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.V) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.V))
                {
                    typedText += "V";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.B))
                {
                    typedText += "B";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.N))
                {
                    typedText += "N";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.M))
                {
                    typedText += "M";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    typedText += " ";
                    type.Play();
                }
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Back))
                {
                    typedText = typedText.Substring(0, typedText.Length - 1);
                    type.Play();
                }

                // Reset the keyboard and mouse states
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }

            /**************************************
             * This script occurs when you are playing the game.
             * ***********************************/
            if (gameState == States.In_Game && isYourTurn)
            {
                keyBoard = Keyboard.GetState();

                // If you press escape, bring up the pause menu
                if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    moveSelected = false;
                    gameState = States.Pause_Menu;
                    inGameState = InGameStates.Null;
                }

                timeLeft--;

                /**************************************
                 * This script occurs while you are in the Looking mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Look)
                {
                    // WASD looks around the map
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && timeLeft == 0)
                    {
                        if (drawStartY > 0)
                        {
                            drawStartY--;
                        }
                        else
                        {
                            tileEngine.expandNorth();
                            assetEngine.expandNorth();
                            spriteEngine.loadMap(tileEngine, assetEngine);
                            spriteEngine.expandNorth();
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && timeLeft == 0)
                    {
                        if (drawStartX > 0)
                        {
                            drawStartX--;
                        }
                        else
                        {
                            tileEngine.expandWest();
                            assetEngine.expandWest();
                            spriteEngine.loadMap(tileEngine, assetEngine);
                            spriteEngine.expandWest();
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && timeLeft == 0)
                    {
                        if(drawStartY + 12 < spriteEngine.worldHeight())
                        {
                            drawStartY++;
                        }
                        else
                        {
                            drawStartY++;
                            tileEngine.expandSouth();
                            assetEngine.expandSouth();
                            spriteEngine.loadMap(tileEngine, assetEngine);
                            spriteEngine.expandSouth();
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && timeLeft == 0)
                    {
                        if (drawStartX + 25 < spriteEngine.worldWidth())
                        {
                            drawStartX++;
                        }
                        else
                        {
                            drawStartX++;
                            tileEngine.expandEast();
                            assetEngine.expandEast();
                            spriteEngine.loadMap(tileEngine, assetEngine);
                            spriteEngine.expandEast();
                        }
                    }
                }

                /**************************************
                 * This script occurs while you are in the Moving mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Move)
                {
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null && moveSelected == false && spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) != 1)
                    {
                        moveSelected = true;
                        selectedX = cursorX;
                        selectedY = cursorY;
                        range.setCoordinates(cursorX, cursorY);
                        range.setRange(k);
                        spriteEngine.startMove(drawStartX + cursorX, drawStartY + cursorY);
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null && moveSelected == true && range.withinRange(cursorX, cursorY))
                    {
                        spriteEngine.moveSprite(drawStartX + cursorX, drawStartY + cursorY);
                        moveSelected = false;
                        if (actionsLeft > 0)
                        {
                            actionsLeft--;
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && timeLeft == 0)
                    {
                        if (cursorY > 0)
                        {
                            cursorY--;
                        }
                        else
                        {
                            if (drawStartY > 0)
                            {
                                drawStartY--;
                                selectedY++;
                                range.moveY(1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && timeLeft == 0)
                    {
                        if (cursorX > 0)
                        {
                            cursorX--;
                        }
                        else
                        {
                            if (drawStartX > 0)
                            {
                                drawStartX--;
                                selectedX++;
                                range.moveX(1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && timeLeft == 0)
                    {
                        if (cursorY < 11)
                        {
                            cursorY++;
                        }
                        else
                        {
                            if (drawStartY + 12 < spriteEngine.worldHeight())
                            {
                                drawStartY++;
                                selectedY--;
                                range.moveY(-1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && timeLeft == 0)
                    {
                        if (cursorX < 24)
                        {
                            cursorX++;
                        }
                        else
                        {
                            if (drawStartX + 25 < spriteEngine.worldWidth())
                            {
                                drawStartX++;
                                selectedX--;
                                range.moveX(-1);
                            }
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                /**************************************
                 * This script occurs while you are in the Attacking mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Attack)
                {
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space))
                    {
                        if (moveSelected == false && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null && spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) != 1)
                        {
                            moveSelected = true;
                            selectedX = cursorX;
                            selectedY = cursorY;
                            range.setCoordinates(cursorX, cursorY);
                            range.setRange(k);
                        }
                        else if(moveSelected == true)
                        {
                            if (spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null && spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) != 0)
                            {
                                attack.Play();
                                spriteEngine.attackSprite(selectedX, selectedY, drawStartX + cursorX, drawStartY + cursorY);
                                moveSelected = false;
                                if (actionsLeft > 0)
                                {
                                    actionsLeft--;
                                }
                            }
                            else if (spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null)
                            {
                                if (tileEngine.tileAt(drawStartX + cursorX, drawStartY + cursorY) == Minerals.Coal)
                                {
                                    mineralInventory[1]++;
                                }
                                else if (tileEngine.tileAt(drawStartX + cursorX, drawStartY + cursorY) == Minerals.Iron)
                                {
                                    mineralInventory[2]++;
                                }
                                else if (tileEngine.tileAt(drawStartX + cursorX, drawStartY + cursorY) == Minerals.Diamond)
                                {
                                    mineralInventory[3]++;
                                }

                                attack.Play();
                                tileEngine.mineTile(drawStartX + cursorX, drawStartY + cursorY);
                                moveSelected = false;
                                if (actionsLeft > 0)
                                {
                                    actionsLeft--;
                                }
                            }
                            else if (assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Tree)
                            {
                                attack.Play();
                                assetEngine.killAsset(drawStartX + cursorX, drawStartY + cursorY);
                                moveSelected = false;
                                if (actionsLeft > 0)
                                {
                                    actionsLeft--;
                                }
                                mineralInventory[0]++;
                            }
                            else if (assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Castle)
                            {
                                assetEngine.killAsset(drawStartX + cursorX, drawStartY + cursorY);
                            }
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && timeLeft == 0)
                    {
                        if (cursorY > 0)
                        {
                            cursorY--;
                        }
                        else
                        {
                            if (drawStartY > 0)
                            {
                                drawStartY--;
                                selectedY++;
                                range.moveY(1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && timeLeft == 0)
                    {
                        if (cursorX > 0)
                        {
                            cursorX--;
                        }
                        else
                        {
                            if (drawStartX > 0)
                            {
                                drawStartX--;
                                selectedX++;
                                range.moveX(1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && timeLeft == 0)
                    {
                        if (cursorY < 11)
                        {
                            cursorY++;
                        }
                        else
                        {
                            if (drawStartY + 12 < spriteEngine.worldHeight())
                            {
                                drawStartY++;
                                selectedY--;
                                range.moveY(-1);
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && timeLeft == 0)
                    {
                        if (cursorX < 24)
                        {
                            cursorX++;
                        }
                        else
                        {
                            if (drawStartX + 25 < spriteEngine.worldWidth())
                            {
                                drawStartX++;
                                selectedX--;
                                range.moveX(-1);
                            }
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                /**************************************
                 * This script occurs while you are in the Adding mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Add)
                {
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null && rupees >= spriteDisplay.getPrice(addBlockSelected))
                    {
                        spriteEngine.buildSprite(drawStartX + cursorX, drawStartY + cursorY, spriteDisplay.getBlock(addBlockSelected), 0);
                        rupees = rupees - spriteDisplay.getPrice(addBlockSelected);
                        if (actionsLeft > 0)
                        {
                            actionsLeft--;
                        }
                    }

                    if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        Point mousePos = new Point(mouse.X, mouse.Y);

                        // When you click on the first block
                        if (new Rectangle(23, 424, 49, 59).Contains(mousePos))
                        {
                            if (addBlockSelected == 1)
                            {
                                moveSelected = false;
                                addBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                addBlockSelected = 1;
                            }
                        }

                        // When you click on the second block
                        if (new Rectangle(80, 424, 49, 59).Contains(mousePos))
                        {
                            if (addBlockSelected == 2)
                            {
                                moveSelected = false;
                                addBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                addBlockSelected = 2;
                            }
                        }

                        // When you click on the third block
                        if (new Rectangle(137, 424, 49, 59).Contains(mousePos))
                        {
                            if (addBlockSelected == 3)
                            {
                                moveSelected = false;
                                addBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                addBlockSelected = 3;
                            }
                        }

                        // When you click on the fourth block
                        if (new Rectangle(194, 424, 49, 59).Contains(mousePos))
                        {
                            if (addBlockSelected == 4)
                            {
                                moveSelected = false;
                                addBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                addBlockSelected = 4;
                            }
                        }

                        // When you click on the fifth block
                        if (new Rectangle(251, 424, 49, 59).Contains(mousePos))
                        {
                            if (addBlockSelected == 5)
                            {
                                moveSelected = false;
                                addBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                addBlockSelected = 5;
                            }
                        }

                        // When you click on the up arrow
                        if (new Rectangle(468, 415, 50, 37).Contains(mousePos) && addDisplayCount > 1)
                        {
                            addDisplayCount--;
                        }

                        // When you click on the down arrow
                        if (new Rectangle(468, 452, 50, 37).Contains(mousePos) && addDisplayCount < 1)
                        {
                            addDisplayCount++;
                        }
                    }

                    if (moveSelected == false)
                    {
                        // WASD looks around the map
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && drawStartY > 0 && timeLeft == 0)
                        {
                            drawStartY--;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && drawStartX > 0 && timeLeft == 0)
                        {
                            drawStartX--;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && drawStartY + 12 < spriteEngine.worldHeight() && timeLeft == 0)
                        {
                            drawStartY++;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && drawStartX + 25 < spriteEngine.worldWidth() && timeLeft == 0)
                        {
                            drawStartX++;
                        }
                    }

                    if (moveSelected == true)
                    {
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && timeLeft == 0)
                        {
                            if (cursorY > 0)
                            {
                                cursorY--;
                            }
                            else
                            {
                                if (drawStartY > 0)
                                {
                                    drawStartY--;
                                    selectedY++;
                                    range.moveY(1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && timeLeft == 0)
                        {
                            if (cursorX > 0)
                            {
                                cursorX--;
                            }
                            else
                            {
                                if (drawStartX > 0)
                                {
                                    drawStartX--;
                                    selectedX++;
                                    range.moveX(1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && timeLeft == 0)
                        {
                            if (cursorY < 11)
                            {
                                cursorY++;
                            }
                            else
                            {
                                if (drawStartY + 12 < spriteEngine.worldHeight())
                                {
                                    drawStartY++;
                                    selectedY--;
                                    range.moveY(-1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && timeLeft == 0)
                        {
                            if (cursorX < 24)
                            {
                                cursorX++;
                            }
                            else
                            {
                                if (drawStartX + 25 < spriteEngine.worldWidth())
                                {
                                    drawStartX++;
                                    selectedX--;
                                    range.moveX(-1);
                                }
                            }
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                /**************************************
                 * This script occurs while you are in the Building mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Build)
                {
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && rupees >= assetDisplay.getPrice(buildBlockSelected))
                    {
                        assetEngine.buildAsset(drawStartX + cursorX, drawStartY + cursorY, assetDisplay.getBlock(buildBlockSelected));
                        rupees = rupees - assetDisplay.getPrice(buildBlockSelected);
                        if (actionsLeft > 0)
                        {
                            actionsLeft--;
                        }
                    }

                    if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        Point mousePos = new Point(mouse.X, mouse.Y);

                        // When you click on the first block
                        if (new Rectangle(23, 424, 49, 59).Contains(mousePos))
                        {
                            if (buildBlockSelected == 1)
                            {
                                moveSelected = false;
                                buildBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                buildBlockSelected = 1;
                            }
                        }

                        // When you click on the second block
                        if (new Rectangle(80, 424, 49, 59).Contains(mousePos))
                        {
                            if (buildBlockSelected == 2)
                            {
                                moveSelected = false;
                                buildBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                buildBlockSelected = 2;
                            }
                        }

                        // When you click on the third block
                        if (new Rectangle(137, 424, 49, 59).Contains(mousePos))
                        {
                            if (buildBlockSelected == 3)
                            {
                                moveSelected = false;
                                buildBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                buildBlockSelected = 3;
                            }
                        }

                        // When you click on the fourth block
                        if (new Rectangle(194, 424, 49, 59).Contains(mousePos))
                        {
                            if (buildBlockSelected == 4)
                            {
                                moveSelected = false;
                                buildBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                buildBlockSelected = 4;
                            }
                        }

                        // When you click on the fifth block
                        if (new Rectangle(251, 424, 49, 59).Contains(mousePos))
                        {
                            if (buildBlockSelected == 5)
                            {
                                moveSelected = false;
                                buildBlockSelected = 0;
                            }
                            else
                            {
                                moveSelected = true;
                                buildBlockSelected = 5;
                            }
                        }

                        // When you click on the up arrow
                        if (new Rectangle(468, 415, 50, 37).Contains(mousePos) && buildDisplayCount > 1)
                        {
                            buildDisplayCount--;
                        }

                        // When you click on the down arrow
                        if (new Rectangle(468, 452, 50, 37).Contains(mousePos) && buildDisplayCount < 1)
                        {
                            buildDisplayCount++;
                        }
                    }

                    if (moveSelected == false)
                    {
                        // WASD looks around the map
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && drawStartY > 0 && timeLeft == 0)
                        {
                            drawStartY--;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && drawStartX > 0 && timeLeft == 0)
                        {
                            drawStartX--;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && drawStartY + 12 < spriteEngine.worldHeight() && timeLeft == 0)
                        {
                            drawStartY++;
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && drawStartX + 25 < spriteEngine.worldWidth() && timeLeft == 0)
                        {
                            drawStartX++;
                        }
                    }

                    if (moveSelected == true)
                    {
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && timeLeft == 0)
                        {
                            if (cursorY > 0)
                            {
                                cursorY--;
                            }
                            else
                            {
                                if (drawStartY > 0)
                                {
                                    drawStartY--;
                                    selectedY++;
                                    range.moveY(1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && timeLeft == 0)
                        {
                            if (cursorX > 0)
                            {
                                cursorX--;
                            }
                            else
                            {
                                if (drawStartX > 0)
                                {
                                    drawStartX--;
                                    selectedX++;
                                    range.moveX(1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && timeLeft == 0)
                        {
                            if (cursorY < 11)
                            {
                                cursorY++;
                            }
                            else
                            {
                                if (drawStartY + 12 < spriteEngine.worldHeight())
                                {
                                    drawStartY++;
                                    selectedY--;
                                    range.moveY(-1);
                                }
                            }
                        }
                        if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && timeLeft == 0)
                        {
                            if (cursorX < 24)
                            {
                                cursorX++;
                            }
                            else
                            {
                                if (drawStartX + 25 < spriteEngine.worldWidth())
                                {
                                    drawStartX++;
                                    selectedX--;
                                    range.moveX(-1);
                                }
                            }
                        }
                    }

                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                /**************************************
                 * This script occurs while you are in the Trading mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Trade)
                {
                    if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        Point mousePos = new Point(mouse.X, mouse.Y);

                        // When you click on sell wood
                        if (new Rectangle(160, 65, 64, 30).Contains(mousePos) && mineralInventory[0] > 0)
                        {
                            sell.Play();
                            mineralInventory[0]--;
                            rupees += 10;
                            if (actionsLeft > 0)
                            {
                                actionsLeft--;
                            }
                        }

                        // When you click on sell coal
                        if (new Rectangle(160, 97, 64, 30).Contains(mousePos) && mineralInventory[1] > 0)
                        {
                            sell.Play();
                            mineralInventory[1]--;
                            rupees += 30;
                            if (actionsLeft > 0)
                            {
                                actionsLeft--;
                            }
                        }

                        // When you click on sell iron
                        if (new Rectangle(160, 129, 64, 30).Contains(mousePos) && mineralInventory[2] > 0)
                        {
                            sell.Play();
                            mineralInventory[2]--;
                            rupees += 50;
                            if (actionsLeft > 0)
                            {
                                actionsLeft--;
                            }
                        }

                        // When you click on sell diamond
                        if (new Rectangle(160, 161, 64, 30).Contains(mousePos) && mineralInventory[3] > 0)
                        {
                            sell.Play();
                            mineralInventory[3]--;
                            rupees += 100;
                            if (actionsLeft > 0)
                            {
                                actionsLeft--;
                            }
                        }
                    }
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                /**************************************
                 * This script occurs while you are in the Inventory mode in the game.
                 * ***********************************/
                if (inGameState == InGameStates.Inventory)
                {
                    if (keyBoard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && previousKeyBoard.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab))
                    {
                        inGameState = InGameStates.Look;
                    }
                }

                if (timeLeft == 0)
                {
                    timeLeft = timeDelay;
                }

                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);

                    // When you click on Move
                    if (new Rectangle(0, 384, 62, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Move)
                        {
                            inGameState = InGameStates.Move;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }

                    // When you click on Attack
                    if (new Rectangle(62, 384, 98, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Attack)
                        {
                            inGameState = InGameStates.Attack;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }

                    // When you click on Add
                    if (new Rectangle(160, 384, 78, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Add)
                        {
                            inGameState = InGameStates.Add;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }

                    // When you click on Build
                    if (new Rectangle(238, 384, 92, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Build)
                        {
                            inGameState = InGameStates.Build;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }

                    // When you click on Trade
                    if (new Rectangle(330, 384, 95, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Trade)
                        {
                            inGameState = InGameStates.Trade;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }

                    // When you click on Inventory
                    if (new Rectangle(425, 384, 103, 30).Contains(mousePos))
                    {
                        click.Play();
                        moveSelected = false;
                        if (inGameState != InGameStates.Inventory)
                        {
                            inGameState = InGameStates.Inventory;
                        }
                        else
                        {
                            inGameState = InGameStates.Look;
                        }
                    }
                }
                // Reset the keyboard and mouse states
                previousMouseState = mouse;
                previousKeyBoard = keyBoard;
                base.Update(gameTime);
            }
            else if (gameState == States.In_Game && !isYourTurn)
            {
                Random rand = new Random();
                compChoice = rand.Next(3);

                /*******************************************************************
                 * When the computer chooses to move a character
                 * ****************************************************************/
                if (compChoice == 0)
                {
                    int coorX;
                    int coorY;
                    do
                    {
                        coorX = rand.Next(spriteEngine.worldWidth());
                        coorY = rand.Next(spriteEngine.worldHeight());
                    }
                    while (spriteEngine.getStat(coorX, coorY, 0) != 1);
                    
                    int rangeX;
                    int rangeY;
                    do
                    {
                        rangeX = rand.Next(5) - 2;
                        if (rangeX == -2 || rangeX == 2)
                        {
                            rangeY = rand.Next(3) - 1;
                        }
                        else
                        {
                            rangeY = rand.Next(5) - 2;
                        }
                    }
                    while (spriteEngine.getStat(coorX, coorY, 0) != 1 && assetEngine.assetAt(coorX + rangeX, coorY + rangeY) == Assets.Null);

                    spriteEngine.startMove(coorX, coorY);
                    spriteEngine.moveSprite(coorX + rangeX, coorY + rangeY);
                }

                /*******************************************************************
                 * When the computer chooses to attack a character
                 * ****************************************************************/
                else if (compChoice == 1)
                {
                    int coorX;
                    int coorY;
                    do
                    {
                        coorX = rand.Next(spriteEngine.worldWidth());
                        coorY = rand.Next(spriteEngine.worldHeight());
                    }
                    while (spriteEngine.getStat(coorX, coorY, 0) != 1);

                    int rangeX;
                    int rangeY;
                    do
                    {
                        rangeX = rand.Next(5) - 2;
                        if(rangeX == -2 || rangeX == 2)
                        {
                            rangeY = rand.Next(3) - 1;
                        }
                        else
                        {
                            rangeY = rand.Next(5) - 2;
                        }
                    }
                    while(spriteEngine.getStat(coorX, coorY, 0) != 1);
                    spriteEngine.attackSprite(coorX, coorY, coorX + rangeX, coorY + rangeY);
                }

                /*******************************************************************
                 * When the computer chooses to add a character
                 * ****************************************************************/
                else if (compChoice == 2)
                {
                    compChoice = rand.Next(3);

                    int coorX;
                    int coorY;
                    do
                    {
                        coorX = rand.Next(spriteEngine.worldWidth());
                        coorY = rand.Next(spriteEngine.worldHeight());
                    }
                    while (spriteEngine.spriteAt(coorX, coorY) != Sprites.Null && assetEngine.assetAt(coorX, coorY) != Assets.Null);
                    
                    /***************************************************************
                     * If the computer adds a swordsman
                     * ************************************************************/
                    if (compChoice == 0)
                    {
                        spriteEngine.buildSprite(rand.Next(spriteEngine.worldWidth()), rand.Next(spriteEngine.worldHeight()), Sprites.SwordsMan, 1);
                    }

                    /***************************************************************
                     * If  the computer adds an axeman
                     * ************************************************************/
                    else if (compChoice == 1)
                    {
                        spriteEngine.buildSprite(rand.Next(spriteEngine.worldWidth()), rand.Next(spriteEngine.worldHeight()), Sprites.AxeMan, 1);
                    }

                    /***************************************************************
                     * If the computer adds a knight
                     * ************************************************************/
                    else if (compChoice == 2)
                    {
                        spriteEngine.buildSprite(rand.Next(spriteEngine.worldWidth()), rand.Next(spriteEngine.worldHeight()), Sprites.Knight, 1);
                    }
                }

                compActionsLeft--;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (isYourTurn && actionsLeft == 0)
            {
                compActionsLeft = turnActions;
                isYourTurn = false;
            }
            else if (!isYourTurn && compActionsLeft == 0)
            {
                MessageBox.Show("The computer has taken its turn.");
                actionsLeft = turnActions;
                isYourTurn = true;
            }

            /**************************************
             * This is drawn when you are on the main menu.
             * ***********************************/
            if (gameState == States.Main_Menu)
            {
                spriteBatch.Draw(mainMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                spriteBatch.DrawString(font, "Difficulty: ", new Vector2(0, 0), Color.White);

                switch (difficulty)
                {
                    case Difficulty.Novice:
                        spriteBatch.DrawString(font, "Novice", new Vector2(100, 0), Color.Green);
                        break;
                    case Difficulty.Adept:
                        spriteBatch.DrawString(font, "Adept", new Vector2(100, 0), Color.Blue);
                        break;
                    case Difficulty.Deft:
                        spriteBatch.DrawString(font, "Deft", new Vector2(100, 0), Color.Red);
                        break;
                }
            }

            /**************************************
             * This is drawn when you are on the tutorials.
             * ***********************************/
            if (gameState == States.Instructions)
            {
                spriteBatch.DrawString(font, "Instructions", new Vector2(0, 0), Color.White);

                spriteBatch.DrawString(font, "The goal of the game is to collect as much territory as possible, and to have the", new Vector2(0, 32), Color.White);
                spriteBatch.DrawString(font, "most units. WASD moves the cursor or looks around the screen, spacebar selects,", new Vector2(0, 64), Color.White);
                spriteBatch.DrawString(font, "tab backs out of options, and escape brings up the pause menu. Use the mouse to", new Vector2(0, 96), Color.White);
                spriteBatch.DrawString(font, "select whatever options not selectable with spacebar.", new Vector2(0, 128), Color.White);

                spriteBatch.DrawString(font, "Move, attack, add, and build bring up a cursor to select your options to move,", new Vector2(0, 160), Color.White);
                spriteBatch.DrawString(font, "attack, and place objects. Inventory allows you to see what you have collected", new Vector2(0, 192), Color.White);
                spriteBatch.DrawString(font, "so far, and Trade allows you to sell your minerals to make money.", new Vector2(0, 224), Color.White);
                spriteBatch.DrawString(font, "The selection cursor is blue when over an ally, and red everywhere else.", new Vector2(0, 256), Color.White);
                spriteBatch.DrawString(font, "When available, a blue highlight signifies that you can do that action there,", new Vector2(0, 288), Color.White);
                spriteBatch.DrawString(font, "and red represents where that is not available.", new Vector2(0, 320), Color.White);
                spriteBatch.DrawString(font, "Back", new Vector2((windowWidth / 2) - (font.MeasureString("Back").X / 2), 400), Color.White);
            }

            /**************************************
             * This is drawn when you are choosing your options.
             * ***********************************/
            if (gameState == States.Options)
            {
                spriteBatch.DrawString(font, "Difficulty: ", new Vector2(100, 200), Color.White);
                spriteBatch.DrawString(font, "Novice", new Vector2(250, 200), Color.Green);
                spriteBatch.DrawString(font, "Adept", new Vector2(400, 200), Color.Blue);
                spriteBatch.DrawString(font, "Deft", new Vector2(550, 200), Color.Red);

                spriteBatch.Draw(cursor, new Rectangle(difficultyPosition, 232, 32, 32), Color.White);
            }

            /**************************************
             * This is drawn when you are on the start game menu.
             * ***********************************/
            if(gameState == States.Start_Game_Menu)
            {
                spriteBatch.Draw(startGameMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                spriteBatch.DrawString(font, "Type your name below:", new Vector2((windowWidth / 2) - (font.MeasureString("Type your name below:").X / 2), 0), Color.White);
                spriteBatch.DrawString(font, typedText, new Vector2((windowWidth / 2) - (font.MeasureString(typedText).X / 2), 32), Color.White);

                spriteBatch.DrawString(font, "Select color:", new Vector2((windowWidth / 2) - (font.MeasureString("Select color:").X / 2), 64), Color.White);
                spriteBatch.DrawString(font, colorChosen, new Vector2((windowWidth / 2) - (font.MeasureString(colorChosen).X / 2), 100), colorSelected);
                spriteBatch.Draw(leftArrow, new Rectangle(200, 96, 32, 32), Color.White);
                spriteBatch.Draw(rightArrow, new Rectangle(windowWidth - 232, 96, 32, 32), Color.White);

                spriteBatch.Draw(colorTexture, new Rectangle(223, 267, 355, 65), Color.White * 0.5f);
                spriteBatch.Draw(colorTexture, new Rectangle(222, 353, 355, 65), Color.White * 0.5f);
            }

            /**************************************
             * This is drawn when you are playing the game.
             * ***********************************/
            if (gameState == States.In_Game)
            {
                unitInventory[0] = spriteEngine.countSprite(Sprites.SwordsMan);
                unitInventory[1] = spriteEngine.countSprite(Sprites.AxeMan);
                unitInventory[2] = spriteEngine.countSprite(Sprites.Knight);
                spriteBatch.Draw(colorTexture, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                int xx = 0 + drawStartX, yy = 0 + drawStartY;
                for (int y = 0; y < 384; y += 32)
                {
                    for (int x = 0; x < 800; x += 32)
                    {
                        // Draw the textures
                        if (tileEngine.tileAt(xx, yy) == Minerals.Grass)
                        {
                            spriteBatch.Draw(grass, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (tileEngine.tileAt(xx, yy) == Minerals.Coal)
                        {
                            spriteBatch.Draw(coal, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (tileEngine.tileAt(xx, yy) == Minerals.Iron)
                        {
                            spriteBatch.Draw(iron, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (tileEngine.tileAt(xx, yy) == Minerals.Diamond)
                        {
                            spriteBatch.Draw(diamond, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(dirt, new Rectangle(x, y, 32, 32), Color.White);
                        }

                        // Draw the assets
                        if (assetEngine.assetAt(xx, yy) == Assets.Tree)
                        {
                            spriteBatch.Draw(tree, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (assetEngine.assetAt(xx, yy) == Assets.Mountain)
                        {
                            spriteBatch.Draw(mountain, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (assetEngine.assetAt(xx, yy) == Assets.Castle)
                        {
                            spriteBatch.Draw(castle, new Rectangle(x, y, 32, 32), Color.White);
                        }

                        // Draw the sprites
                        if (spriteEngine.spriteAt(xx, yy) == Sprites.Knight)
                        {
                            spriteBatch.Draw(knight, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (spriteEngine.spriteAt(xx, yy) == Sprites.SwordsMan)
                        {
                            spriteBatch.Draw(swordsMan, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else if (spriteEngine.spriteAt(xx, yy) == Sprites.AxeMan)
                        {
                            spriteBatch.Draw(axeMan, new Rectangle(x, y, 32, 32), Color.White);
                        }
                        else
                        {
                        }
                        xx++;
                    }
                    xx = 0 + drawStartX;
                    yy++;
                }

                /**************************************
                * This is drawn while you are in the Moving mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Move)
                {
                    if (moveSelected == true)
                    {
                        for (int i = -k; i < k + 1; i++)
                        {
                            for (int j = -k; j < k + 1 - i; j++)
                            {
                                if (Math.Abs(selectedX - (selectedX + i)) + Math.Abs(selectedY - (selectedY + j)) <= k)
                                {
                                    if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null && assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null)
                                    {
                                        spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                }
                                if (Math.Abs(selectedX + (selectedX + i)) + Math.Abs(selectedY - (selectedY + j)) <= k)
                                {
                                    if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null && assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null)
                                    {
                                        spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                }
                                if (Math.Abs(selectedX - (selectedX + i)) + Math.Abs(selectedY + (selectedY + j)) <= k)
                                {
                                    if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null && assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null)
                                    {
                                        spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                }
                                if (Math.Abs(selectedX + (selectedX + i)) + Math.Abs(selectedY + (selectedY + j)) <= k)
                                {
                                    if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null && assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null)
                                    {
                                        spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                    }
                                }
                            }
                        }
                    }
                    if (spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) == 0 && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null)
                    {
                        spriteBatch.Draw(goodselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(badselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                    }
                }

                /**************************************
                * This is drawn while you are in the Attacking mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Attack)
                {
                    if (moveSelected == true)
                    {
                        for (int i = -k; i < k + 1; i++)
                        {
                            for (int j = -k; j < k + 1 - i; j++)
                            {
                                if (!(i == 0 && j == 0))
                                {
                                    if (Math.Abs(selectedX - (selectedX + i)) + Math.Abs(selectedY - (selectedY + j)) <= k)
                                    {
                                        if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Sprites.Null && spriteEngine.getStat(selectedX + drawStartX + i, selectedY + drawStartY + j, 0) != 0)
                                        {
                                            spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                        else if (assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Tree || ((assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null && spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null) && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Dirt && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Grass))
                                        {
                                            spriteBatch.Draw(delete, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                    }
                                    if (Math.Abs(selectedX + (selectedX + i)) + Math.Abs(selectedY - (selectedY + j)) <= k)
                                    {
                                        if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Sprites.Null && spriteEngine.getStat(selectedX + drawStartX + i, selectedY + drawStartY + j, 0) != 0)
                                        {
                                            spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                        else if (assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Tree || ((assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null && spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null) && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Dirt && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Grass))
                                        {
                                            spriteBatch.Draw(delete, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                    }
                                    if (Math.Abs(selectedX - (selectedX + i)) + Math.Abs(selectedY + (selectedY + j)) <= k)
                                    {
                                        if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Sprites.Null && spriteEngine.getStat(selectedX + drawStartX + i, selectedY + drawStartY + j, 0) != 0)
                                        {
                                            spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                        else if (assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Tree || ((assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null && spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null) && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Dirt && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Grass))
                                        {
                                            spriteBatch.Draw(delete, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                    }
                                    if (Math.Abs(selectedX + (selectedX + i)) + Math.Abs(selectedY + (selectedY + j)) <= k)
                                    {
                                        if (spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Sprites.Null && spriteEngine.getStat(selectedX + drawStartX + i, selectedY + drawStartY + j, 0) != 0)
                                        {
                                            spriteBatch.Draw(goodSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                        else if (assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Tree || ((assetEngine.assetAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Assets.Null && spriteEngine.spriteAt(selectedX + drawStartX + i, selectedY + drawStartY + j) == Sprites.Null) && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Dirt && tileEngine.tileAt(selectedX + drawStartX + i, selectedY + drawStartY + j) != Minerals.Grass))
                                        {
                                            spriteBatch.Draw(delete, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(badSpace, new Rectangle((selectedX + i) * 32, (selectedY + j) * 32, 32, 32), Color.White * 0.4f);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) == 0 && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null)
                    {
                        spriteBatch.Draw(goodselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(badselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                    }
                }

                spriteBatch.Draw(toolBox, new Rectangle(0, 384, windowWidth, 107), Color.White);

                // Highlight selected option
                Color optionSelected = new Color();
                switch (difficulty)
                {
                    case Difficulty.Novice:
                        optionSelected = Color.Green;
                        break;
                    case Difficulty.Adept:
                        optionSelected = Color.Blue;
                        break;
                    case Difficulty.Deft:
                        optionSelected = Color.Red;
                        break;
                }
                switch (inGameState)
                {
                    case InGameStates.Move:
                        spriteBatch.Draw(colorTexture, new Rectangle(0, 384, 62, 30), optionSelected * 0.5f);
                        break;
                    case InGameStates.Attack:
                        spriteBatch.Draw(colorTexture, new Rectangle(62, 384, 98, 30), optionSelected * 0.5f);
                        break;
                    case InGameStates.Add:
                        spriteBatch.Draw(colorTexture, new Rectangle(160, 384, 78, 30), optionSelected * 0.5f);
                        break;
                    case InGameStates.Build:
                        spriteBatch.Draw(colorTexture, new Rectangle(238, 384, 92, 30), optionSelected * 0.5f);
                        break;
                    case InGameStates.Trade:
                        spriteBatch.Draw(colorTexture, new Rectangle(330, 384, 95, 30), optionSelected * 0.5f);
                        break;
                    case InGameStates.Inventory:
                        spriteBatch.Draw(colorTexture, new Rectangle(425, 384, 103, 30), optionSelected * 0.5f);
                        break;
                    default:
                        break;
                }

                /**************************************
                * This is drawn while you are in the Adding mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Add)
                {
                    addSprite = spriteDisplay.getDisplay(addDisplayCount, colorChosen);
                    spriteBatch.Draw(Content.Load<Texture2D>(addSprite[0]), new Rectangle(32, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(addSprite[1]), new Rectangle(89, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(addSprite[2]), new Rectangle(146, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(addSprite[3]), new Rectangle(203, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(addSprite[4]), new Rectangle(260, 434, 32, 32), Color.White);

                    if (moveSelected == true)
                    {
                        if (spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null)
                        {
                            spriteBatch.Draw(goodSpace, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White * 0.5f);
                        }
                        else
                        {
                            spriteBatch.Draw(badSpace, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White * 0.5f);
                        }

                        if (spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) == 0 && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null)
                        {
                            spriteBatch.Draw(goodselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(badselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                        }
                    }
                }

                /**************************************
                * This is drawn while you are in the Building mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Build)
                {
                    buildSprite = assetDisplay.getDisplay(buildDisplayCount);
                    spriteBatch.Draw(Content.Load<Texture2D>(buildSprite[0]), new Rectangle(32, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(buildSprite[1]), new Rectangle(89, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(buildSprite[2]), new Rectangle(146, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(buildSprite[3]), new Rectangle(203, 434, 32, 32), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>(buildSprite[4]), new Rectangle(260, 434, 32, 32), Color.White);

                    if (moveSelected == true)
                    {
                        if (spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) == Sprites.Null && assetEngine.assetAt(drawStartX + cursorX, drawStartY + cursorY) == Assets.Null)
                        {
                            spriteBatch.Draw(goodSpace, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White * 0.5f);
                        }
                        else
                        {
                            spriteBatch.Draw(badSpace, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White * 0.5f);
                        }

                        if (spriteEngine.getStat(drawStartX + cursorX, drawStartY + cursorY, 0) == 0 && spriteEngine.spriteAt(drawStartX + cursorX, drawStartY + cursorY) != Sprites.Null)
                        {
                            spriteBatch.Draw(goodselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(badselector, new Rectangle(cursorX * 32, cursorY * 32, 32, 32), Color.White);
                        }
                    }
                }

                // Draw the mini map
                spriteBatch.Draw(grid, new Rectangle(windowWidth - 225, windowHeight - 107, 225, 107), Color.White);
                int drawX = drawStartX, drawY = drawStartY;
                for (int dotY = 384; dotY < 492; dotY += 9)
                {
                    for (int dotX = 575; dotX < 800; dotX += 9)
                    {
                        if (spriteEngine.spriteAt(drawX, drawY) != 0)
                        {
                            if (spriteEngine.getStat(drawX, drawY, 0) == 0)
                            {
                                spriteBatch.Draw(allyDot, new Rectangle(dotX, dotY, 7, 7), Color.White);
                            }
                            if (spriteEngine.getStat(drawX, drawY, 0) == 1)
                            {
                                spriteBatch.Draw(enemyDot, new Rectangle(dotX, dotY, 7, 7), Color.White);
                            }
                        }
                        drawX++;
                    }
                    drawY++;
                    drawX = drawStartX;
                }

                // Draw the rupees
                spriteBatch.Draw(rupee, new Rectangle(330, 435, 32, 32), Color.White);
                spriteBatch.DrawString(font, rupees.ToString(), new Vector2(362, 440), Color.White);

                /**************************************
                * This is drawn while you are in the Trading mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Trade)
                {
                    spriteBatch.Draw(inventoryMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    spriteBatch.DrawString(font, "Trade", new Vector2((windowWidth / 2) - (font.MeasureString("Inventory").X / 2), 0), Color.White);

                    // Displays minerals
                    spriteBatch.DrawString(font, "Minerals:", new Vector2(0, 32), Color.White);

                    spriteBatch.Draw(tree, new Rectangle(0, 64, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Wood X" + mineralInventory[0].ToString(), new Vector2(32, 64), Color.White);
                    spriteBatch.Draw(sellButton, new Rectangle(160, 64, 64, 32), Color.White);

                    spriteBatch.Draw(coal, new Rectangle(0, 96, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Coal X" + mineralInventory[1].ToString(), new Vector2(32, 96), Color.White);
                    spriteBatch.Draw(sellButton, new Rectangle(160, 96, 64, 32), Color.White);

                    spriteBatch.Draw(iron, new Rectangle(0, 128, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Iron X" + mineralInventory[2].ToString(), new Vector2(32, 128), Color.White);
                    spriteBatch.Draw(sellButton, new Rectangle(160, 128, 64, 32), Color.White);

                    spriteBatch.Draw(diamond, new Rectangle(0, 160, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Diamond X" + mineralInventory[3].ToString(), new Vector2(32, 160), Color.White);
                    spriteBatch.Draw(sellButton, new Rectangle(160, 160, 64, 32), Color.White);
                }

                /**************************************
                * This is drawn while you are in the Inventory mode in the game.
                * ***********************************/
                if (inGameState == InGameStates.Inventory)
                {
                    spriteBatch.Draw(inventoryMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    spriteBatch.DrawString(font, "Inventory", new Vector2((windowWidth / 2) - (font.MeasureString("Inventory").X / 2), 0), Color.White);

                    // Displays minerals
                    spriteBatch.DrawString(font, "Minerals:", new Vector2(0, 32), Color.White);

                    spriteBatch.Draw(tree, new Rectangle(0, 64, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Wood X" + mineralInventory[0].ToString(), new Vector2(32, 64), Color.White);

                    spriteBatch.Draw(coal, new Rectangle(0, 96, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Coal X" + mineralInventory[1].ToString(), new Vector2(32, 96), Color.White);

                    spriteBatch.Draw(iron, new Rectangle(0, 128, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Iron X" + mineralInventory[2].ToString(), new Vector2(32, 128), Color.White);

                    spriteBatch.Draw(diamond, new Rectangle(0, 160, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Diamond X" + mineralInventory[3].ToString(), new Vector2(32, 160), Color.White);

                    // Displays units
                    spriteBatch.DrawString(font, "Units:", new Vector2(200, 32), Color.White);

                    spriteBatch.Draw(swordsMan, new Rectangle(200, 64, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Swordsman X" + unitInventory[0].ToString(), new Vector2(232, 64), Color.White);

                    spriteBatch.Draw(axeMan, new Rectangle(200, 96, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Axeman X" + unitInventory[1].ToString(), new Vector2(232, 96), Color.White);

                    spriteBatch.Draw(knight, new Rectangle(200, 128, 32, 32), Color.White);
                    spriteBatch.DrawString(font, "Halberdman X" + unitInventory[2].ToString(), new Vector2(232, 128), Color.White);
                }
            }

            /**************************************
             * This is drawn when you are on the pause menu inside the game.
             * ***********************************/
            if (gameState == States.Pause_Menu)
            {
                spriteBatch.Draw(pauseMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            }

            /**************************************
             * This pauses the game while it is the computer's turn.
             * ***********************************/
            if (!isYourTurn)
            {
            }

            spriteBatch.Draw(cursor, new Rectangle(mouse.X, mouse.Y, 32, 32), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}