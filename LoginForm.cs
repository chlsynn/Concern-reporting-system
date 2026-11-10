namespace ConcernReportingSystem;

public class LoginForm : Form
{
    private const string AdminUser = "admin";
    private const string AdminPass = "admin123";

    private readonly TextBox txtUsername = new();
    private readonly TextBox txtPassword = new() { UseSystemPasswordChar = true };

    public LoginForm()
    {
        Text = "Admin Login";
        ClientSize = new Size(300, 150);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var lblUsername = new Label { Text = "Username", Left = 20, Top = 24, Width = 75 };
        var lblPassword = new Label { Text = "Password", Left = 20, Top = 58, Width = 75 };
        txtUsername.SetBounds(100, 20, 175, 23);
        txtPassword.SetBounds(100, 54, 175, 23);

        var btnLogin = new Button { Text = "Login", Left = 100, Top = 100, Width = 80 };
        var btnCancel = new Button { Text = "Cancel", Left = 195, Top = 100, Width = 80 };

        btnLogin.Click += (s, e) =>
        {
            if (txtUsername.Text == AdminUser && txtPassword.Text == AdminPass)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Incorrect username or password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        };
        btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

        AcceptButton = btnLogin;
        CancelButton = btnCancel;
        Controls.AddRange(new Control[] { lblUsername, lblPassword, txtUsername, txtPassword, btnLogin, btnCancel });
    }
}