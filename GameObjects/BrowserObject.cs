using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Horn_War_II.GameObjects
{
    class BrowserObject : DrawableObject
    {
        private ChromiumWebBrowser browser;
        private Rectangle Bounds;

        private Texture2D Texture;
        private Texture2D NewTexture;
        public BrowserObject(Scenes.GameScene GameScene) : base(GameScene)
        {
            Bounds = new Rectangle(0, 0, Game.Graphics.PreferredBackBufferWidth, Game.Graphics.PreferredBackBufferHeight);

            this.Overlay = true;

            //string testUrl = "https://www.google.com/";
            string testUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI\\MainPage.html");

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            browser = new ChromiumWebBrowser(testUrl);
            browser.NewScreenshot += Browser_NewScreenshot;
            browser.Size = new System.Drawing.Size(Bounds.Width, Bounds.Height);
            browser.LoadingStateChanged += BrowserLoadingStateChanged;
        }

        private void Browser_NewScreenshot(object sender, EventArgs e)
        {
            var task = browser.ScreenshotAsync();
            task.ContinueWith(x =>
            {

                NewTexture = GetTexture2DFromBitmap(GameScene.Game.GraphicsDevice, task.Result);

                task.Result.Dispose();

            }, TaskScheduler.Default);
        }

        int OldMouseWheel = Mouse.GetState().ScrollWheelValue;
        KeyboardState OldKeyState = Keyboard.GetState();
        public override void Update(GameTime gameTime)
        {
            var MouseState = Mouse.GetState();
            if(MouseState.LeftButton == ButtonState.Pressed)
                browser.GetBrowser().GetHost().SendMouseClickEvent((int)Game.InputManager.MousePosition.X, (int)Game.InputManager.MousePosition.Y, MouseButtonType.Left, false, 1, CefEventFlags.LeftMouseButton);
            else if(MouseState.LeftButton == ButtonState.Released)
                browser.GetBrowser().GetHost().SendMouseClickEvent((int)Game.InputManager.MousePosition.X, (int)Game.InputManager.MousePosition.Y, MouseButtonType.Left, true, 1, CefEventFlags.LeftMouseButton);

            browser.GetBrowser().GetHost().SendMouseMoveEvent(new MouseEvent((int)Game.InputManager.MousePosition.X, (int)Game.InputManager.MousePosition.Y, CefEventFlags.None), false);
            browser.GetBrowser().GetHost().SendMouseWheelEvent(new MouseEvent((int)Game.InputManager.MousePosition.X, (int)Game.InputManager.MousePosition.Y, CefEventFlags.None), 0, MouseState.ScrollWheelValue - OldMouseWheel);




            OldMouseWheel = MouseState.ScrollWheelValue;

            var keyState = Keyboard.GetState();

            var pressedKeys = keyState.GetPressedKeys();
            var pressedKeysOld = OldKeyState.GetPressedKeys();

            var allKeys = Enum.GetNames(typeof(Keys));
            

            foreach(var key in pressedKeys)
            {
                if(!pressedKeysOld.Contains(key) && KeyToWin.ContainsKey(key))
                    browser.GetBrowser().GetHost().SendKeyEvent(new KeyEvent()
                    {
                        Type = KeyEventType.KeyDown,
                        WindowsKeyCode = KeyToWin[key]
                    });
            }

            foreach (var key in pressedKeysOld)
            {
                if (!pressedKeys.Contains(key) && KeyToWin.ContainsKey(key))
                    browser.GetBrowser().GetHost().SendKeyEvent(new KeyEvent()
                    {
                        Type = KeyEventType.KeyUp,
                        WindowsKeyCode = KeyToWin[key],
                    });
            }


            OldKeyState = keyState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (NewTexture != null)
            {
                if (Texture != null)
                    Texture.Dispose();
                Texture = NewTexture;
                NewTexture = null;
            }

            if(Texture != null)
                Game.SpriteBatch.Draw(Texture, this.Bounds, Microsoft.Xna.Framework.Color.White);

            base.Draw(gameTime);
        }

        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                browser.LoadingStateChanged -= BrowserLoadingStateChanged;

                var task = browser.ScreenshotAsync();
                task.ContinueWith(x =>
                {
                    Texture = GetTexture2DFromBitmap(GameScene.Game.GraphicsDevice, task.Result);


                    // Save the Bitmap to the path.
                    // The image type is auto-detected via the ".png" extension.
                    //task.Result.Save(screenshotPath);

                    task.Result.Dispose();

                }, TaskScheduler.Default);
            }
        }
        public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, System.Drawing.Bitmap bitmap)
        {
            Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height);

            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int bufferSize = data.Height * data.Stride;

            //create data buffer 
            byte[] bytes = new byte[bufferSize];

            // copy bitmap data into buffer
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // copy our buffer to the texture
            tex.SetData(bytes);

            // unlock the bitmap data
            bitmap.UnlockBits(data);

            return tex;
        }

        public static Dictionary<Keys, int> KeyToWin = new Dictionary<Keys, int>()
        {
            { Keys.A, 'A' },
            { Keys.B, 'B' },
            { Keys.C, 'C' },
            { Keys.D, 'D' },
            { Keys.E, 'E' },
            { Keys.F, 'F' },
            { Keys.G, 'G' },
            { Keys.H, 'H' },
            { Keys.I, 'I' },
            { Keys.J, 'J' },
            { Keys.K, 'K' },
            { Keys.L, 'L' },
            { Keys.M, 'M' },
            { Keys.N, 'N' },
            { Keys.O, 'O' },
            { Keys.P, 'P' },
            { Keys.Q, 'Q' },
            { Keys.R, 'R' },
            { Keys.S, 'S' },
            { Keys.T, 'T' },
            { Keys.U, 'U' },
            { Keys.V, 'V' },
            { Keys.W, 'W' },
            { Keys.X, 'X' },
            { Keys.Y, 'Y' },
            { Keys.Z, 'Z' },
            { Keys.D0, 10 },
            { Keys.D1, 10 },
            { Keys.D2, 10 },
            { Keys.D3, 10 },
            { Keys.D4, 10 },
            { Keys.D5, 10 },
            { Keys.D6, 10 },
            { Keys.D7, 10 },
            { Keys.D8, 10 },
            { Keys.D9, 10 },
            { Keys.NumPad0, 10 },
            { Keys.NumPad1, 10 },
            { Keys.NumPad2, 10 },
            { Keys.NumPad3, 10 },
            { Keys.NumPad4, 10 },
            { Keys.NumPad5, 10 },
            { Keys.NumPad6, 10 },
            { Keys.NumPad7, 10 },
            { Keys.NumPad8, 10 },
            { Keys.NumPad9, 10 },
        };
    }
}
