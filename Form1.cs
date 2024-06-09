using System.Diagnostics;

namespace final_odev
{
    public partial class Form1 : Form
    {
        private Label firstClicked = null;
        private Label secondClicked = null;
        private System.Threading.Timer gameTimer;
        private int matchedPairs = 0; // Eşleşen çiftlerin sayısını takip eden değişken
        private Stopwatch stopwatch; // Oyun süresini takip eden nesne

        public Form1()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.button1_Click); // Yeni Oyun butonunun Click olayını ekleyin
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeGame();
        }
        private void InitializeGame()
        {
            // TableLayoutPanel'e rastgele ikonları atayın
            Random random = new Random();
            List<int> icons = new List<int>()
        {
            0, 0, 1, 1, 2, 2, 3, 3, 4, 4,
            5, 5, 6, 6, 7, 7, 8, 8, 9, 9
        }; // 10 farklı ikon, her biri iki kez

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label)
                {
                    int randomIndex = random.Next(icons.Count);
                    int iconIndex = icons[randomIndex];
                    icons.RemoveAt(randomIndex);
                    label.Tag = iconIndex;
                    label.Click += new EventHandler(Label_Click);
                }
            }
            // Eşleşen çiftlerin sayısını sıfırla
            matchedPairs = 0;

            // İlk ve ikinci tıklanan Label'ları sıfırla
            firstClicked = null;
            secondClicked = null;
            // Stopwatch başlatın
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (firstClicked != null && secondClicked != null)
                return;

            Label clickedLabel = sender as Label;
            if (clickedLabel == null || clickedLabel.Image != null)
                return;

            int iconIndex = (int)clickedLabel.Tag;
            clickedLabel.Image = imageList1.Images[iconIndex];

            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                return;
            }

            secondClicked = clickedLabel;
            CheckForMatch();
        }
        private void CheckForMatch()
        {
            if (firstClicked.Tag.Equals(secondClicked.Tag))
            {
                // Eşleşme varsa, etiketleri null yaparak sonraki tıklamalara hazır hale getirin
                firstClicked = null;
                secondClicked = null;
                // Eşleşen çiftlerin sayısını artır
                matchedPairs++;
                // Oyunun tamamlanıp tamamlanmadığını kontrol edin
                CheckForCompletion();
                return;
            }

            // Eşleşme yoksa, resimleri kapatmak için bir Timer ayarlayın
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000; //2 saniye bekleyin
            timer.Tick += (s, e) =>
            {
                firstClicked.Image = null;
                secondClicked.Image = null;
                firstClicked = null;
                secondClicked = null;
                timer.Stop();
            };
            timer.Start();
        }
        private void CheckForCompletion()
        {
            // 10 çift eşleştiğinde oyunu bitirin
            if (matchedPairs == 10)
            {
                stopwatch.Stop(); // Stopwatch durdur
                MessageBox.Show("Tebrikler!Bütün emojileri eşleştirdiniz:)");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            // Tüm eşleşen resimleri kapatın
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label)
                {
                    label.Image = null; // Eşleşen resimleri kapat
                }
            }
            InitializeGame();
        }
    }
}
