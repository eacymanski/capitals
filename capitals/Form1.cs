using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capitals
{
    public partial class capitalQuiz : Form
    {
        public capitalQuiz()
        {
            //questionBox Tag = statesNCaps
            //answer1 Tag = unUsed
            //answer4 Tag = correct button number
            InitializeComponent();     
        }

        private void capitalQuiz_Load(object sender, EventArgs e)
        {
            easyPanel.Location = startPanel.Location;
            hardPanel.Location = startPanel.Location;
            correctPanel.Location = startPanel.Location;
            startPanel.Show();
            easyPanel.Hide();
            hardPanel.Hide();
            correctPanel.Hide();
            this.Height = 252;
            this.Width = 334;
            //easyPanel.Visible = false;
            //correctPanel.Visible = false;
           // hardPanel.Visible = false;       
            Dictionary<string,string> statesNCaps=statesAndCapitals.capMaker();
            List<string> unused = Enumerable.ToList(statesNCaps.Values);
            easyQuestionBox.Tag = statesNCaps;
            hardQuestionBox.Tag = statesNCaps;
            answer1.Tag = unused;
        }

        private void easyButton_Click(object sender, EventArgs e)
        {         
            startPanel.Hide();
            easyPanel.Show();
            List < RadioButton >buttons= new List<RadioButton>() { answer1, answer2, answer3, answer4 };
            easyQuestion(buttons, easyQuestionBox);
            answerBox.Tag = false;
        }

        private void hardButton_Click(object sender, EventArgs e)
        {
            startPanel.Hide();
            hardPanel.Show();
            hardQuestion(hardQuestionBox, answer1, answer4);
            answerBox.Tag = true;
        }

        internal static void easyQuestion(List<RadioButton> buttons, Label questionBox)
        {
            Dictionary<string, string> statesAndCaps = (Dictionary<string, string>)questionBox.Tag;
            List<string> capitals = Enumerable.ToList(statesAndCaps.Values);
            List<string> unUsed = (List<string>)buttons[0].Tag;
            Random rand = new Random();
            List<string> capsToUse = new List<string>();
            int answerSize = unUsed.Count;
            string answer = unUsed[rand.Next(answerSize)];
            capsToUse.Add(answer);
            for (int i = 0; i < 3; i++)
            {
                string testCap = capitals[rand.Next(50)];
                while (capsToUse.Contains(testCap))
                {
                    testCap = capitals[rand.Next(50)];
                }
                capsToUse.Add(testCap);
            }
            for (int i = 0; i < 4; i++)
            {
                setButtons(buttons[i], capsToUse, answer, buttons[2], i);
            }
            string state = statesAndCaps.FirstOrDefault(x => x.Value.Equals(answer)).Key;
            questionBox.Text = ("What is the capital of " + state + "?");
            unUsed.Remove(answer);
            buttons[0].Tag = unUsed;
            buttons[3].Tag = answer;
        }

        private static void setButtons(RadioButton button, List<string> capsToUse, string answer, RadioButton button4, int number)
        {
            Random rand = new Random();
            int size = capsToUse.Count;
            int rInt = rand.Next(0, size);
            button.Text = capsToUse[rInt];
            button.Checked = false;
            if (capsToUse[rInt].Equals(answer))
            {
                button4.Tag = number;
            }
            capsToUse.Remove(capsToUse[rInt]);
        }

        private void easyCheckButton_Click(object sender, EventArgs e)
        {
            easyPanel.Visible = false;
            startPanel.Visible = false;
            List<string> unused = (List<string>)answer1.Tag;
            List<RadioButton> buttons = new List<RadioButton>() { answer1, answer2, answer3, answer4 };
            int correctButton = (int)answer3.Tag;
            if (buttons[correctButton].Checked)
            {
                if (unused.Count != 0)
                {
                    correctPanel.Visible = true;
                    correctLabel.Text = "Correct!";
                    correctLabel.Font = new Font("Arial", 24, FontStyle.Bold);
                    if (unused.Count % 5 == 0)
                    {
                        correctLabel.Font = new Font("Arial", 12, FontStyle.Bold);
                        correctLabel.Text = ("Correct! \r\n You are on a \r\n" + (50 - unused.Count) + " question streak!");
                    }
                }

                else
                {
                    correctPanel.Visible = true;
                    correctLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                    correctLabel.Text = ("Congratulations! \r\n You win!");
                    if (MessageBox.Show("Would you like to play again?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        startPanel.Visible = true;
                        easyPanel.Visible = false;
                        correctPanel.Visible = false;
                        hardPanel.Visible = false;
                        Dictionary<string, string> statesNCaps = statesAndCapitals.capMaker();
                        unused = Enumerable.ToList(statesNCaps.Values);
                        easyQuestionBox.Tag = statesNCaps;
                        hardQuestionBox.Tag = statesNCaps;
                        answer1.Tag = unused;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                string answer = (string)answer4.Tag;
                MessageBox.Show(("Incorrect! The corrrect answer is "+answer +". You got "+(49-unused.Count)+" capitals correct."));
                if (MessageBox.Show("Would you like to play again?", "", MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    startPanel.Visible = true;
                    easyPanel.Visible = false;
                    correctPanel.Visible = false;
                    hardPanel.Visible = false;
                    answerBox.Clear();

                    Dictionary<string, string> statesNCaps = statesAndCapitals.capMaker();
                    unused = Enumerable.ToList(statesNCaps.Values);
                    easyQuestionBox.Tag = statesNCaps;
                    hardQuestionBox.Tag = statesNCaps;
                    answer1.Tag = unused;
                }
                else
                {
                    this.Close();
                }   
            }
        }           
        

        private void nextButton_Click(object sender, EventArgs e)
        {
            correctPanel.Visible = false;
            if ((bool)answerBox.Tag == false)
            {
                List<RadioButton> buttons = new List<RadioButton>() { answer1, answer2, answer3, answer4 };
                easyPanel.Visible = true;
                startPanel.Hide();
                easyQuestion(buttons, easyQuestionBox);
            }
            else
            {
                answerBox.Clear();
                hardPanel.Visible = true;
                startPanel.Hide();
                hardQuestion(hardQuestionBox, answer1, answer4);
            }
        }

        private static void hardQuestion(Label hardQuestionBox, RadioButton answer1, RadioButton answer4)
        {           
            Dictionary<string, string> statesAndCaps = (Dictionary<string, string>)hardQuestionBox.Tag;
            List<string> capitals = Enumerable.ToList(statesAndCaps.Values);
            List<string> unUsed = (List<string>)answer1.Tag;
            Random rand = new Random();
            int answerSize = unUsed.Count;
            string answer = unUsed[rand.Next(answerSize)];
            unUsed.Remove(answer);
            answer1.Tag = unUsed;
            answer4.Tag = answer;
            string state = statesAndCaps.FirstOrDefault(x => x.Value.Equals(answer)).Key;
            hardQuestionBox.Text = ("What is the capital of " + state + "?");
        }

        private void hardCheckButton_Click(object sender, EventArgs e)
        {
            hardPanel.Visible = false;
            List<string> unused = (List<string>)answer1.Tag;
            string answer = (string)answer4.Tag;      
            if (answer.Equals(answerBox.Text))
            {
                if (unused.Count != 0)
                {
                    correctPanel.Visible = true;
                    correctLabel.Text = "Correct!";
                    correctLabel.Font = new Font("Arial", 24, FontStyle.Bold);
                    if (unused.Count % 5 == 0)
                    {
                        correctLabel.Font = new Font("Arial", 12, FontStyle.Bold);
                        correctLabel.Text = ("Correct! \r\n You are on a \r\n" + (50 - unused.Count) + " question streak!");
                    }
                }

                else
                {
                    correctPanel.Visible = true;
                    correctLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                    correctLabel.Text = ("Congratulations! \r\n You win!");
                }
            }
            else
            {                
                MessageBox.Show(("Incorrect! The corrrect answer is " + answer + ". You got " + (49 - unused.Count) + " capitals correct."));
                if (MessageBox.Show("Would you like to play again?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    startPanel.Visible = true;
                    easyPanel.Visible = false;
                    correctPanel.Visible = false;
                    hardPanel.Visible = false;
                    Dictionary<string, string> statesNCaps = statesAndCapitals.capMaker();
                    unused = Enumerable.ToList(statesNCaps.Values);
                    easyQuestionBox.Tag = statesNCaps;
                    hardQuestionBox.Tag = statesNCaps;
                    answer1.Tag = unused;
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
