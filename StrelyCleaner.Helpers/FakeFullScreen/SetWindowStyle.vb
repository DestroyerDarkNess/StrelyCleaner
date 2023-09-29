' ***********************************************************************
' Author           : Destroyer
' Last Modified On : 29-09-2020
' ***********************************************************************
' <copyright file="SetWindowStyle.vb" company="Elektro Studios">
'     Copyright (c) All rights reserved.
' </copyright>
' ***********************************************************************

#Region " Usage Examples "

'Dim HWND As IntPtr = Process.GetProcessesByName("devenv").First.MainWindowHandle
'
'SetWindowState.SetWindowStyle(HWND, SetWindowStyle.WindowStyles.WS_BORDER)
'SetWindowState.SetWindowStyle("devenv", SetWindowStyle.WindowStyles.WS_BORDER, Recursivity:=False)

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

''' <summary>
''' Sets the style of a window.
''' </summary>
Public NotInheritable Class SetWindowStyle

#Region " P/Invoke "

    ''' <summary>
    ''' Platform Invocation methods (P/Invoke), access unmanaged code.
    ''' This class does not suppress stack walks for unmanaged code permission.
    ''' <see cref="System.Security.SuppressUnmanagedCodeSecurityAttribute"/>  must not be applied to this class.
    ''' This class is for methods that can be used anywhere because a stack walk will be performed.
    ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/ms182161.aspx
    ''' </summary>
    Protected NotInheritable Class NativeMethods

#Region " Methods "

        ''' <summary>
        ''' Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        ''' This function does not search child windows.
        ''' This function does not perform a case-sensitive search.
        ''' To search child windows, beginning with a specified child window, use the FindWindowEx function.
        ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/windows/desktop/ms633499%28v=vs.85%29.aspx
        ''' </summary>
        ''' <param name="lpClassName">The class name.
        ''' If this parameter is NULL, it finds any window whose title matches the lpWindowName parameter.</param>
        ''' <param name="lpWindowName">The window name (the window's title).
        ''' If this parameter is NULL, all window names match.</param>
        ''' <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.
        ''' If the function fails, the return value is NULL.</returns>
        <DllImport("user32.dll", SetLastError:=False, CharSet:=CharSet.Auto, BestFitMapping:=False)>
        Friend Shared Function FindWindow(
           ByVal lpClassName As String,
           ByVal lpWindowName As String
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Retrieves a handle to a window whose class name and window name match the specified strings. 
        ''' The function searches child windows, beginning with the one following the specified child window. 
        ''' This function does not perform a case-sensitive search.
        ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/windows/desktop/ms633500%28v=vs.85%29.aspx
        ''' </summary>
        ''' <param name="hwndParent">
        ''' A handle to the parent window whose child windows are to be searched.
        ''' If hwndParent is NULL, the function uses the desktop window as the parent window. 
        ''' The function searches among windows that are child windows of the desktop. 
        ''' </param>
        ''' <param name="hwndChildAfter">
        ''' A handle to a child window. 
        ''' The search begins with the next child window in the Z order. 
        ''' The child window must be a direct child window of hwndParent, not just a descendant window.
        ''' If hwndChildAfter is NULL, the search begins with the first child window of hwndParent.
        ''' </param>
        ''' <param name="strClassName">
        ''' The window class name.
        ''' </param>
        ''' <param name="strWindowName">
        ''' The window name (the window's title). 
        ''' If this parameter is NULL, all window names match.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to the window that has the specified class and window names.
        ''' If the function fails, the return value is NULL.
        ''' </returns>
        <DllImport("User32.dll", SetLastError:=False, CharSet:=CharSet.Auto, BestFitMapping:=False)>
        Friend Shared Function FindWindowEx(
           ByVal hwndParent As IntPtr,
           ByVal hwndChildAfter As IntPtr,
           ByVal strClassName As String,
           ByVal strWindowName As String
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Retrieves the identifier of the thread that created the specified window 
        ''' and, optionally, the identifier of the process that created the window.
        ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/windows/desktop/ms633522%28v=vs.85%29.aspx
        ''' </summary>
        ''' <param name="hWnd">A handle to the window.</param>
        ''' <param name="ProcessId">
        ''' A pointer to a variable that receives the process identifier. 
        ''' If this parameter is not NULL, GetWindowThreadProcessId copies the identifier of the process to the variable; 
        ''' otherwise, it does not.
        ''' </param>
        ''' <returns>The identifier of the thread that created the window.</returns>
        <DllImport("user32.dll")>
        Friend Shared Function GetWindowThreadProcessId(
            ByVal hWnd As IntPtr,
            ByRef ProcessId As Integer
        ) As Integer
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowLong")> _
        Friend Shared Function SetWindowLong32(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlags, ByVal dwNewLong As Integer) As Integer
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowLongPtr")> _
        Friend Shared Function SetWindowLongPtr64(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlags, ByVal dwNewLong As IntPtr) As IntPtr
        End Function

        Friend Shared Function SetWindowLongPtr(ByVal hWnd As IntPtr, nIndex As WindowLongFlags, ByVal dwNewLong As IntPtr) As IntPtr
            If IntPtr.Size = 8 Then
                Return SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
            Else
                Return New IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32))
            End If
        End Function

#End Region

    End Class

#End Region

