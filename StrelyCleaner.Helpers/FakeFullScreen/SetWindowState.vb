' ***********************************************************************
' Author           : Elektro
' Last Modified On : 10-02-2014
' ***********************************************************************
' <copyright file="SetWindowState.vb" company="Elektro Studios">
'     Copyright (c) Elektro Studios. All rights reserved.
' </copyright>
' ***********************************************************************

#Region " Usage Examples "

'Dim HWND As IntPtr = Process.GetProcessesByName("devenv").First.MainWindowHandle
'
'SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Hide)
'SetWindowState.SetWindowState("devenv", SetWindowState.WindowState.Restore, Recursivity:=False)

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

''' <summary>
''' Sets the state of a window.
''' </summary>
Public NotInheritable Class SetWindowState

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

        ''' <summary>
        ''' Sets the specified window's show state.
        ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/windows/desktop/ms633548%28v=vs.85%29.aspx
        ''' </summary>
        ''' <param name="hwnd">A handle to the window.</param>
        ''' <param name="nCmdShow">Controls how the window is to be shown.</param>
        ''' <returns><c>true</c> if the function succeeds, <c>false</c> otherwise.</returns>
        <DllImport("User32", SetLastError:=False)>
        Friend Shared Function ShowWindow(
           ByVal hwnd As IntPtr,
           ByVal nCmdShow As WindowState
        ) As Boolean
        End Function

#End Region

    End Class

#End Region

#Region " Enumerations "

    ''' <summary>
    ''' Controls how the window is to be shown.
    ''' MSDN Documentation: http://msdn.microsoft.com/en-us/library/windows/desktop/ms633548%28v=vs.85%29.aspx
    ''' </summary>
    Public Enum WindowState As Integer

        ''' <summary>
        ''' Hides the window and activates another window.
        ''' </summary>
        Hide = 0I

        ''' <summary>
        ''' Activates and displays a window. 
        ''' If the window is minimized or maximized, the system restores it to its original size and position.
        ''' An application should specify this flag when displaying the window for the first time.
        ''' </summary>
        Normal = 1I

        ''' <summary>
        ''' Activates the window and displays it as a minimized window.
        ''' </summary>
        ShowMinimized = 2I

        ''' <summary>
        ''' Maximizes the specified window.
        ''' </summary>
        Maximize = 3I

        ''' <summary>
        ''' Activates the window and displays it as a maximized window.
        ''' </summary>      
        ShowMaximized = Maximize

        ''' <summary>
        ''' Displays a window in its most recent size and position. 
        ''' This value is similar to <see cref="WindowState.Normal"/>, except the window is not actived.
        ''' </summary>
        ShowNoActivate = 4I

        ''' <summary>
        ''' Activates the window and displays it in its current size and position.
        ''' </summary>
        Show = 5I

        ''' <summary>
        ''' Minimizes the specified window and activates the next top-level window in the Z order.
        ''' </summary>
        Minimize = 6I

        ''' <summary>
        ''' Displays the window as a minimized window. 
        ''' This value is similar to <see cref="WindowState.ShowMinimized"/>, except the window is not activated.
        ''' </summary>
        ShowMinNoActive = 7I

        ''' <summary>
        ''' Displays the window in its current size and position.
        ''' This value is similar to <see cref="WindowState.Show"/>, except the window is not activated.
        ''' </summary>
        ShowNA = 8I

        ''' <summary>
        ''' Activates and displays the window. 
        ''' If the window is minimized or maximized, the system restores it to its original size and position.
        ''' An application should specify this flag when restoring a minimized window.
        ''' </summary>
        Restore = 9I

        ''' <summary>
        ''' Sets the show state based on the SW_* value specified in the STARTUPINFO structure 
        ''' passed to the CreateProcess function by the program that started the application.
        ''' </summary>
        ShowDefault = 10I

        ''' <summary>
        ''' <b>Windows 2000/XP:</b> 
        ''' Minimizes a window, even if the thread that owns the window is not responding. 
        ''' This flag should only be used when minimizing windows from a different thread.
        ''' </summary>
        ForceMinimize = 11I

    End Enum

#End Region

#Region " Public Methods "

    ''' <summary>
    ''' Set the state of a window by an HWND.
    ''' </summary>
    ''' <param name="WindowHandle">A handle to the window.</param>
    ''' <param name="WindowState">The state of the window.</param>
    ''' <returns><c>true</c> if the function succeeds, <c>false</c> otherwise.</returns>
    Public Shared Function SetWindowState(ByVal WindowHandle As IntPtr,
                                          ByVal WindowState As WindowState) As Boolean

        Return NativeMethods.ShowWindow(WindowHandle, WindowState)

    End Function

    ''' <summary>
    ''' Set the state of a window by a process name.
    ''' </summary>
    ''' <param name="ProcessName">The name of the process.</param>
    ''' <param name="WindowState">The state of the window.</param>
    ''' <param name="Recursivity">If set to <c>false</c>, only the first process instance will be processed.</param>
    Public Shared Sub SetWindowState(ByVal ProcessName As String,
                                     ByVal WindowState As WindowState,
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
                SetWindowState(p.MainWindowHandle, WindowState)

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

                        NativeMethods.ShowWindow(pHandle, WindowState)

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