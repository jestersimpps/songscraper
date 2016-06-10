<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class qrform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.QrCodeGraphicControl1 = New Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeGraphicControl()
        Me.SuspendLayout()
        '
        'QrCodeGraphicControl1
        '
        Me.QrCodeGraphicControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.QrCodeGraphicControl1.ErrorCorrectLevel = Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M
        Me.QrCodeGraphicControl1.Location = New System.Drawing.Point(0, 0)
        Me.QrCodeGraphicControl1.Name = "QrCodeGraphicControl1"
        Me.QrCodeGraphicControl1.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Two
        Me.QrCodeGraphicControl1.Size = New System.Drawing.Size(294, 276)
        Me.QrCodeGraphicControl1.TabIndex = 0
        Me.QrCodeGraphicControl1.Text = "QrCodeGraphicControl1"
        '
        'qrform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(294, 276)
        Me.Controls.Add(Me.QrCodeGraphicControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximumSize = New System.Drawing.Size(300, 300)
        Me.MinimumSize = New System.Drawing.Size(300, 300)
        Me.Name = "qrform"
        Me.Text = "qrform"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents QrCodeGraphicControl1 As Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeGraphicControl
End Class
