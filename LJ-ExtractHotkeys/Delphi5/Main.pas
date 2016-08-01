unit Main;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, ComCtrls;


Const
   UM_GETFUNC                  = WM_USER+110;  // Local app Message


  _R_ExternalConfiguration     = WM_USER+1600; // Message known to LJ

  _R_LJReady                   = WM_USER+1502;

  _R_ExecuteFunctionOn         = WM_USER+1006; // Execute Function on
  _R_ExecuteFunctionOff        = WM_USER+1007; // Execute Function off

  _WMCOPY_FunctionsList        = 257; // LJ Functions List Export
  _R_RequestFunctionList       = 261; // Return Handle in LParam


type

  TLJFunctionItem     = Packed Record
    ItemCode          : DWord;      // 0-$FFFF : Function Code, $10000 : Seperator, $30000 Bold Seperator
    Caption           : String[35]; // Note this is a 'Pascal' string, not a 0-terminated one
                                    // Caption[0] contains the actual length of the string
                                    // to typecast to 0-terminated string in non-delphi do the following (pseudo code)
                                    // buffer : char[36];     // create buffer, 36 chars
                                    // fillmemory(char,0,36); // initialize with 0
                                    // move(caption[1],buffer,caption[0]); // move Caption[1] to Caption[length] to buffer
  End;
  PLJFunctionItem     = ^TLJFunctionItem;


  TForm1 = class(TForm)
    Panel1: TPanel;
    bGetList: TButton;
    lv1: TListView;
    bDoFunction: TButton;
    procedure bGetListClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure bDoFunctionMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure bDoFunctionMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure lv1Change(Sender: TObject; Item: TListItem; Change: TItemChange);
  private
    FunctionsBuffer      : Pointer;
    FunctionsBufferSize  : Integer;
    FunctionsList        : TList;

    Procedure              CheckCopyData(Var Msg : TMessage); Message WM_COPYDATA;
    Procedure              GetFunctions(Var Msg : TMessage);  Message UM_GETFUNC;
    Procedure              ShowFunctions(Var FList : TList);
    Function               GetLJHandle : THandle;

  public
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}

procedure TForm1.FormCreate(Sender: TObject);
begin
  FunctionsBuffer     := NIL;
  FunctionsBufferSize := 0;
  FunctionsList := TList.Create;
end;

Function TForm1.GetLJhandle : THandle;
// Function returns handle to LJ if the main LJ window is found and LJ returns ready
// Otherwise the function returns 0 (function returns value of result) 

Begin
  Result := FindWindow('TLJMainForm',NIL); // Get LJ Handle - returns 0 if LJ is not found
  If (Result > 0) And (SendMessage(Result,_R_LJReady,0,0) <> 1) Then Result := 0; // check if LJ returns ready
End;

procedure TForm1.bGetListClick(Sender: TObject);
Var
  LJHandle : THandle;
begin
  LJHandle := GetLJHandle;
  If LJHandle > 0 Then SendMessage(LJHandle,_R_ExternalConfiguration,_R_RequestFunctionList,Self.Handle); // Request functions list
end;

Procedure TForm1.ShowFunctions(Var FList : TList);
Var
  I            : Integer;
  L            : TListItem;
  FunctionCode : Integer;
Begin
// Each item in the Flist is a pointer that points to a TLJFunctionItem record
// In the loop each item is typecasted to a PLJFunctionItem (to allow it to access fields in the TLJFunctionItem record)-
// since it is a generic pointer (that does not have fields)
  With lv1 Do begin
    Items.BeginUpdate;
    Items.Clear;
    If FList.Count > 0 Then begin
      For I := 0 To FList.Count-1 Do Begin
        FunctionCode := PLJFunctionItem(FList[I]).ItemCode; // Typecast generic pointers to TLJFunctionItem pointers
        If (FunctionCode And $FFFF0000) = 0 Then Begin      // function seperators has code  > $0000FFFF, so this is a function
          L := Items.Add;
          L.Caption := IntToStr(FunctionCode);
          L.SubItems.Add(PLJFunctionItem(FList[I]).Caption);
        End
        Else Begin                                          // this is a seperator
          L := Items.Add;
          L.Caption := '($'+IntToHex(PLJFunctionItem(FList[I]).ItemCode,8)+')';
          L.SubItems.Add(PLJFunctionItem(FList[I]).Caption);
        End;
      End;
    End;
    Items.EndUpdate;
  End;
  Width := Width+1; Width := Width-1; // danmed listview column autosize
