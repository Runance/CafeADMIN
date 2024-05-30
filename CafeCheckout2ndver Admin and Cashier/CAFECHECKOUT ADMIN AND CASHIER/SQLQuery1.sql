SELECT 
    CAST(Entry_Time AS DATE) AS Date,
    COUNT(*) AS NumberOfCustomers
FROM 
    dbo.Customer
GROUP BY 
    CAST(Entry_Time AS DATE)
ORDER BY 
    Date;