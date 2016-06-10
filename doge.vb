Public Class doge

 

    Private Sub doge_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Panel1.Visible = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Panel1.Visible = True
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Process.Start("http://dogecoin.com/")
    End Sub

    Private Sub doge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Process.Start("http://dogecoin.com/")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            CreateObject("Wscript.Shell").Run(Application.StartupPath & "\startltcproxymining.bat", 0, True)
        Catch ex As Exception

        End Try


    End Sub
End Class