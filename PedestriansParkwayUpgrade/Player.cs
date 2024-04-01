using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedestriansParkwayUpgrade
{
    internal class Player
    {
        public int x, y;
        public int width = 30;
        public int height = 30;
        public int speed = 40;

        public Player(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public void Move(string direction)
        { 
            if (direction == "up" && y > 0)
            {
                y -= speed;
                
            }
            if (direction == "down")
            {
                y += speed;
               
            }
            if (direction == "left" && x > 0)
            {
                x -= speed;
            }
            if (direction == "right" && x < GameScreen.width - width)
            {
                x += speed;
            }
            if (direction == "no")
            {

            }
        }

        public bool Collision(Obstacle car)
        {
            Rectangle playerRec = new Rectangle(x, y, width, height);
            Rectangle carRec = new Rectangle(car.x, car.y, car.xSize, car.ySize);

            //check for intersections between player and obstacles
            if (playerRec.IntersectsWith(carRec))
            {
                return true;
            }
            //check if player goes off screen
            if (y > GameScreen.height + height)
            {
                return true;
            }
            return false;
        }
    }
}
