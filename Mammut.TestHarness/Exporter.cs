using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Mammut.TestHarness
{
	public static class Exporter
	{        
		public static void ExportAll()
		{
				(new Repository.dbo_AWBuildVersionRepository()).Export_dbo_AWBuildVersion();
				(new Repository.dbo_DatabaseLogRepository()).Export_dbo_DatabaseLog();
				(new Repository.dbo_ErrorLogRepository()).Export_dbo_ErrorLog();
				(new Repository.HumanResources_DepartmentRepository()).Export_HumanResources_Department();
				(new Repository.HumanResources_EmployeeRepository()).Export_HumanResources_Employee();
				(new Repository.HumanResources_EmployeeDepartmentHistoryRepository()).Export_HumanResources_EmployeeDepartmentHistory();
				(new Repository.HumanResources_EmployeePayHistoryRepository()).Export_HumanResources_EmployeePayHistory();
				(new Repository.HumanResources_JobCandidateRepository()).Export_HumanResources_JobCandidate();
				(new Repository.HumanResources_ShiftRepository()).Export_HumanResources_Shift();
				(new Repository.Person_AddressRepository()).Export_Person_Address();
				(new Repository.Person_AddressTypeRepository()).Export_Person_AddressType();
				(new Repository.Person_BusinessEntityRepository()).Export_Person_BusinessEntity();
				(new Repository.Person_BusinessEntityAddressRepository()).Export_Person_BusinessEntityAddress();
				(new Repository.Person_BusinessEntityContactRepository()).Export_Person_BusinessEntityContact();
				(new Repository.Person_ContactTypeRepository()).Export_Person_ContactType();
				(new Repository.Person_CountryRegionRepository()).Export_Person_CountryRegion();
				(new Repository.Person_EmailAddressRepository()).Export_Person_EmailAddress();
				(new Repository.Person_PasswordRepository()).Export_Person_Password();
				(new Repository.Person_PersonRepository()).Export_Person_Person();
				(new Repository.Person_PersonPhoneRepository()).Export_Person_PersonPhone();
				(new Repository.Person_PhoneNumberTypeRepository()).Export_Person_PhoneNumberType();
				(new Repository.Person_StateProvinceRepository()).Export_Person_StateProvince();
				(new Repository.Production_BillOfMaterialsRepository()).Export_Production_BillOfMaterials();
				(new Repository.Production_CultureRepository()).Export_Production_Culture();
				(new Repository.Production_DocumentRepository()).Export_Production_Document();
				(new Repository.Production_IllustrationRepository()).Export_Production_Illustration();
				(new Repository.Production_LocationRepository()).Export_Production_Location();
				(new Repository.Production_ProductRepository()).Export_Production_Product();
				(new Repository.Production_ProductCategoryRepository()).Export_Production_ProductCategory();
				(new Repository.Production_ProductCostHistoryRepository()).Export_Production_ProductCostHistory();
				(new Repository.Production_ProductDescriptionRepository()).Export_Production_ProductDescription();
				(new Repository.Production_ProductDocumentRepository()).Export_Production_ProductDocument();
				(new Repository.Production_ProductInventoryRepository()).Export_Production_ProductInventory();
				(new Repository.Production_ProductListPriceHistoryRepository()).Export_Production_ProductListPriceHistory();
				(new Repository.Production_ProductModelRepository()).Export_Production_ProductModel();
				(new Repository.Production_ProductModelIllustrationRepository()).Export_Production_ProductModelIllustration();
				(new Repository.Production_ProductModelProductDescriptionCultureRepository()).Export_Production_ProductModelProductDescriptionCulture();
				(new Repository.Production_ProductPhotoRepository()).Export_Production_ProductPhoto();
				(new Repository.Production_ProductProductPhotoRepository()).Export_Production_ProductProductPhoto();
				(new Repository.Production_ProductReviewRepository()).Export_Production_ProductReview();
				(new Repository.Production_ProductSubcategoryRepository()).Export_Production_ProductSubcategory();
				(new Repository.Production_ScrapReasonRepository()).Export_Production_ScrapReason();
				(new Repository.Production_TransactionHistoryRepository()).Export_Production_TransactionHistory();
				(new Repository.Production_TransactionHistoryArchiveRepository()).Export_Production_TransactionHistoryArchive();
				(new Repository.Production_UnitMeasureRepository()).Export_Production_UnitMeasure();
				(new Repository.Production_WorkOrderRepository()).Export_Production_WorkOrder();
				(new Repository.Production_WorkOrderRoutingRepository()).Export_Production_WorkOrderRouting();
				(new Repository.Purchasing_ProductVendorRepository()).Export_Purchasing_ProductVendor();
				(new Repository.Purchasing_PurchaseOrderDetailRepository()).Export_Purchasing_PurchaseOrderDetail();
				(new Repository.Purchasing_PurchaseOrderHeaderRepository()).Export_Purchasing_PurchaseOrderHeader();
				(new Repository.Purchasing_ShipMethodRepository()).Export_Purchasing_ShipMethod();
				(new Repository.Purchasing_VendorRepository()).Export_Purchasing_Vendor();
				(new Repository.Sales_CountryRegionCurrencyRepository()).Export_Sales_CountryRegionCurrency();
				(new Repository.Sales_CreditCardRepository()).Export_Sales_CreditCard();
				(new Repository.Sales_CurrencyRepository()).Export_Sales_Currency();
				(new Repository.Sales_CurrencyRateRepository()).Export_Sales_CurrencyRate();
				(new Repository.Sales_CustomerRepository()).Export_Sales_Customer();
				(new Repository.Sales_PersonCreditCardRepository()).Export_Sales_PersonCreditCard();
				(new Repository.Sales_SalesOrderDetailRepository()).Export_Sales_SalesOrderDetail();
				(new Repository.Sales_SalesOrderHeaderRepository()).Export_Sales_SalesOrderHeader();
				(new Repository.Sales_SalesOrderHeaderSalesReasonRepository()).Export_Sales_SalesOrderHeaderSalesReason();
				(new Repository.Sales_SalesPersonRepository()).Export_Sales_SalesPerson();
				(new Repository.Sales_SalesPersonQuotaHistoryRepository()).Export_Sales_SalesPersonQuotaHistory();
				(new Repository.Sales_SalesReasonRepository()).Export_Sales_SalesReason();
				(new Repository.Sales_SalesTaxRateRepository()).Export_Sales_SalesTaxRate();
				(new Repository.Sales_SalesTerritoryRepository()).Export_Sales_SalesTerritory();
				(new Repository.Sales_SalesTerritoryHistoryRepository()).Export_Sales_SalesTerritoryHistory();
				(new Repository.Sales_ShoppingCartItemRepository()).Export_Sales_ShoppingCartItem();
				(new Repository.Sales_SpecialOfferRepository()).Export_Sales_SpecialOffer();
				(new Repository.Sales_SpecialOfferProductRepository()).Export_Sales_SpecialOfferProduct();
				(new Repository.Sales_StoreRepository()).Export_Sales_Store();
				}
	}
}

