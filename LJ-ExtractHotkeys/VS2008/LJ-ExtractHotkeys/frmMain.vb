Imports System.Runtime.InteropServices

Public Class frmMain
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    Public Const WM_USER As Int32 = &H400 ''1024
    Private Const _R_ExternalConfiguration = WM_USER + 1600
    Private Const _R_LJReady = WM_USER + 1502
    Private Const _R_ExecuteFunctionOn = WM_USER + 1006
    Private Const _R_ExecuteFunctionOff = WM_USER + 1007
    Public Const WM_COPYDATA As Integer = &H4A ''74
    Private Const _WMCOPY_FunctionsList = 257
    Private Const _R_RequestFunctionList = 261
    Private Structure CopyData
        Public dwData As IntPtr ''Integer works on 32-bit, but must be IntPtr for compatibility with 64-bit
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure
    Private Structure LJDataPair
        Dim DataCode As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> Dim DataDesc As String
    End Structure
    Dim FunctionArray() As LJDataPair
    Dim LJWindowHandle As Integer

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        LJWindowHandle = FindWindow("TLJMainForm", vbNullString)
        If 1 <> CInt(SendMessage(LJWindowHandle, _R_LJReady, 0, 0)) Then
            LJWindowHandle = 0
        End If

        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_COPYDATA And m.HWnd.Equals(Me.Handle) Then
            Dim NewData As CopyData = Marshal.PtrToStructure(m.LParam, GetType(CopyData))
            If NewData.dwData = _WMCOPY_FunctionsList Then
                ''The format of this message is alternating Int32/String pairs
                ''Int32 (4 bytes), String (35 bytes)
                ''First byte of string is actual string length

                'Get number of Integer/String records
                Dim NumberOfPairs As Integer = NewData.cbData / 40

                'Extract data and populate array
                Dim currOffset As Integer = 0 ''the byte offset to start reading the data
                For i = 0 To (NumberOfPairs - 1)
                    ReDim Preserve FunctionArray(i) ''Add another record to array
                    FunctionArray(i).DataCode = Marshal.ReadInt32(NewData.lpData, currOffset) ''Read and set the pair's code
                    Dim strLength As Byte = Marshal.ReadByte(NewData.lpData, currOffset + 4) ''Figure out how long the string is using byte at (String[0])
                    For l = 1 To strLength
                        ''assemble the string by reading the appropriate number of bytes
                        FunctionArray(i).DataDesc &= Convert.ChangeType(Marshal.ReadByte(NewData.lpData, currOffset + 4 + l), TypeCode.Char)
                    Next
                    currOffset += 40
                Next

                ''We now have an Array of Structures (Int/Str pairs) for all external commands LJ sent us
                ''We'll use it to populate our DataGrid
                dgvFunctions.Rows.Clear()

                For i = 0 To (FunctionArray.Count - 1)
                    dgvFunctions.Rows.Add(1)
                    If FunctionArray(i).DataCode > 65536 Then ''Line is not a command, is a separator
                        dgvFunctions.Rows(i).Cells(0).Value = "GROUP"
                        dgvFunctions.Rows(i).Cells(1).Value = UCase(FunctionArray(i).DataDesc)
                    Else
                        dgvFunctions.Rows(i).Cells(0).Value = FunctionArray(i).DataCode
                        dgvFunctions.Rows(i).Cells(1).Value = FunctionArray(i).DataDesc
                    End If
                Next
            End If

            m.Msg = vbNull ''Clear message to release processing back to LJ
        Else
            MyBase.WndProc(m)
        End If
    End Sub

    Private Sub cmdGetList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetList.Click
        SendMessage(LJWindowHandle, _R_ExternalConfiguration, _R_RequestFunctionList, Me.Handle)
    End Sub

    Private Sub cmdDoFunc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdDoFunc.MouseDown
        Dim whichRow As Int16 = dgvFunctions.SelectedRows.Item(0).Index ''Get selected row
        Dim whichCode As Byte = FunctionArray(whichRow).DataCode ''Translate to slected Code
        If Not whichCode >= 65536 Then ''If valid code, NOT separator
            SendMessage(LJWindowHandle, _R_ExecuteFunctionOn, whichCode, 0)
        End If
    End Sub

    Private Sub cmdDoFunc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdDoFunc.MouseUp
        Dim whichRow As Int16 = dgvFunctions.SelectedRows.Item(0).Index ''Get selected row
        Dim whichCode As Byte = FunctionArray(whichRow).DataCode ''Translate to slected Code
        If Not whichCode >= 65536 Then ''If valid code, NOT separator
            SendMessage(LJWindowHandle, _R_ExecuteFunctionOff, whichCode, 0)
        End If
    End Sub
End Class
