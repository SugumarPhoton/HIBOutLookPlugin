Set Nocount On

Select * Into #HIBOPEmployeeStructure From HIBOPEmployeeStructure (Nolock)

Alter Table #HIBOPEmployeeStructure Add Lookupcode VARCHAR(6) Null

UPDATE T Set T.Lookupcode = E.LookupCode From #HIBOPEmployeeStructure T INNER JOIN EpicDMZSub.DBO.Employee E ON E.UniqEntity = T.UniqEntity

Delete T From HIBOPGetActivityAccountTemp T 
Where Not Exists(Select 1 From #HIBOPEmployeeStructure ES Where ES.Lookupcode = T.Lookupcode AND ES.UniqAgency = T.UniqAgency AND ES.UniqBranch = T.UniqBranch)

Drop Table #HIBOPEmployeeStructure

Set Nocount Off