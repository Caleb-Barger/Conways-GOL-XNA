using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

class Program : Game
{
    GraphicsDeviceManager gdm;
    SpriteBatch spriteBatch;

    int rows = 80;
    int cols = 120;
    double millisecondsPerFrame = 250;
    double timeSinceLastUpdate = 0;


    Color[] colors = {
        Color.Black,
        Color.Red,
        Color.Green,
        Color.Blue
    };

    Color currentColor;

    bool[,] gameBoard = new bool[80, 120];
    bool[,] nextGameBoard = new bool[80, 120];

    Texture2D cellSprite;


    // Input
    private KeyboardState keyboardPrev = new KeyboardState();

    public void Clear(bool[,] board)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                board[row, col] = false;
            }
        }
    }


    public void RandomizeBoard()
    {
        Random r = new Random(DateTime.Now.Millisecond); // generate with seed
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (r.NextDouble() > 0.5f)
                {
                    gameBoard[row, col] = true;
                    Console.WriteLine(gameBoard[row, col]);
                }
                else
                {
                    gameBoard[row, col] = false;
                }
            }
        }
    }

    public Color PickColor()
    {
        Random r = new Random();
        int index = r.Next(colors.Length);
        return colors[index];
    }

    public int CountAlive(int row, int col)
    {
        int count = 0;

        if ((col > 0) && (row > 0) && gameBoard[row - 1, col - 1])
        {
            count++;
        }

        if ((row > 0) && gameBoard[row - 1, col])
        {
            count++;
        }

        if ((row > 0) && (col < cols - 1) && gameBoard[row - 1, col + 1])
        {
            count++;
        }

        if ((col > 0) && gameBoard[row, col - 1]) // HERE <-----
        {
            count++;
        }

        if (col < (cols - 1) && gameBoard[row, col + 1])
        {
            count++;
        }

        if ((col > 0) && (row < rows - 1) && gameBoard[row + 1, col - 1])
        {
            count++;
        }

        if (row < (rows - 1) && gameBoard[row + 1, col])
        {
            count++;
        }

        if (col < (cols - 1) && row < (rows - 1) && gameBoard[row + 1, col])
        {
            count++;
        }


        return count;
    }


    static void Main(string[] args)
    {
        using (Program g = new Program())
        {
            g.Run();
        }
    }

    private Program()
    {
        gdm = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        // Typically you would load a config here...
        gdm.PreferredBackBufferWidth = 1200;
        gdm.PreferredBackBufferHeight = 800;
        gdm.IsFullScreen = false;
        gdm.SynchronizeWithVerticalRetrace = true;
    }

    protected override void Initialize()
    {
        /* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
        RandomizeBoard();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load textures, sounds, and so on in here...
        spriteBatch = new SpriteBatch(GraphicsDevice);
        cellSprite = Content.Load<Texture2D>("sprite");
        base.LoadContent();
    }

    protected override void UnloadContent()
    {
        // Clean up after yourself!
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    { 
        // wait to update the game (game speed)
        timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (timeSinceLastUpdate >= millisecondsPerFrame)
        {

            timeSinceLastUpdate = 0;

            for (int row = 0; row < rows; row++)
            {
                //Clear(gameBoard);

                for (int col = 0; col < cols; col++)
                {
                    int count = CountAlive(row, col);

                    if (gameBoard[row, col])
                    {
                        if (count < 2)
                        {
                            gameBoard[row, col] = false;
                        }

                        if ((count == 2) || (count == 3))
                        {
                            gameBoard[row, col] = true;
                        }

                        if (count > 3)
                        {
                            gameBoard[row, col] = false;
                        }
                    }
                    else
                    {
                        if (count == 3)
                        {
                            gameBoard[row, col] = true;
                        }
                    }
                }

                //bool[,] temp;
                //temp = gameBoard;
                //gameBoard = nextGameBoard;
                //nextGameBoard = temp;
            }
        }

        // Input handler
        KeyboardState keyboardCur = Keyboard.GetState();

        if (keyboardCur.IsKeyDown(Keys.C) && keyboardPrev.IsKeyUp(Keys.C))
        {
            // C was pressed
            currentColor = PickColor();
        }
        if (keyboardCur.IsKeyDown(Keys.R) && keyboardPrev.IsKeyUp(Keys.R))
        {
            // R was pressed
            RandomizeBoard();
        }

        if (keyboardCur.IsKeyDown(Keys.W) && keyboardPrev.IsKeyUp(Keys.W))
        {
            // increase game speed
            millisecondsPerFrame -= 30; 
        }

        if (keyboardCur.IsKeyDown(Keys.S) && keyboardPrev.IsKeyUp(Keys.S))
        {
            // decrease game speed
            millisecondsPerFrame += 30;
        }

        keyboardPrev = keyboardCur;

        // Run game logic in here. Do NOT render anything here!
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Render stuff in here. Do NOT run game logic in here!
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2 pos = new Vector2();

                pos.X = col * (cellSprite.Width + 1);
                pos.Y = row * (cellSprite.Height + 1);

                if (gameBoard[row, col])
                {
                    spriteBatch.Draw(cellSprite, pos, currentColor);
                }
                else
                {
                    spriteBatch.Draw(cellSprite, pos, Color.White);
                }
            }
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }
}