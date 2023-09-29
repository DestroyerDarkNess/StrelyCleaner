Public Class Functions

#Region " Round Bytes "

    ' [ Round Bytes Function ]
    '
    ' // By Elektro H@cker
    '
    ' Examples :
    '
    ' MsgBox(Round_Bytes(1023))             ' Result: 1.023 Bytes
    ' MsgBox(Round_Bytes(80060, 1))         ' Result: 78,2 KB
    ' MsgBox(Round_Bytes(60877579))         ' Result: 58,06 MB
    ' MsgBox(Round_Bytes(4485888579))       ' Result: 4,18 GB
    ' MsgBox(Round_Bytes(20855564677579))   ' Result: 18,97 TB
    ' MsgBox(Round_Bytes(990855564677579))  ' Result: 901,18 PB
    ' MsgBox(Round_Bytes(1987464809247272)) ' Result: 1,77 PB

    Enum xByte As Long
        kilobyte = 1024L
        megabyte = 1024L * kilobyte
        gigabyte = 1024L * megabyte
        terabyte = 1024L * gigabyte
        petabyte = 1024L * terabyte
    End Enum

    Public Shared Function Round_Bytes(ByVal bytes As Long,
                                  Optional ByVal decimals As Integer = 2) As String

        Dim Result As Integer = 0

        Dim Suffix As String = String.Empty

        Select Case bytes

            Case Is >= xByte.petabyte
                Suffix = "PB"
                Result = (Convert.ToDouble(bytes) / xByte.petabyte).ToString("n" & decimals)
                'Return String.Format("{0} PB", (Convert.ToDouble(bytes) / xByte.petabyte).ToString("n" & decimals))

            Case Is >= xByte.terabyte
                Suffix = "TB"
                Result = (Convert.ToDouble(bytes) / xByte.terabyte).ToString("n" & decimals)
                'Return String.Format("{0} TB", (Convert.ToDouble(bytes) / xByte.terabyte).ToString("n" & decimals))

            Case Is >= xByte.gigabyte
                Suffix = "GB"
                Result = (Convert.ToDouble(bytes) / xByte.gigabyte).ToString("n" & decimals)
                'Return String.Format("{0} GB", (Convert.ToDouble(bytes) / xByte.gigabyte).ToString("n" & decimals))

            Case Is >= xByte.megabyte
                Suffix = "MB"
                Result = (Convert.ToDouble(bytes) / xByte.megabyte).ToString("n" & decimals)
                'Return String.Format("{0} MB", (Convert.ToDouble(bytes) / xByte.megabyte).ToString("n" & decimals))

            Case Is >= xByte.kilobyte
                Suffix = "KB"
                Result = (Convert.ToDouble(bytes) / xByte.kilobyte).ToString("n" & decimals)
                'Return String.Format("{0} KB", (Convert.ToDouble(bytes) / xByte.kilobyte).ToString("n" & decimals))

            Case Is >= 0
                Suffix = "Bytes"
                Return String.Format("{0} Bytes", Convert.ToDouble(bytes).ToString("n0"))

            Case Else
                Return String.Empty

        End Select
        Return String.Format("{0} {1}", Result, Suffix)
    End Function

#End Region

    Public Shared Function IsFolder(ByVal path As String) As Boolean
        Return ((IO.File.GetAttributes(path) And IO.FileAttributes.Directory) = IO.FileAttributes.Directory)
    End Function

    Public Shared Function OpenAsAdmin(ByVal FilePth As String, Optional ByVal Argument As String = "") As Boolean
        Try
            Dim procStartInfo As New ProcessStartInfo
            Dim procExecuting As New Process

            With procStartInfo
                .UseShellExecute = True
                .FileName = FilePth
                .Arguments = Argument
                .WindowStyle = ProcessWindowStyle.Normal
                .Verb = "runas"
            End With

            procExecuting = Process.Start(procStartInfo)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function IsAdmin() As Boolean
        Try
            Dim Identity As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()
            Dim Principal As System.Security.Principal.WindowsPrincipal = New System.Security.Principal.WindowsPrincipal(Identity)
            Dim IsElevated As Boolean = Principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
            Return IsElevated
        Catch ex As Exception
            Return False
        End Try
    End Function


End Class
