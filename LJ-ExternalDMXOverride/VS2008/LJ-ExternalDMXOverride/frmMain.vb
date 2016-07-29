''This import allows us to use DllImport
Imports System.Runtime.InteropServices

''' <summary>
''' Demonstrates how to override LightJockey's DMX output values from an external application.
''' </summary>
''' <remarks>
''' Uses WM_COPYDATA Windows Messages to aggregate an send DMX values and channel override flags to LJ.
''' </remarks>
Public Class frmMain
    ''' <summary>
    ''' Retrieves a handle to the top-level window whose class name and window name match the specified strings.
    ''' </summary>
    ''' <param name="lpClassName">The class name or a class atom (optional). If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.</param>
    ''' <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match.</param>
    ''' <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name. If the function fails, the return value is NULL.</returns>
    ''' <remarks>This will be used to ask Windows where we can find LJ.</remarks>
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    ''' <summary>
    ''' P/Invoke function for sending Windows Messages
    ''' </summary>
    ''' <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    ''' <param name="Msg">The message to be sent.</param>
    ''' <param name="wParam">Additional message-specific information</param>
    ''' <param name="lParam">Additional message-specific information</param>
    ''' <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    ''' <remarks>This is the function used to send data to LJ.</remarks>
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    ''All non-standard messages (custom to LJ) start above WM_USER (https://msdn.microsoft.com/en-us/library/windows/desktop/ms644931.aspx)
    Public Const WM_USER As Int32 = &H400 ''1024
    ''The message sent to LJ to ask if it is ready to receive more messages
    Public Const _R_LJReady As Int32 = WM_USER + 1502
    ''The standard Windows Message used to transfer data (https://msdn.microsoft.com/en-us/library/windows/desktop/ms649011.aspx)
    Public Const WM_COPYDATA As Integer = &H4A ''74
    ''CopyData value to tell lightjockey message contains DMX Override data
    Public Const _WMCOPY_DMXOverride As Integer = 267
    ''The structure that is expected when sending a WM_COPYDATA message (https://msdn.microsoft.com/en-us/library/windows/desktop/ms649010.aspx)
    Private Structure CopyDataStruct
        Public dwData As IntPtr ''This can be Integer on 32-bit systems, but must be IntPtr for 64-bit
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure
    Private CopyData As CopyDataStruct

    ''The structure sent inside a CopyDataStruct that contains information LJ can use
    <StructLayout(LayoutKind.Sequential, Pack:=0)> _
    Private Structure DMXDataStruct
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16, ArraySubType:=UnmanagedType.U4)> _
        Public Reserved() As Integer
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2048)> _
        Public ChFlags() As Byte ''0:ignore, 1:override
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2048)> _
        Public Values() As Byte

        Public Sub New(ByVal initVal As Byte)
            ReDim Reserved(15)
            ReDim ChFlags(2047)
            ReDim Values(2047)

            For f As Integer = 0 To 2047
                ChFlags(f) = 1
                Values(f) = initVal
            Next
        End Sub
    End Structure
    Private _dmxData As DMXDataStruct = New DMXDataStruct(0)

    ''We'll store the window handle to LJ here
    Public LJWindowHandle As IntPtr
    ''This will hold a pointer to the memory location of CopyData, which holds our IntensitySettings data
    Private ptrCopyData As IntPtr

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Timer1.Interval = 500 'Every half second

        LJWindowHandle = DetectLightJockey()
        If LJWindowHandle <> IntPtr.Zero Then
            lblLJReady.Text = "LJ Detected"
            If DetectLJReadyState() Then
                lblLJReady.Text = "LJ Ready"
                Timer1.Enabled = True
            Else
                lblLJReady.Text = "LJ Not Ready"
            End If
        End If

        ''Prepare static data in CopyDataStruct
        CopyData.dwData = CType(_WMCOPY_DMXOverride, IntPtr)
        CopyData.cbData = Marshal.SizeOf(_dmxData)
        CopyData.lpData = Marshal.AllocHGlobal(Marshal.SizeOf(_dmxData))
        Marshal.StructureToPtr(_dmxData, CopyData.lpData, False)

        ''Prepare pointer to CopyData
        ptrCopyData = Marshal.AllocHGlobal(Marshal.SizeOf(CopyData))
        Marshal.StructureToPtr(CopyData, ptrCopyData, False)

        MyBase.OnLoad(e)
    End Sub

    ''' <summary>
    ''' Ask Windows to return a window handle for LightJockey
    ''' </summary>
    ''' <returns>IntPtr to LJ, or NULL if not found</returns>
    Public Function DetectLightJockey() As IntPtr
        ''Class name "TLJMainForm", window name "LightJockey"
        Return FindWindow("TLJMainForm", vbNullString)
    End Function

    ''' <summary>
    ''' Ask LightJockey whether it is ready to receive messages
    ''' </summary>
    ''' <returns>Returns 1 if ready, otherwise 0</returns>
    Public Function DetectLJReadyState() As Boolean
        If CInt(SendMessage(LJWindowHandle, _R_LJReady, IntPtr.Zero, IntPtr.Zero)) = 1 Then Return True
        Return False
    End Function

    Public Sub SendDMXValues()
        Marshal.StructureToPtr(_dmxData, CopyData.lpData, True)
        Marshal.StructureToPtr(CopyData, ptrCopyData, True)

        Try
            Dim res As IntPtr = SendMessage(LJWindowHandle, WM_COPYDATA, Me.Handle, ptrCopyData)
            If CInt(res) = 1 Then
                ''Success
            Else
                ''Error
                Debug.WriteLine("WM_COPYDATA returned a non-One value: " & res.ToString)
            End If
        Catch ex As Exception
            Debug.WriteLine("There was a fatal error sending to LightJockey.")
            Timer1.Enabled = False
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        SendDMXValues()
    End Sub

    Private Sub rdo1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdo1.CheckedChanged
        If rdo1.Checked Then
            ''All channels at 0
            For f As Integer = 0 To 2047
                _dmxData.ChFlags(f) = 1
                _dmxData.Values(f) = 0
            Next
        End If
    End Sub

    Private Sub rdo2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdo2.CheckedChanged
        ''All channels at 255
        For f As Integer = 0 To 2047
            _dmxData.ChFlags(f) = 1
            _dmxData.Values(f) = 255
        Next
    End Sub

    Private Sub rdo3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdo3.CheckedChanged
        ''Odd channels at 128
        For f As Integer = 0 To 2047
            If (f + 1) Mod 2 = 0 Then
                ''Even
                _dmxData.ChFlags(f) = 0
                _dmxData.Values(f) = 0
            Else
                ''Odd
                _dmxData.ChFlags(f) = 1
                _dmxData.Values(f) = 128
            End If
        Next
    End Sub

    Private Sub rdo4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdo4.CheckedChanged
        Dim rnd As New Random()
        ''Even channels at random
        For f As Integer = 0 To 2047
            If (f + 1) Mod 2 = 0 Then
                ''Even
                _dmxData.ChFlags(f) = 1
                _dmxData.Values(f) = rnd.Next(0, 256)
            Else
                ''Odd
                _dmxData.ChFlags(f) = 0
                _dmxData.Values(f) = 0
            End If
        Next
    End Sub
End Class
