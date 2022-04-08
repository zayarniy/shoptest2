﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopVik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnEnter_Click(object sender, EventArgs e)
        {
            Service.User user;
            int? status = ShopVik.Service.Check(tbLogin.Text, tbPassword.Text,out user);

            switch (status)
            {
                case null:
                    MessageBox.Show("Не прошли");
                    break;
                case 0://Admin
                    Form3 form3 = new Form3();
                    form3.ShowDialog();

                    break;
                case 1://User
                    Form4 form4 = new Form4();
                    form4.ShowDialog();

                    break;

                default:
                    MessageBox.Show("что-то не то");
                    break;
            }

          
               
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            Form2 registration = new Form2(this);
            this.Hide();
            registration.ShowDialog();
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Blue; 
        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.White;
        }

        Point lastPoint;
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;

            }
        }

        private void closeButton_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void btnShowChars_Click(object sender, EventArgs e)
        {
            tbPassword.UseSystemPasswordChar = !tbPassword.UseSystemPasswordChar;
        }
    }
}
