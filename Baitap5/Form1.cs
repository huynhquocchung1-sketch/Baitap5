using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Baitap5
{
    public partial class Form1 : Form
    {
        // Biến lưu trữ đường dẫn file hiện tại để phục vụ chức năng Lưu
        private string currentFilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        // --- 1. SỰ KIỆN KHI MỞ FORM ---
        private void Form1_Load(object sender, EventArgs e)
        {
            // Lấy tất các fonts từ hệ thống đưa vào combobox Fonts[cite: 3]
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                cmbFonts.Items.Add(font.Name);
            }

            // Tạo dữ liệu cho ComboBox Size: chứa các giá trị từ 8 đến 72[cite: 3]
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in sizes)
            {
                cmbSizes.Items.Add(size.ToString());
            }

            // Tạo giá trị mặc định ban đầu là Font Tahoma, Size 14[cite: 3]
            cmbFonts.Text = "Tahoma";
            cmbSizes.Text = "14";
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);
        }

        // --- 2. XỬ LÝ MENU HỆ THỐNG ---

        // 2.2 Tạo văn bản mới
        private void tạoVănBảnMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Xóa nội dung hiện có trên RichTextBox và tạo lại các giá trị mặc định như Font, Size[cite: 3]
            richText.Clear();
            cmbFonts.Text = "Tahoma";
            cmbSizes.Text = "14";
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);
            currentFilePath = "";
        }

        // 2.3 Mở tập tin
        private void mởTậpTinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại mở tập tin cho phép người dùng chọn tập tin văn bản (*.txt hoặc *.rtf)[cite: 3]
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog1.FileName;
                if (currentFilePath.EndsWith(".rtf"))
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.RichText); //[cite: 3]
                else
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
            }
        }

        // 2.4 Lưu nội dung văn bản
        private void lưuNộiDungVănBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Nếu là văn bản mới và trước đó chưa lưu lần nào[cite: 3]
            if (string.IsNullOrEmpty(currentFilePath))
            {
                saveFileDialog1.Filter = "Rich Text Format (*.rtf)|*.rtf";

                // Hiển thị hộp thoại lưu tập tin (SaveFileDialog) cho phép người dùng chọn thư mục cần lưu[cite: 3]
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog1.FileName;
                    // Lưu nội dung văn bản trên RichTextBox xuống tập tin[cite: 3]
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Nếu là văn bản đã được mở trước đó thì thông báo cho người dùng lưu văn bản thành công[cite: 3]
                richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Nút Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // --- 3. XỬ LÝ MENU ĐỊNH DẠNG ---

        // Mở hộp thoại Font
        private void địnhDạngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Khi click vào menu định dạng gọi FontDialog có sẵn của windows[cite: 1]
            fontDialog1.ShowColor = true; //[cite: 2]
            fontDialog1.ShowApply = true; //[cite: 2]
            fontDialog1.ShowEffects = true; //[cite: 2]
            fontDialog1.ShowHelp = true; //[cite: 2]

            if (fontDialog1.ShowDialog() != DialogResult.Cancel) //[cite: 2]
            {
                richText.ForeColor = fontDialog1.Color; //[cite: 2]
                richText.SelectionFont = fontDialog1.Font;
            }
        }


        // --- 4. XỬ LÝ THANH CÔNG CỤ (TOOLSTRIP) ---

        // 2.5 Nút In đậm (B)
        private void btnBold_Click(object sender, EventArgs e)
        {
            // Tùy thuộc vào trạng thái của nút để xử lý nội dung của vùng văn bản đang được chọn có được in đậm hay không[cite: 3]
            ChangeFontStyle(FontStyle.Bold);
        }

        // Nút In nghiêng (I)
        private void btnItalic_Click(object sender, EventArgs e)
        {
            // Tùy thuộc vào trạng thái của nút để xử lý nội dung của vùng văn bản đang được chọn có được in nghiêng hay không[cite: 3]
            ChangeFontStyle(FontStyle.Italic);
        }

        // Nút Gạch dưới (U)
        private void btnUnderline_Click(object sender, EventArgs e)
        {
            // Tùy thuộc vào trạng thái của nút để xử lý nội dung của vùng văn bản đang được chọn có được gạch dưới hay không[cite: 3]
            ChangeFontStyle(FontStyle.Underline);
        }

        // Hàm hỗ trợ đổi FontStyle (dùng chung cho B, I, U)
        private void ChangeFontStyle(FontStyle style)
        {
            if (richText.SelectionFont != null)
            {
                FontStyle currentStyle = richText.SelectionFont.Style;
                if (richText.SelectionFont.Style.HasFlag(style))
                    currentStyle &= ~style;
                else
                    currentStyle |= style;

                richText.SelectionFont = new Font(richText.SelectionFont, currentStyle);
            }
        }

        // Thay đổi Font chữ trên ComboBox
        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null)
            {
                richText.SelectionFont = new Font(cmbFonts.Text, richText.SelectionFont.Size, richText.SelectionFont.Style);
            }
        }

        // Thay đổi Size chữ trên ComboBox
        private void cmbSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null && float.TryParse(cmbSizes.Text, out float newSize))
            {
                richText.SelectionFont = new Font(richText.SelectionFont.FontFamily, newSize, richText.SelectionFont.Style);
            }
        }


        // --- 5. ĐẾM SỐ TỪ TRÊN STATUSSTRIP ---
        private void richText_TextChanged(object sender, EventArgs e)
        {
            string text = richText.Text.Trim();
            int wordCount = string.IsNullOrWhiteSpace(text) ? 0 : text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

            // thanh trạng thái hiển thị tổng số từ của văn bản[cite: 1]
            lblWordCount.Text = $"Tổng số từ: {wordCount}";
        }
    }
}