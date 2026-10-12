using PlanetWeightCalculator.Controllers;
using PlanetWeightCalculator.Models;

namespace PlanetWeightCalculator.Views;

/// <summary>
/// Главное окно приложения (Main View).
/// Реализовано на базе TableLayoutPanel для исключения наслоения элементов при любом разрешении и DPI.
/// </summary>
public class MainForm : Form, IMainView
{
    private readonly PlanetWeightModel _model;
    private PlanetWeightController? _controller;

    // Главные контейнеры
    private TableLayoutPanel _rootLayout = null!;
    private Panel _headerPanel = null!;
    private Panel _actionCard = null!;
    private Panel _extremumCard = null!;
    private DataGridView _gridResults = null!;
    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;

    // Элементы шапки
    private Label _titleLabel = null!;
    private Label _subtitleLabel = null!;
    private Button _btnHeaderEnter = null!;

    // Элементы карточки действий
    private Label _lblWeightTitle = null!;
    private Label _lblWeightValue = null!;
    private Label _lblHint = null!;
    private Button _btnCardEnter = null!;

    // Элементы экстремумов
    private Label _lblLightest = null!;
    private Label _lblHeaviest = null!;

    public MainForm(PlanetWeightModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));

        InitializeComponent();

        // Подписка на событие активной модели (Active Model)
        _model.ModelChanged += OnModelChanged;

        // Первоначальный вывод списка небесных тел
        DisplayInitialBodies();
    }

    public void SetController(PlanetWeightController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    private void InitializeComponent()
    {
        Text = "Калькулятор веса на планетах Солнечной системы — MVC (ЖЦРПО)";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1060, 720);
        MinimumSize = new Size(920, 600);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        BackColor = Color.FromArgb(240, 243, 246);
        KeyPreview = true;

        // Строка состояния (Dock = Bottom)
        _statusStrip = new StatusStrip
        {
            BackColor = Color.White,
            Font = new Font("Segoe UI", 9F)
        };
        _statusLabel = new ToolStripStatusLabel
        {
            Text = "Готов к работе. Нажмите большую кнопку «Ввести данные» для расчёта веса на планетах.",
            ForeColor = Color.FromArgb(100, 110, 120)
        };
        _statusStrip.Items.Add(_statusLabel);

        // Корневой табличный макет (исключает наслоения)
        _rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = Color.FromArgb(240, 243, 246),
            Padding = new Padding(0),
            Margin = new Padding(0)
        };
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 95F));   // 0: Header
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 135F));  // 1: Action Card (расширено с запасом)
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));   // 2: Extremum Banner
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // 3: Grid Results

        BuildHeader();
        BuildActionCard();
        BuildExtremumCard();
        BuildResultsGrid();

        _rootLayout.Controls.Add(_headerPanel, 0, 0);
        _rootLayout.Controls.Add(_actionCard, 0, 1);
        _rootLayout.Controls.Add(_extremumCard, 0, 2);
        _rootLayout.Controls.Add(_gridResults, 0, 3);

        Controls.Add(_rootLayout);
        Controls.Add(_statusStrip);

        // Горячие клавиши
        KeyDown += (s, e) =>
        {
            if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.F2)
            {
                _controller?.HandleEnterDataCommand(this);
                e.Handled = true;
            }
        };
    }

    private void BuildHeader()
    {
        _headerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(20, 38, 66),
            Margin = new Padding(0)
        };

        var headerTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(24, 12, 24, 12)
        };
        headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

        var textPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.Transparent,
            Margin = new Padding(0)
        };

        _titleLabel = new Label
        {
            Text = "🪐 Калькулятор веса на планетах Солнечной системы",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 4)
        };

        _subtitleLabel = new Label
        {
            Text = "Архитектурный шаблон MVC с активной моделью | ЖЦРПО Лабораторная работа №1",
            ForeColor = Color.FromArgb(170, 195, 225),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            AutoSize = true,
            Margin = new Padding(0)
        };

        textPanel.Controls.Add(_titleLabel);
        textPanel.Controls.Add(_subtitleLabel);

        _btnHeaderEnter = new Button
        {
            Text = "📥 Ввести данные (Ctrl+E)",
            Dock = DockStyle.Right,
            Width = 230,
            Height = 48,
            BackColor = Color.FromArgb(13, 110, 253),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 8, 0, 8)
        };
        _btnHeaderEnter.FlatAppearance.BorderSize = 0;
        _btnHeaderEnter.Click += (s, e) => _controller?.HandleEnterDataCommand(this);

        headerTable.Controls.Add(textPanel, 0, 0);
        headerTable.Controls.Add(_btnHeaderEnter, 1, 0);

        _headerPanel.Controls.Add(headerTable);
    }

    private void BuildActionCard()
    {
        _actionCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(16, 8, 16, 6),
            Padding = new Padding(22, 10, 22, 10)
        };

        var actionTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        actionTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        actionTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));

        var leftTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Color.Transparent,
            Margin = new Padding(0)
        };
        leftTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        leftTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        leftTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        leftTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        _lblWeightTitle = new Label
        {
            Text = "ТЕКУЩИЙ ВЕС НА ЗЕМЛЕ:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(108, 117, 125),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0)
        };

        _lblWeightValue = new Label
        {
            Text = "Вес на Земле: [ Не задан ] — Нажмите кнопку справа для ввода",
            Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(73, 80, 87),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0)
        };

        _lblHint = new Label
        {
            Text = "💡 Подсказка: Вы можете дважды кликнуть по любой строке в таблице для быстрого ввода веса (или Ctrl+E).",
            Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
            ForeColor = Color.FromArgb(120, 130, 140),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0)
        };

        leftTable.Controls.Add(_lblWeightTitle, 0, 0);
        leftTable.Controls.Add(_lblWeightValue, 0, 1);
        leftTable.Controls.Add(_lblHint, 0, 2);

        _btnCardEnter = new Button
        {
            Text = "⚡ Ввести вес человека",
            Dock = DockStyle.Right,
            Width = 220,
            Height = 48,
            BackColor = Color.FromArgb(25, 135, 84), // Яркий зелёный цвет действия
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 22, 0, 22)
        };
        _btnCardEnter.FlatAppearance.BorderSize = 0;
        _btnCardEnter.Click += (s, e) => _controller?.HandleEnterDataCommand(this);

        actionTable.Controls.Add(leftTable, 0, 0);
        actionTable.Controls.Add(_btnCardEnter, 1, 0);

        _actionCard.Controls.Add(actionTable);
    }

    private void BuildExtremumCard()
    {
        _extremumCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(232, 238, 245),
            Margin = new Padding(16, 4, 16, 6),
            Padding = new Padding(16, 6, 16, 6)
        };

        var extTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        extTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        extTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        _lblLightest = new Label
        {
            Text = "🟢 Минимальный вес: Введите вес для анализа экстремумов",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(25, 135, 84),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _lblHeaviest = new Label
        {
            Text = "🔴 Максимальный вес: Введите вес для анализа экстремумов",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(220, 53, 69),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight
        };

        extTable.Controls.Add(_lblLightest, 0, 0);
        extTable.Controls.Add(_lblHeaviest, 1, 0);

        _extremumCard.Controls.Add(extTable);
    }

    private void BuildResultsGrid()
    {
        _gridResults = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false,
            GridColor = Color.FromArgb(225, 230, 235),
            RowTemplate = { Height = 38 },
            Margin = new Padding(16, 2, 16, 8)
        };

        _gridResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 245);
        _gridResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        _gridResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _gridResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        _gridResults.ColumnHeadersHeight = 42;

        _gridResults.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
        _gridResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(207, 226, 255);
        _gridResults.DefaultCellStyle.SelectionForeColor = Color.FromArgb(10, 40, 80);
        _gridResults.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 254);

        // Двойной клик открывает диалог
        _gridResults.CellDoubleClick += (s, e) => _controller?.HandleEnterDataCommand(this);

        ConfigureGridColumns();
    }

    private void ConfigureGridColumns()
    {
        _gridResults.Columns.Clear();

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Symbol",
            HeaderText = "Символ",
            Width = 75,
            FillWeight = 30,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 13F) }
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "NameRu",
            HeaderText = "Небесное тело",
            Width = 150,
            FillWeight = 65,
            DefaultCellStyle = { Font = new Font("Segoe UI", 10F, FontStyle.Bold) }
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Type",
            HeaderText = "Категория",
            Width = 130,
            FillWeight = 50
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Gravity",
            HeaderText = "Ускорение g (м/с²)",
            Width = 140,
            FillWeight = 55,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Ratio",
            HeaderText = "% от Земли",
            Width = 120,
            FillWeight = 50,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Weight",
            HeaderText = "Вес на объекте (кг)",
            Width = 170,
            FillWeight = 75,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold) }
        });

        _gridResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Diff",
            HeaderText = "Разница с Землей (кг)",
            Width = 160,
            FillWeight = 70,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) }
        });
    }

    private void DisplayInitialBodies()
    {
        _gridResults.Rows.Clear();

        foreach (var body in CelestialBody.GetAllBodies())
        {
            string typeRu = body.BodyType switch
            {
                CelestialBodyType.Planet => "Планета",
                CelestialBodyType.DwarfPlanet => "Карликовая пл.",
                CelestialBodyType.Moon => "Спутник",
                _ => "Тело"
            };

            _gridResults.Rows.Add(
                body.Icon,
                body.NameRu,
                typeRu,
                $"{body.SurfaceGravity:F2}",
                $"{body.RelativeGravity * 100.0:F1} %",
                "—",
                "—"
            );
        }
    }

    private void OnModelChanged(object? sender, ModelChangedEventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<object?, ModelChangedEventArgs>(OnModelChanged), sender, e);
            return;
        }

        DisplayResults(e.EarthWeight, e.Results);
    }

    public void DisplayResults(double earthWeight, IReadOnlyList<PlanetCalculationResult> results)
    {
        _lblWeightValue.Text = $"🌍 Ваш вес на Земле: {earthWeight:F2} кг";
        _lblWeightValue.ForeColor = Color.FromArgb(13, 110, 253);

        _gridResults.Rows.Clear();

        PlanetCalculationResult? minWeight = null;
        PlanetCalculationResult? maxWeight = null;

        foreach (var item in results)
        {
            string typeRu = item.Body.BodyType switch
            {
                CelestialBodyType.Planet => "Планета",
                CelestialBodyType.DwarfPlanet => "Карликовая пл.",
                CelestialBodyType.Moon => "Спутник",
                _ => "Тело"
            };

            string diffStr = item.WeightDifference > 0
                ? $"+{item.WeightDifference:F2} кг"
                : $"{item.WeightDifference:F2} кг";

            int rowIndex = _gridResults.Rows.Add(
                item.Body.Icon,
                item.Body.NameRu,
                typeRu,
                $"{item.Body.SurfaceGravity:F2}",
                $"{item.PercentageOfEarthWeight:F1} %",
                $"{item.CalculatedWeight:F2}",
                diffStr
            );

            var row = _gridResults.Rows[rowIndex];
            if (item.Body.Id == "earth")
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(232, 244, 253);
            }
            else if (item.WeightDifference < 0)
            {
                row.Cells["Diff"].Style.ForeColor = Color.FromArgb(25, 135, 84); // Зелёный
            }
            else if (item.WeightDifference > 0)
            {
                row.Cells["Diff"].Style.ForeColor = Color.FromArgb(220, 53, 69); // Красный
            }

            if (minWeight == null || item.CalculatedWeight < minWeight.CalculatedWeight)
                minWeight = item;
            if (maxWeight == null || item.CalculatedWeight > maxWeight.CalculatedWeight)
                maxWeight = item;
        }

        if (minWeight != null && maxWeight != null)
        {
            _lblLightest.Text = $"🟢 Легче всего: {minWeight.Body.NameRu} ({minWeight.CalculatedWeight:F2} кг — {minWeight.PercentageOfEarthWeight:F1}% от Земли)";
            _lblHeaviest.Text = $"🔴 Тяжелее всего: {maxWeight.Body.NameRu} ({maxWeight.CalculatedWeight:F2} кг — {maxWeight.PercentageOfEarthWeight:F1}% от Земли)";
        }

        _statusLabel.Text = $"Расчёт успешно выполнен для веса {earthWeight:F2} кг на {results.Count} небесных телах. Состояние сохранено.";
        _statusLabel.ForeColor = Color.FromArgb(25, 135, 84);
    }

    public void ShowError(string title, string message)
    {
        MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowInfo(string title, string message)
    {
        MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _model.ModelChanged -= OnModelChanged;
        }
        base.Dispose(disposing);
    }
}
