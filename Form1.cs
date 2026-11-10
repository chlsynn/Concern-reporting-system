namespace ConcernReportingSystem;

public partial class Form1 : Form
{
    private readonly string[] categories =
    {
        "Road / Pothole", "Streetlight", "Flooding / Drainage", "Garbage / Waste",
        "Water Supply", "Electricity", "Peace and Order", "Other"
    };
    private readonly string[] statuses = { "Pending", "In Progress", "Resolved" };

    private readonly TextBox txtName = new();
    private readonly TextBox txtLocation = new();
    private readonly TextBox txtDescription = new();
    private readonly ComboBox cboCategory = new();
    private readonly Button btnSubmit = new();

    private readonly Label lblSummary = new();
    private readonly Button btnLogin = new();
    private readonly Button btnLogout = new();

    private readonly TextBox txtSearch = new();
    private readonly ComboBox cboFilter = new();
    private readonly DataGridView dgvConcerns = new();

    private readonly FlowLayoutPanel pnlAdmin = new();
    private readonly ComboBox cboStatus = new();
    private readonly TextBox txtRemarks = new();
    private readonly Button btnUpdate = new();
    private readonly Button btnDelete = new();

    private TableLayoutPanel layout = new();
    private List<Concern> concerns = new();

    public Form1()
    {
        InitializeComponent();

        Text = "Community Concern Reporting System";
        ClientSize = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterScreen;

        BuildLayout();
        Database.Initialize();
        SetAdminMode(false);
        LoadConcerns();
    }

