using System;

public class MyScreen1 : GameScreen
{
	private Game1 _myGame;
	private SpriteFont _font;

	public MyScreen1(Game1 game) : base(game) { _myGame = game }

	public override void LoadContent()
	{
		_font = Content.Load<SpriteFont>("Font");
		base.LoadContent();
	}
	public override void Update(GameTime gameTime)
	{ }
	public override void Draw(GameTime gameTime)
	{
		//_myGame.GraphicsDevice.Clear(Color.Red);
		_myGame.SpriteBatch.Begin();
		//_myGame.SpriteBatch.DrawString(_font, "Scene 1", new Vector2(350, 200), Color.White);
		_myGame.SpriteBatch.End();
	}
}




{
	}
}
