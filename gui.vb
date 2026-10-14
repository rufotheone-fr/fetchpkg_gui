Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Threading

Public Class FetchpkgGUI
    Inherits Form

    Private lblLang, lblUrl, lblProgress, lblEta As Label
    Private txtUrl As TextBox
    Private cbLang As ComboBox
    Private btnDownload, btnTheme As Button
    Private pbProgress As ProgressBar
    Private lnkGithub As LinkLabel
    Private fetchProcess As Process
    Private logAccumulator As String = ""
    
    Private Const WidthFenetre As Integer = 600
    Private Const HeightCompacte As Integer = 270
    Private isEnglish As Boolean = False
    Private isDarkMode As Boolean = True

    Public Sub New()
        Me.Text = "fetchpkg GUI v0.6"
        Me.Size = New Size(WidthFenetre, HeightCompacte)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        btnTheme = New Button()
        btnTheme.Text = "☀️"
        btnTheme.Font = New Font("Segoe UI Emoji", 10)
        btnTheme.Location = New Point(20, 12)
        btnTheme.Size = New Size(35, 25)
        AddHandler btnTheme.Click, AddressOf BtnTheme_Click
        Me.Controls.Add(btnTheme)

        lblLang = New Label()
        lblLang.Location = New Point(400, 15)
        lblLang.Size = New Size(80, 20)
        lblLang.TextAlign = ContentAlignment.MiddleRight
        Me.Controls.Add(lblLang)

        cbLang = New ComboBox()
        cbLang.Items.AddRange(New Object() {"Français", "English"})
        cbLang.SelectedIndex = 0
        cbLang.Location = New Point(485, 12)
        cbLang.Size = New Size(75, 25)
        cbLang.DropDownStyle = ComboBoxStyle.DropDownList
        AddHandler cbLang.SelectedIndexChanged, AddressOf CbLang_SelectedIndexChanged
        Me.Controls.Add(cbLang)

        lblUrl = New Label()
        lblUrl.Location = New Point(20, 45)
        lblUrl.Size = New Size(540, 20)
        Me.Controls.Add(lblUrl)

        txtUrl = New TextBox()
        txtUrl.Location = New Point(20, 68)
        txtUrl.Size = New Size(540, 25)
        Me.Controls.Add(txtUrl)
        btnDownload = New Button()
        btnDownload.Location = New Point(20, 105)
        btnDownload.Size = New Size(540, 35)
        AddHandler btnDownload.Click, AddressOf BtnDownload_Click
        Me.Controls.Add(btnDownload)

        pbProgress = New ProgressBar()
        pbProgress.Location = New Point(20, 155)
        pbProgress.Size = New Size(440, 23)
        pbProgress.Minimum = 0
        pbProgress.Maximum = 100
        Me.Controls.Add(pbProgress)

        lblProgress = New Label()
        lblProgress.Text = "0%"
        lblProgress.Location = New Point(470, 158)
        lblProgress.Size = New Size(90, 20)
        lblProgress.TextAlign = ContentAlignment.MiddleRight
        Me.Controls.Add(lblProgress)

        lblEta = New Label()
        lblEta.Location = New Point(20, 185)
        lblEta.Size = New Size(540, 20)
        lblEta.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        Me.Controls.Add(lblEta)

        lnkGithub = New LinkLabel()
        lnkGithub.Text = "fetchpkg - github.com/ps5-payload-dev/fetchpkg"
        lnkGithub.Location = New Point(20, 210)
        lnkGithub.Size = New Size(540, 20)
        lnkGithub.TextAlign = ContentAlignment.MiddleCenter
        lnkGithub.Links.Add(11, 45, "https://github.com/ps5-payload-dev/fetchpkg")
        AddHandler lnkGithub.LinkClicked, AddressOf LnkGithub_LinkClicked
        Me.Controls.Add(lnkGithub)

        ApplyTheme()
        MettreAJourLangue()
    End Sub
    Private Sub CbLang_SelectedIndexChanged(sender As Object, e As EventArgs)
        isEnglish = (cbLang.SelectedIndex = 1)
        MettreAJourLangue()
    End Sub

    Private Sub BtnTheme_Click(sender As Object, e As EventArgs)
        isDarkMode = Not isDarkMode
        ApplyTheme()
    End Sub

    Private Sub ApplyTheme()
        If isDarkMode Then
            Me.BackColor = Color.FromArgb(30, 30, 30)
            lblLang.ForeColor = Color.White : lblUrl.ForeColor = Color.White
            lblProgress.ForeColor = Color.White : lblEta.ForeColor = Color.White
            lnkGithub.ForeColor = Color.White : lnkGithub.LinkColor = Color.LightSkyBlue
            btnDownload.BackColor = Color.FromArgb(50, 50, 50) : btnDownload.ForeColor = Color.White
            btnTheme.BackColor = Color.FromArgb(50, 50, 50) : btnTheme.ForeColor = Color.White
            btnTheme.Text = "☀️"
        Else
            Me.BackColor = SystemColors.Control
            lblLang.ForeColor = Color.Black : lblUrl.ForeColor = Color.Black
            lblProgress.ForeColor = Color.Black : lblEta.ForeColor = Color.Black
            lnkGithub.ForeColor = Color.Black : lnkGithub.LinkColor = Color.Blue
            btnDownload.BackColor = Color.LightBlue : btnDownload.ForeColor = Color.Black
            btnTheme.BackColor = SystemColors.Control : btnTheme.ForeColor = Color.Black
            btnTheme.Text = "🌙"
        End If
    End Sub

    Private Sub MettreAJourLangue()
        If isEnglish Then
            lblLang.Text = "Language:" : lblUrl.Text = "Paste CS, CRC or JSON link:"
            btnDownload.Text = If(btnDownload.Enabled, "Start Download", "Downloading...")
        Else
            lblLang.Text = "Langue :" : lblUrl.Text = "Collez le lien CS, CRC ou JSON :"
            btnDownload.Text = If(btnDownload.Enabled, "Démarrer le téléchargement", "Téléchargement en cours...")
        End If
    End Sub

    Private Sub LnkGithub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Try
            Process.Start(New ProcessStartInfo(e.Link.LinkData.ToString()) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub BtnDownload_Click(sender As Object, e As EventArgs)
        Dim url As String = txtUrl.Text.Trim()
        If String.IsNullOrEmpty(url) Then Return

        If url.EndsWith("_sc.pkg", StringComparison.OrdinalIgnoreCase) Then
            url = url.Substring(0, url.Length - 7) & ".json"
        ElseIf url.EndsWith(".crc", StringComparison.OrdinalIgnoreCase) Then
            url = url.Substring(0, url.Length - 4) & ".json"
        End If
        txtUrl.Text = url

        btnDownload.Enabled = False : MettreAJourLangue()
        pbProgress.Value = 0 : lblProgress.Text = "0%"
        lblEta.Text = If(isEnglish, "Calculating...", "Calcul en cours...")
        logAccumulator = ""

        Dim t As New Thread(Sub() StartFetchpkg(url))
        t.IsBackground = True : t.Start()
    End Sub

    Private Sub StartFetchpkg(url As String)
        Try
            fetchProcess = New Process()
            fetchProcess.StartInfo.FileName = "fetchpkg.exe"
            fetchProcess.StartInfo.Arguments = """" & url & """"
            fetchProcess.StartInfo.UseShellExecute = False
            fetchProcess.StartInfo.RedirectStandardOutput = True
            fetchProcess.StartInfo.RedirectStandardError = True
            fetchProcess.StartInfo.CreateNoWindow = True

            AddHandler fetchProcess.OutputDataReceived, AddressOf Process_DataReceived
            AddHandler fetchProcess.ErrorDataReceived, AddressOf Process_DataReceived

            fetchProcess.Start()
            fetchProcess.BeginOutputReadLine() : fetchProcess.BeginErrorReadLine()
            fetchProcess.WaitForExit()

            Dim exitCode As Integer = fetchProcess.ExitCode
            Me.Invoke(Sub()
                btnDownload.Enabled = True : MettreAJourLangue()
                If exitCode = 0 Then
                    pbProgress.Value = 100 : lblProgress.Text = "100%"
                    lblEta.Text = If(isEnglish, "Done", "Terminé")
                Else
                    lblEta.Text = If(isEnglish, "Failed", "Échec")
                    OuvrirFenetreLogs()
                End If
            End Sub)
        Catch ex As Exception
            Me.Invoke(Sub()
                btnDownload.Enabled = True : logAccumulator &= ex.Message
                OuvrirFenetreLogs()
            End Sub)
        End Try
    End Sub

    Private Sub Process_DataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(e.Data) Then
            Me.Invoke(Sub()
                logAccumulator &= e.Data & Environment.NewLine
                Dim idx As Integer = e.Data.IndexOf("%")
                If idx > 0 Then
                    Dim start As Integer = idx - 1
                    While start >= 0 AndAlso (Char.IsDigit(e.Data(start)) OrElse e.Data(start) = "."c OrElse e.Data(start) = ","c)
                        start -= 1
                    End While
                    Dim numStr As String = e.Data.Substring(start + 1, idx - start - 1).Replace(",", ".")
                    Dim pct As Double
                    If Double.TryParse(numStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pct) Then
                        Dim intPct As Integer = Convert.ToInt32(Math.Floor(pct))
                        If intPct >= 0 AndAlso intPct <= 100 Then
                            pbProgress.Value = intPct
                            lblProgress.Text = intPct & "%"
                        End If
                    End If
                End If
                Dim idxEta As Integer = e.Data.IndexOf("eta:", StringComparison.OrdinalIgnoreCase)
                If idxEta >= 0 Then lblEta.Text = If(isEnglish, "Time remaining: ", "Temps restant : ") & e.Data.Substring(idxEta + 4).Trim()
            End Sub)
        End If
    End Sub

    Private Sub OuvrirFenetreLogs()
        Dim logForm As New Form With {.Text = "Logs", .Size = New Size(600, 400)}
        Dim txtBox As New TextBox With {.Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical, .ReadOnly = True, .BackColor = Color.Black, .ForeColor = Color.OrangeRed, .Text = logAccumulator}
        logForm.Controls.Add(txtBox) : logForm.ShowDialog()
    End Sub

    <STAThread>
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New FetchpkgGUI())
    End Sub
End Class