Imports Microsoft.VisualBasic
	Imports System
	Imports DevExpress.Xpo
Namespace ObjectProperties


	<Persistent("Customers")> _
	Public Class Customer
		Inherits XPLiteObject
		<Key> _
		Public CustomerID As String
		Public CompanyName As String
		Public ContactTitle As String

		<Association("CustomerOrders", GetType(Order))> _
		Public ReadOnly Property Orders() As XPCollection
			Get
				Return GetCollection("Orders")
			End Get
		End Property
	End Class

	<Persistent("Orders")> _
	Public Class Order
		Inherits XPLiteObject
		<Key> _
		Public OrderID As String

		<Persistent("CustomerID"), Association("CustomerOrders")> _
		Public Customer As Customer

		<Persistent("EmployeeID"), Association("EmployeeOrders")> _
		Public Employee As Employee

		<Association("FK_Orders_Shippers")> _
		Public ShipVia As Shipper
	End Class

	<Persistent("Shippers")> _
	Public Class Shipper
		Inherits XPLiteObject
		<Key> _
		Public ShipperID As Integer
		Public CompanyName As String
		Public Phone As String

		<Association("FK_Orders_Shippers", GetType(Order))> _
		Public ReadOnly Property Orders() As XPCollection
			Get
				Return GetCollection("Orders")
			End Get
		End Property
	End Class

	<Persistent("Employees")> _
	Public Class Employee
		Inherits XPLiteObject
		<Key> _
		Public EmployeeID As String
		Public FirstName As String
		Public LastName As String

		<Association("EmployeeOrders", GetType(Order))> _
		Public ReadOnly Property Orders() As XPCollection
			Get
				Return GetCollection("Orders")
			End Get
		End Property

		<Persistent("TerritoryID"), Association("EmployeeTerritories", GetType(Territory))> _
		Public ReadOnly Property Territories() As XPCollection
			Get
				Return GetCollection("Territories")
			End Get
		End Property
	End Class

	<Persistent("Territories")> _
	Public Class Territory
		Inherits XPLiteObject
		<Key> _
		Public TerritoryID As String
		Public TerritoryDescription As String

		<Persistent("EmployeeID"), Association("EmployeeTerritories", GetType(Employee))> _
		Public ReadOnly Property Employees() As XPCollection
			Get
				Return GetCollection("Employees")
			End Get
		End Property
	End Class
End Namespace
