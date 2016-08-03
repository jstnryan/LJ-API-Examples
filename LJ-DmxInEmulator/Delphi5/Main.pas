unit Main;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, Buttons;

type

  TCopyData = Packed Record
    Code : DWord;
    Size : DWord;
    DPtr : Pointer;
  End;

  TMainForm = class(TForm)
    S1: TScrollBar;
    l1: TLabel;
    S2: TScrollBar;
    S3: TScrollBar;
    S4: TScrollBar;
    S5: TScrollBar;
    S6: TScrollBar;
    S7: TScrollBar;
    S8: TScrollBar;
    l3: TLabel;
    l4: TLabel;
    l5: TLabel;
    l6: TLabel;
    l7: TLabel;
    l8: TLabel;
    l2: TLabel;
    Timer1: TTimer;
    S9: TScrollBar;
    S10: TScrollBar;
    S11: TScrollBar;
    S12: TScrollBar;
    L9: TLabel;
    L10: TLabel;
    L11: TLabel;
    L12: TLabel;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    Label10: TLabel;
    Label11: TLabel;
    Label12: TLabel;
    Shape1: TShape;
    SpeedButton1: TSpeedButton;
    SpeedButton2: TSpeedButton;
    SpeedButton3: TSpeedButton;
    SpeedButton4: TSpeedButton;
    SpeedButton5: TSpeedButton;
    SpeedButton7: TSpeedButton;
    SpeedButton8: TSpeedButton;
    SpeedButton9: TSpeedButton;
    SpeedButton10: TSpeedButton;
    SpeedButton11: TSpeedButton;
    SpeedButton12: TSpeedButton;
    SpeedButton6: TSpeedButton;
    procedure S1Change(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure SpeedButton1MouseUp(Sender: TObject; Button: TMouseButton;  Shift: TShiftState; X, Y: Integer);
    procedure SpeedButton1MouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
  private
    PreBumpLevels   : Array[1..12] Of Integer;
  public
    Procedure SendData;
  end;

var
  MainForm  : TMainForm;
  Buffer    : Packed Array[0..11] Of Byte;

Const
    _WMCOPYDATA_DMXIN    = 256; // Received by LJ (DMXIN Emu)

implementation

{$R *.DFM}

Function Str3(B : Byte):String;
Begin
  Result := IntToStr(B);
  While Length(Result) < 3 Do Result := '0'+Result;
End;

Procedure TMainForm.SendData;
Var
  CopyData : TCopyData;
  Handle   : THandle;
Begin
  Handle := FindWindow('TLJMainForm',NIL);
  If Handle > 0 Then Begin
    FillChar(Buffer,SizeOf(Buffer),0);
    Buffer[0]  := 255-S1.Position;
    Buffer[1]  := 255-S2.Position;
    Buffer[2]  := 255-S3.Position;
    Buffer[3]  := 255-S4.Position;
    Buffer[4]  := 255-S5.Position;
    Buffer[5]  := 255-S6.Position;
    Buffer[6]  := 255-S7.Position;
    Buffer[7]  := 255-S8.Position;
    Buffer[8]  := 255-S9.Position;
    Buffer[9]  := 255-S10.Position;
    Buffer[10] := 255-S11.Position;
    Buffer[11] := 255-S12.Position;

    CopyData.Code      := _WMCOPYDATA_DMXIN;
    CopyData.Size      := SizeOf(Buffer);
    CopyData.DPtr      := @Buffer;
    SendMessage(Handle,WM_COPYDATA,0,LongInt(@CopyData));
  End;
End;

procedure TMainForm.S1Change(Sender: TObject);
begin
  L1.Caption := Str3(255-S1.Position);
  L2.Caption := Str3(255-S2.Position);
  L3.Caption := Str3(255-S3.Position);
  L4.Caption := Str3(255-S4.Position);
  L5.Caption := Str3(255-S5.Position);
  L6.Caption := Str3(255-S6.Position);
  L7.Caption := Str3(255-S7.Position);
  L8.Caption := Str3(255-S8.Position);
  L9.Caption := Str3(255-S9.Position);
  L10.Caption := Str3(255-S10.Position);
  L11.Caption := Str3(255-S11.Position);
  L12.Caption := Str3(255-S12.Position);
  SendData;
end;

procedure TMainForm.Timer1Timer(Sender: TObject);
begin
  SendData;
end;

procedure TMainForm.SpeedButton1MouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  With Sender As TSpeedButton Do Begin
    Case Tag Of
      1 : S1.Position := 255-PreBumpLevels[Tag];
      2 : S2.Position := 255-PreBumpLevels[Tag];
      3 : S3.Position := 255-PreBumpLevels[Tag];
      4 : S4.Position := 255-PreBumpLevels[Tag];
      5 : S5.Position := 255-PreBumpLevels[Tag];
      6 : S6.Position := 255-PreBumpLevels[Tag];
      7 : S7.Position := 255-PreBumpLevels[Tag];
      8 : S8.Position := 255-PreBumpLevels[Tag];
      9 : S9.Position := 255-PreBumpLevels[Tag];
     10 : S10.Position := 255-PreBumpLevels[Tag];
     11 : S11.Position := 255-PreBumpLevels[Tag];
     12 : S12.Position := 255-PreBumpLevels[Tag];
    End;
  End;
  SendData;
end;

procedure TMainForm.SpeedButton1MouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  With Sender As TSpeedButton Do Begin
    Case Tag Of
      1 : PreBumpLevels[Tag] := 255-S1.Position;
      2 : PreBumpLevels[Tag] := 255-S2.Position;
      3 : PreBumpLevels[Tag] := 255-S3.Position;
      4 : PreBumpLevels[Tag] := 255-S4.Position;
      5 : PreBumpLevels[Tag] := 255-S5.Position;
      6 : PreBumpLevels[Tag] := 255-S6.Position;
      7 : PreBumpLevels[Tag] := 255-S7.Position;
      8 : PreBumpLevels[Tag] := 255-S8.Position;
      9 : PreBumpLevels[Tag] := 255-S9.Position;
     10 : PreBumpLevels[Tag] := 255-S10.Position;
     11 : PreBumpLevels[Tag] := 255-S11.Position;
     12 : PreBumpLevels[Tag] := 255-S12.Position;
    End;
  End;
  With Sender As TSpeedButton Do Begin
    Case Tag Of
      1 : S1.Position := 0;
      2 : S2.Position := 0;
      3 : S3.Position := 0;
      4 : S4.Position := 0;
      5 : S5.Position := 0;
      6 : S6.Position := 0;
      7 : S7.Position := 0;
      8 : S8.Position := 0;
      9 : S9.Position := 0;
     10 : S10.Position := 0;
     11 : S11.Position := 0;
     12 : S12.Position := 0;
    End;
  End;
  SendData;
end;













end.
