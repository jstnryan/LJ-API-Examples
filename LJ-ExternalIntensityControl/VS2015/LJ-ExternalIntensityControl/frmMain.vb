''This import allows us to use DllImport
Imports System.Runtime.InteropServices

''' <summary>
''' Demonstrates how to set LightJockey's Intensity Master levels from an external application.
''' </summary>
''' <remarks>
''' Uses WM_COPYDATA Windows Messages to aggregate an send external control information to LJ.
''' Note:
''' * This works in LightJockey version 2.2 build 2 and above.
''' * Remote setting can be overriden by direct user interaction within LJ.
''' * Function does not work on intensity masters which are patched to DMX-In.
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
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    ''All non-standard messages (custom to LJ) start above WM_USER (https://msdn.microsoft.com/en-us/library/windows/desktop/ms644931.aspx)
    Public Const WM_USER As Int32 = &H400 ''1024
    ''The message sent to LJ to ask if it is ready to receive more messages
    Public Const _R_LJReady As Int32 = WM_USER + 1502
    ''The standard Windows Message used to transfer data (https://msdn.microsoft.com/en-us/library/windows/desktop/ms649011.aspx)
    Public Const WM_COPYDATA As Integer = &H4A ''74
    ''CopyData value to tell lightjockey message contains Master Intensity data
    Public Const _WMCOPY_SetIntensityMasters As Integer = 266
    ''The structure that is expected when sending a WM_COPYDATA message (https://msdn.microsoft.com/en-us/library/windows/desktop/ms649010.aspx)
    Private Structure CopyDataStruct
        Public dwData As IntPtr ''This can be Integer on 32-bit systems, but must be IntPtr for 64-bit
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure
    Private CopyData As CopyDataStruct
    ''The structure sent inside a CopyDataStruct that contains information LJ can use
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Private Structure IntensitySettingsStruct
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=9)>
        Public Flags As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=9)>
        Public Values As Byte()

        Sub New(Optional ByVal initVal As Byte = 255)
            ReDim Flags(8)
            ReDim Values(8)
            For i As Integer = 0 To 8
                Flags(i) = 1
                Values(i) = initVal
            Next
        End Sub
    End Structure
    Private IntensitySettings As New IntensitySettingsStruct

    ''We'll store the window handle to LJ here
    Public LJWindowHandle As IntPtr
    ''This will hold a pointer to the memory location of CopyData, which holds our IntensitySettings data
    Private ptrCopyData As IntPtr

    ''Since most of the controls on the form do the same thing (but affect different channels),
    '' its easiest to simply create a collection of controls and target them by index.
    Private tbCollection As Windows.Forms.TrackBar()
    Private lblCollection As Windows.Forms.Label()
    Private chkCollection As Windows.Forms.CheckBox()

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        LJWindowHandle = DetectLightJockey()
        If LJWindowHandle <> IntPtr.Zero Then
            lblLJReady.Text = "LJ Detected"
            If DetectLJReadyState() Then
                lblLJReady.Text = "LJ Ready"
            Else
                lblLJReady.Text = "LJ Not Ready"
            End If
        End If

        ''Populate control collections (see declarations for explanation)
        tbCollection = New Windows.Forms.TrackBar() {tb0, tb1, tb2, tb3, tb4, tb5, tb6, tb7, tb8}
        lblCollection = New Windows.Forms.Label() {lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8}
        chkCollection = New Windows.Forms.CheckBox() {chk0, chk1, chk2, chk3, chk4, chk5, chk6, chk7, chk8}

        ''Loop through all controls to make sure their settings match what is stored in IntensitySettings
        For which As Integer = 0 To tbCollection.Length - 1
            IntensitySettings.Values(which) = CByte(tbCollection(which).Value)
            lblCollection(which).Text = tbCollection(which).Value.ToString
            IntensitySettings.Flags(which) = CByte(If(chkCollection(which).Checked, 1, 0))
        Next

        ''Prepare static data in CopyDataStruct
        CopyData.dwData = CType(_WMCOPY_SetIntensityMasters, IntPtr)
        CopyData.cbData = Marshal.SizeOf(IntensitySettings)
        CopyData.lpData = Marshal.AllocHGlobal(Marshal.SizeOf(IntensitySettings))
        Marshal.StructureToPtr(IntensitySettings, CopyData.lpData, False)

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

    ''' <summary>
    ''' Scroll event fired when changing any of the master faders
    ''' </summary>
    ''' <remarks>Handles Scroll events for all TrackBars</remarks>
    Private Sub tb_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tb0.Scroll, tb1.Scroll, tb2.Scroll, tb3.Scroll, tb4.Scroll, tb5.Scroll, tb6.Scroll, tb7.Scroll, tb8.Scroll
        ''Capture the sender object, and assign a reference to 'fader'
        Dim fader As Windows.Forms.TrackBar = DirectCast(sender, Windows.Forms.TrackBar)
        ''Use the Tag property to determine which channel it controls
        Dim which As Byte = CByte(fader.Tag)
        ''Update intensity data with new setting
        IntensitySettings.Values(which) = CByte(fader.Value)
        ''Send new data to LightJockey
        SendIntensityData()
        ''Update the value label
        lblCollection(which).Text = fader.Value.ToString
    End Sub

    ''' <summary>
    ''' CheckChanged event fired when changing the check state of any of the enable/disable CheckBoxes
    ''' </summary>
    ''' <remarks>Handles CheckChanged events for all CheckBoxes</remarks>
    Private Sub chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk0.CheckedChanged, chk1.CheckedChanged, chk2.CheckedChanged, chk3.CheckedChanged, chk4.CheckedChanged, chk5.CheckedChanged, chk6.CheckedChanged, chk7.CheckedChanged, chk8.CheckedChanged
        Dim check As Windows.Forms.CheckBox = DirectCast(sender, Windows.Forms.CheckBox)
        Dim which As Byte = CByte(check.Tag)
        ''Update intensity data with new setting
        IntensitySettings.Flags(which) = CByte(If(check.Checked, 1, 0))
    End Sub

    ''' <summary>
    ''' Sends COPYDATA to LightJockey
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendIntensityData()
        If LJWindowHandle <> IntPtr.Zero Then
            Marshal.StructureToPtr(IntensitySettings, CopyData.lpData, True)
            Marshal.StructureToPtr(CopyData, ptrCopyData, True)
            Dim retVal As IntPtr = SendMessage(LJWindowHandle, WM_COPYDATA, Me.Handle, ptrCopyData)
            If CInt(retVal) <> 1 Then
                ''Something went wrong
                Debug.WriteLine("SendMessage returned a non-One value: " & retVal.ToString)
            End If
        End If
    End Sub
End Class
