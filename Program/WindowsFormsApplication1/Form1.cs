using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WMPLib;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string fullPath;
        
        public void DriveTreeInit()
        {
            string[] drivesArray = Directory.GetLogicalDrives();

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            foreach (string s in drivesArray)
            {
                TreeNode drive = new TreeNode(s, 0, 0);
                treeView1.Nodes.Add(drive);

                GetDirs(drive);
            }


            treeView1.EndUpdate();
        }
        public void GetDirs(TreeNode node)
        {
            DirectoryInfo[] diArray;

            node.Nodes.Clear();

            string fullPath = node.FullPath;
            DirectoryInfo di = new DirectoryInfo(fullPath);

            try
            {
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo dirinfo in diArray)
            {
                TreeNode dir = new TreeNode(dirinfo.Name, 0, 1);
                node.Nodes.Add(dir);
            }
        }
        public Form1()
        {
            InitializeComponent();
            GetDat.EventHandler = new GetDat.GetData(GetDataFunc);
            DriveTreeInit();
            listBox1.BeginUpdate();
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\asd.txt", System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                }
            }
            listBox1.EndUpdate();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeView1.BeginUpdate();

            foreach (TreeNode node in e.Node.Nodes)
            {
                GetDirs(node);
            }

            treeView1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            fullPath = selectedNode.FullPath;

            DirectoryInfo di = new DirectoryInfo(fullPath);
            FileInfo[] fiArray;
            DirectoryInfo[] diArray;

            try
            {
                fiArray = di.GetFiles();
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            listView1.Items.Clear();

            foreach (DirectoryInfo dirInfo in diArray)
            {
                ListViewItem lvi = new ListViewItem(dirInfo.Name);
                lvi.SubItems.Add("0");
                lvi.SubItems.Add(dirInfo.LastWriteTime.ToString());
                lvi.ImageIndex = 0;

                listView1.Items.Add(lvi);
            }


            foreach (FileInfo fileInfo in fiArray)
            {
                ListViewItem lvi = new ListViewItem(fileInfo.Name);
                lvi.Tag = fileInfo.FullName;
                lvi.Tag = fileInfo.FullName;
                lvi.SubItems.Add(fileInfo.Length.ToString());
                lvi.SubItems.Add(fileInfo.LastWriteTime.ToString());

                string filenameExtension = Path.GetExtension(fileInfo.Name).ToLower();
                listView1.Items.Add(lvi);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (listBox1.SelectedItem != null && (Path.GetExtension(listBox1.SelectedItem.ToString()) == ".exe" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".EXE" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".JPG" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".jpg" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".png" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".PNG"))
            {
                CheckExtension(listBox1.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Невозможно запустить этот файл");
            }
            //this.WindowsMediaPlayer1.URL = this.listView1.SelectedItems[0].Tag.ToString();


        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(listView1.SelectedItems[0].Text);
           using (Form2 fm2 = new Form2())
            {
                try
                {
                    SetDat.EventHandler(listView1.SelectedItems[0].Text);
                    fm2.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

              }
        }

        private void добавитьВИзбранноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            try
            {
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\asd.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(listView1.SelectedItems[0].Tag.ToString());
                }

                Console.WriteLine("Запись выполнена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            
            listBox1.Items.Add(listView1.SelectedItems[0].Tag.ToString());
            listBox1.EndUpdate();
            MessageBox.Show("Добавлено в избранное\n" + this.listView1.SelectedItems[0].Tag.ToString());
        }

        void GetDataFunc(string name,DialogResult dr)
        {
            if (dr == DialogResult.OK)
            {
                try
                {
                  // пробуем переименовать
                    System.IO.Directory.Move(fullPath + "\\" + listView1.SelectedItems[0].Text, fullPath + "\\" + name);
                    listView1.SelectedItems[0].Text = name;
                    //MessageBox.Show(fullPath + "\\" + listView1.SelectedItems[0].Text);
                    //MessageBox.Show(fullPath + "\\" + name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && (Path.GetExtension(listBox1.SelectedItem.ToString()) == ".exe" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".EXE" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".JPG" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".jpg" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".png" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".PNG"))
            {
                CheckExtension(listBox1.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Невозможно запустить этот файл");
            }
            
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            listBox1.Items.RemoveAt(listBox1.Items.IndexOf(listBox1.SelectedItem));
            listBox1.EndUpdate();
            MessageBox.Show("Элемент удален");
        }

        private void запуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && (Path.GetExtension(listBox1.SelectedItem.ToString()) == ".exe" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".EXE" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".JPG" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".jpg" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".png" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".PNG"))
            {
                CheckExtension(listBox1.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Невозможно запустить этот файл");
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void CheckExtension(string s)
        {
            if (listBox1.SelectedItem != null && (Path.GetExtension(listBox1.SelectedItem.ToString()) == ".exe" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".EXE" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".JPG" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".jpg" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".png" || Path.GetExtension(listBox1.SelectedItem.ToString()) == ".PNG"))
            {
                Process.Start(s);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}


       
 

  

