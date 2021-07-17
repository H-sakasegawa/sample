﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class FormJuniSinKanHou : Form
    {
        JuniSinKanHou juniSinKanHou = null;

        DrawJuniSinKanhoun drawJuniSinKanhoun = null;
        public event CloseHandler OnClose;

        //文字描画領域サイズ
        Rectangle rectKan;
        public FormJuniSinKanHou()
        {
            InitializeComponent();

            juniSinKanHou = new JuniSinKanHou();
            drawJuniSinKanhoun = new DrawJuniSinKanhoun();

        }
        public void Update(Person person)
        {
            var node = juniSinKanHou.Create(person);

            drawJuniSinKanhoun.Draw(person, pictureBox1, node);

        }

        private void FormJuniSinKanHou_FormClosed(object sender, FormClosedEventArgs e)
        {
            drawJuniSinKanhoun.Dispose();

            OnClose?.Invoke();
        }
    }
}