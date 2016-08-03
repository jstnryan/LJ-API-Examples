''This import allows us to use DllImport
Imports System.Runtime.InteropServices

''' <summary>
''' Demonstrates how to send LightJockey DMX values from an external application.
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
    ''CopyData value to tell lightjockey message contains DMX Input data
    Public Const _WMCOPY_DmxIn As Integer = 256
    ''The structure that is expected when sending a WM_COPYDATA message (https://msdn.microsoft.com/en-us/library/windows/desktop/ms649010.aspx)
    Private Structure CopyDataStruct
        Public dwData As IntPtr
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure
    Private CopyData As CopyDataStruct

    ''' <summary>
    ''' The structure sent inside a CopyDataStruct that contains information LJ can use
    ''' </summary>
    ''' <remarks>All versions of LightJockey (current to v.2.100.2 - "One Key") contain a bug
    ''' which causes a memory read fault when more than 511 channels of DMX-In are sent. All 512
    ''' channels will be properly updated, but an error message is presented to the user for every
    ''' SendMessage event, and execution is blocked until the message is dismissed.</remarks>
    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Private Structure DMXDataStruct
        ''Between 1 and 512
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=12)> _
        Public Values() As Byte

        Public Sub New(ByVal initVal As Byte)
            ReDim Values(11) ''Between 0 and 511 (one less than above)

            For f As Integer = 0 To 11
                Values(f) = initVal
            Next
        End Sub
    End Structure
    Private DmxData As DMXDataStruct = New DMXDataStruct(0)

    ''We'll store the window handle to LJ here
    Public LJWindowHandle As IntPtr
    ''This will hold a pointer to the memory location of CopyData, which holds our IntensitySettings data
    Private ptrCopyData As IntPtr

    ''Since most of the controls on the form do the same thing (but affect different channels),
    '' its easiest to simply create a collection of controls and target them by index.
    Private tbCollection As Windows.Forms.TrackBar()
    Private lblCollection As Windows.Forms.Label()
    Private cmdCollection As Windows.Forms.Button()

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        ''First, find a window handle for LJ.
        LJWindowHandle = FindWindow("TLJMainForm", vbNullString)
        ''Then, ask LJ if it is ready to receive messages.
        If 1 <> CInt(SendMessage(LJWindowHandle, _R_LJReady, IntPtr.Zero, IntPtr.Zero)) Then
            LJWindowHandle = IntPtr.Zero
            ''If this block is hit, LJ is not ready; program will not function.
        End If

        ''Populate control collections (see declarations for explanation)
        tbCollection = New Windows.Forms.TrackBar() {tb1, tb2, tb3, tb4, tb5, tb6, tb7, tb8, tb9, tb10, tb11, tb12}
        lblCollection = New Windows.Forms.Label() {lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8, lbl9, lbl10, lbl11, lbl12}
        cmdCollection = New Windows.Forms.Button() {cmd1, cmd2, cmd3, cmd4, cmd5, cmd6, cmd7, cmd8, cmd9, cmd10, cmd11, cmd12}

        ''Loop through all controls to make sure their settings match what is stored in DmxData
        For which As Integer = 0 To tbCollection.Length - 1
            tbCollection(which).Value = DmxData.Values(which)
            lblCollection(which).Text = Pad(DmxData.Values(which).ToString)
            lblCollection(which).Tag = DmxData.Values(which)
        Next

        ''Prepare static data in CopyDataStruct
        'CopyData.dwData = _WMCOPY_DmxIn
        CopyData.dwData = CType(256, IntPtr)
        CopyData.cbData = Marshal.SizeOf(DmxData)
        CopyData.lpData = Marshal.AllocHGlobal(Marshal.SizeOf(DmxData))
        Marshal.StructureToPtr(DmxData, CopyData.lpData, False)

        ''Prepare pointer to CopyData
        ptrCopyData = Marshal.AllocHGlobal(Marshal.SizeOf(CopyData))
        Marshal.StructureToPtr(CopyData, ptrCopyData, False)

        MyBase.OnLoad(e)
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Marshal.FreeHGlobal(CopyData.lpData)
        Marshal.FreeHGlobal(ptrCopyData)
    End Sub

    ''' <summary>
    ''' Scroll event fired when changing any of the master faders
    ''' </summary>
    ''' <remarks>Handles Scroll events for all TrackBars</remarks>
    Private Sub tb_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tb1.Scroll, tb2.Scroll, tb3.Scroll, tb4.Scroll, tb5.Scroll, tb6.Scroll, tb7.Scroll, tb8.Scroll, tb9.Scroll, tb10.Scroll, tb11.Scroll, tb12.Scroll
        ''Capture the sender object, and assign a reference to 'fader'
        Dim fader As Windows.Forms.TrackBar = DirectCast(sender, Windows.Forms.TrackBar)
        ''Use the Tag property to determine which channel it controls
        Dim which As Byte = CByte(fader.Tag)
        ''Update intensity data with new setting
        DmxData.Values(which) = CByte(fader.Value)
        ''Send new data to LightJockey
        SendDmxData()
        ''Update the value label
        lblCollection(which).Text = Pad(fader.Value.ToString)
    End Sub

    Private Sub cmd_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmd1.MouseDown, cmd2.MouseDown, cmd3.MouseDown, cmd4.MouseDown, cmd5.MouseDown, cmd6.MouseDown, cmd7.MouseDown, cmd8.MouseDown, cmd9.MouseDown, cmd10.MouseDown, cmd11.MouseDown, cmd12.MouseDown
        Dim bump As Windows.Forms.Button = DirectCast(sender, Windows.Forms.Button)
        Dim which As Byte = CByte(bump.Tag)
        lblCollection(which).Tag = DmxData.Values(which)
        DmxData.Values(which) = 255
        lblCollection(which).Text = "255"
        tbCollection(which).Value = 255
        SendDmxData()
    End Sub

    Private Sub cmd_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmd1.MouseUp, cmd2.MouseUp, cmd3.MouseUp, cmd4.MouseUp, cmd5.MouseUp, cmd6.MouseUp, cmd7.MouseUp, cmd8.MouseUp, cmd9.MouseUp, cmd10.MouseUp, cmd11.MouseUp, cmd12.MouseUp
        Dim bump As Windows.Forms.Button = DirectCast(sender, Windows.Forms.Button)
        Dim which As Byte = CByte(bump.Tag)
        DmxData.Values(which) = CByte(lblCollection(which).Tag)
        lblCollection(which).Text = Pad(lblCollection(which).Tag.ToString)
        tbCollection(which).Value = CInt(lblCollection(which).Tag)
        SendDmxData()
    End Sub

    ''' <summary>
    ''' Sends COPYDATA to LightJockey
    ''' </summary>
    Private Sub SendDmxData()
        If LJWindowHandle <> IntPtr.Zero Then
            Marshal.StructureToPtr(DmxData, CopyData.lpData, True)
            Marshal.StructureToPtr(CopyData, ptrCopyData, True)
            Dim retVal As IntPtr = SendMessage(LJWindowHandle, WM_COPYDATA, Me.Handle, ptrCopyData)
            ''LJ always seems to return UInteger.MaxValue (4294967295, or -1 if you try to .ToString) no matter what
        End If
    End Sub

    ''' <summary>Zero-pads fader value for display in label</summary>
    Private Function Pad(ByVal val As String) As String
        Return val.ToString.PadLeft(3, "0"c)
    End Function
End Class
