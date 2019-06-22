using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace MatchingGame
{
    public partial class SecondForm : Form
    {
        Random random = new Random();
        Label firstClick = null;
        Label secondClick = null;
        int winner = 0;
        int winnerTime = 0;
        List<string> pictures = new List<string>()
        {
            "a","a","b","b","c","c",
            "d","d","e","e","f","f",
            "g","g","h","h","i","i",
            "j","j","k","k","l","l",
            "m","m","n","n","o","o",
            "p","p","q","q","r","r"
        };

        public SecondForm()
        {
            InitializeComponent();
        }

        private void AssignPicturesToSquare()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label picturesLabel = control as Label;
                if (picturesLabel != null)
                {
                    int randomNumber = random.Next(pictures.Count);
                    picturesLabel.Text = pictures[randomNumber];
                    pictures.RemoveAt(randomNumber);
                }
            }
        }

        private void HidePicturesBack()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label picturesLabel = control as Label;
                if (picturesLabel != null)
                {
                    int randomNumber = random.Next(pictures.Count);
                    picturesLabel.ForeColor = picturesLabel.BackColor;
                }
            }
        }

        private void picture_Click(object sender, EventArgs e)
        {
            if (howLong.Enabled == true)
            {
                if (timer1.Enabled == true)
                    return;

                winner++;

                Label clickedPicture = sender as Label;

                if (clickedPicture != null)
                {
                    if (clickedPicture.ForeColor == Color.Black)
                        return;

                    if (firstClick == null)
                    {
                        firstClick = clickedPicture;
                        firstClick.ForeColor = Color.Black;
                        hide.Start();
                        return;
                    }

                    secondClick = clickedPicture;
                    secondClick.ForeColor = Color.Black;

                    if (winner > 0)
                    {
                        CheckForWinner();
                    }


                    if (firstClick.Text == secondClick.Text)
                    {
                        firstClick = null;
                        secondClick = null;
                        CorrectPlay();
                        return;
                    }

                    timer1.Start();
                    WrongPlay();

                }
            }
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label pictureLabel = control as Label;

                if (pictureLabel != null)
                {
                    if (pictureLabel.ForeColor == pictureLabel.BackColor)
                        return;
                }
            }

            DialogResult dialogResult = MessageBox.Show("Do you want play again ?",
                "You won in " + winnerTime + " seconds!!!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                howLong.Stop();
                this.Hide();
                SecondForm newForm = new SecondForm();
                newForm.ShowDialog();
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            HidePlay();

            firstClick.ForeColor = firstClick.BackColor;
            secondClick.ForeColor = secondClick.BackColor;

            firstClick = null;
            secondClick = null;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            countdownTimer.Start();
            startButton.Enabled = false;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            int timer = Convert.ToInt32(label1.Text);
            timer -= 1;
            label1.Text = Convert.ToString(timer);
            if (timer == 3)
            {
                tableLayoutPanel1.Enabled = false;
                AssignPicturesToSquare();
            }
            else if (timer == 0)
            {
                tableLayoutPanel1.Enabled = true;
                countdownTimer.Stop();
                HidePicturesBack();
                howLong.Start();
            }
        }

        private void Hide_Tick(object sender, EventArgs e)
        {
            hide.Stop();
            if (secondClick == null && secondClick != firstClick)
            {
                firstClick.ForeColor = firstClick.BackColor;
                firstClick = null;
            }
        }

        private void HowLong_Tick(object sender, EventArgs e)
        {
            int timer = Convert.ToInt32(label1.Text);
            timer += 1;
            label1.Text = Convert.ToString(timer);
            winnerTime++;
        }

        private void HidePlay()
        {
            Stream soundfile = Properties.Resources.hide;
            SoundPlayer sound = new SoundPlayer(soundfile);
            sound.Play();
        }

        private void CorrectPlay()
        {
            Stream soundfile = Properties.Resources.correct;
            SoundPlayer sound = new SoundPlayer(soundfile);
            sound.Play();
        }

        private void WrongPlay()
        {
            Stream soundfile = Properties.Resources.wrong;
            SoundPlayer sound = new SoundPlayer(soundfile);
            sound.Play();
        }

        private void ChangeSize_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm newForm = new MainForm();
            newForm.ShowDialog();
            this.Close();
        }
    }
}

