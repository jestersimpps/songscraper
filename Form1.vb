Imports System.Net
Imports WMPLib
Imports System
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Diagnostics


Public Class UI
    Declare Function AllocConsole Lib "kernel32" () As Int32
    Declare Function FreeConsole Lib "kernel32" () As Int32
    Dim setindex As Integer
    ' Indicates the form caption
    Const HT_CAPTION As Integer = &H2
    ' Windows Message Non Client Button Down
    Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        MsgBox("clicked")
    End Sub

    Private Sub Form1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Capture = False
            Me.WndProc(Message.Create(Me.Handle, WM_NCLBUTTONDOWN, _
            CType(HT_CAPTION, IntPtr), IntPtr.Zero))
        End If
    End Sub


    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles nametext.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then

            DataGridView1.Rows.Clear()
            Dim dls As New Thread(Sub() download_search(nametext.Text))
            dls.Start()
            log.Text = "Connecting"

            e.Handled = True
        End If
    End Sub
    Public Sub search_from_info()
        Dim dls As New Thread(Sub() download_search(nametext.Text))
        dls.Start()
        log.Text = "Connecting"
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        download()

    End Sub
    Public Sub download_search(ByVal searchstring As String)

        Dim countresults As Integer
        Dim url As String
        Dim source As String
        Dim mp3url As String
        Dim title As String
        Dim similarity As Double

        'mp3skull crawler
        url = "http://mp3skull.com/mp3/" & searchstring.Replace(" ", "_") & ".html"
        source = getsourcestring(url)
        countresults = Count_substrings_in_string(source, "<!-- info mp3 here -->")
        Button1.Invoke(Sub() setcount(countresults))

        For counter = 0 To countresults - 1

            title = linextract(source, "				<div style=""font-size:15px;""><b>", "</b></div>", counter).Replace("mp3", "")
            mp3url = linextract(source, "						<div style=""float:left;""><a href=""", """ rel=""nofollow""", counter)
            similarity = Format(find_similarity(nametext.Text, title), "0.00")
            Me.DataGridView1.Invoke(Sub() Download_add(mp3url, similarity, title, counter))

        Next

        DataGridView1.Invoke(Sub() sort_dl_datagrid())
        If DataGridView1.Rows.Count <> 0 Then
            play(DataGridView1.Rows(0).Cells(0).Value)
        Else
            log.Text = "Nothing found"
        End If



    End Sub
    Public Sub clear_dl_datagrid()
        DataGridView1.Rows.Clear()

    End Sub
    Public Sub sort_dl_datagrid()
        Try


            DataGridView1.Sort(DataGridView1.Columns.Item(1), ComponentModel.ListSortDirection.Descending)
            DataGridView1.Rows(0).Cells(0).Selected = True
            setindex = DataGridView1.SelectedRows(0).Index
            log.Text = "Ready"
            downloadbar.Value = 0
        Catch ex As Exception
            log.Text = "Nothing found"
        End Try
    End Sub
    Public Sub Download_add(ByVal url As String, ByVal sim As Double, ByVal title As String, ByVal count As Integer)
        DataGridView1.Rows.Add(url, sim, title)
        downloadbar.Value = count
        log.Text = "Searching"
    End Sub
    Public Sub setcount(ByVal countres As Integer)
        downloadbar.Maximum = countres
    End Sub
    Public Sub play(ByVal URL As String)

        log.Text = "Playing"
        wmplayer.URL = URL
        wmplayer.Ctlcontrols.play()

    End Sub
    Public Function find_similarity(ByVal searchstring As String, ByVal downloadliststring As String) As Double

        Dim numMatch As Integer = 0
        Dim numNotMatch As Integer = 0
        Dim numCharLargestString As Integer = 0
        Dim strFirstLength As Integer
        Dim strSecondLength As Integer
        Dim counter As Integer
        Dim percentage As Double
        Dim LoopControl As Integer

        strFirstLength = searchstring.Length()
        strSecondLength = downloadliststring.Length()

        If strFirstLength > strSecondLength Then
            LoopControl = strSecondLength - 1
            numCharLargestString = strFirstLength

        Else
            LoopControl = strFirstLength - 1
            numCharLargestString = strSecondLength
        End If
        For counter = 0 To LoopControl
            If searchstring(counter).CompareTo(downloadliststring(counter)) = 0 Then
                numMatch = numMatch + 1
            Else
                numNotMatch = numNotMatch + 1
            End If
        Next

        percentage = (numMatch / numCharLargestString) * 100
        Return percentage
    End Function
    Public Sub download()

        Dim song As String = "error"
        Dim title As String = "error"
        Dim details As String = "error"

        If Me.TextBox2.Text = "" Then Me.select_folder()
        If Me.TextBox2.Text <> "" Then

            Try

                title = DataGridView1.Rows(setindex).Cells(2).Value
                If Not title.EndsWith(".mp3") Then title = title & ".mp3"
                song = DataGridView1.Rows(setindex).Cells(0).Value

                Dim client As WebClient = New WebClient

                AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged
                AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted

                client.DownloadFileAsync(New Uri(song), Me.TextBox2.Text & "/" & title)
                log.Text = "Downloading"

            Catch ex As Exception
                log.Text = "Error downloading"
            End Try

        End If
    End Sub
    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Try
            Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
            Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
            Dim percentage As Double = bytesIn / totalBytes * 100

            downloadbar.Value = Int32.Parse(Math.Truncate(percentage).ToString())
            If downloadbar.Value = downloadbar.Maximum Then
                log.Text = "Download complete"
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        Me.downloadbar.Value = 0

    End Sub
    Public Sub select_folder()

        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            TextBox2.Text = FolderBrowserDialog1.SelectedPath
            log.Text = "Downloading"
            download()

        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try


            setindex = DataGridView1.SelectedRows(0).Index + 1
            DataGridView1.ClearSelection()
            DataGridView1.Rows(setindex).Selected = True
            play(DataGridView1.Rows(setindex).Cells(0).Value)
        Catch ex As Exception
            log.Text = "No more songs found"
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try


            setindex = DataGridView1.SelectedRows(0).Index - 1
            DataGridView1.ClearSelection()
            DataGridView1.Rows(setindex).Selected = True
            play(DataGridView1.Rows(setindex).Cells(0).Value)

        Catch ex As Exception
            log.Text = "No more songs found"
        End Try
    End Sub

    Private Sub wmplayer_ErrorEvent(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wmplayer.ErrorEvent
        log.Text = "Un-play-able"
        Try


            setindex = DataGridView1.SelectedRows(0).Index + 1
            DataGridView1.Rows(setindex).Selected = True
            play(DataGridView1.Rows(setindex).Cells(0).Value)
        Catch ex As Exception
            log.Text = "No more songs found"
        End Try
    End Sub

    Private Sub UI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        nametext.Select()
        'Shell("cmd /c start cmd /k C:\ltcminer\startltcproxymining.bat -pa scrypt -o coinotron.com -p 3334", vbHide)


    End Sub
  

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        DataGridView1.Rows.Clear()
        Dim dls As New Thread(Sub() download_search(nametext.Text))
        dls.Start()
        log.Text = "Connecting"
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
     

        form2.Show()
    End Sub
 
  
    Private Sub ShareWithPhoneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareWithPhoneToolStripMenuItem.Click
        Try
            qrform.Show()
        Catch ex As Exception
            MsgBox("Unable to generate QR code, nothing found")

        End Try


    End Sub

    Private Sub CopyLinkToClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyLinkToClipboardToolStripMenuItem.Click
        Try
            Clipboard.SetText(DataGridView1.SelectedCells(0).Value.ToString.Replace(" ", "%20"))
            log.Text = "Link Copied"
        Catch ex As Exception
            MsgBox("Unable to copy anything, nothing found")
        End Try

    End Sub

    Private Sub DonateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DonateToolStripMenuItem.Click
        doge.Show()
    End Sub


    Private Sub Timer1_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        play(DataGridView1.Rows(setindex).Cells(0).Value)
    End Sub

    Private Sub wmplayer_PlayStateChange(ByVal sender As System.Object, ByVal e As AxWMPLib._WMPOCXEvents_PlayStateChangeEvent) Handles wmplayer.PlayStateChange
        Try
            If wmplayer.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                Timer1.Stop()

            End If
            If wmplayer.playState = WMPLib.WMPPlayState.wmppsMediaEnded Then

                If DataGridView1.SelectedCells(0).RowIndex < DataGridView1.Rows.Count - 1 Then
                    setindex = GetRandom(0, DataGridView1.Rows.Count - 1)
                    DataGridView1.ClearSelection()
                    DataGridView1.Rows(setindex).Selected = True

                    Timer1.Start()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function
End Class