    private void BuildLayout()
    {
        layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(10)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 215));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));

        layout.Controls.Add(CreateTopBar(), 0, 0);
        layout.Controls.Add(CreateSubmitGroup(), 0, 1);
        layout.Controls.Add(CreateFilterBar(), 0, 2);
        layout.Controls.Add(CreateGrid(), 0, 3);
        layout.Controls.Add(CreateAdminPanel(), 0, 4);

        Controls.Add(layout);
    }

    private Control CreateTopBar()
    {
        var panel = new Panel { Dock = DockStyle.Fill };

        lblSummary.Dock = DockStyle.Fill;
        lblSummary.TextAlign = ContentAlignment.MiddleLeft;

        btnLogin.Text = "Admin Login";
        btnLogin.Dock = DockStyle.Right;
        btnLogin.Width = 110;
        btnLogin.Click += btnLogin_Click;

        btnLogout.Text = "Logout";
        btnLogout.Dock = DockStyle.Right;
        btnLogout.Width = 110;
        btnLogout.Click += btnLogout_Click;

        panel.Controls.Add(lblSummary);
        panel.Controls.Add(btnLogin);
        panel.Controls.Add(btnLogout);
        return panel;
    }

    private Control CreateSubmitGroup()
    {
        var group = new GroupBox { Text = "New Concern", Dock = DockStyle.Fill };

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 4,
            Padding = new Padding(8)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.Items.AddRange(categories);
        cboCategory.Dock = DockStyle.Fill;

        txtName.Dock = DockStyle.Fill;
        txtLocation.Dock = DockStyle.Fill;
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Multiline = true;
        txtDescription.ScrollBars = ScrollBars.Vertical;

        btnSubmit.Text = "Submit";
        btnSubmit.Anchor = AnchorStyles.Left;
        btnSubmit.Width = 100;
        btnSubmit.Click += btnSubmit_Click;

        table.Controls.Add(CreateLabel("Name"), 0, 0);
        table.Controls.Add(txtName, 1, 0);
        table.Controls.Add(CreateLabel("Category"), 2, 0);
        table.Controls.Add(cboCategory, 3, 0);
        table.Controls.Add(CreateLabel("Location"), 0, 1);
        table.Controls.Add(txtLocation, 1, 1);
        table.SetColumnSpan(txtLocation, 3);
        table.Controls.Add(CreateLabel("Description"), 0, 2);
        table.Controls.Add(txtDescription, 1, 2);
        table.SetColumnSpan(txtDescription, 3);
        table.Controls.Add(btnSubmit, 1, 3);

        group.Controls.Add(table);
        return group;
    }

    private Control CreateFilterBar()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(0, 6, 0, 0) };

        txtSearch.Width = 250;
        txtSearch.TextChanged += (s, e) => ShowConcerns();

        cboFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFilter.Width = 140;
        cboFilter.Items.Add("All");
        cboFilter.Items.AddRange(statuses);
        cboFilter.SelectedIndex = 0;
        cboFilter.SelectedIndexChanged += (s, e) => ShowConcerns();

        flow.Controls.Add(CreateLabel("Search", 50));
        flow.Controls.Add(txtSearch);
        flow.Controls.Add(CreateLabel("  Status", 60));
        flow.Controls.Add(cboFilter);
        return flow;
    }

    private Control CreateGrid()
    {
        dgvConcerns.Dock = DockStyle.Fill;
        dgvConcerns.AutoGenerateColumns = false;
        dgvConcerns.AllowUserToAddRows = false;
        dgvConcerns.AllowUserToDeleteRows = false;
        dgvConcerns.ReadOnly = true;
        dgvConcerns.MultiSelect = false;
        dgvConcerns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvConcerns.RowHeadersVisible = false;
        dgvConcerns.BackgroundColor = SystemColors.Window;
        dgvConcerns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvConcerns.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvConcerns.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        AddColumn("Id", "ID", 30);
        AddColumn("CreatedAt", "Date", 70, "MMM dd, yyyy");
        AddColumn("Category", "Category", 90);
        AddColumn("Location", "Location", 100);
        AddColumn("Description", "Description", 200);
        AddColumn("ReporterName", "Reported By", 70);
        AddColumn("Status", "Status", 70);
        AddColumn("AdminRemarks", "Remarks", 120);

        dgvConcerns.SelectionChanged += dgvConcerns_SelectionChanged;
        return dgvConcerns;
    }

    private void AddColumn(string property, string header, float weight, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            DataPropertyName = property,
            HeaderText = header,
            FillWeight = weight
        };
        if (format != null)
            column.DefaultCellStyle.Format = format;
        dgvConcerns.Columns.Add(column);
    }

    private Control CreateAdminPanel()
    {
        pnlAdmin.Dock = DockStyle.Fill;
        pnlAdmin.Padding = new Padding(0, 10, 0, 0);

        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Width = 130;
        cboStatus.Items.AddRange(statuses);

        txtRemarks.Width = 380;

        btnUpdate.Text = "Save";
        btnUpdate.Width = 90;
        btnUpdate.Click += btnUpdate_Click;

        btnDelete.Text = "Delete";
        btnDelete.Width = 90;
        btnDelete.Click += btnDelete_Click;

        pnlAdmin.Controls.Add(CreateLabel("Status", 50));
        pnlAdmin.Controls.Add(cboStatus);
        pnlAdmin.Controls.Add(CreateLabel("  Remarks", 65));
        pnlAdmin.Controls.Add(txtRemarks);
        pnlAdmin.Controls.Add(btnUpdate);
        pnlAdmin.Controls.Add(btnDelete);
        return pnlAdmin;
    }

    private static Label CreateLabel(string text, int width = 0)
    {
        var label = new Label { Text = text, TextAlign = ContentAlignment.MiddleLeft };
        if (width > 0)
        {
            label.Width = width;
            label.Height = 25;
        }
        else
        {
            label.Dock = DockStyle.Fill;
        }
        return label;
    }

    private void LoadConcerns()
    {
        concerns = Database.GetAll();
        ShowConcerns();
    }

    private void ShowConcerns()
    {
        string keyword = txtSearch.Text.Trim().ToLower();
        string status = cboFilter.SelectedItem?.ToString() ?? "All";

        dgvConcerns.DataSource = concerns.Where(c =>
            (status == "All" || c.Status == status) &&
            (keyword == "" ||
             c.Category.ToLower().Contains(keyword) ||
             c.Location.ToLower().Contains(keyword) ||
             c.Description.ToLower().Contains(keyword))).ToList();

        lblSummary.Text = string.Format("Total: {0}   Pending: {1}   In Progress: {2}   Resolved: {3}",
            concerns.Count,
            concerns.Count(c => c.Status == "Pending"),
            concerns.Count(c => c.Status == "In Progress"),
            concerns.Count(c => c.Status == "Resolved"));
    }

    private Concern? GetSelectedConcern()
    {
        return dgvConcerns.CurrentRow?.DataBoundItem as Concern;
    }

    private void dgvConcerns_SelectionChanged(object? sender, EventArgs e)
    {
        var concern = GetSelectedConcern();
        if (concern != null)
        {
            cboStatus.SelectedItem = concern.Status;
            txtRemarks.Text = concern.AdminRemarks;
        }
    }

    private void btnSubmit_Click(object? sender, EventArgs e)
    {
        if (cboCategory.SelectedItem == null ||
            txtLocation.Text.Trim() == "" ||
            txtDescription.Text.Trim() == "")
        {
            MessageBox.Show("Category, location and description are required.", "Submit",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Database.Add(new Concern
        {
            ReporterName = txtName.Text.Trim() == "" ? "Anonymous" : txtName.Text.Trim(),
            Category = cboCategory.SelectedItem.ToString()!,
            Location = txtLocation.Text.Trim(),
            Description = txtDescription.Text.Trim()
        });

        txtName.Clear();
        txtLocation.Clear();
        txtDescription.Clear();
        cboCategory.SelectedIndex = -1;
        LoadConcerns();
    }

    private void btnUpdate_Click(object? sender, EventArgs e)
    {
        var concern = GetSelectedConcern();
        if (concern == null)
        {
            MessageBox.Show("Select a concern first.", "Update");
            return;
        }

        Database.UpdateStatus(concern.Id, cboStatus.SelectedItem?.ToString() ?? concern.Status,
            txtRemarks.Text.Trim());
        LoadConcerns();
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        var concern = GetSelectedConcern();
        if (concern == null)
        {
            MessageBox.Show("Select a concern first.", "Delete");
            return;
        }

        if (MessageBox.Show("Delete the selected concern?", "Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            Database.Delete(concern.Id);
            LoadConcerns();
        }
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        using var login = new LoginForm();
        if (login.ShowDialog(this) == DialogResult.OK)
            SetAdminMode(true);
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        SetAdminMode(false);
    }

    private void SetAdminMode(bool isAdmin)
    {
        btnLogin.Visible = !isAdmin;
        btnLogout.Visible = isAdmin;
        pnlAdmin.Visible = isAdmin;
        layout.RowStyles[4].Height = isAdmin ? 50 : 0;
    }
}