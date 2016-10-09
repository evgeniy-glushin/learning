using System;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Android.Views;
using Snake.Lib;
using static System.Diagnostics.Debug;
using static Snake.Droid.Resource.Id;

namespace Snake.Droid
{
    [Activity(Label = "Snake",
        MainLauncher = true,
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class MainActivity : Activity
    {
        private Button _btnPause;
        private Button _btnRestart;
        //private Button _lbtnUp;
        //private Button _lbtnLeft;
        //private Button _lbtnDown;
        //private Button _rbtnUp;
        //private Button _rbtnRight;
        //private Button _rbtnDown;
        private Button _btnPlay;
        private TextView _tvScore;


        //private RelativeLayout _lytFieldContainer;
        private CanvasSnakeField _snakeField;
        private SnakeGame _game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //_lbtnUp = FindViewById<Button>(Resource.Id.lbtnUp);
            //_lbtnLeft = FindViewById<Button>(Resource.Id.lbtnLeft);
            //_lbtnDown = FindViewById<Button>(Resource.Id.lbtnDown);
            //_rbtnUp = FindViewById<Button>(Resource.Id.rbtnUp);
            //_rbtnRight = FindViewById<Button>(Resource.Id.rbtnRight);
            //_rbtnDown = FindViewById<Button>(Resource.Id.rbtnDown);
            _btnRestart = FindViewById<Button>(rbtnRestart);
            _btnPlay = FindViewById<Button>(lbtnPlay);
            _btnPause = FindViewById<Button>(lbtnPause);

            _tvScore = FindViewById<TextView>(tvScore);

            //_lbtnLeft.Click += LeftBtnOnClick;
            //_lbtnUp.Click += UpBtnOnClick;
            //_lbtnDown.Click += DownBtnOnClick;

            //_rbtnRight.Click += RightBtnOnClick;
            //_rbtnUp.Click += UpBtnOnClick;
            //_rbtnDown.Click += DownBtnOnClick;

            _btnPlay.Click += PlayBtnOnClick;
            _btnPause.Click += PauseBtnOnClick;
            _btnRestart.Click += RestartBtnOnClick;

            //_lytFieldContainer = FindViewById<RelativeLayout>(Resource.Id.lytFieldContainer);
            //_lytFieldContainer.ViewTreeObserver.GlobalLayout += ContainerSnakeFieldOnGlobalLayout;
            _snakeField = FindViewById<CanvasSnakeField>(lytField);
            _snakeField.ViewTreeObserver.GlobalLayout += SnakeFieldOnGlobalLayout;
            _snakeField.Touch += SnakeFieldOnTouch;

        }


        private Position down;

        private void SnakeFieldOnTouch(object sender, View.TouchEventArgs e)
        {
            e.Handled = true;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    _game.TouchDown(new Position((int)e.Event.RawX, (int)e.Event.RawY));
                    //if (_game.Status == GameStatus.None || _game.Status == GameStatus.Pause || _game.Status == GameStatus.Restart)
                    //    _game.Play();
                    //else if (down == null)
                    //    down = new Position((int)e.Event.RawX, (int)e.Event.RawY);
                    break;
                case MotionEventActions.Up:
                    _game.TouchUp(new Position((int)e.Event.RawX, (int)e.Event.RawY));
                    //if(down != null) RecognizeDirection(e);
                    //down = null;
                    break;
                case MotionEventActions.Move:
                    break;
            }
        }

        //private void Recognize(View.TouchEventArgs e)
        //{
        //    var dir = MotionRecognizer.Recognize(down, new Position((int)e.Event.RawX, (int)e.Event.RawY));
        //    WriteLine($"dir: {dir}");
        //    switch (dir)
        //    {
        //        case Direction.Left:
        //            LeftBtnOnClick(null, null);
        //            break;
        //        case Direction.Right:
        //            RightBtnOnClick(null, null);
        //            break;
        //        case Direction.Up:
        //            UpBtnOnClick(null, null);
        //            break;
        //        case Direction.Down:
        //            DownBtnOnClick(null, null);
        //            break;
        //    }
        //}

        private void InitGame()
        {
            if (_game == null)
            {
                var bodyFactory = new BodyFactory(Resources);
                var foodFactory = new FoodFactory(Resources);
                _game = new SnakeGame(_snakeField, bodyFactory, foodFactory, 34, 200);
                _game.OnGameOver += OnGameOver;
                _game.OnScoreChanged += GameOnOnScoreChanged;
            }
        }

        private void GameOnOnScoreChanged()
        {
            _tvScore.Text = _game.Score.ToString();
        }

        private void OnGameOver()
        {
            //var lbl = FindViewById<TextView>(Resource.Id.tvGameStatus);
            //lbl.Text = "Game Over";
        }

        private void PauseBtnOnClick(object sender, EventArgs e)
        {
            _game.Pause();
        }

        private void PlayBtnOnClick(object sender, EventArgs e)
        {
            _game.Play();
        }

        private void RestartBtnOnClick(object sender, EventArgs e)
        {
            _game.Restart();
        }


        //private bool _started = false;
        private void SnakeFieldOnGlobalLayout(object s, EventArgs e)
        {
            //if (_started) return;
            //_started = true;
            //_snakeField.ViewTreeObserver.GlobalLayout -= SnakeFieldOnGlobalLayout;

            InitGame();
        }

        private void DownBtnOnClick(object sender, EventArgs eventArgs)
        {
            _game.Down();
        }

        private void LeftBtnOnClick(object sender, EventArgs eventArgs)
        {
            _game.Left();
        }

        private void RightBtnOnClick(object sender, EventArgs eventArgs)
        {
            _game.Right();
        }

        private void UpBtnOnClick(object sender, EventArgs eventArgs)
        {
            _game.Up();
        }
    }
}

