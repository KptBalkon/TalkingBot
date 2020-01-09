using ExpertSystem.Models;
using System;
using System.Windows.Forms;

namespace ExpertSystem
{
    public partial class Form1 : Form
    {
        private DialogService _dialogService;
        public Form1()
        {
            InitializeComponent();
            _dialogService = new DialogService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DialogBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && DialogBox.Text != "")
            {
                e.Handled = true;
                DialogWindow.Items.Add(DialogBox.Text);
                BotAnswer botAnswer = _dialogService.PrepareAnswer(DialogBox.Text);
                DialogWindow.Items.Add(botAnswer.Response);
                CurrentTopic.Text = botAnswer.CurrentTopic;
                DialogBox.Text = "";
            }
        }
    }
}
