unit Main;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  Buttons, StdCtrls, ExtCtrls;

Const

  _R_LJReady                  = WM_USER+1502;        // LJ Returns 1 when ready, 0 otherwise
    _LJVersion                = 1;                   // LParam : Returns TLJVersion

    _WMCOPY_DMXOverride       = 267;                 // WM_COPYDATA Override DMX output channels subcode

  _MinVersion                 : DWord = $02050100;   // Major,Minor,Build,NA; // previous versions does not support this function
  _MaxCh                      = 2047;

type

  TExternalDMXOverride = Packed Record
    Reserved           : Array[0..15] Of Integer;
    ChFlags            : Array[0.._MaxCh] Of Byte;  // Bit 0 : Override (=1)
    Values             : Array[0.._MaxCh] Of Byte;
  End;

  TMainForm = class(TForm)
    sb1: TSpeedButton;
    sb2: TSpeedButton;
    sb3: TSpeedButton;
    sb4: TSpeedButton;
    lVersion: TLabel;
    Timer1: TTimer;
    procedure FormCreate(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure sbClick(Sender: TObject);
  private
    LJHandle            : THandle;
    VersionErrorShown   : Boolean;
    ExternalDMXOverride : TExternalDMXOverride;

  public

  end;

var
  MainForm: TMainForm;

implementation

{$R *.DFM}

procedure TMainForm.FormCreate(Sender: TObject);
begin
//
end;

procedure TMainForm.Timer1Timer(Sender: TObject);
Var
  Version : DWord;
  CopyDataStruct : TCopyDataStruct;
begin
  LJHandle := FindWindow('TLJMainForm',NIL);
  If LJHandle <> 0 Then Begin
    If SendMessage(LJHandle,_R_LJReady,0,0) = 1 Then Begin
      Version := SendMessage(LJHandle,_R_LJReady,_LJVersion,0);
      lVersion.Caption := Format('Version %d.%d - build %d',[Version SHR 24 And $FF, Version SHR 16 And $FF, Version SHR 8 And $FF]);
      If (Version <  _MinVersion) Then Begin
        Timer1.Enabled := False;
        If Not VersionErrorShown Then MessageDlg('This function requires LJ version 2.5 build 1 or higher.',mtError,[mbOk],0);
        LJHandle := 0;
        Close;
      End;
    End
    Else LJHandle := 0;
  End;
  If LJHandle = 0 Then lVersion.Caption := 'Looking for  LightJockey';
  SB1.Enabled := LJHandle <> 0;
  SB2.Enabled := LJHandle <> 0;
  SB3.Enabled := LJHandle <> 0;
  SB4.Enabled := LJHandle <> 0;
  If LJHandle <> 0 Then Begin
    CopyDataStruct.dwData :=_WMCOPY_DMXOverride;
    CopyDataStruct.cbData := SizeOf(TExternalDMXOverride);
    CopyDataStruct.lpData := @ExternalDMXOverride;
    SendMessage(LJHandle,WM_COPYDATA,Self.Handle,Integer(@CopyDataStruct));
  End;
end;

procedure TMainForm.sbClick(Sender: TObject);
Var
  I : Integer;
begin
  ZeroMemory(@ExternalDMXOverride,SizeOf(TExternalDMXOverride));
  If Sender = SB1 Then Begin
    For I := 0 To _MaxCh Do Begin
      ExternalDMXOverride.ChFlags[I] := 1; // Override all channels
      ExternalDMXOverride.Values[I]  := 0; // With 0-value
    End;
  End
  Else If Sender = SB2 Then Begin
    For I := 0 To _MaxCh Do Begin
      ExternalDMXOverride.ChFlags[I] := 1;   // Override all channels
      ExternalDMXOverride.Values[I]  := 255; // With 255-value
    End;
  End
  Else If Sender = SB3 Then Begin
    For I := 0 To _MaxCh Do Begin
      If Odd(I) then Begin // Odd Channels Only
        ExternalDMXOverride.ChFlags[I] := 1;   // Override channel
        ExternalDMXOverride.Values[I]  := 128; // With 128-value
      End;
    End;
  End 
  Else If Sender = SB4 Then Begin
    For I := 0 To _MaxCh Do Begin
      If Not Odd(I) then Begin // Even Channels Only
        ExternalDMXOverride.ChFlags[I] := 1;           // Override channel
        ExternalDMXOverride.Values[I]  := Random(256); //  With Random value
      End;
    End;
  End;
End;

end.
