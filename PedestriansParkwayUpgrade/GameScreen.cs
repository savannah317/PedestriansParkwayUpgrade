using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PedestriansParkwayUpgrade
{
    public partial class GameScreen : UserControl
    {
        public static List<Obstacle> roads = new List<Obstacle>();
        List<Obstacle> cars = new List<Obstacle>();

        Player hero;

        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        SoundPlayer moveSound = new SoundPlayer(Properties.Resources.jumpSound);

        Random randGen = new Random();
        public static int width, height;
        int startPos;

        bool rightArrowDown = false;
        bool leftArrowDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        bool canMove = false;
        int counter = 0;
        int score = 0;

        public GameScreen()
        {
            InitializeComponent();
            InitializeGame();
        }

        public void InitializeGame()
        {
            width = this.Width;
            height = this.Height;

            startPos = 645;

            for (int i = 0; i < 2; i++)
            {
                //player exists
                hero = new Player(GameScreen.width / 2, 645);

                //create a number of obstacles on start
                newObstacle();
                
            }
        }

        public void newObstacle()
        {
            int randValue = randGen.Next(0, 5);
            int y = startPos - 40;

            if (randValue == 0)
            {
                startPos -= 40;
            }
            if (randValue == 1)
            {
                startPos -= randValue * 40;
            }
            if (randValue == 2) 
            {
                startPos -= randValue * 40;   
            }
            if (randValue == 3)
            {
                startPos -= randValue * 40;
            }
            else
            {
                startPos -= randValue * 40;
            }
            int x = randGen.Next(0, width);
            int xSpeed = randGen.Next(5, 20);
            int ySpeed = 1;
            
            if (randValue % 2 == 0)
            {
                xSpeed *= -1;
            }

            //create roads
            Obstacle road = new Obstacle(0, y - 5, 45, ySpeed);
            roads.Add(road);

            for (int i = 0; i < 4; i++)
            {
                //create cars with the same y values as roads
                Obstacle car = new Obstacle(x, y, xSpeed, ySpeed);
                cars.Add(car);

                x += randGen.Next(60, width);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //create a new obstacle row
            int create = 0;

            if (create % 20 == 0)
            {
                newObstacle();
                create = 0;
            }
            else
            {
                create++;
            }

            foreach (Obstacle car in cars)
            {
                //cars move
                car.Move();
            }

            for (int i = 0; i < cars.Count; i++)
            {
                //create collisions
                if (hero.Collision(cars[i]))
                {
                    cars.Clear();

                    for (int r = 0; r < roads.Count; r++)
                    {
                        roads.Clear();
                    }

                    //stop the game you lose
                    gameTimer.Stop();

                    Form1.ChangeScreen(this, new MenuScreen());
                }
            }
            
            if (leftArrowDown && canMove == true)
            {
                moveSound.Play();
                //player moves left
                hero.Move("left");
                //allows the player to "hop" from place to place instead of smooth movements
                counter = 0;
                canMove = false;
            }
            if (rightArrowDown && canMove == true)
            {
                //player moves right
                hero.Move("right");
                counter = 0;
                canMove = false;
            }
            if (upArrowDown && canMove == true)
            {
                //player moves forward
                //adds to score
                score++;
                moveSound.Play();
                if (hero.y < 500)
                {
                    //moves everything on screen downward so player can't go off screen
                    foreach (Obstacle car in cars)
                    {
                        car.Forward();
                    }
                    foreach (Obstacle road in roads)
                    {
                        road.Forward();
                    }
                }
                if (hero.y >= 500)
                {
                    //at a certain y value player can move upwards
                    hero.Move("up");
                }
                counter = 0;
                canMove = false;
            }
            if (downArrowDown && canMove == true)
            {
                moveSound.Play();
                //player moves backward
                hero.Move("down");
                counter = 0;
                canMove = false;
            }

            if (canMove == false)
            {
                //player can only move one "square" at a time
                downArrowDown = false;
                upArrowDown = false;
                rightArrowDown = false;
                leftArrowDown = false;
            }

            counter++;
            if (counter % 3 == 0)
            {
                canMove = true;
            }

            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].y > height)
                {
                    cars.RemoveAt(i);
                }
            }

            if (score > 5)
            {
                //moves the screen downward to make the game more difficult
                hero.y += 1;
                foreach (Obstacle road in roads)
                {
                    road.Down();
                }
                foreach (Obstacle car in cars)
                {
                    car.Down();
                }
            }
            scoreLabel.Text = Convert.ToString(score);
            Refresh();
        }


        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw roads
            foreach (Obstacle road in roads)
            {
                e.Graphics.FillRectangle(grayBrush, 0, road.y, GameScreen.width, 40);
            }
            //draw cars
            foreach (Obstacle car in cars)
            {
                e.Graphics.FillRectangle(greenBrush, car.x, car.y, car.xSize, car.ySize);
            }
            //draw player
            e.Graphics.FillRectangle(whiteBrush, hero.x, hero.y, hero.width, hero.height);
        }
    }
}
