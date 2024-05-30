SELECT 
    p.Product_Name,
    SUM(tc.Quantity) AS TotalQuantity
FROM 
    dbo.Trsn_Complete tc
JOIN 
    dbo.Products p ON tc.Product_Id = p.Product_Id
GROUP BY 
    p.Product_Name
ORDER BY 
    TotalQuantity DESC;