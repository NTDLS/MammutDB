using System;
using System.Runtime.Serialization;

namespace Mammut.TestHarness.Models
{
	public partial class Production_ProductDocument
	{
		#region Properties
		private int _productID;
		public int ProductID
		{
			get
			{
				return this._productID;
			}
			set
			{
				if (this._productID != value)
				{
					this._productID = value;
				}            
			}
		}
		private string _documentNode;
		public string DocumentNode
		{
			get
			{
				return this._documentNode;
			}
			set
			{
				if (this._documentNode != value)
				{
					this._documentNode = value;
				}            
			}
		}
		private DateTime _modifiedDate;
		public DateTime ModifiedDate
		{
			get
			{
				return this._modifiedDate;
			}
			set
			{
				if (this._modifiedDate != value)
				{
					this._modifiedDate = value;
				}            
			}
		}
			
		#endregion
	}
}
