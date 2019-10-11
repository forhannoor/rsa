// Class that drives GUI window.

using System.Globalization;
using System.Windows.Forms;

namespace RSAv2
{
    class MainWindow
    {
        private const int WIDTH = 580, HEIGHT = 560, LEFT = 300, TOP = 300, SMALL_GAP = 10, BIG_GAP = 350;
        // Array of labels.
        private readonly string[] LABELS = { "Original Text", "Ciphered Text", "Public Key", "Private Key", "Select a file using the button below", "RSA Encryption Tool", "Browse", "Encrypt", "Decrypt", "Exit", "Status: Idle" };
        private Form _form;
        private TextBox _original;
        private TextBox _ciphered;
        private TextBox _publicKey;
        private TextBox _privateKey;
        private TextBox _status;
        private Button _fileDialog;
        private Button _encrypt;
        private Button _decrypt;
        private Button _exit;
        private RSA _rsa;
        private FileOperation _fo;
        private string _fileExtension = ".txt";

        // GUI initializer.
        public MainWindow()
        {
            _rsa = new RSA();
            _fo = new FileOperation();
            
            // Initialize controls.
            _form = new Form
            {
                Text = LABELS[5],
                Width = WIDTH,
                Height = HEIGHT,
                StartPosition = FormStartPosition.Manual,
                Left = LEFT,
                Top = TOP,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };
            
            _original = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Top = 40, Left = SMALL_GAP, Height = 200, Width = 300 };
            _ciphered = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Top = 290, Left = SMALL_GAP, Height = 200, Width = 300 };
            _publicKey = new TextBox { Top = 160, Left = BIG_GAP, Width = 200 };
            _privateKey = new TextBox { Top = 220, Left = BIG_GAP, Width = 200 };
            _status = new TextBox { Top = SMALL_GAP + 490, Left = SMALL_GAP, ReadOnly = true, Width = 540, Text = LABELS[10] };
            _fileDialog = new Button { Text = LABELS[6], Top = 290, Left = BIG_GAP, Width = 200 };
            _fileDialog.Click += FileDialogClicked;
            _encrypt = new Button { Text = LABELS[7], Top = 320, Left = BIG_GAP, Width = 200 };
            _encrypt.Click += EncryptClicked;
            _decrypt = new Button { Text = LABELS[8], Top = 350, Left = BIG_GAP, Width = 200 };
            _decrypt.Click += DecryptClicked;
            _exit = new Button { Text = LABELS[9], Top = 380, Left = BIG_GAP, Width = 200 };
            _exit.Click += (sender, e) => { Application.Exit(); };

            // Add controls into form.
            var sampleLabel = new Label { Text = LABELS[0], Top = SMALL_GAP, Left = SMALL_GAP };
            _form.Controls.Add(sampleLabel);
            sampleLabel = new Label { Text = LABELS[1], Top = SMALL_GAP + 250, Left = SMALL_GAP };
            _form.Controls.Add(sampleLabel);
            sampleLabel = new Label { Text = LABELS[2], Top = SMALL_GAP + 120, Left = BIG_GAP };
            _form.Controls.Add(sampleLabel);
            sampleLabel = new Label { Text = LABELS[3], Top = SMALL_GAP + 180, Left = BIG_GAP };
            _form.Controls.Add(sampleLabel);
            sampleLabel = new Label { Text = LABELS[4], Top = SMALL_GAP + 250, Left = BIG_GAP, Width = 200 };
            _form.Controls.Add(sampleLabel);
            _form.Controls.Add(_original);
            _form.Controls.Add(_ciphered);
            _form.Controls.Add(_publicKey);
            _form.Controls.Add(_privateKey);
            _form.Controls.Add(_status);
            _form.Controls.Add(_fileDialog);
            _form.Controls.Add(_encrypt);
            _form.Controls.Add(_decrypt);
            _form.Controls.Add(_exit);
            // Display form.
            _form.ShowDialog();
        }

        // Invoked on "Browse" clicked.
        private void FileDialogClicked(object sender, System.EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            string fileName;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = fileDialog.FileName;
                
                if(! fileName.EndsWith(_fileExtension, true, new CultureInfo("en-US")))
                {
                    Status("Please select text file only");
                }

                else
                {
                    Status("File processed");
                    _fo.FileName = fileName; // specify file name to read from
                    _fo.Read(); // read content
                    _original.Text = _fo.Content; // display content
                }
            }
        }

        // Invoked on "Encrypt" clicked.
        private void EncryptClicked(object sender, System.EventArgs e)
        {
            string t = _original.Text; // original text
            string enc = _rsa.Encrypt(t);
            _ciphered.Text = enc;
            _publicKey.Text = "( " + _rsa.GetN + ", " + _rsa.GetE + " )";
            Status("Ciphering complete");
        }

        // Invoked on "Decrypt" clicked.
        private void DecryptClicked(object sender, System.EventArgs e)
        {
            string t = _ciphered.Text; // ciphered text
            string dec = _rsa.Decrypt(t);
            _original.Text = dec;
            _privateKey.Text = "( " + _rsa.GetN + ", " + _rsa.GetD + " )";
            Status("Deciphering complete");
        }

        // set/clear message on the status bar
        private void Status(string message = "")
        {
            _status.Text = message;
        }
    }
}