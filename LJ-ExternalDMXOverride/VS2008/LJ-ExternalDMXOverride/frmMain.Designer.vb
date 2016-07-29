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
        Me.components = New System.ComponentModel.Container
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblLJReady = New System.Windows.Forms.Label
        Me.rdo1 = New System.Windows.Forms.RadioButton
        Me.rdo2 = New System.Windows.Forms.RadioButton
        Me.rdo3 = New System.Windows.Forms.RadioButton
        Me.rdo4 = New System.Windows.Forms.RadioButton
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "LightJockey:"
        '
        'lblLJReady
        '
        Me.lblLJReady.AutoSize = True
        Me.lblLJReady.Location = New System.Drawing.Point(97, 9)
        Me.lblLJReady.Name = "lblLJReady"
        Me.lblLJReady.Size = New System.Drawing.Size(129, 13)
        Me.lblLJReady.TabIndex = 1
        Me.lblLJReady.Text = "Looking for LightJockey..."
        '
        'rdo1
        '
        Me.rdo1.Appearance = System.Windows.Forms.Appearance.Button
        Me.rdo1.Location = New System.Drawing.Point(12, 34)
        Me.rdo1.Name = "rdo1"
        Me.rdo1.Size = New System.Drawing.Size(142, 24)
        Me.rdo1.TabIndex = 6
        Me.rdo1.TabStop = True
        Me.rdo1.Text = "All Channels @0"
        Me.rdo1.UseVisualStyleBackColor = True
        '
        'rdo2
        '
        Me.rdo2.Appearance = System.Windows.Forms.Appearance.Button
        Me.rdo2.Location = New System.Drawing.Point(160, 34)
        Me.rdo2.Name = "rdo2"
        Me.rdo2.Size = New System.Drawing.Size(142, 24)
        Me.rdo2.TabIndex = 7
        Me.rdo2.TabStop = True
        Me.rdo2.Text = "All Channels @255"
        Me.rdo2.UseVisualStyleBackColor = True
        '
        'rdo3
        '
        Me.rdo3.Appearance = System.Windows.Forms.Appearance.Button
        Me.rdo3.Location = New System.Drawing.Point(12, 64)
        Me.rdo3.Name = "rdo3"
        Me.rdo3.Size = New System.Drawing.Size(142, 24)
        Me.rdo3.TabIndex = 8
        Me.rdo3.TabStop = True
        Me.rdo3.Text = "Odd Channels @128"
        Me.rdo3.UseVisualStyleBackColor = True
        '
        'rdo4
        '
        Me.rdo4.Appearance = System.Windows.Forms.Appearance.Button
        Me.rdo4.Location = New System.Drawing.Point(160, 64)
        Me.rdo4.Name = "rdo4"
        Me.rdo4.Size = New System.Drawing.Size(142, 24)
        Me.rdo4.TabIndex = 9
        Me.rdo4.TabStop = True
        Me.rdo4.Text = "Even Channels @Random"
        Me.rdo4.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(314, 100)
        Me.Controls.Add(Me.rdo4)
        Me.Controls.Add(Me.rdo3)
        Me.Controls.Add(Me.rdo2)
        Me.Controls.Add(Me.rdo1)
        Me.Controls.Add(Me.lblLJReady)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmMain"
        Me.Text = "LJ DMX Override"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblLJReady As System.Windows.Forms.Label
    Friend WithEvents rdo1 As System.Windows.Forms.RadioButton
    Friend WithEvents rdo2 As System.Windows.Forms.RadioButton
    Friend WithEvents rdo3 As System.Windows.Forms.RadioButton
    Friend WithEvents rdo4 As System.Windows.Forms.RadioButton
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
