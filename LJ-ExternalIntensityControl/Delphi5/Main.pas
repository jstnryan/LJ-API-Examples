unit Main;
// April 16 - 2002
// Sample app for Martin LightJockey
// Sample app. that demonstrates how to set the Intensity Master levels from another Windows app. using
// WM_COPYDATA messagees. Note this function is only implemented in LightJockey versions from version 2.2 build 2.
// Notes :
// Setting the level remotely will not stop users from setting the levels manually
// The function will not work on intensity masters that are patched to DMX in
//
//


interface
uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, Buttons, ExtCtrls;


Const
  _R_LJReady                  = WM_USER+1502;        // LJ Returns 1 when ready, 0 otherwise
    _LJVersion                = 1;                   // LParam : Returns TLJVersion

  _WMCOPY_SetIntensityMasters = 266;                 // Message sub-code to decode COPYDATA as Master Intensity Data

  _MinVersion                 : DWord = $02020200;   // Major,Minor,Build,NA; // previous versions does not support this function



type

  TMasterIntensitySettings = Packed Record // structure must be byte aligned !
//Index 0   : Master
//Index 1-8 : Submasters
    Flags  : Array[0..8] Of Byte;          // = 0 LJ Ignore, <> 0 :LJ set-value
    Values : Array[0..8] Of Byte;          // Master value (0-255)
  End;


  TMainForm = class(TForm)
    ScrollBar1: TScrollBar;
    ScrollBar2: TScrollBar;
    ScrollBar3: TScrollBar;
    ScrollBar4: TScrollBar;
    ScrollBar5: TScrollBar;
    ScrollBar6: TScrollBar;
    ScrollBar7: TScrollBar;
    ScrollBar8: TScrollBar;
    ScrollBar9: TScrollBar;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    lv3: TLabel;
    lv4: TLabel;
    lv5: TLabel;
    lv6: TLabel;
    lv7: TLabel;
    lv8: TLabel;
    lv0: TLabel;
    lv1: TLabel;
    lv2: TLabel;
    SpeedButton1: TSpeedButton;
    SpeedButton2: TSpeedButton;
    SpeedButton3: TSpeedButton;
    SpeedButton4: TSpeedButton;
    SpeedButton5: TSpeedButton;
    SpeedButton6: TSpeedButton;
    SpeedButton7: TSpeedButton;
    SpeedButton8: TSpeedButton;
    SpeedButton9: TSpeedButton;
    Shape1: TShape;
    Label19: TLabel;
    Timer1: TTimer;
    lVersion: TLabel;
    procedure Timer1Timer(Sender: TObject);
    procedure ScrollBarChange(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure SpeedButtonMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
  private
    LJHandle       : THandle;
    ErrorMsgShown  : Boolean;
    IntensityData  : TMasterIntensitySettings;

    Procedure       UpdateFaderValues;
    Procedure       SendData;
  public
  end;

var
  MainForm: TMainForm;

implementation
{$R *.DFM}

procedure TMainForm.FormCreate(Sender: TObject);
Var
  I : Integer;
begin
  ZeroMemory(@IntensityData,SizeOf(TMasterIntensitySettings));
  For I := 0 To 8 Do IntensityData.Values[I] := 255;
  For I := 0 To 8 Do IntensityData.Flags[I] := 1;
  lVersion.Caption := '';
  ScrollBarChange(NIL);
  UpdateFaderValues;
end;


procedure TMainForm.Timer1Timer(Sender: TObject);
Var
  Version : DWord;
Const
  _ReadyCol : Array[Boolean] Of TColor = (clRed,clLime);

begin
  LJHandle := FindWindow('TLJMainForm',NIL);
  If LJHandle <> 0 Then Begin
    If SendMessage(LJHandle,_R_LJReady,0,0) = 1 Then Begin
      Version := SendMessage(LJHandle,_R_LJReady,_LJVersion,0);
      lVersion.Caption := Format('Version %d.%d - build %d',[Version SHR 24 And $FF, Version SHR 16 And $FF, Version SHR 8 And $FF]);
      If (Version <  _MinVersion) Then Begin
        If Not ErrorMsgShown Then Begin
          ErrorMsgShown := True;
          MessageDlg('This function requires LJ version 2.2 build 2 or higher.',mtError,[mbOk],0);
        End;
        LJHandle := 0;
      End;
    End
    Else LJHandle := 0;
  End;
  Shape1.Brush.Color := _ReadyCol[LJHandle <> 0];
  If LJHandle = 0 Then lVersion.Caption := '';
end;

Procedure TMainForm.UpdateFaderValues;
Begin
  ScrollBar1.Position := 255-IntensityData.Values[0];
  ScrollBar2.Position := 255-IntensityData.Values[1];
  ScrollBar3.Position := 255-IntensityData.Values[2];
  ScrollBar4.Position := 255-IntensityData.Values[3];
  ScrollBar5.Position := 255-IntensityData.Values[4];
  ScrollBar6.Position := 255-IntensityData.Values[5];
  ScrollBar7.Position := 255-IntensityData.Values[6];
  ScrollBar8.Position := 255-IntensityData.Values[7];
  ScrollBar9.Position := 255-IntensityData.Values[8];

  SpeedButton1.Down := IntensityData.Flags[0] <> 0;
  SpeedButton2.Down := IntensityData.Flags[1] <> 0;
  SpeedButton3.Down := IntensityData.Flags[2] <> 0;
  SpeedButton4.Down := IntensityData.Flags[3] <> 0;
  SpeedButton5.Down := IntensityData.Flags[4] <> 0;
  SpeedButton6.Down := IntensityData.Flags[5] <> 0;
  SpeedButton7.Down := IntensityData.Flags[6] <> 0;
  SpeedButton8.Down := IntensityData.Flags[7] <> 0;
  SpeedButton9.Down := IntensityData.Flags[8] <> 0;
End;



procedure TMainForm.ScrollBarChange(Sender: TObject);
begin
  IntensityData.Values[0] := 255-ScrollBar1.Position;
  IntensityData.Values[1] := 255-ScrollBar2.Position;
  IntensityData.Values[2] := 255-ScrollBar3.Position;
  IntensityData.Values[3] := 255-ScrollBar4.Position;
  IntensityData.Values[4] := 255-ScrollBar5.Position;
  IntensityData.Values[5] := 255-ScrollBar6.Position;
  IntensityData.Values[6] := 255-ScrollBar7.Position;
  IntensityData.Values[7] := 255-ScrollBar8.Position;
  IntensityData.Values[8] := 255-ScrollBar9.Position;
  SendData;
  lv0.Caption := Format('%d',[IntensityData.Values[0]]);
  lv1.Caption := Format('%d',[IntensityData.Values[1]]);
  lv2.Caption := Format('%d',[IntensityData.Values[2]]);
  lv3.Caption := Format('%d',[IntensityData.Values[3]]);
  lv4.Caption := Format('%d',[IntensityData.Values[4]]);
  lv5.Caption := Format('%d',[IntensityData.Values[5]]);
  lv6.Caption := Format('%d',[IntensityData.Values[6]]);
  lv7.Caption := Format('%d',[IntensityData.Values[7]]);
  lv8.Caption := Format('%d',[IntensityData.Values[8]]);
end;

procedure TMainForm.SpeedButtonMouseUp(Sender: TObject;  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  If Sender = SpeedButton1 Then Begin
    If SpeedButton1.Down Then IntensityData.Flags[0] := 0
    Else                      IntensityData.Flags[0] := 1;
  End;
  If Sender = SpeedButton2 Then Begin
    If SpeedButton2.Down Then IntensityData.Flags[1] := 0
    Else                      IntensityData.Flags[1] := 1;
  End;
  If Sender = SpeedButton3 Then Begin
    If SpeedButton3.Down Then IntensityData.Flags[2] := 0
    Else                      IntensityData.Flags[2] := 1;
  End;
  If Sender = SpeedButton4 Then Begin
    If SpeedButton4.Down Then IntensityData.Flags[3] := 0
    Else                      IntensityData.Flags[3] := 1;
  End;
  If Sender = SpeedButton5 Then Begin
    If SpeedButton5.Down Then IntensityData.Flags[4] := 0
    Else                      IntensityData.Flags[4] := 1;
  End;
  If Sender = SpeedButton6 Then Begin
    If SpeedButton6.Down Then IntensityData.Flags[5] := 0
    Else                      IntensityData.Flags[5] := 1;
  End;
  If Sender = SpeedButton7 Then Begin
    If SpeedButton7.Down Then IntensityData.Flags[6] := 0
    Else                      IntensityData.Flags[6] := 1;
  End;
  If Sender = SpeedButton8 Then Begin
    If SpeedButton8.Down Then IntensityData.Flags[7] := 0
    Else                      IntensityData.Flags[7] := 1;
  End;
  If Sender = SpeedButton9 Then Begin
    If SpeedButton9.Down Then IntensityData.Flags[8] := 0
    Else                      IntensityData.Flags[8] := 1;
  End;
  SendData;
end;


Procedure TMainForm.SendData;
var
  CopyDataStruct : TCopyDataStruct;
Begin
  If (LJHandle <>0) Then Begin
    CopyDataStruct.dwData :=_WMCOPY_SetIntensityMasters;
    CopyDataStruct.cbData := SizeOf(TMasterIntensitySettings);
    CopyDataStruct.lpData := @IntensityData;
    SendMessage(LJHandle,WM_COPYDATA,Self.Handle,Integer(@CopyDataStruct));
  End;
End;

end.
