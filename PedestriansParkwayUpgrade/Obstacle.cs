using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PedestriansParkwayUpgrade
{
    public class Obstacle
    {
        public int x, y, xSpeed, ySpeed, xSize, ySize, playerSpeed;

        Random random = new Random();
        public Obstacle(int x, int y, int xSpeed, int ySpeed)
        {
            this.x = x;
            this.y = y;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;

            xSize = 40;
            ySize = 30;

            playerSpeed = 40;
        }

        public void Move()
        {
            x += xSpeed;

            if (x > GameScreen.width)
            {
                x = -40;
            }
            if (x < xSize * -1)
            {
                x = GameScreen.width;
            }
        }

        public void Forward()
        {
            y += playerSpeed;
        }

        public void Down()
        {
            y += ySpeed;
        }
       
    }
}
