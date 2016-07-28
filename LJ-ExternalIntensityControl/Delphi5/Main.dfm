object MainForm: TMainForm
  Left = 518
  Top = 244
  Width = 293
  Height = 257
  Caption = 'LJ Remote Intensity'
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
  object Label1: TLabel
    Left = 16
    Top = 34
    Width = 32
    Height = 13
    Caption = 'Master'
  end
  object Label2: TLabel
    Left = 68
    Top = 34
    Width = 6
    Height = 13
    Caption = '1'
  end
  object Label3: TLabel
    Left = 95
    Top = 34
    Width = 6
    Height = 13
    Caption = '2'
  end
  object Label4: TLabel
    Left = 122
    Top = 34
    Width = 6
    Height = 13
    Caption = '3'
  end
  object Label5: TLabel
    Left = 148
    Top = 34
    Width = 6
    Height = 13
    Caption = '4'
  end
  object Label6: TLabel
    Left = 175
    Top = 34
    Width = 6
    Height = 13
    Caption = '5'
  end
  object Label7: TLabel
    Left = 202
    Top = 34
    Width = 6
    Height = 13
    Caption = '6'
  end
  object Label8: TLabel
    Left = 229
    Top = 34
    Width = 6
    Height = 13
    Caption = '7'
  end
  object Label9: TLabel
    Left = 256
    Top = 34
    Width = 6
    Height = 13
    Caption = '8'
  end
  object lv3: TLabel
    Left = 121
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv4: TLabel
    Left = 147
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv5: TLabel
    Left = 174
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv6: TLabel
    Left = 201
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv7: TLabel
    Left = 228
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv8: TLabel
    Left = 255
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv0: TLabel
    Left = 23
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv1: TLabel
    Left = 67
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object lv2: TLabel
    Left = 94
    Top = 186
    Width = 18
    Height = 13
    Alignment = taCenter
    Caption = '000'
  end
  object SpeedButton1: TSpeedButton
    Left = 16
    Top = 202
    Width = 32
    Height = 22
    AllowAllUp = True
    GroupIndex = 1
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton2: TSpeedButton
    Left = 64
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 2
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton3: TSpeedButton
    Left = 91
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 3
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton4: TSpeedButton
    Left = 118
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 5
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton5: TSpeedButton
    Left = 144
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 4
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton6: TSpeedButton
    Left = 171
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 6
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton7: TSpeedButton
    Left = 198
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 7
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton8: TSpeedButton
    Left = 225
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 8
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object SpeedButton9: TSpeedButton
    Left = 252
    Top = 202
    Width = 24
    Height = 22
    AllowAllUp = True
    GroupIndex = 9
    Down = True
    Caption = 'On'
    OnMouseUp = SpeedButtonMouseUp
  end
  object Shape1: TShape
    Left = 7
    Top = 8
    Width = 15
    Height = 15
    Brush.Color = clRed
    Shape = stCircle
  end
  object Label19: TLabel
    Left = 25
    Top = 9
    Width = 45
    Height = 13
    Caption = 'LJ Ready'
  end
  object lVersion: TLabel
    Left = 88
    Top = 9
    Width = 37
    Height = 13
    Caption = 'lVersion'
  end
  object ScrollBar1: TScrollBar
    Left = 24
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 0
    OnChange = ScrollBarChange
  end
  object ScrollBar2: TScrollBar
    Left = 64
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 1
    OnChange = ScrollBarChange
  end
  object ScrollBar3: TScrollBar
    Left = 91
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 2
    OnChange = ScrollBarChange
  end
  object ScrollBar4: TScrollBar
    Left = 118
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 3
    OnChange = ScrollBarChange
  end
  object ScrollBar5: TScrollBar
    Left = 144
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 4
    OnChange = ScrollBarChange
  end
  object ScrollBar6: TScrollBar
    Left = 171
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 5
    OnChange = ScrollBarChange
  end
  object ScrollBar7: TScrollBar
    Left = 198
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 6
    OnChange = ScrollBarChange
  end
  object ScrollBar8: TScrollBar
    Left = 225
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 7
    OnChange = ScrollBarChange
  end
  object ScrollBar9: TScrollBar
    Left = 252
    Top = 50
    Width = 16
    Height = 128
    Kind = sbVertical
    Max = 255
    PageSize = 1
    TabOrder = 8
    OnChange = ScrollBarChange
  end
  object Timer1: TTimer
    Interval = 250
    OnTimer = Timer1Timer
    Top = 136
  end
end
