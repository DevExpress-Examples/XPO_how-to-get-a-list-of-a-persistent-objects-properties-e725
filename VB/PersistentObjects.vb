Imports DevExpress.Xpo

Namespace ObjectProperties

    <Persistent("Customers")>
    Public Class Customer
        Inherits XPLiteObject

        <Key>
        Public CustomerID As String

        Public CompanyName As String

        Public ContactTitle As String

        <Association("CustomerOrders", GetType(Order))>
        Public ReadOnly Property Orders As XPCollection
            Get
                Return GetCollection("Orders")
            End Get
        End Property
    End Class

    <Persistent("Orders")>
    Public Class Order
        Inherits XPLiteObject

        <Key>
        Public OrderID As String

        <Persistent("CustomerID"), Association("CustomerOrders")>
        Public Customer As Customer

        <Persistent("EmployeeID"), Association("EmployeeOrders")>
        Public Employee As Employee

        <Association("FK_Orders_Shippers")>
        Public ShipVia As Shipper
    End Class

    <Persistent("Shippers")>
    Public Class Shipper
        Inherits XPLiteObject

        <Key>
        Public ShipperID As Integer

        Public CompanyName As String

        Public Phone As String

        <Association("FK_Orders_Shippers", GetType(Order))>
        Public ReadOnly Property Orders As XPCollection
            Get
                Return GetCollection("Orders")
            End Get
        End Property
    End Class

    <Persistent("Employees")>
    Public Class Employee
        Inherits XPLiteObject

        <Key>
        Public EmployeeID As String

        Public FirstName As String

        Public LastName As String

        <Association("EmployeeOrders", GetType(Order))>
        Public ReadOnly Property Orders As XPCollection
            Get
                Return GetCollection("Orders")
            End Get
        End Property

        <Persistent("TerritoryID"), Association("EmployeeTerritories", GetType(Territory))>
        Public ReadOnly Property Territories As XPCollection
            Get
                Return GetCollection("Territories")
            End Get
        End Property
    End Class

    <Persistent("Territories")>
    Public Class Territory
        Inherits XPLiteObject

        <Key>
        Public TerritoryID As String

        Public TerritoryDescription As String

        <Persistent("EmployeeID"), Association("EmployeeTerritories", GetType(Employee))>
        Public ReadOnly Property Employees As XPCollection
            Get
                Return GetCollection("Employees")
            End Get
        End Property
    End Class
End Namespace