End;


Procedure TForm1.GetFunctions(Var Msg : TMessage); //Message UM_GETFUNC;
Var
  PFunc : PLJFunctionItem;       // This is a pointer to a TLJFunctionItem
Begin
  FunctionsList.Clear;           // clear any items (just in case you click the button twice)
  PFunc := FunctionsBuffer;      // Setup PFunc to the start of the buffer
  While (Integer(PFunc) < Integer(FunctionsBuffer)+FunctionsBufferSize) Do Begin // as long as the value of pBuffer < (start+length) of buffer = still more items
    FunctionsList.Add(pFunc);                                                    // Add a pointer to the record in the buffer
    Inc(PFunc);                                                                  // Atually increments the value of pFunc with the size of a TLJFunctionItem record
  End;
  ShowFunctions(FunctionsList);
End;

Procedure TForm1.CheckCopyData(Var Msg : TMessage); // Message WM_COPYDATA;
Var
  CopyData : PCopyDataStruct;
Begin
  With Msg Do Begin
    CopyData := Pointer(LParam);
    Case CopyData.dwData Of
     _WMCOPY_FunctionsList : Begin
                               If Assigned(FunctionsBuffer) then FreeMem(FunctionsBuffer,FunctionsBufferSize); // if you click more than once, free the buffer first
                               FunctionsBufferSize := CopyData^.cbData;                                        // save the size of the buffer
                               GetMem(FunctionsBuffer,FunctionsBufferSize);                                    // set aside memory for the buffer (FunctionsBuffer now points to the start of that area)
                               Move(CopyData.lpData^,FunctionsBuffer^,FunctionsBufferSize);                    // move the data from the message to the function buffer
                               PostMessage(Self.Handle,UM_GETFUNC,0,0);                                        // post messsage, so the data gets delt with - will eventually call get functions
                             End;
    End;
  End;
  Msg.Msg := 0; // release LJ waiting for the message call to finish
End;


procedure TForm1.lv1Change(Sender: TObject; Item: TListItem;  Change: TItemChange);
Var
  E     : Boolean;
  V,Err : Integer;
begin
  E := Assigned(lv1.Selected);
  If E Then Begin
    Val(lv1.Selected.Caption,V,Err); // err > 0 if not a proper string
    E := Err = 0;
  End;
  bDoFunction.Enabled := E;
end;


procedure TForm1.bDoFunctionMouseDown(Sender: TObject;  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
Var
  Err,FunctionCode: Integer;
  LJHandle        : THandle;
begin
  If Assigned(lv1.Selected) Then Begin
    LJHandle := GetLJHandle;
    Val(lv1.Selected.Caption,FunctionCode,Err); // err > 0 if not a proper string
    If (LJHandle > 0) And (Err = 0) Then SendMessage(LJHandle,_R_ExecuteFunctionOn,FunctionCode,0);
  End;
end;


procedure TForm1.bDoFunctionMouseUp(Sender: TObject; Button: TMouseButton;  Shift: TShiftState; X, Y: Integer);
Var
  Err,FunctionCode: Integer;
  LJHandle : THandle;
begin
  If Assigned(lv1.Selected) Then Begin
    LJHandle := GetLJHandle;
    Val(lv1.Selected.Caption,FunctionCode,Err); // err > 0 if not a proper string
    If (LJHandle > 0) And (Err = 0) Then SendMessage(LJHandle,_R_ExecuteFunctionOff,FunctionCode,0);
  End;

end;



end.
