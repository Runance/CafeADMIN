CREATE TABLE [dbo].[Trsn_Complete] (
    [Transaction_Id] INT             NOT NULL,
    [Customer_Id]    VARCHAR (50)    NOT NULL,
    [Product_Id]     VARCHAR (10)    NOT NULL,
    [Quantity]       INT             NOT NULL,
    [Addon_Id]       VARCHAR (10)    NULL,
    [Addon_Quantity] INT             NULL,
    [Total_Price]    DECIMAL (18, 2) NOT NULL,
    [Costumer_Money] DECIMAL         NULL,
    [Discount_Code]  VARCHAR (50)    NULL,
    [Final_Price]    DECIMAL (18, 2) NOT NULL,
    [Change_Amount]  DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Transaction_Id] ASC),
    FOREIGN KEY ([Discount_Code]) REFERENCES [dbo].[DiscountCodes] ([Code]),
    FOREIGN KEY ([Transaction_Id]) REFERENCES [dbo].[Transactions] ([Transaction_Id]),
    FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[Customer] ([Customer_Id]),
    FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[Products] ([Product_Id]),
    FOREIGN KEY ([Addon_Id]) REFERENCES [dbo].[Add_Ons] ([Addon_Id])
);

