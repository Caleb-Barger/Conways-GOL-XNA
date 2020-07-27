using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Program : Game
{
	GraphicsDeviceManager gdm;
	SpriteBatch spriteBatch;

	int rows = 100;
	int cols = 100;

	bool[,] gameBoard = new bool[100, 100];

	Texture2D cellSprite;

	public void Clear(bool[,] board)
    {
		for (int row=0; row < rows; row++)
        {
			for (int col=0; col < cols; col++)
            {
				board[row, col] = false;
            }
        }
    }

	public int CountAlive(int row, int col)
    {
		int count = 0;

		if ((col > 0) && (row > 0) && gameBoard[row - 1, col -1]) {
			count++;
        }

		if ((row > 0) && gameBoard[row -1, col])
        {
			count++;
        }

		if ((row > 0) && (col < cols - 1) && gameBoard[row -1, col + 1])
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

		if ((col > 0) && (row < rows - 1) && gameBoard[row + 1 ,col - 1])
        {
			count++;
        }

		if (row < (rows - 1) && gameBoard[row + 1, col]) {
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
		GraphicsDeviceManager gdm = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";

		// Typically you would load a config here...
		gdm.PreferredBackBufferWidth = 1000;
		gdm.PreferredBackBufferHeight = 1000;
		gdm.IsFullScreen = false;
		gdm.SynchronizeWithVerticalRetrace = true;
	}

	protected override void Initialize()
	{
		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */

		Random r = new Random(DateTime.Now.Millisecond); // generate with seed
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (r.NextDouble() > 0.5f)
                {
					gameBoard[row, col] = true;
                } else
                {
					gameBoard[row, col] = false;
                }
			}
		}

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
		//for (int row = 0; row < rows; row++)
		//{

		//	bool[,] nextGameBoard = (bool[,])gameBoard.Clone();

		//	Clear(nextGameBoard);
		//	for (int col = 0; col < cols; col++)
		//	{
		//		int count = CountAlive(row, col);

		//		if (gameBoard[row, col])
  //              {
		//			if (count < 2)
  //                  {
		//				nextGameBoard[row, col] = false;
  //                  }

		//			if ((count  == 2) || (count == 3))
  //                  {
		//				nextGameBoard[row, col] = true;
  //                  }

		//			if (count > 3)
  //                  {
		//				nextGameBoard[row, col] = false;
  //                  }
  //              } else
  //              {
		//			if (count == 3)
  //                  {
		//				nextGameBoard[row, col] = true;
  //                  }
  //              }
		//	}

  //          bool[,] temp;
  //          temp = gameBoard;
		//	gameBoard = nextGameBoard;
		//	nextGameBoard = temp;

		//}

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
					spriteBatch.Draw(cellSprite, pos, Color.Black);
                } else
                {
					spriteBatch.Draw(cellSprite, pos, Color.White);
                }
			}
		}

		spriteBatch.End();

		base.Draw(gameTime);
	}
}