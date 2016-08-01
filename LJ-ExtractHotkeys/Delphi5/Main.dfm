object Form1: TForm1
  Left = 321
  Top = 211
  Width = 485
  Height = 386
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Panel1: TPanel
    Left = 0
    Top = 318
    Width = 477
    Height = 41
    Align = alBottom
    TabOrder = 0
    object bGetList: TButton
      Left = 22
      Top = 8
      Width = 213
      Height = 25
      Caption = 'Get List'
      TabOrder = 0
      OnClick = bGetListClick
    end
    object bDoFunction: TButton
      Left = 242
      Top = 8
      Width = 213
      Height = 25
      Caption = 'Do Function'
      Enabled = False
      TabOrder = 1
      OnMouseDown = bDoFunctionMouseDown
      OnMouseUp = bDoFunctionMouseUp
    end
  end
  object lv1: TListView
    Left = 0
    Top = 0
    Width = 477
    Height = 318
    Align = alClient
    Columns = <
      item
        Caption = 'FunctionCode'
        Width = 100
      end
      item
        AutoSize = True
        Caption = 'Function Caption'
      end>
    ColumnClick = False
    GridLines = True
    HideSelection = False
    ReadOnly = True
    RowSelect = True
    TabOrder = 1
    ViewStyle = vsReport
    OnChange = lv1Change
  end
end
