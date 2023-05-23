ALTER TABLE dbo.CustomerService
ADD CONSTRAINT DF_CustomerService_StatusID DEFAULT 1 FOR StatusID;

ALTER TABLE dbo.CustomerService
ADD email nvarchar(50) NULL,
    note nvarchar(MAX) NULL,
  subtitle nvarchar(50) NULL;