using System.Globalization;

namespace PlanetWeightCalculator.Views;

/// <summary>
/// Улучшенное модальное окно ввода веса с быстрыми пресетами и валидацией.
/// </summary>
public class DataEntryForm : Form
{
    private readonly TextBox _txtWeight;
    private readonly Button _btnOk;
    private readonly Button _btnCancel;
    private readonly Label _lblPrompt;
    private readonly Label _lblHelp;
    private readonly FlowLayoutPanel _presetsPanel;

    public double? ResultWeight { get; private set; }

    public DataEntryForm(double? initialWeight = null)
    {
        Text = "Ввод данных — Вес на Земле";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(480, 260);
        Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        BackColor = Color.FromArgb(248, 249, 251);

        _lblPrompt = new Label
        {
            Text = "Введите ваш вес на Земле (в килограммах):",
            Location = new Point(32, 22),
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(33, 37, 41)
        };

        _txtWeight = new TextBox
        {
            Location = new Point(32, 56),
            Size = new Size(416, 34),
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            Text = initialWeight.HasValue ? initialWeight.Value.ToString("0.##", CultureInfo.CurrentCulture) : string.Empty
        };

        _lblHelp = new Label
        {
            Text = "Диапазон: от 0.1 до 1000 кг (поддерживаются как запятая, так и точка)",
            Location = new Point(32, 98),
            AutoSize = true,
            ForeColor = Color.FromArgb(108, 117, 125),
            Font = new Font("Segoe UI", 8.5F, FontStyle.Italic)
        };

        // Панель быстрых пресетов
        _presetsPanel = new FlowLayoutPanel
        {
            Location = new Point(32, 128),
            Size = new Size(416, 36),
            BackColor = Color.Transparent,
            Margin = new Padding(0)
        };

        var lblPresetTitle = new Label
        {
            Text = "Быстрый выбор:",
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(100, 110, 120),
            Margin = new Padding(0, 6, 8, 0)
        };
        _presetsPanel.Controls.Add(lblPresetTitle);

        foreach (var val in new[] { 50, 65, 75, 85, 100 })
        {
            var btnPreset = new Button
            {
                Text = $"{val} кг",
                Size = new Size(58, 28),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F),
                Margin = new Padding(2, 2, 2, 2)
            };
            btnPreset.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            int v = val;
            btnPreset.Click += (s, e) =>
            {
                _txtWeight.Text = v.ToString();
                _txtWeight.Focus();
                _txtWeight.SelectAll();
            };
            _presetsPanel.Controls.Add(btnPreset);
        }

        _btnOk = new Button
        {
            Text = "Рассчитать",
            Location = new Point(200, 190),
            Size = new Size(130, 42),
            BackColor = Color.FromArgb(13, 110, 253),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold)
        };
        _btnOk.FlatAppearance.BorderSize = 0;
        _btnOk.Click += OnOkClicked;

        _btnCancel = new Button
        {
            Text = "Отмена",
            Location = new Point(342, 190),
            Size = new Size(106, 42),
            BackColor = Color.FromArgb(222, 226, 230),
            ForeColor = Color.FromArgb(33, 37, 41),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 10F)
        };
        _btnCancel.FlatAppearance.BorderSize = 0;
        _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        AcceptButton = _btnOk;
        CancelButton = _btnCancel;

        Controls.Add(_lblPrompt);
        Controls.Add(_txtWeight);
        Controls.Add(_lblHelp);
        Controls.Add(_presetsPanel);
        Controls.Add(_btnOk);
        Controls.Add(_btnCancel);

        _txtWeight.SelectAll();
    }

    private void OnOkClicked(object? sender, EventArgs e)
    {
        string rawText = _txtWeight.Text.Trim();

        if (string.IsNullOrWhiteSpace(rawText))
        {
            MessageBox.Show(
                "Поле ввода не может быть пустым! Пожалуйста, укажите ваш вес.",
                "Ошибка ввода данных",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            _txtWeight.Focus();
            return;
        }

        string normalized = rawText.Replace(',', '.');

        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedWeight))
        {
            MessageBox.Show(
                $"Значение «{rawText}» не является корректным числом! Введите числовое значение (например: 75.5 или 68).",
                "Некорректный формат",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            _txtWeight.Focus();
            _txtWeight.SelectAll();
            return;
        }

        if (parsedWeight <= 0.0 || parsedWeight > 1000.0)
        {
            MessageBox.Show(
                $"Вес {parsedWeight} кг выходит за рамки допустимого диапазона (от 0.1 до 1000 кг)!",
                "Ошибка диапазона",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            _txtWeight.Focus();
            _txtWeight.SelectAll();
            return;
        }

        ResultWeight = parsedWeight;
        DialogResult = DialogResult.OK;
        Close();
    }
}
