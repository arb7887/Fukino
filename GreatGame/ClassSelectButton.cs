using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class ClassSelectButton : MenuButton
    {
        private List<MenuButton> classOptions;
        private bool clicked;
        private FileInput<Unit> listOfUnits;

        public bool Clicked { get { return clicked; } set { clicked = value; } }

        public ClassSelectButton(Rectangle loc, Texture2D t, string n, Color s,  SpriteFont sf)
            :base(loc, t, n, s, sf)
        {
            listOfUnits = new FileInput<Unit>("Content/Units.txt");
            listOfUnits.LoadUnit();


            classOptions = new List<MenuButton>();

            for(int i = 0; i < listOfUnits.UnitList.Count(); i++)
            {
                // This now loads from the notepad document instead
                String temp = "";
                temp = listOfUnits.UnitList[i].Name;
                #region
                /* switch (i)
                 {
                     case 0:
                         temp = listOfUnits.UnitList[i].Name;
                         break;
                     case 1:
                         temp = "Shotgun";
                         break;
                     case 2:
                         temp = "Alien";
                         break;
                     case 3:
                         temp = "Engineer";
                         break;
                     case 4:
                         temp = "Minigun";
                         break;
                     case 5:
                         temp = "Assasin";
                         break;
                     case 6:
                         temp = "Sniper";
                         break;
                     case 7:
                         temp = "Medic";
                         break;
                     case 8:
                         temp = "Buff";
                         break;
                 }*/
                #endregion
                classOptions.Add(new MenuButton(loc, t, temp, s, sf, false));
                classOptions[i].Y -= 50 * (i + 1);
            }
            clicked = false;
        }

        public override bool CheckClicked(MouseState ms)
        {
            if (!Enabled)
                return false;

            if (clicked)
            {//if you're already active (ie. been clicked) wait for a class to be selected
                CheckClassClicked(ms);
                return true;//So long as the button is active, I'm considering it clicked.
            }
            else
            {//otherwise, check to see if you get clicked
                if (base.CheckClicked(ms))
                {//if so, expand the list of other buttons to select class
                    for (int j = 0; j < classOptions.Count; j++)
                        classOptions[j].Enabled = true;
                    clicked = true;//let the world know you've been clicked
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        public void CheckClassClicked(MouseState ms)
        {
            for(int i = 0; i < classOptions.Count; i++)
            {//check every button in the sub set (one for each class
                if (classOptions[i].CheckClicked(ms))
                {//if one of them was clicked, change the name of this button to the clicked buttons name
                    //and retract all of the options once more.  

                    this.Name = classOptions[i].Name;
                    clicked = false;
                    for (int j = 0; j < classOptions.Count; j++)
                        classOptions[j].Enabled = false; 
                }

            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            
            for(int i = 0; i < classOptions.Count; i++)
            {
                classOptions[i].Draw(sb);
            }
            
        }
    }
}
