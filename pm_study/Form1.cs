using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pm_study
{
    public partial class Form1 : Form
    {
        // 初期化
        ChromiumWebBrowser CEF_BROWSER = new ChromiumWebBrowser();
        int STUDY_SEC = 0;

        public Form1()
        {
            InitializeComponent();

            InitCefBrowser();
            LoadPMDojo();
        }

        public virtual void InitCefBrowser()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            pnlBrowser.Controls.Add(CEF_BROWSER);
            CEF_BROWSER.Dock = DockStyle.Fill;
            // 黒い余白による描画ズレの防止
            Cef.EnableHighDPISupport();
        }

        public virtual void LoadPMDojo()
        {
            CEF_BROWSER.LoadUrl("https://www.pm-siken.com/s/pmkakomon.php");
            lblFileName.Text = "";
        }


        //---------------------
        // イベントハンドラ
        //---------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (STUDY_SEC <= 2100000000)
            {
                STUDY_SEC += 1;
                this.Text = "kurobi" + " | 現在の学習成果: " + STUDY_SEC.ToString() + "秒";
            }
            else
            {
                this.Text = "kurobi" + " | 現在の学習成果: 21億秒以上";
                timer1.Enabled = false;
            }
        }

        /// <summary>
        /// フォームが開かれた直後のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Shown(object sender, EventArgs e)
        {
            // 最初にノートを開く
            timer1.Enabled = false;
            this.button3_Click(null, null);
            timer1.Enabled = true;
        }


        /// <summary>
        /// ノートを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            // ディレクトリ存在チェック
            if (Directory.Exists(System.Environment.CurrentDirectory + @"\note"))
            {
                // 初回ディレクトリの指定
                openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory + @"\note";
            }
            else
            {
                // 存在しない場合ディレクトリを作成してから初回ディレクトリとして指定
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\note");
                openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory + @"\note";
            }

            // 日付yyyyMMdd形式文字列
            string strYMD = System.DateTime.Now.ToString("yyyyMMdd");

            // その他ダイアログ設定
            openFileDialog1.Title = "ノートを開いてください";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ノートファイル | *.txt";
            //openFileDialog1.FileName = strYMD + "_kurobi_note.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("UTF-8"));

                string wk_text = sr.ReadToEnd();

                // 確実に破棄する
                sr.Close();
                sr.Dispose();

                textBox1.Text = wk_text;
                lblFileName.Text = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) == false)
            {
                Clipboard.SetText(textBox1.Text);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            // NULLチェック
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("空白のノートは保存できませんよ", "ノートに保存します");
                return;
            }

            // ディレクトリ存在チェック
            if (Directory.Exists(System.Environment.CurrentDirectory + @"\note"))
            {
                // 初回ディレクトリの指定
                saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory + @"\note";
            }
            else
            {
                // 存在しない場合ディレクトリを作成してから初回ディレクトリとして指定
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\note");
                saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory + @"\note";
            }


            // ファイルを既に開いている場合はセーブダイアログ無しで保存
            // そうでない場合(新規保存の場合)は、セーブダイアログ後に保存
            if (string.IsNullOrEmpty(lblFileName.Text))
            {
                // 日付yyyyMMdd形式文字列
                string strYMD = System.DateTime.Now.ToString("yyyyMMdd");

                // その他ダイアログ設定
                saveFileDialog1.Title = "ノートに保存します";
                saveFileDialog1.FileName = strYMD + "_kurobi_note.txt";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                    sw.Write(textBox1.Text);

                    sw.Close();
                    sw.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            }
            else
            {
                FileStream fs = new FileStream(lblFileName.Text, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                sw.Write(textBox1.Text);

                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }

        // プロジェクトマネージャ(PM)
        private void button1_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.pm-siken.com/s/pmkakomon.php");
        }

        // 情報安全確保支援士(SC)
        private void button7_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.sc-siken.com/s/sckakomon.php");
        }

        // ネットワークスペシャリスト(NW)
        private void button8_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.nw-siken.com/s/nwkakomon.php");
        }

        // データベーススペシャリスト(DB)
        private void button9_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.db-siken.com/s/dbkakomon.php");
        }

        // 応用情報(AP)
        private void button2_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.ap-siken.com/s/apkakomon.php");
        }

        // 基本情報(FE)
        private void button5_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.fe-siken.com/s/fekakomon.php");
        }

        // IPA
        private void button10_Click(object sender, EventArgs e)
        {
            CEF_BROWSER.LoadUrl("https://www.jitec.ipa.go.jp/1_11seido/seido_gaiyo.html");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
            //MessageBox.Show("今回の学習成果は " + STUDY_SEC + "秒 でした", "お疲れ様でした");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            timer1.Enabled = false;

            dr = MessageBox.Show("勉強を終わりますか？\r\n今回の学習成果は " + STUDY_SEC + "秒 です", "終了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // 保存処理
                if (String.IsNullOrEmpty(textBox1.Text) == false)
                {
                    button4_Click(sender, e);
                }
            }
            else
            { 
                timer1.Enabled = true;
                e.Cancel = true;
            }


        }

        /// <summary>
        /// ファイルクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblFileName.Text))
            {
                MessageBox.Show("まだノートは開かれていません。\r\n\r\n[SAVE]ボタンで勉強用のノートを作成し\r\n[LOAD]ボタンから過去のノートを開きましょう");
            }
            else
            {
                DialogResult dr;
                dr = MessageBox.Show("ノートを閉じますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    textBox1.Text = "";
                    lblFileName.Text = "";
                }
            }
        }
    }
}
