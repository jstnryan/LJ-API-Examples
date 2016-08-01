<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dgvFunctions = New System.Windows.Forms.DataGridView
        Me.cmdGetList = New System.Windows.Forms.Button
        Me.cmdDoFunc = New System.Windows.Forms.Button
        Me.colCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colDesc = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.dgvFunctions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvFunctions
        '
        Me.dgvFunctions.AllowUserToAddRows = False
        Me.dgvFunctions.AllowUserToDeleteRows = False
        Me.dgvFunctions.AllowUserToOrderColumns = True
        Me.dgvFunctions.AllowUserToResizeColumns = False
        Me.dgvFunctions.AllowUserToResizeRows = False
        Me.dgvFunctions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvFunctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFunctions.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colCode, Me.colDesc})
        Me.dgvFunctions.Location = New System.Drawing.Point(12, 12)
        Me.dgvFunctions.Name = "dgvFunctions"
        Me.dgvFunctions.RowHeadersVisible = False
        Me.dgvFunctions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFunctions.Size = New System.Drawing.Size(414, 383)
        Me.dgvFunctions.TabIndex = 0
        '
        'cmdGetList
        '
        Me.cmdGetList.Location = New System.Drawing.Point(12, 401)
        Me.cmdGetList.Name = "cmdGetList"
        Me.cmdGetList.Size = New System.Drawing.Size(203, 23)
        Me.cmdGetList.TabIndex = 1
        Me.cmdGetList.Text = "Get List"
        Me.cmdGetList.UseVisualStyleBackColor = True
        '
        'cmdDoFunc
        '
        Me.cmdDoFunc.Location = New System.Drawing.Point(223, 401)
        Me.cmdDoFunc.Name = "cmdDoFunc"
        Me.cmdDoFunc.Size = New System.Drawing.Size(203, 23)
        Me.cmdDoFunc.TabIndex = 2
        Me.cmdDoFunc.Text = "Do Function"
        Me.cmdDoFunc.UseVisualStyleBackColor = True
        '
        'colCode
        '
        Me.colCode.FillWeight = 50.0!
        Me.colCode.HeaderText = "Code"
        Me.colCode.Name = "colCode"
        Me.colCode.ReadOnly = True
        '
        'colDesc
        '
        Me.colDesc.FillWeight = 150.0!
        Me.colDesc.HeaderText = "Function Description"
        Me.colDesc.Name = "colDesc"
        Me.colDesc.ReadOnly = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(438, 436)
        Me.Controls.Add(Me.cmdDoFunc)
        Me.Controls.Add(Me.cmdGetList)
        Me.Controls.Add(Me.dgvFunctions)
        Me.Name = "frmMain"
        Me.Text = "Extract Hotkeys"
        CType(Me.dgvFunctions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvFunctions As System.Windows.Forms.DataGridView
    Friend WithEvents cmdGetList As System.Windows.Forms.Button
    Friend WithEvents cmdDoFunc As System.Windows.Forms.Button
    Friend WithEvents colCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDesc As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
