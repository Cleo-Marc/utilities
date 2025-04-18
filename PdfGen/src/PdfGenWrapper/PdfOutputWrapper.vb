Imports System.Runtime.InteropServices
Imports PdfGen

<ComClass(PdfOutputWrapper.ClassId, PdfOutputWrapper.InterfaceId, PdfOutputWrapper.EventsId), ProgId("Cleopatra.PdfOutput")>
Public Class PdfOutputWrapper
    Implements IDisposable

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "09eab156-cc86-4c1a-85ad-2d05a36a79e2"
    Public Const InterfaceId As String = "d729b538-6c63-4203-b5a7-df33a7fb7470"
    Public Const EventsId As String = "483b02db-0603-421f-8468-b19ba720a9fb"
#End Region

    Private output As New PdfOutput

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub Close()
        output.Dispose()
    End Sub

    Public Property Background As String
        Get
            Return output.Background
        End Get

        Set(value As String)
            output.Background = value
        End Set
    End Property

#Disable Warning BC42102 ' Property cannot be exposed to COM as a property 'Let'
    Public Property Margins As Object
#Enable Warning BC42102 ' Property cannot be exposed to COM as a property 'Let'
        Get
            Return output.Margins
        End Get

        Set(value As Object)
            Dim marginsArray As Integer() = TryCast(value, Integer())
            If marginsArray Is Nothing Then
                Throw New ArgumentException("Margins must be an array of 4 integers")
            End If

            output.Margins = marginsArray
        End Set
    End Property

    Public Property FontName As String
        Get
            Return output.FontName
        End Get

        Set(value As String)
            output.FontName = value
        End Set
    End Property

    Public Property FontSize As Integer
        Get
            Return output.FontSize
        End Get

        Set(value As Integer)
            output.FontSize = value
        End Set
    End Property

    Public Property FontStyle As String
        Get
            Return output.FontStyle
        End Get
        Set(value As String)
            output.FontStyle = value
        End Set
    End Property

    Public Sub NewPage()
        output.NewPage()
    End Sub

    Public Sub AddText(text As String(), xmm As Integer, ymm As Integer)
        If text Is Nothing Then
            Throw New ArgumentException("Text cannot be null")
        End If
        output.AddText(text, xmm, ymm)
    End Sub

    Public Sub Save(filename As String)
        output.Save(filename)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        output.Dispose()
    End Sub
End Class


