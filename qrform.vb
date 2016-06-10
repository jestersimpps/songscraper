Public Class qrform

    Private Sub qrform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = UI.DataGridView1.SelectedCells(2).Value
        QrCodeGraphicControl1.Text = UI.DataGridView1.SelectedCells(0).Value.ToString.Replace(" ", "%20")

    End Sub
End Class