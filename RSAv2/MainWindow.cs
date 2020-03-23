// GUI class.

using System.Globalization;
using System.Windows.Forms;

namespace RSAv2
{
    class MainWindow
    {
        private Form _form;
        private TextBox _plainTextBox;
        private TextBox _cipheredTextBox;
        private TextBox _publicKeyBox;
        private TextBox _privateKeyBox;
        private TextBox _statusBox;
        private Button _fileDialogButton;
        private Button _encryptButton;
        private Button _decryptButton;
        private Button _exitButton;
        private RSA _rsa;
        private FileOperation _fileOperation;
        // Constant parameters used for GUI initialization.
        private const int WIDTH = 580, HEIGHT = 560, LEFT = 300, TOP = 300, SMALL_GAP = 10, BIG_GAP = 350;
        // Array of labels.
        private readonly string[] LABELS = { "Original Text", "Ciphered Text", "Public Key", "Private Key"
            , "Select a file using the button below", "RSA Encryption Tool", "Browse", "Encrypt", "Decrypt", "Exit"
            , "Status: Idle" };
        private const string EXTENSION = ".txt";

        // Constructor that initializes GUI.
        public MainWindow()
        {
            _rsa = new RSA();
            _fileOperation = new FileOperation();
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
            
            _plainTextBox = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Top = 40, Left = SMALL_GAP
                , Height = 200, Width = 300 };
            _cipheredTextBox = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Top = 290
                , Left = SMALL_GAP, Height = 200, Width = 300 };
            _publicKeyBox = new TextBox { Top = 160, Left = BIG_GAP, Width = 200 };
            _privateKeyBox = new TextBox { Top = 220, Left = BIG_GAP, Width = 200 };
            _statusBox = new TextBox { Top = SMALL_GAP + 490, Left = SMALL_GAP, ReadOnly = true, Width = 540
                , Text = LABELS[10] };
            _fileDialogButton = new Button { Text = LABELS[6], Top = 290, Left = BIG_GAP, Width = 200 };
            _fileDialogButton.Click += FileDialogClicked;
            _encryptButton = new Button { Text = LABELS[7], Top = 320, Left = BIG_GAP, Width = 200 };
            _encryptButton.Click += EncryptClicked;
            _decryptButton = new Button { Text = LABELS[8], Top = 350, Left = BIG_GAP, Width = 200 };
            _decryptButton.Click += DecryptClicked;
            _exitButton = new Button { Text = LABELS[9], Top = 380, Left = BIG_GAP, Width = 200 };
            _exitButton.Click += (sender, e) => { Application.Exit(); };
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
            _form.Controls.Add(_plainTextBox);
            _form.Controls.Add(_cipheredTextBox);
            _form.Controls.Add(_publicKeyBox);
            _form.Controls.Add(_privateKeyBox);
            _form.Controls.Add(_statusBox);
            _form.Controls.Add(_fileDialogButton);
            _form.Controls.Add(_encryptButton);
            _form.Controls.Add(_decryptButton);
            _form.Controls.Add(_exitButton);
            _form.ShowDialog();
        }

        // Invoked on "Browse" button clicked.
        private void FileDialogClicked(object sender, System.EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            string fileName;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = fileDialog.FileName;
                
                if(! fileName.EndsWith(EXTENSION, true, new CultureInfo("en-US")))
                {
                    Status("Please select a valid text file");
                }

                else
                {
                    Status("File processed");
                    // Set file name.
                    _fileOperation.FileName = fileName;
                    // Read from file.
                    _fileOperation.Read();
                    // Display file content.
                    _plainTextBox.Text = _fileOperation.Content;
                }
            }
        }

        // Invoked on "Encrypt" button clicked.
        private void EncryptClicked(object sender, System.EventArgs e)
        {
            // Translate plain text into ciphered text.
            string cipheredText = _rsa.Encrypt(_plainTextBox.Text);
            // Display ciphered text.
            _cipheredTextBox.Text = cipheredText;
            // Display public key.
            _publicKeyBox.Text = $"({_rsa.GetN}, {_rsa.GetE})";
            // Update status.
            Status("Ciphering complete");
        }

        // Invoked on "Decrypt" button clicked.
        private void DecryptClicked(object sender, System.EventArgs e)
        {
            // Translate ciphered text into plain text.
            string plainText = _rsa.Decrypt(_cipheredTextBox.Text);
            // Display deciphered text.
            _plainTextBox.Text = plainText;
            // Display private key.
            _privateKeyBox.Text = $"({_rsa.GetN}, {_rsa.GetD})";
            // Update status.
            Status("Deciphering complete");
        }

        // Updates message on the _statusBox.
        private void Status(string message = "")
        {
            _statusBox.Text = message;
        }
    }
}