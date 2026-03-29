using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.LinkLabel;

namespace Wordle
{

    public partial class Form1 : Form
    {
        List<Label> letterLabel = new List<Label>();
        public int curCol, curRow, guesses;
        public int n; // numarul de cuvinte din fisierul de cuvinte - 11062
        public string word = "";
        Random rand = new Random();
        public string[] words = [];
        public Form1()
        {
            InitializeComponent();
            n = 0;
            WordsCollector();
            word = words[rand.Next(words.Length)];
            curCol = curRow = 0;
            guesses = 1;
            generateLabels();
            this.KeyPress += Form1_KeyPress;
            this.KeyPreview = true;
        }

        public void WordsCollector()
        {
            if (!File.Exists("words.txt"))
            {
                MessageBox.Show("Fisierul nu exista!");
                return;
            }

            StreamReader fin = new StreamReader("words.txt");
            string line;
            string[] cuv;
            while ((line = fin.ReadLine()) != null)
            {
                cuv = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string e in cuv)
                    words[++n] = e;
            }
            fin.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void generateLabels()
        {
            int width = 60;
            int height = 60;
            int spacing = 4;
            int rowWidth = (width * 5) + 4 * spacing;
            int x = (this.ClientSize.Width - rowWidth) / 2;
            int y = 150;
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    Label lblLetter = new Label();
                    lblLetter.Text = "";
                    lblLetter.Font = new Font("Rockwell", 24, FontStyle.Bold);
                    lblLetter.Size = new Size(width, height);
                    lblLetter.TextAlign = ContentAlignment.MiddleCenter;
                    lblLetter.BorderStyle = BorderStyle.FixedSingle;
                    lblLetter.BackColor = Color.WhiteSmoke;
                    lblLetter.Location = new Point(x + j * (width + spacing), y);
                    this.Controls.Add(lblLetter);
                    letterLabel.Add(lblLetter);
                }

                y += 80;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string guess = "";
            char letter = ' ';
            if (char.IsLetter(e.KeyChar))
            {
                if (curCol < (curRow + 1) * 5)
                {
                    letter = char.ToUpper(e.KeyChar);
                    letterLabel[curCol].Text = letter.ToString();
                    curCol++;
                }
                else if (e.KeyChar == (char)(Keys.Enter))
                {
                    // build the word from our list of labels
                    guess = buildWord();

                    // check if it is a valid word
                    if (checkValid(guess))
                    {
                        guesses++;
                        updateLabels(guess);// update the labels with bg color
                    }
                    // check for the win
                    if (checkWin(guess))
                    {
                        MessageBox.Show("You win!");
                        Application.Exit();
                        return;
                    }
                    else if (guesses == 6)// check to see if guesses = 6
                    {
                        MessageBox.Show("You ran out of guesses. The word was: " + word);
                        Application.Exit();
                        return;
                    }

                    curRow++;
                    curCol = curRow * 5;

                   
                    
                    
                }
                else MessageBox.Show("Not a valid word!"); // message box for a invalid word
            }
        }

        public string buildWord()
        {
            string builtWord = "";
            for (int i = curRow * 5; i < curRow * 5 + 5; i++)
            {
                builtWord += letterLabel[i].Text;
            }
            return builtWord;
        }

        public void updateLabels(string tempGuess)
        {
            tempGuess = tempGuess.ToUpper();
            string tempWord = word.ToUpper();

            for (int i = 0; i < 5; i++)
            {
                Label lbl = letterLabel[curRow * 5 + i];

                if (tempGuess[i] == tempWord[i])
                    lbl.BackColor = Color.Green;
                else if (tempWord.Contains(tempGuess[i]))
                    lbl.BackColor = Color.Goldenrod;
                else lbl.BackColor = Color.WhiteSmoke;
            }
        }
        public bool checkWin(string tempGuess)
        {
            return tempGuess.ToUpper() == word.ToUpper();
        }

        public bool checkValid(string tempGuess)
        {
            return tempGuess.Length == 5;
        }

    }
}
