﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            SetDat.EventHandler = new SetDat.SetData(SetData);
        }
        void SetData(string name)
        {
            textBox1.Text = name;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetDat.EventHandler(textBox1.Text,this.DialogResult);
        }

    }
}
