object MainForm: TMainForm
  Left = 408
  Top = 368
  Width = 172
  Height = 192
  BorderIcons = [biSystemMenu]
  Caption = 'LJ external DMX'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object sb1: TSpeedButton
    Left = 7
    Top = 40
    Width = 150
    Height = 25
    GroupIndex = 1
    Caption = 'All Channels @ 0'
    Enabled = False
    OnClick = sbClick
  end
  object sb2: TSpeedButton
    Left = 7
    Top = 72
    Width = 150
    Height = 25
    GroupIndex = 1
    Caption = 'All Channels @255'
    Enabled = False
    OnClick = sbClick
  end
  object sb3: TSpeedButton
    Left = 7
    Top = 104
    Width = 150
    Height = 25
    GroupIndex = 1
    Caption = 'Odd Channels @128'
    Enabled = False
    OnClick = sbClick
  end
  object sb4: TSpeedButton
    Left = 7
    Top = 136
    Width = 150
    Height = 25
    GroupIndex = 1
    Caption = 'Even Channels @Random'
    Enabled = False
    OnClick = sbClick
  end
  object lVersion: TLabel
    Left = 21
    Top = 8
    Width = 122
    Height = 13
    Caption = 'Looking for LightJockey...'
  end
  object Timer1: TTimer
    Interval = 100
    OnTimer = Timer1Timer
    Left = 64
    Top = 40
  end
end
