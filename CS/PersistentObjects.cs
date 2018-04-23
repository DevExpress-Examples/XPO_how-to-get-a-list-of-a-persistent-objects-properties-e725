namespace ObjectProperties {
    using System;
    using DevExpress.Xpo;
    
    
	[Persistent("Customers")]
	public class Customer : XPLiteObject {
		[Key]
		public string CustomerID;
		public string CompanyName;
		public string ContactTitle;

		[Association("CustomerOrders", typeof(Order))]
		public XPCollection Orders {
			get {
				return GetCollection("Orders");
			}
		}
	}

	[Persistent("Orders")]
	public class Order : XPLiteObject {
		[Key]
		public string OrderID;
		
		[Persistent("CustomerID"), Association("CustomerOrders")]
		public Customer Customer;

		[Persistent("EmployeeID"), Association("EmployeeOrders")]
		public Employee Employee;

		[Association("FK_Orders_Shippers")]
		public Shipper ShipVia;
	}

	[Persistent("Shippers")]
	public class Shipper : XPLiteObject {
		[Key]
		public int ShipperID;
		public string CompanyName;
		public string Phone;

		[Association("FK_Orders_Shippers", typeof(Order))]
		public XPCollection Orders {
			get { return GetCollection("Orders"); }
		}
	}

	[Persistent("Employees")]
	public class Employee : XPLiteObject {
		[Key]
		public string EmployeeID;
		public string FirstName;
		public string LastName;

		[Association("EmployeeOrders", typeof(Order))]
		public XPCollection Orders {
			get {
				return GetCollection("Orders");
			}
		}

		[Persistent("TerritoryID"), Association("EmployeeTerritories", typeof(Territory))]
		public XPCollection Territories {
			get {
				return GetCollection("Territories");
			}
		}
	}

	[Persistent("Territories")]
	public class Territory : XPLiteObject {
		[Key]
		public string TerritoryID;
		public string TerritoryDescription;

		[Persistent("EmployeeID"), Association("EmployeeTerritories", typeof(Employee))]
		public XPCollection Employees {
			get {
				return GetCollection("Employees");
			}
		}
	}
}
