Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.Xpo
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Xpo.Metadata.Helpers
Imports DevExpress.Xpo.DB

Namespace ObjectProperties

    ''' <summary>
    ''' Summary description for Form1.
    ''' </summary>
    Public Class Form1
        Inherits Form

        Private comboBox1 As ComboBox

        Private listBox1 As ListBox

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If

            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            comboBox1 = New ComboBox()
            listBox1 = New ListBox()
            Me.SuspendLayout()
            ' 
            ' comboBox1
            ' 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList
            comboBox1.Location = New System.Drawing.Point(24, 8)
            comboBox1.Name = "comboBox1"
            comboBox1.Size = New System.Drawing.Size(272, 24)
            comboBox1.TabIndex = 0
            AddHandler comboBox1.SelectedValueChanged, New EventHandler(AddressOf comboBox1_SelectedValueChanged)
            ' 
            ' listBox1
            ' 
            listBox1.ItemHeight = 16
            listBox1.Location = New System.Drawing.Point(24, 64)
            listBox1.Name = "listBox1"
            listBox1.Size = New System.Drawing.Size(272, 260)
            listBox1.TabIndex = 1
            ' 
            ' Form1
            ' 
            AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            ClientSize = New System.Drawing.Size(312, 341)
            Me.Controls.AddRange(New Control() {listBox1, comboBox1})
            Name = "Form1"
            Text = "Form1"
            AddHandler Load, New EventHandler(AddressOf Form1_Load)
            Me.ResumeLayout(False)
        End Sub

#End Region
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread>
        Shared Sub Main()
            Dim conn As String = MSSqlConnectionProvider.GetConnectionString("(local)", "Northwind")
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(conn, AutoCreateOption.SchemaAlreadyExists)
            XpoDefault.Session.Dictionary.CollectClassInfos(GetType(Customer).Assembly)
            Call Application.Run(New Form1())
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            For Each info As XPClassInfo In XpoDefault.Session.Dictionary.Classes
                If info.IsPersistent AndAlso info.IsVisibleInDesignTime Then comboBox1.Items.Add(info)
            Next
        End Sub

        Private Function GetObjectProperties(ByVal classInfo As XPClassInfo) As String()
            If classInfo IsNot Nothing Then Return GetObjectProperties(classInfo, New ArrayList())
            Return New String() {}
        End Function

        Private Function GetObjectProperties(ByVal xpoInfo As XPClassInfo, ByVal processed As ArrayList) As String()
            If processed.Contains(xpoInfo) Then Return New String() {}
            processed.Add(xpoInfo)
            Dim result As ArrayList = New ArrayList()
            For Each m As XPMemberInfo In xpoInfo.PersistentProperties
                If Not(TypeOf m Is ServiceField) AndAlso m.IsPersistent Then
                    result.Add(m.Name)
                    If m.ReferenceType IsNot Nothing Then
                        Dim childProps As String() = GetObjectProperties(m.ReferenceType, processed)
                        For Each child As String In childProps
                            result.Add(String.Format("{0}.{1}", m.Name, child))
                        Next
                    End If
                End If
            Next

            For Each m As XPMemberInfo In xpoInfo.CollectionProperties
                Dim childProps As String() = GetObjectProperties(m.CollectionElementType, processed)
                For Each child As String In childProps
                    result.Add(String.Format("{0}.{1}", m.Name, child))
                Next
            Next

            Return TryCast(result.ToArray(GetType(String)), String())
        End Function

        Private Sub comboBox1_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
            listBox1.Items.Clear()
            listBox1.Items.AddRange(GetObjectProperties(CType(comboBox1.SelectedItem, XPClassInfo)))
        End Sub
    End Class
End Namespace