#Region " Enumerations "

    Public Enum WindowLongFlags As Integer
        GWL_EXSTYLE = -20
        GWLP_HINSTANCE = -6
        GWLP_HWNDPARENT = -8
        GWL_ID = -12
        GWL_STYLE = -16
        GWL_USERDATA = -21
        GWL_WNDPROC = -4
        DWLP_USER = &H8
        DWLP_MSGRESULT = &H0
        DWLP_DLGPROC = &H4
    End Enum

    <FlagsAttribute()> _
    Public Enum WindowStyles As Long

        Todo1 = 2
        Todo2 = 2048
        Todo3 = 32768

        WS_OVERLAPPED = 0
        WS_POPUP = 2147483648
        WS_CHILD = 1073741824
        WS_MINIMIZE = 536870912
        WS_VISIBLE = 268435456
        WS_DISABLED = 134217728
        WS_CLIPSIBLINGS = 67108864
        WS_CLIPCHILDREN = 33554432
        WS_MAXIMIZE = 16777216
        WS_BORDER = 8388608
        WS_DLGFRAME = 4194304
        WS_VSCROLL = 2097152
        WS_HSCROLL = 1048576
        WS_SYSMENU = 524288
        WS_THICKFRAME = 262144
        WS_GROUP = 131072
        WS_TABSTOP = 65536

        WS_MINIMIZEBOX = 131072
        WS_MAXIMIZEBOX = 65536

        WS_CAPTION = WS_BORDER Or WS_DLGFRAME
        WS_TILED = WS_OVERLAPPED
        WS_ICONIC = WS_MINIMIZE
        WS_SIZEBOX = WS_THICKFRAME
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW

        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or _
                  WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
        WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU
        WS_CHILDWINDOW = WS_CHILD

        WS_EX_DLGMODALFRAME = 1
        WS_EX_NOPARENTNOTIFY = 4
        WS_EX_TOPMOST = 8
        WS_EX_ACCEPTFILES = 16
        WS_EX_TRANSPARENT = 32

        '#If (WINVER >= 400) Then
        WS_EX_MDICHILD = 64
        WS_EX_TOOLWINDOW = 128
        WS_EX_WINDOWEDGE = 256
        WS_EX_CLIENTEDGE = 512
        WS_EX_CONTEXTHELP = 1024

        WS_EX_RIGHT = 4096
        WS_EX_LEFT = 0
        WS_EX_RTLREADING = 8192
        WS_EX_LTRREADING = 0
        WS_EX_LEFTSCROLLBAR = 16384
        WS_EX_RIGHTSCROLLBAR = 0

        WS_EX_CONTROLPARENT = 65536
        WS_EX_STATICEDGE = 131072
        WS_EX_APPWINDOW = 262144

        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE Or WS_EX_CLIENTEDGE
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST
        '#End If

        '#If (WIN32WINNT >= 500) Then
        WS_EX_LAYERED = 524288
        '#End If

        '#If (WINVER >= 500) Then
        WS_EX_NOINHERITLAYOUT = 1048576 ' Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 4194304 ' Right to left mirroring
        '#End If

        '#If (WIN32WINNT >= 500) Then
        WS_EX_COMPOSITED = 33554432
        WS_EX_NOACTIVATE = 67108864
        '#End If

    End Enum

#End Region

#Region " Public Methods "

    ''' <summary>
    ''' Set the state of a window by an HWND.
    ''' </summary>
    ''' <param name="WindowHandle">A handle to the window.</param>
    ''' <param name="WindowStyle">The Style of the window.</param>
    ''' <returns><c>true</c> if the function succeeds, <c>false</c> otherwise.</returns>
    Public Shared Function SetWindowStyle(ByVal WindowHandle As IntPtr,
                                          ByVal WindowStyle As WindowStyles) As Boolean

        Return NativeMethods.SetWindowLongPtr(WindowHandle, WindowLongFlags.GWL_STYLE, WindowStyle)

    End Function

    ''' <summary>
    ''' Set the state of a window by a process name.
    ''' </summary>
    ''' <param name="ProcessName">The name of the process.</param>
    ''' <param name="WindowStyle">The Style of the window.</param>
    ''' <param name="Recursivity">If set to <c>false</c>, only the first process instance will be processed.</param>
    Public Shared Sub SetWindowStyle(ByVal ProcessName As String,
                                     ByVal WindowStyle As WindowStyles,
                                     Optional ByVal Recursivity As Boolean = False)

        If ProcessName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) Then
            ProcessName = ProcessName.Remove(ProcessName.Length - ".exe".Length)
        End If

        Dim pHandle As IntPtr = IntPtr.Zero
        Dim pID As Integer = 0I

        Dim Processes As Process() = Process.GetProcessesByName(ProcessName)

        ' If any process matching the name is found then...
        If Processes.Count = 0 Then
            Exit Sub
        End If

        For Each p As Process In Processes

            ' If Window is visible then...
            If p.MainWindowHandle <> IntPtr.Zero Then
                SetWindowStyle(p.MainWindowHandle, WindowStyle)

            Else ' Window is hidden

                ' Check all open windows (not only the process we are looking),
                ' begining from the child of the desktop, phandle = IntPtr.Zero initialy.
                While pID <> p.Id ' Check all windows.

                    ' Get child handle of window who's handle is "pHandle".
                    pHandle = NativeMethods.FindWindowEx(IntPtr.Zero, pHandle, Nothing, Nothing)

                    ' Get ProcessId from "pHandle".
                    NativeMethods.GetWindowThreadProcessId(pHandle, pID)

                    ' If the ProcessId matches the "pID" then...
                    If pID = p.Id Then

                        NativeMethods.SetWindowLongPtr(pHandle, WindowLongFlags.GWL_STYLE, WindowStyle)

                        If Not Recursivity Then
                            Exit For
                        End If

                    End If

                End While

            End If

        Next p

    End Sub

#End Region

End Class
